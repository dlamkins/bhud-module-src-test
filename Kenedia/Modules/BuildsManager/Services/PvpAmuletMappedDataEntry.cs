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
		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			try
			{
				bool saveRequired = false;
				MappedDataEntry<int, Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet> loaded = null;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load and if required update " + name);
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<MappedDataEntry<int, Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet>>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"{name} Version {base.Version} | version {map.Version}");
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Check for missing values for " + name);
				Locale value = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<int> fetchIds = base.Items.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet item) => item.Names[lang] == null)?.Select((Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet e) => e.Id);
				if (map.Version > base.Version)
				{
					base.Version = map.Version;
					fetchIds = fetchIds.Concat(base.Map.Values.Except(base.Items.Keys).Except(base.Map.Ignored.Values));
					saveRequired = true;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
				}
				if (fetchIds.Count() > 0)
				{
					List<List<int>> idSets = fetchIds.ToList().ChunkBy(200);
					saveRequired = saveRequired || idSets.Count > 0;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"Fetch a total of {fetchIds.Count()} in {idSets.Count} sets.");
					foreach (List<int> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.PvpAmulet> items = await gw2ApiManager.Gw2ApiClient.V2.Pvp.Amulets.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						foreach (Gw2Sharp.WebApi.V2.Models.PvpAmulet item2 in items)
						{
							Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet entryItem;
							bool num = base.Items.Values.TryFind((Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet e) => e.Id == item2.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet
								{
									MappedId = (base.Map?.Items?.FirstOrDefault((KeyValuePair<byte, int> e) => e.Value == item2.Id).Key).GetValueOrDefault()
								};
							}
							entryItem?.Apply(item2);
							if (!num)
							{
								base.Items.Add(item2.Id, entryItem);
							}
						}
					}
				}
				if (saveRequired)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Saving " + name + ".json");
					string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
					System.IO.File.WriteAllText(path, json);
				}
				DataLoaded = DataLoaded || base.Items.Count > 0;
				return true;
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn(ex, "Failed to load " + name + " data.");
				return false;
			}
		}
	}
}