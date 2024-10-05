using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class GW2API
	{
		private readonly Logger _logger = Logger.GetLogger(typeof(GW2API));

		private CancellationTokenSource _cancellationTokenSource;

		private Exception _lastException;

		private readonly List<int> _infusions = new List<int>(80)
		{
			39336, 39335, 39337, 39339, 39338, 39340, 39616, 39617, 39618, 39619,
			39620, 39621, 85971, 86338, 37133, 37134, 37123, 37127, 37128, 37129,
			85881, 86150, 37130, 37131, 37132, 37135, 37136, 37125, 86180, 86113,
			43250, 43251, 43252, 43253, 43254, 43255, 86986, 87218, 49424, 49425,
			49426, 49427, 49428, 49429, 49430, 49431, 49432, 49433, 49434, 49435,
			49436, 49437, 49438, 49439, 49440, 49441, 49442, 49443, 49444, 49445,
			49446, 49447, 87528, 87518, 87493, 87503, 87526, 87496, 87497, 87508,
			87516, 87532, 87495, 87525, 87511, 87512, 87527, 87502, 87538, 87504
		};

		private Func<NotificationBadge> _getNotificationBadge;

		private Func<LoadingSpinner> _getSpinner;

		public NotificationBadge NotificationBadge
		{
			get
			{
				NotificationBadge badge = _getNotificationBadge?.Invoke();
				if (badge == null)
				{
					return null;
				}
				return badge;
			}
		}

		public LoadingSpinner Spinner
		{
			get
			{
				LoadingSpinner spinner = _getSpinner?.Invoke();
				if (spinner == null)
				{
					return null;
				}
				return spinner;
			}
		}

		public Gw2ApiManager Gw2ApiManager { get; }

		private Data Data { get; }

		public Paths Paths { get; }

		public GW2API(Gw2ApiManager gw2ApiManager, Data data, Paths paths, Func<NotificationBadge> notificationBadge, Func<LoadingSpinner> spinner)
		{
			Gw2ApiManager = gw2ApiManager;
			Data = data;
			Paths = paths;
			_getNotificationBadge = notificationBadge;
			_getSpinner = spinner;
		}

		public void Cancel()
		{
			_cancellationTokenSource?.Cancel();
		}

		public bool IsCanceled()
		{
			if (_cancellationTokenSource != null)
			{
				return _cancellationTokenSource.IsCancellationRequested;
			}
			return false;
		}

		public async Task UpdateData()
		{
			if (Paths == null)
			{
				_logger.Info("No Paths set for UpdateData!");
				return;
			}
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Info("UpdateData: Fetch data ...");
			new LocalizedString();
			NotificationBadge notificationBadge = NotificationBadge;
			if (notificationBadge != null)
			{
				notificationBadge.Visible = false;
			}
			_ = GameService.Overlay.UserLocale.Value;
		}

		public async Task UpdateMappedIds()
		{
			_ = 5;
			try
			{
				_cancellationTokenSource?.Cancel();
				_cancellationTokenSource = new CancellationTokenSource();
				StaticVersion mapCollection = StaticVersion.LoadFromFile(Paths.ModuleDataPath + "DataMap.json");
				Dictionary<string, Version> mapVersions = mapCollection.GetVersions();
				IApiV2ObjectList<int> raw_itemids = await Gw2ApiManager.Gw2ApiClient.V2.Items.IdsAsync(_cancellationTokenSource.Token);
				List<int> invalidIds = new List<int> { 11126, 63366, 90369, 100262 };
				if (_cancellationTokenSource.IsCancellationRequested)
				{
					return;
				}
				List<List<int>> itemid_lists = raw_itemids.Except(invalidIds).ToList().ChunkBy(200);
				int count = 0;
				foreach (List<int> ids in itemid_lists)
				{
					if (_cancellationTokenSource.IsCancellationRequested)
					{
						return;
					}
					if (ids == null)
					{
						continue;
					}
					count++;
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info($"Fetching chunk {count}/{itemid_lists.Count}");
					try
					{
						foreach (Item item in await Gw2ApiManager.Gw2ApiClient.V2.Items.ManyAsync(ids, _cancellationTokenSource.Token))
						{
							try
							{
								List<ByteIntMap> maps = new List<ByteIntMap>();
								ItemArmor armor = item as ItemArmor;
								if (armor == null)
								{
									ItemBack back = item as ItemBack;
									if (back == null)
									{
										ItemWeapon weapon = item as ItemWeapon;
										if (weapon == null)
										{
											ItemTrinket trinket = item as ItemTrinket;
											if (trinket == null)
											{
												ItemConsumable consumable = item as ItemConsumable;
												if (consumable == null)
												{
													ItemUpgradeComponent upgrade = item as ItemUpgradeComponent;
													if (upgrade != null)
													{
														ApiFlags<ItemInfusionFlag> infusionUpgradeFlags = upgrade.Details.InfusionUpgradeFlags;
														if ((object)infusionUpgradeFlags != null && infusionUpgradeFlags.ToList()?.Contains((ApiEnum<ItemInfusionFlag>)ItemInfusionFlag.Infusion) == true && _infusions.Contains(upgrade.Id))
														{
															maps.Add(mapCollection.Infusions);
														}
														else
														{
															ApiFlags<ItemInfusionFlag> infusionUpgradeFlags2 = upgrade.Details.InfusionUpgradeFlags;
															if ((object)infusionUpgradeFlags2 != null && infusionUpgradeFlags2.ToList()?.Contains((ApiEnum<ItemInfusionFlag>)ItemInfusionFlag.Enrichment) == true)
															{
																maps.Add(mapCollection.Enrichments);
															}
															else if (upgrade.Rarity.Value == ItemRarity.Exotic)
															{
																bool num = upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.HeavyArmor) && !upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.Trinket) && !upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.Sword);
																bool isSigil = !upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.HeavyArmor) && !upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.Trinket) && upgrade.Details.Flags.ToList().Contains((ApiEnum<ItemUpgradeComponentFlag>)ItemUpgradeComponentFlag.Sword);
																if (num)
																{
																	if ((object)upgrade.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pvp) != null)
																	{
																		maps.Add(mapCollection.PvpRunes);
																	}
																	if ((object)upgrade.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pve) != null)
																	{
																		maps.Add(mapCollection.PveRunes);
																	}
																}
																if (isSigil)
																{
																	if ((object)upgrade.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pvp) != null)
																	{
																		maps.Add(mapCollection.PvpSigils);
																	}
																	if ((object)upgrade.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pve) != null)
																	{
																		maps.Add(mapCollection.PveSigils);
																	}
																}
															}
														}
													}
													else
													{
														string text = item.Type.ToString();
														bool flag = ((text == "Relic" || text == "Mwcc") ? true : false);
														if (flag && item.Rarity == ItemRarity.Exotic)
														{
															if ((object)item.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pvp) != null)
															{
																maps.Add(mapCollection.PvpRelics);
															}
															if ((object)item.GameTypes.FirstOrDefault((ApiEnum<ItemGameType> e) => e.Value == ItemGameType.Pve) != null)
															{
																maps.Add(mapCollection.PveRelics);
															}
														}
														else if (item.Type == ItemType.PowerCore)
														{
															maps.Add(mapCollection.PowerCores);
														}
													}
												}
												else if (consumable.Level == 80 && (consumable.Details.ApplyCount.HasValue || consumable.Rarity.Value == ItemRarity.Ascended))
												{
													List<ByteIntMap> list = maps;
													list.Add(consumable.Details.Type.Value switch
													{
														ItemConsumableType.Food => mapCollection.Nourishments, 
														ItemConsumableType.Utility => mapCollection.Enhancements, 
														_ => null, 
													});
												}
											}
											else if (Kenedia.Modules.BuildsManager.Services.Data.SkinDictionary.ContainsKey(trinket.Id))
											{
												maps.Add(mapCollection.Trinkets);
											}
										}
										else if (Kenedia.Modules.BuildsManager.Services.Data.SkinDictionary.ContainsKey(weapon.Id))
										{
											maps.Add(mapCollection.Weapons);
										}
									}
									else if (Kenedia.Modules.BuildsManager.Services.Data.SkinDictionary.ContainsKey(back.Id))
									{
										maps.Add(mapCollection.Backs);
									}
								}
								else if (Kenedia.Modules.BuildsManager.Services.Data.SkinDictionary.ContainsKey(armor.Id))
								{
									maps.Add(mapCollection.Armors);
								}
								foreach (ByteIntMap map3 in maps)
								{
									if (map3 != null && map3.Items.FirstOrDefault((KeyValuePair<byte, int> x) => x.Value == item.Id).Value <= 0 && map3.Count < 255)
									{
										BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info($"Adding {item.Id} to {item.Type}");
										map3.Add((byte)(map3.Count + 1), item.Id);
										if (mapVersions.TryGetValue(map3.Name, out var mapVersion) && ((object)mapVersion).ToString() == ((object)map3.Version).ToString())
										{
											map3.Version = map3.Version.Increment();
											BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info($"Updating {item.Type} version from {mapVersion} to {map3.Version}");
										}
									}
								}
							}
							catch (Exception ex2)
							{
								BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Warn($"{item.Id}Exception {ex2}");
							}
						}
					}
					catch (Exception ex)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Warn("Exception thrown for ids: " + Environment.NewLine + string.Join("," + Environment.NewLine, ids) + " " + Environment.NewLine + ex);
					}
				}
				ItemTrinket ring = (ItemTrinket)(await Gw2ApiManager.Gw2ApiClient.V2.Items.GetAsync(91234, _cancellationTokenSource.Token));
				ItemWeapon legyWeapon = (ItemWeapon)(await Gw2ApiManager.Gw2ApiClient.V2.Items.GetAsync(30698, _cancellationTokenSource.Token));
				IEnumerable<int> statIds = ring.Details.StatChoices.Concat(legyWeapon.Details.StatChoices);
				IApiV2ObjectList<Itemstat> apiStats = await Gw2ApiManager.Gw2ApiClient.V2.Itemstats.AllAsync(_cancellationTokenSource.Token);
				if (_cancellationTokenSource.IsCancellationRequested)
				{
					return;
				}
				foreach (Itemstat e3 in apiStats)
				{
					if (statIds.Contains(e3.Id))
					{
						ByteIntMap map2 = mapCollection.Stats;
						if (map2 != null && map2.Items.FirstOrDefault((KeyValuePair<byte, int> x) => x.Value == e3.Id).Value <= 0 && map2.Count < 255)
						{
							BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info($"Adding {e3.Id} to Stats.");
							map2.Add((byte)(map2.Count + 1), e3.Id);
						}
					}
				}
				IApiV2ObjectList<PvpAmulet> apiAmulets = await Gw2ApiManager.Gw2ApiClient.V2.Pvp.Amulets.AllAsync(_cancellationTokenSource.Token);
				if (_cancellationTokenSource.IsCancellationRequested)
				{
					return;
				}
				foreach (PvpAmulet e2 in apiAmulets)
				{
					ByteIntMap map = mapCollection.PvpAmulets;
					if (map != null && map.Items.FirstOrDefault((KeyValuePair<byte, int> x) => x.Value == e2.Id).Value <= 0 && map.Count < 255)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info($"Adding {e2.Id} to Pvp Amulets.");
						map.Add((byte)(map.Count + 1), e2.Id);
					}
				}
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info("Save to " + Paths.ModuleDataPath + "DataMap.json");
				mapCollection.Save(Paths.ModuleDataPath + "DataMap.json");
			}
			catch
			{
			}
		}
	}
}
