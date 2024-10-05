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

		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			_ = 3;
			try
			{
				bool saveRequired = false;
				RaceDataEntry loaded = null;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load and if required update " + name);
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<RaceDataEntry>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"{name} Version {base.Version} | version {map.Version}");
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Check for missing values for " + name);
				IApiV2ObjectList<string> raceIds = await gw2ApiManager.Gw2ApiClient.V2.Races.IdsAsync(cancellationToken);
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				Locale value = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<string> localeMissing = base.Items.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Race item) => item.Names[lang] == null)?.Select((Kenedia.Modules.BuildsManager.DataModels.Race e) => $"{e.Id}");
				IEnumerable<string> missing = raceIds.Except(base.Items.Keys.Select((Races e) => $"{e}")).Concat(localeMissing).Except(new string[1] { $"{Races.None}" });
				if (map.Version > base.Version)
				{
					base.Version = map.Version;
					missing = raceIds;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
				}
				if (missing.Count() > 0)
				{
					List<List<string>> idSets = missing.ToList().ChunkBy(200);
					saveRequired = saveRequired || idSets.Count > 0;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"Fetch a total of {missing.Count()} {name} in {idSets.Count} sets.");
					IApiV2ObjectList<Skill> apiSkills = await gw2ApiManager.Gw2ApiClient.V2.Skills.AllAsync(cancellationToken);
					Profession profession = await gw2ApiManager.Gw2ApiClient.V2.Professions.GetAsync(ProfessionType.Guardian, cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						return false;
					}
					foreach (List<string> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.Race> items = await gw2ApiManager.Gw2ApiClient.V2.Races.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
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
