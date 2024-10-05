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
		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			_ = 5;
			try
			{
				bool saveRequired = false;
				ProfessionDataEntry loaded = null;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load and if required update " + name);
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<ProfessionDataEntry>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"{name} Version {base.Version} | version {map.Version}");
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Check for missing values for " + name);
				IApiV2ObjectList<string> professionIds = await gw2ApiManager.Gw2ApiClient.V2.Professions.IdsAsync(cancellationToken);
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				ProfessionType result;
				IEnumerable<ProfessionType> professionTypes = professionIds?.Select((string value) => (!Enum.TryParse<ProfessionType>(value, out result)) ? ProfessionType.Guardian : result).Distinct();
				Locale value2 = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value2 - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<ProfessionType> localeMissing = base.Items.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Profession item) => item.Names[lang] == null)?.Select((Kenedia.Modules.BuildsManager.DataModels.Professions.Profession e) => e.Id);
				IEnumerable<ProfessionType> missing = professionTypes.Except(base.Items.Keys).Concat(localeMissing);
				if (map.Version > base.Version)
				{
					base.Version = map.Version;
					missing = professionTypes;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
				}
				if (missing.Count() > 0)
				{
					List<List<ProfessionType>> idSets = missing.ToList().ChunkBy(200);
					saveRequired = saveRequired || idSets.Count > 0;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"Fetch a total of {missing.Count()} {name} in {idSets.Count} sets.");
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Specialization> apiSpecializations = await gw2ApiManager.Gw2ApiClient.V2.Specializations.AllAsync(cancellationToken);
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Legend> apiV2ObjectList = ((!missing.Contains(ProfessionType.Revenant)) ? null : (await gw2ApiManager.Gw2ApiClient.V2.Legends.AllAsync(cancellationToken)));
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Legend> apiLegends = apiV2ObjectList;
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Trait> apiTraits = await gw2ApiManager.Gw2ApiClient.V2.Traits.AllAsync(cancellationToken);
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> apiSkills = await gw2ApiManager.Gw2ApiClient.V2.Skills.AllAsync(cancellationToken);
					IEnumerable<Gw2Sharp.WebApi.V2.Models.Legend> allLegends = apiLegends.Append<Gw2Sharp.WebApi.V2.Models.Legend>(new Gw2Sharp.WebApi.V2.Models.Legend
					{
						Id = "Legend7",
						Swap = 62891,
						Heal = 62719,
						Elite = 62942,
						Utilities = new List<int> { 62832, 62962, 62878 }
					});
					if (cancellationToken.IsCancellationRequested)
					{
						return false;
					}
					foreach (List<ProfessionType> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.Profession> items = await gw2ApiManager.Gw2ApiClient.V2.Professions.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
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
