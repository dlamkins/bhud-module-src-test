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
using Kenedia.Modules.BuildsManager.DataModels;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class RaceDataEntry : MappedDataEntry<Races, Kenedia.Modules.BuildsManager.DataModels.Race>
	{
		public RaceDataEntry()
		{
			Kenedia.Modules.BuildsManager.DataModels.Race race = null;
			Dictionary<Races, Kenedia.Modules.BuildsManager.DataModels.Race> items = base.Items;
			Kenedia.Modules.BuildsManager.DataModels.Race obj = new Kenedia.Modules.BuildsManager.DataModels.Race
			{
				Id = Races.None
			};
			race = obj;
			items.Add(Races.None, obj);
			race.Names[Locale.English] = "Any Race";
			race.Names[Locale.German] = "Jede Rasse";
			race.Names[Locale.French] = "Toute race";
			race.Names[Locale.Spanish] = "Cualquier raza";
		}

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
				RaceDataEntry loaded = JsonConvert.DeserializeObject<RaceDataEntry>(json, SerializerSettings.Default);
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
			try
			{
				if (base.Ids == null)
				{
					base.Ids = (await gw2ApiManager.Gw2ApiClient.V2.Races.IdsAsync(token)).Select((string value) => (!Enum.TryParse<Races>(value, out var result)) ? Races.None : result).Distinct().ToList();
				}
				if (token.IsCancellationRequested)
				{
					return (true, new List<object>());
				}
				Locale value2 = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value2 - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				Kenedia.Modules.BuildsManager.DataModels.Race value3;
				IEnumerable<string> missing = ((map.Version > base.Version) ? base.Ids.Select((Races x) => x.ToString()) : (from id in base.Ids
					where !base.Items.TryGetValue(id, out value3) || value3.Names[lang] == null
					select id into x
					select x.ToString()));
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
			_ = 3;
			try
			{
				if (token.IsCancellationRequested)
				{
					return false;
				}
				List<object> missing = (await IsIncomplete(name, map, path, gw2ApiManager, token)).Item2;
				if (missing.Any() && missing.All((object item) => item is string))
				{
					List<List<string>> idSets = missing.Cast<string>().ToList().ChunkBy(200);
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"{name} updating {missing.Count()} entries in {idSets.Count} sets.");
					IApiV2ObjectList<Skill> apiSkills = await gw2ApiManager.Gw2ApiClient.V2.Skills.AllAsync(token);
					Profession profession = await gw2ApiManager.Gw2ApiClient.V2.Professions.GetAsync(ProfessionType.Guardian, token);
					foreach (List<string> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.Race> items = await gw2ApiManager.Gw2ApiClient.V2.Races.ManyAsync(ids, token);
						if (token.IsCancellationRequested)
						{
							return false;
						}
						foreach (Gw2Sharp.WebApi.V2.Models.Race item2 in items)
						{
							Kenedia.Modules.BuildsManager.DataModels.Race entryItem;
							bool num = base.Items.Values.TryFind((Kenedia.Modules.BuildsManager.DataModels.Race e) => $"{e.Id}" == item2.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new Kenedia.Modules.BuildsManager.DataModels.Race();
							}
							entryItem.Apply(item2, apiSkills, profession.SkillsByPalette);
							if (!num)
							{
								base.Items.Add((Races)Enum.Parse(typeof(Races), item2.Id), entryItem);
							}
						}
					}
					base.Version = base.Map.Version;
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"Saving updated {name} data with {missing.Count()} updated entries. Version {base.Version}");
					string json = JsonConvert.SerializeObject(this, SerializerSettings.Default);
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
