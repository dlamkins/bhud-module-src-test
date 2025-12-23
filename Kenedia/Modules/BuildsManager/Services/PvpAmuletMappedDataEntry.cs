using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class PvpAmuletMappedDataEntry : MappedDataEntry<int, Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet>
	{
		public override async Task<bool> LoadCached(string name, string path, CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				return false;
			}
			if (!System.IO.File.Exists(path))
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("No local data for " + name + " found at '" + Path.GetFileName(path) + "'");
				return false;
			}
			BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("Loading local data for " + name + " from '" + Path.GetFileName(path) + "'");
			if (System.IO.File.Exists(path))
			{
				string json = System.IO.File.ReadAllText(path);
				if (token.IsCancellationRequested)
				{
					return false;
				}
				PvpAmuletMappedDataEntry loaded = JsonConvert.DeserializeObject<PvpAmuletMappedDataEntry>(json, SerializerSettings.Default);
				if (loaded != null)
				{
					base.Map = loaded.Map;
					base.Items = loaded.Items;
					base.Version = loaded.Version;
					DataLoaded = true;
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"Loaded local data for {name} with {base.Items.Count} entries. Version {base.Version}");
					return true;
				}
			}
			return false;
		}

		public override async Task<(bool, List<object>)> IsIncomplete(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			base.Map = map;
			Locale value = GameService.Overlay.UserLocale.Value;
			bool flag = (((uint)(value - 4) <= 1u) ? true : false);
			Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
			IEnumerable<int> enumerable;
			if (!(map.Version > base.Version))
			{
				enumerable = base.Map.Items.Values.Where((int id) => !base.Items.TryGetValue(id, out var value2) || value2.Names[lang] == null);
			}
			else
			{
				IEnumerable<int> values = map.Items.Values;
				enumerable = values;
			}
			IEnumerable<int> missing = enumerable;
			return (missing.Any(), missing.Cast<object>().ToList());
		}

		public override async Task<bool> Update(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			_ = 1;
			try
			{
				if (token.IsCancellationRequested)
				{
					return false;
				}
				List<object> missing = (await IsIncomplete(name, map, path, gw2ApiManager, token)).Item2;
				if (missing.Any() && missing.All((object e) => e is int))
				{
					List<List<int>> idSets = missing.Cast<int>().ToList().ChunkBy(200);
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"{name} updating {missing.Count()} entries in {idSets.Count} sets.");
					foreach (List<int> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.PvpAmulet> items = await gw2ApiManager.Gw2ApiClient.V2.Pvp.Amulets.ManyAsync(ids, token);
						if (token.IsCancellationRequested)
						{
							return false;
						}
						foreach (Gw2Sharp.WebApi.V2.Models.PvpAmulet item in items)
						{
							Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet entryItem;
							bool num = base.Items.Values.TryFind((Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet e) => e.Id == item.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet
								{
									MappedId = (base.Map?.Items?.FirstOrDefault((KeyValuePair<byte, int> e) => e.Value == item.Id).Key).GetValueOrDefault()
								};
							}
							entryItem?.Apply(item);
							if (!num)
							{
								base.Items.Add(item.Id, entryItem);
							}
						}
					}
					base.Version = base.Map.Version;
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"Saving updated {name} data with {missing.Count()} updated entries. Version {base.Version}");
					string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
					System.IO.File.WriteAllText(path, json);
					DataLoaded = true;
					return true;
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Warn(ex, "Failed to update " + name + " data.");
			}
			return false;
		}
	}
}
