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
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class ItemMappedDataEntry<T> : MappedDataEntry<int, T> where T : BaseItem, new()
	{
		private List<int> _pendingIds = new List<int>();

		public async Task<List<int>> LoadAndGetPending(string name, ByteIntMap map, string path)
		{
			try
			{
				MappedDataEntry<int, T> loaded = null;
				if (!DataLoaded && System.IO.File.Exists(path))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("Load " + name + ".json");
					loaded = JsonConvert.DeserializeObject<MappedDataEntry<int, T>>(System.IO.File.ReadAllText(path), SerializerSettings.Default);
					DataLoaded = true;
				}
				base.Map = map;
				base.Items = loaded?.Items ?? base.Items;
				base.Version = loaded?.Version ?? base.Version;
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"{name} Current Version: {base.Version} | Required Version: {map.Version}");
				foreach (int id in base.Map.Ignored.Values)
				{
					base.Items.Remove(id);
				}
				Locale value = GameService.Overlay.UserLocale.Value;
				bool flag = (((uint)(value - 4) <= 1u) ? true : false);
				Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
				_pendingIds = new List<int>();
				if (map.Version > base.Version)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("The current version does not match the map version. Updating all values for " + name + ".");
					base.Version = map.Version;
					ItemMappedDataEntry<T> itemMappedDataEntry = this;
					List<int> list = new List<int>();
					list.AddRange(_pendingIds);
					list.AddRange(base.Map.Values.Except(base.Items.Keys).Except(base.Map.Ignored.Values));
					itemMappedDataEntry._pendingIds = list;
				}
				else
				{
					_pendingIds = (base.Items.Values.Where((T item) => item.Names[lang] == null)?.Select((T e) => e.Id)).ToList();
				}
				if (_pendingIds.Count > 0)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"A total of {_pendingIds.Count} {name} need to be fetched.");
					return _pendingIds;
				}
				return new List<int>();
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Warn(ex, "Failed to load " + name + " data.");
				return new List<int>();
			}
		}

		public override async Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			_ = 2;
			try
			{
				bool saveRequired = _pendingIds.Count > 0;
				if (_pendingIds.Count > 0)
				{
					List<List<int>> idSets = _pendingIds.ChunkBy(200);
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
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug($"Fetch a total of {_pendingIds.Count} {name} in {idSets.Count} sets.");
					foreach (List<int> ids in idSets)
					{
						IReadOnlyList<Item> items = await gw2ApiManager.Gw2ApiClient.V2.Items.ManyAsync(ids, cancellationToken);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						foreach (Item item in items)
						{
							T entryItem;
							bool exists = base.Items.Values.TryFind((T e) => e.Id == item.Id, out entryItem);
							if (entryItem == null)
							{
								entryItem = new T
								{
									MappedId = (base.Map?.Items?.FirstOrDefault((KeyValuePair<byte, int> e) => e.Value == item.Id).Key).GetValueOrDefault()
								};
							}
							entryItem?.Apply(item);
							if (entryItem != null && entryItem.Type == Kenedia.Modules.Core.DataModels.ItemType.Relic)
							{
								entryItem.TemplateSlot = ((name == "PvpRelics") ? TemplateSlotType.PvpRelic : TemplateSlotType.PveRelic);
							}
							if (entryItem != null && Data.SkinDictionary.TryGetValue(item.Id, out var assetId) && assetId.HasValue)
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
								base.Items.Add(item.Id, entryItem);
							}
							entryItem = null;
						}
					}
				}
				if (saveRequired)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Debug("Saving " + name + ".json");
					string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
					System.IO.File.WriteAllText(path, json);
				}
				DataLoaded = DataLoaded || base.Items.Count > 0;
				return true;
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.Logger.Warn(ex, "Failed to load " + name + " data.");
				return false;
			}
		}
	}
}
