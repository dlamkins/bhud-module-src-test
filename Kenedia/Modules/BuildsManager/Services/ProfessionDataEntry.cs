using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class ProfessionDataEntry : MappedDataEntry<ProfessionType, Kenedia.Modules.BuildsManager.DataModels.Professions.Profession>
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
				ProfessionDataEntry loaded = JsonConvert.DeserializeObject<ProfessionDataEntry>(json, SerializerSettings.Default);
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
			else
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("No local data for " + name + " found at '" + Path.GetFileName(path) + "'");
			}
			return false;
		}

		public override async Task<(bool, List<object>)> IsIncomplete(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			base.Map = map;
			try
			{
				if (base.Ids == null)
				{
					base.Ids = (await gw2ApiManager.Gw2ApiClient.V2.Professions.IdsAsync(token)).Select((string value) => (!Enum.TryParse<ProfessionType>(value, out var result)) ? ProfessionType.Guardian : result).Distinct().ToList();
				}
				if (token.IsCancellationRequested)
				{
					return (true, new List<object>());
				}
				Locale value2 = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value2 - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<ProfessionType> enumerable;
				if (!(map.Version > base.Version))
				{
					enumerable = base.Ids.Where((ProfessionType professionId) => !base.Items.TryGetValue(professionId, out var value3) || value3.Names[lang] == null);
				}
				else
				{
					IEnumerable<ProfessionType> ids = base.Ids;
					enumerable = ids;
				}
				IEnumerable<ProfessionType> missing = enumerable;
				return (missing.Any(), missing.Cast<object>().ToList());
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Warn(ex, "Failed to check completeness of " + name + " data.");
			}
			return (true, new List<object>());
		}

		public override async Task<bool> Update(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			_ = 6;
			try
			{
				if (token.IsCancellationRequested)
				{
					return false;
				}
				await gw2ApiManager.Gw2ApiClient.V2.Professions.IdsAsync(token);
				if (token.IsCancellationRequested)
				{
					return false;
				}
				List<object> missing = (await IsIncomplete(name, map, path, gw2ApiManager, token)).Item2;
				if (missing.Any() && missing.All((object item) => item is ProfessionType))
				{
					List<List<ProfessionType>> idSets = missing.Cast<ProfessionType>().ToList().ChunkBy(200);
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"{name} updating {missing.Count()} entries in {idSets.Count} sets.");
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Specialization> apiSpecializations = await gw2ApiManager.Gw2ApiClient.V2.Specializations.AllAsync(token);
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Legend> apiV2ObjectList = ((!missing.Contains(ProfessionType.Revenant)) ? null : (await gw2ApiManager.Gw2ApiClient.V2.Legends.AllAsync(token)));
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Legend> apiLegends = apiV2ObjectList;
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Trait> apiTraits = await gw2ApiManager.Gw2ApiClient.V2.Traits.AllAsync(token);
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> apiSkills = await gw2ApiManager.Gw2ApiClient.V2.Skills.AllAsync(token);
					IEnumerable<Gw2Sharp.WebApi.V2.Models.Legend> allLegends = apiLegends.Append<Gw2Sharp.WebApi.V2.Models.Legend>(new Gw2Sharp.WebApi.V2.Models.Legend
					{
						Id = "Legend7",
						Swap = 62891,
						Heal = 62719,
						Elite = 62942,
						Utilities = new _003C_003Ez__ReadOnlyArray<int>(new int[3] { 62832, 62962, 62878 })
					});
					if (token.IsCancellationRequested)
					{
						return false;
					}
					foreach (List<ProfessionType> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.Profession> items = await gw2ApiManager.Gw2ApiClient.V2.Professions.ManyAsync(ids, token);
						if (token.IsCancellationRequested)
						{
							return false;
						}
						foreach (Gw2Sharp.WebApi.V2.Models.Profession item2 in items)
						{
							Kenedia.Modules.BuildsManager.DataModels.Professions.Profession entryItem;
							bool num = base.Items.Values.TryFind((Kenedia.Modules.BuildsManager.DataModels.Professions.Profession e) => $"{e.Id}" == item2.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new Kenedia.Modules.BuildsManager.DataModels.Professions.Profession();
							}
							entryItem.Apply(item2, apiSpecializations, allLegends, apiTraits, apiSkills);
							if (!num)
							{
								base.Items.Add(entryItem.Id, entryItem);
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
