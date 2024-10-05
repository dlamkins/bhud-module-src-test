using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
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
	public class PetDataEntry : MappedDataEntry<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Pet>
	{
		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				bool saveRequired = false;
				PetDataEntry loaded = null;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load and if required update " + name);
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<PetDataEntry>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"{name} Version {base.Version} | version {map.Version}");
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Check for missing values for " + name);
				IApiV2ObjectList<int> petIds = await gw2ApiManager.Gw2ApiClient.V2.Pets.IdsAsync(cancellationToken);
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				Locale value = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<int> localeMissing = base.Items.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Pet item) => item.Names[lang] == null)?.Select((Kenedia.Modules.BuildsManager.DataModels.Professions.Pet e) => e.Id);
				IEnumerable<int> missing = petIds.Except(base.Items.Keys).Concat(localeMissing);
				if (map.Version > base.Version)
				{
					base.Version = map.Version;
					missing = petIds;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
				}
				if (missing.Count() > 0)
				{
					List<List<int>> idSets = missing.ToList().ChunkBy(200);
					saveRequired = saveRequired || idSets.Count > 0;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"Fetch a total of {missing.Count()} {name} in {idSets.Count} sets.");
					foreach (List<int> ids in idSets)
					{
						IReadOnlyList<Gw2Sharp.WebApi.V2.Models.Pet> items = await gw2ApiManager.Gw2ApiClient.V2.Pets.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						foreach (Gw2Sharp.WebApi.V2.Models.Pet item2 in items)
						{
							Kenedia.Modules.BuildsManager.DataModels.Professions.Pet entryItem;
							bool num = base.Items.TryGetValue(item2.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new Kenedia.Modules.BuildsManager.DataModels.Professions.Pet();
							}
							entryItem.Apply(item2);
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
