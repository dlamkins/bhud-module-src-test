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
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class ItemMappedDataEntry<T> : MappedDataEntry<int, T> where T : BaseItem, new()
	{
		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			_ = 2;
			try
			{
				bool saveRequired = false;
				MappedDataEntry<int, T> loaded = null;
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load and if required update " + name);
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<MappedDataEntry<int, T>>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				foreach (int id in base.Map.Ignored.Values)
				{
					base.Items.Remove(id);
				}
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"{name} Version {base.Version} | version {map.Version}");
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Check for missing values for " + name);
				Locale value = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				IEnumerable<int> fetchIds = base.Items.Values.Where((T item) => item.Names[lang] == null)?.Select((T e) => e.Id);
				bool fetchAll = map.Version > base.Version;
				if (map.Version > base.Version)
				{
					base.Version = map.Version;
					fetchIds = fetchIds.Concat(base.Map.Values.Except(base.Items.Keys).Except(base.Map.Ignored.Values));
					saveRequired = true;
					if (fetchAll)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
					}
				}
				if (fetchIds.Count() > 0)
				{
					List<List<int>> idSets = fetchIds.ToList().ChunkBy(200);
					ItemArmor armor = (await gw2ApiManager.Gw2ApiClient.V2.Items.GetAsync(80384, cancellationToken)) as ItemArmor;
					IReadOnlyList<int> readOnlyList2;
					if (armor == null)
					{
						IReadOnlyList<int> readOnlyList = new List<int>();
						readOnlyList2 = readOnlyList;
					}
					else
					{
						readOnlyList2 = armor.Details.StatChoices;
					}
					IReadOnlyList<int> statChoices = readOnlyList2;
					saveRequired = saveRequired || idSets.Count > 0;
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug($"Fetch a total of {fetchIds.Count()} {name} in {idSets.Count} sets.");
					foreach (List<int> ids in idSets)
					{
						IReadOnlyList<Item> items = await gw2ApiManager.Gw2ApiClient.V2.Items.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						foreach (Item item2 in items)
						{
							T entryItem;
							bool exists = base.Items.Values.TryFind((T e) => e.Id == item2.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new T
								{
									MappedId = (base.Map?.Items?.FirstOrDefault((KeyValuePair<byte, int> e) => e.Value == item2.Id).Key).GetValueOrDefault()
								};
							}
							entryItem?.Apply(item2);
							if (entryItem != null && entryItem.Type == Kenedia.Modules.Core.DataModels.ItemType.Relic)
							{
								entryItem.TemplateSlot = ((name == "PvpRelics") ? TemplateSlotType.PvpRelic : TemplateSlotType.PveRelic);
							}
							if (entryItem != null && Data.SkinDictionary.TryGetValue(item2.Id, out var assetId) && assetId.HasValue)
							{
								entryItem.Rarity = ItemRarity.Ascended;
								if (entryItem.TemplateSlot == TemplateSlotType.AquaBreather)
								{
									Armor aquaBreather = entryItem as Armor;
									if (aquaBreather != null)
									{
										aquaBreather.StatChoices = statChoices;
									}
								}
								if (entryItem.Type == Kenedia.Modules.Core.DataModels.ItemType.Trinket)
								{
									entryItem.AssetId = assetId.Value;
									entryItem.Name = entryItem.TemplateSlot switch
									{
										TemplateSlotType.Amulet => strings.Amulet, 
										TemplateSlotType.Ring_1 => strings.Ring, 
										TemplateSlotType.Ring_2 => strings.Ring, 
										TemplateSlotType.Accessory_1 => strings.Accessory, 
										TemplateSlotType.Accessory_2 => strings.Accessory, 
										_ => entryItem.Name, 
									};
								}
								else
								{
									Skin skin = await gw2ApiManager.Gw2ApiClient.V2.Skins.GetAsync(assetId.Value);
									entryItem.AssetId = skin?.Icon.GetAssetIdFromRenderUrl() ?? 0;
									entryItem.Name = skin?.Name;
								}
							}
							if (!exists)
							{
								base.Items.Add(item2.Id, entryItem);
							}
							entryItem = null;
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
