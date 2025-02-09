using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Hero.Equipment.Wardrobe;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Tooltips
{
	public sealed class ItemTooltipView : View, ITooltipView, IView
	{
		private static readonly Color Gray = new Color(153, 153, 153);

		private static readonly Color ActiveBuffColor = new Color(85, 153, 255);

		private readonly FlowPanel _layout;

		public ItemTooltipViewModel ViewModel { get; }

		public ItemTooltipView(ItemTooltipViewModel viewModel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(350);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			_layout = val;
			ViewModel = viewModel;
			((View)this)._002Ector();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await ViewModel.Load(progress);
			return true;
		}

		private void PrintArmor(Armor armor)
		{
			PrintHeader(armor);
			PrintAttributes(armor.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Key.ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin();
			PrintItemRarity(armor.Rarity);
			PrintWeightClass(armor.WeightClass);
			if (!(armor is Boots))
			{
				if (!(armor is Coat))
				{
					if (!(armor is Gloves))
					{
						if (!(armor is Helm) && !(armor is HelmAquatic))
						{
							if (!(armor is Leggings))
							{
								if (armor is Shoulders)
								{
									PrintPlainText((string)ViewModel.Localizer["Shoulder Armor"]);
								}
							}
							else
							{
								PrintPlainText((string)ViewModel.Localizer["Leg Armor"]);
							}
						}
						else
						{
							PrintPlainText((string)ViewModel.Localizer["Head Armor"]);
						}
					}
					else
					{
						PrintPlainText((string)ViewModel.Localizer["Hand Armor"]);
					}
				}
				else
				{
					PrintPlainText((string)ViewModel.Localizer["Chest Armor"]);
				}
			}
			else
			{
				PrintPlainText((string)ViewModel.Localizer["Foot Armor"]);
			}
			PrintRequiredLevel(armor.Level);
			PrintDescription(armor.Description);
			PrintInBank();
			PrintStatChoices(armor);
			PrintUniqueness(armor);
			PrintItemBinding(armor);
			PrintVendorValue(armor);
		}

		private void PrintBackpack(Backpack back)
		{
			PrintHeader(back);
			PrintAttributes(back.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin();
			PrintItemRarity(back.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Back Item"]);
			PrintRequiredLevel(back.Level);
			PrintDescription(back.Description);
			PrintInBank();
			PrintStatChoices(back);
			PrintUniqueness(back);
			PrintItemBinding(back);
			PrintVendorValue(back);
		}

		private void PrintBag(Bag bag)
		{
			PrintHeader(bag);
			PrintDescription(bag.Description);
			PrintInBank();
			PrintUniqueness(bag);
			PrintItemBinding(bag);
			PrintVendorValue(bag);
		}

		private void PrintConsumable(Consumable consumable)
		{
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0715: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Unknown result type (might be due to invalid IL or missing references)
			//IL_0804: Unknown result type (might be due to invalid IL or missing references)
			//IL_082b: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0932: Unknown result type (might be due to invalid IL or missing references)
			//IL_0963: Unknown result type (might be due to invalid IL or missing references)
			PrintHeader(consumable);
			if (consumable is Currency || consumable is Service)
			{
				PrintPlainText((string)ViewModel.Localizer["Takes effect immediately upon receipt"]);
			}
			else
			{
				PrintPlainText((string)ViewModel.Localizer["Double-click to consume"]);
			}
			Food food = consumable as Food;
			if ((object)food != null)
			{
				if ((object)food.Effect != null)
				{
					PrintEffect(food.Effect);
				}
			}
			else
			{
				Utility utility = consumable as Utility;
				if ((object)utility != null)
				{
					if ((object)utility.Effect != null)
					{
						PrintEffect(utility.Effect);
					}
				}
				else
				{
					Service service = consumable as Service;
					if ((object)service != null)
					{
						if ((object)service.Effect != null)
						{
							PrintEffect(service.Effect);
						}
					}
					else
					{
						GenericConsumable generic = consumable as GenericConsumable;
						if ((object)generic != null && (object)generic.Effect != null)
						{
							PrintEffect(generic.Effect);
						}
					}
				}
			}
			PrintDescription(consumable.Description);
			if (!(consumable is Currency) && !(consumable is Service))
			{
				if (!(consumable is Transmutation))
				{
					if (!(consumable is Booze))
					{
						ContentUnlocker unlocker = consumable as ContentUnlocker;
						if ((object)unlocker == null)
						{
							Dye unlocker2 = consumable as Dye;
							if ((object)unlocker2 == null)
							{
								GliderSkinUnlocker unlocker3 = consumable as GliderSkinUnlocker;
								if ((object)unlocker3 == null)
								{
									JadeBotSkinUnlocker unlocker4 = consumable as JadeBotSkinUnlocker;
									if ((object)unlocker4 == null)
									{
										MistChampionSkinUnlocker unlocker5 = consumable as MistChampionSkinUnlocker;
										if ((object)unlocker5 == null)
										{
											OutfitUnlocker unlocker6 = consumable as OutfitUnlocker;
											if ((object)unlocker6 == null)
											{
												RecipeSheet unlocker7 = consumable as RecipeSheet;
												if ((object)unlocker7 != null)
												{
													if (!string.IsNullOrEmpty(unlocker7.Description))
													{
														PrintPlainText(" ");
													}
													if (ViewModel.DefaultLocked)
													{
														if (ViewModel.Unlocked.HasValue)
														{
															if (ViewModel.Unlocked.Value)
															{
																PrintPlainText(ViewModel.UnlockedText + "\r\n", ViewModel.UnlockedTextColor);
															}
														}
														else
														{
															PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
														}
													}
													else
													{
														PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
													}
													PrintPlainText((string)ViewModel.Localizer["Consumable"]);
												}
												else if (string.IsNullOrEmpty(consumable.Description))
												{
													PrintPlainText((string)ViewModel.Localizer["Consumable"]);
												}
												else
												{
													PrintPlainText("\r\n" + ViewModel.Localizer["Consumable"]);
												}
											}
											else
											{
												if (!string.IsNullOrEmpty(unlocker6.Description))
												{
													PrintPlainText(" ");
												}
												if (ViewModel.DefaultLocked)
												{
													if (ViewModel.Unlocked.HasValue)
													{
														if (ViewModel.Unlocked.Value)
														{
															PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["You have already unlocked this outfit"]), Color.get_Red());
														}
													}
													else
													{
														PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
													}
												}
												else
												{
													PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
												}
												PrintPlainText((string)ViewModel.Localizer["Consumable"]);
											}
										}
										else
										{
											if (!string.IsNullOrEmpty(unlocker5.Description))
											{
												PrintPlainText(" ");
											}
											if (ViewModel.DefaultLocked)
											{
												if (ViewModel.Unlocked.HasValue)
												{
													if (ViewModel.Unlocked.Value)
													{
														PrintPlainText("You have already unlocked this outfit!\r\n", Color.get_Red());
													}
												}
												else
												{
													PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
												}
											}
											else
											{
												PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
											}
											PrintPlainText((string)ViewModel.Localizer["Consumable"]);
										}
									}
									else
									{
										if (!string.IsNullOrEmpty(unlocker4.Description))
										{
											PrintPlainText(" ");
										}
										if (ViewModel.DefaultLocked)
										{
											if (ViewModel.Unlocked.HasValue)
											{
												if (ViewModel.Unlocked.Value)
												{
													PrintPlainText("You have already unlocked this Jade Bot!\r\n", Color.get_Red());
												}
											}
											else
											{
												PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
											}
										}
										else
										{
											PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
										}
										PrintPlainText((string)ViewModel.Localizer["Consumable"]);
									}
								}
								else
								{
									if (!string.IsNullOrEmpty(unlocker3.Description))
									{
										PrintPlainText(" ");
									}
									if (ViewModel.DefaultLocked)
									{
										if (ViewModel.Unlocked.HasValue)
										{
											if (ViewModel.Unlocked.Value)
											{
												PrintPlainText("You have already unlocked this glider!\r\n", Color.get_Red());
											}
										}
										else
										{
											PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
										}
									}
									else
									{
										PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
									}
									PrintPlainText((string)ViewModel.Localizer["Consumable"]);
								}
							}
							else
							{
								if (!string.IsNullOrEmpty(unlocker2.Description))
								{
									PrintPlainText(" ");
								}
								if (ViewModel.DefaultLocked)
								{
									if (ViewModel.Unlocked.HasValue)
									{
										if (ViewModel.Unlocked.Value)
										{
											PrintPlainText("You have already unlocked this dye!\r\n", Color.get_Red());
										}
									}
									else
									{
										PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
									}
								}
								else
								{
									PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
								}
								PrintPlainText((string)ViewModel.Localizer["Consumable"]);
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(unlocker.Description))
							{
								PrintPlainText(" ");
							}
							if (ViewModel.DefaultLocked)
							{
								if (ViewModel.Unlocked.HasValue)
								{
									if (ViewModel.Unlocked.Value)
									{
										PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["You already have that content unlocked"]), Color.get_Red());
									}
								}
								else
								{
									PrintPlainText(ViewModel.AuthorizationText + "\r\n", Gray);
								}
							}
							else
							{
								PrintPlainText(string.Format("{0}\r\n", ViewModel.Localizer["Unlock status unknown"]), Gray);
							}
							PrintPlainText((string)ViewModel.Localizer["Consumable"]);
						}
					}
					else
					{
						PrintPlainText(string.Format("\r\n{0}\r\n\r\n{1}", ViewModel.Localizer["Excessive alcohol consumption will result in intoxication"], ViewModel.Localizer["Consumable"]));
					}
				}
				else
				{
					PrintTransmutation();
					PrintPlainText("\r\n" + ViewModel.Localizer["Consumable"]);
				}
			}
			else if (string.IsNullOrEmpty(consumable.Description))
			{
				PrintPlainText((string)ViewModel.Localizer["Service"]);
			}
			else
			{
				PrintPlainText("\r\n" + ViewModel.Localizer["Service"]);
			}
			PrintRequiredLevel(consumable.Level);
			PrintInBank();
			PrintUniqueness(consumable);
			PrintItemBinding(consumable);
			PrintVendorValue(consumable);
		}

		private void PrintContainer(Container container)
		{
			PrintHeader(container);
			PrintDescription(container.Description);
			if (string.IsNullOrEmpty(container.Description))
			{
				PrintPlainText((string)ViewModel.Localizer["Consumable"]);
			}
			else
			{
				PrintPlainText("\r\n" + ViewModel.Localizer["Consumable"]);
			}
			PrintInBank();
			PrintUniqueness(container);
			PrintItemBinding(container);
			PrintVendorValue(container);
		}

		private void PrintCraftingMaterial(CraftingMaterial craftingMaterial)
		{
			PrintHeader(craftingMaterial);
			PrintDescription(craftingMaterial.Description);
			PrintInBank();
			PrintUniqueness(craftingMaterial);
			PrintItemBinding(craftingMaterial);
			PrintVendorValue(craftingMaterial);
		}

		private void PrintGatheringTool(GatheringTool gatheringTool)
		{
			PrintHeader(gatheringTool);
			PrintDescription(gatheringTool.Description);
			PrintInBank();
			PrintUniqueness(gatheringTool);
			PrintItemBinding(gatheringTool);
			PrintVendorValue(gatheringTool);
		}

		private void PrintTrinket(Trinket trinket)
		{
			PrintHeader(trinket);
			PrintAttributes(trinket.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemRarity(trinket.Rarity);
			if (!(trinket is Accessory))
			{
				if (!(trinket is Amulet))
				{
					if (trinket is Ring)
					{
						PrintPlainText((string)ViewModel.Localizer["Ring"]);
					}
				}
				else
				{
					PrintPlainText((string)ViewModel.Localizer["Amulet"]);
				}
			}
			else
			{
				PrintPlainText((string)ViewModel.Localizer["Accessory"]);
			}
			PrintRequiredLevel(trinket.Level);
			PrintDescription(trinket.Description);
			PrintInBank();
			PrintStatChoices(trinket);
			PrintUniqueness(trinket);
			PrintItemBinding(trinket);
			PrintVendorValue(trinket);
		}

		private void PrintGizmo(Gizmo gizmo)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			PrintHeader(gizmo);
			PrintDescription(gizmo.Description, gizmo.Level > 0);
			if (ViewModel.DefaultLocked)
			{
				if (ViewModel.Unlocked.HasValue)
				{
					if (ViewModel.Unlocked.Value)
					{
						PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Novelty Unlocked"]));
					}
					else
					{
						PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Novelty Locked"]), Gray);
					}
				}
				else
				{
					PrintPlainText("\r\n" + ViewModel.AuthorizationText, Gray);
				}
				PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Consumable"]));
			}
			PrintRequiredLevel(gizmo.Level);
			PrintInBank();
			PrintUniqueness(gizmo);
			PrintItemBinding(gizmo);
			PrintVendorValue(gizmo);
		}

		private void PrintJadeTechModule(JadeTechModule jadeTechModule)
		{
			PrintHeader(jadeTechModule);
			PrintDescription(jadeTechModule.Description);
			PrintItemRarity(jadeTechModule.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Module"]);
			PrintRequiredLevel(jadeTechModule.Level);
			PrintPlainText((string)ViewModel.Localizer["Required Mastery: Jade Bots"]);
			PrintInBank();
			PrintUniqueness(jadeTechModule);
			PrintItemBinding(jadeTechModule);
			PrintVendorValue(jadeTechModule);
		}

		private void PrintMiniature(Miniature miniature)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			PrintHeader(miniature);
			PrintDescription(miniature.Description);
			if (ViewModel.DefaultLocked)
			{
				if (ViewModel.Unlocked.HasValue)
				{
					if (ViewModel.Unlocked.Value)
					{
						PrintPlainText(string.Format("\r\n{0}\r\n", ViewModel.Localizer["Mini Unlocked"]));
					}
					else
					{
						PrintPlainText(string.Format("\r\n{0}\r\n", ViewModel.Localizer["Mini Locked"]), Gray);
					}
				}
				else
				{
					PrintPlainText("\r\n" + ViewModel.AuthorizationText + "\r\n", Gray);
				}
			}
			PrintPlainText((string)ViewModel.Localizer["Mini"]);
			PrintInBank();
			PrintUniqueness(miniature);
			PrintItemBinding(miniature);
			PrintVendorValue(miniature);
		}

		private void PrintPowerCore(PowerCore powerCore)
		{
			PrintHeader(powerCore);
			PrintDescription(powerCore.Description, finalNewLine: true);
			PrintItemRarity(powerCore.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Power Core"]);
			PrintRequiredLevel(powerCore.Level);
			PrintInBank();
			PrintUniqueness(powerCore);
			PrintItemBinding(powerCore);
			PrintVendorValue(powerCore);
		}

		private void PrintRelic(Relic relic)
		{
			PrintHeader(relic);
			PrintDescription(relic.Description, finalNewLine: true);
			PrintItemRarity(relic.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Relic"]);
			PrintRequiredLevel(relic.Level);
			PrintInBank();
			PrintUniqueness(relic);
			PrintItemBinding(relic);
			PrintVendorValue(relic);
		}

		private void PrintSalvageTool(SalvageTool salvageTool)
		{
			PrintHeader(salvageTool);
			PrintPlainText(" ");
			PrintItemRarity(salvageTool.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Consumable"]);
			PrintDescription(salvageTool.Description);
			PrintInBank();
			PrintUniqueness(salvageTool);
			PrintItemBinding(salvageTool);
			PrintVendorValue(salvageTool);
		}

		private void PrintTrophy(Trophy trophy)
		{
			PrintHeader(trophy);
			PrintDescription(trophy.Description);
			PrintPlainText((string)ViewModel.Localizer["Trophy"]);
			PrintInBank();
			PrintUniqueness(trophy);
			PrintItemBinding(trophy);
			PrintVendorValue(trophy);
		}

		private void PrintUpgradeComponent(UpgradeComponent upgradeComponent)
		{
			PrintHeader(upgradeComponent);
			Rune rune = upgradeComponent as Rune;
			if ((object)rune != null)
			{
				PrintBonuses(rune.Bonuses ?? Array.Empty<string>());
			}
			else
			{
				Buff buff = upgradeComponent.Buff;
				if ((object)buff != null)
				{
					string description = buff.Description;
					if (description != null && description.Length > 0)
					{
						PrintBuff(upgradeComponent.Buff);
						goto IL_008f;
					}
				}
				PrintAttributes(upgradeComponent.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			}
			goto IL_008f;
			IL_008f:
			PrintDescription(upgradeComponent.Description);
			PrintRequiredLevel(upgradeComponent.Level);
			PrintInBank();
			PrintUniqueness(upgradeComponent);
			PrintItemBinding(upgradeComponent);
			PrintVendorValue(upgradeComponent);
		}

		private void PrintWeapon(Weapon weapon)
		{
			PrintHeader(weapon);
			PrintWeaponStrength(weapon);
			PrintDefense(weapon.Defense);
			PrintAttributes(weapon.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin();
			PrintItemRarity(weapon.Rarity);
			if (!(weapon is Axe))
			{
				if (!(weapon is Dagger))
				{
					if (!(weapon is Focus))
					{
						if (!(weapon is Greatsword))
						{
							if (!(weapon is Hammer))
							{
								if (!(weapon is HarpoonGun))
								{
									if (!(weapon is LargeBundle))
									{
										if (!(weapon is Longbow))
										{
											if (!(weapon is Mace))
											{
												if (!(weapon is Pistol))
												{
													if (!(weapon is Rifle))
													{
														if (!(weapon is Scepter))
														{
															if (!(weapon is Shield))
															{
																if (!(weapon is Shortbow))
																{
																	if (!(weapon is SmallBundle))
																	{
																		if (!(weapon is Spear))
																		{
																			if (!(weapon is Staff))
																			{
																				if (!(weapon is Sword))
																				{
																					if (!(weapon is Torch))
																					{
																						if (!(weapon is Toy) && !(weapon is ToyTwoHanded))
																						{
																							if (!(weapon is Trident))
																							{
																								if (weapon is Warhorn)
																								{
																									PrintPlainText((string)ViewModel.Localizer["Warhorn"]);
																								}
																							}
																							else
																							{
																								PrintPlainText((string)ViewModel.Localizer["Trident"]);
																							}
																						}
																						else
																						{
																							PrintPlainText((string)ViewModel.Localizer["Toy"]);
																						}
																					}
																					else
																					{
																						PrintPlainText((string)ViewModel.Localizer["Torch"]);
																					}
																				}
																				else
																				{
																					PrintPlainText((string)ViewModel.Localizer["Sword"]);
																				}
																			}
																			else
																			{
																				PrintPlainText((string)ViewModel.Localizer["Staff"]);
																			}
																		}
																		else
																		{
																			PrintPlainText((string)ViewModel.Localizer["Spear"]);
																		}
																	}
																	else
																	{
																		PrintPlainText((string)ViewModel.Localizer["Small Bundle"]);
																	}
																}
																else
																{
																	PrintPlainText((string)ViewModel.Localizer["Shortbow"]);
																}
															}
															else
															{
																PrintPlainText((string)ViewModel.Localizer["Shield"]);
															}
														}
														else
														{
															PrintPlainText((string)ViewModel.Localizer["Scepter"]);
														}
													}
													else
													{
														PrintPlainText((string)ViewModel.Localizer["Rifle"]);
													}
												}
												else
												{
													PrintPlainText((string)ViewModel.Localizer["Pistol"]);
												}
											}
											else
											{
												PrintPlainText((string)ViewModel.Localizer["Mace"]);
											}
										}
										else
										{
											PrintPlainText((string)ViewModel.Localizer["Longbow"]);
										}
									}
									else
									{
										PrintPlainText((string)ViewModel.Localizer["Large Bundle"]);
									}
								}
								else
								{
									PrintPlainText((string)ViewModel.Localizer["Harpoon Gun"]);
								}
							}
							else
							{
								PrintPlainText((string)ViewModel.Localizer["Hammer"]);
							}
						}
						else
						{
							PrintPlainText((string)ViewModel.Localizer["Greatsword"]);
						}
					}
					else
					{
						PrintPlainText((string)ViewModel.Localizer["Focus"]);
					}
				}
				else
				{
					PrintPlainText((string)ViewModel.Localizer["Dagger"]);
				}
			}
			else
			{
				PrintPlainText((string)ViewModel.Localizer["Axe"]);
			}
			PrintRequiredLevel(weapon.Level);
			PrintDescription(weapon.Description);
			PrintInBank();
			PrintStatChoices(weapon);
			PrintUniqueness(weapon);
			PrintItemBinding(weapon);
			PrintVendorValue(weapon);
		}

		private void Print(Item item)
		{
			PrintHeader(item);
			PrintDescription(item.Description);
			PrintInBank();
			PrintUniqueness(item);
			PrintItemBinding(item);
			PrintVendorValue(item);
		}

		public void PrintPlainText(string text, Color? textColor = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_layout);
			((Control)val).set_Width(((Control)_layout).get_Width());
			val.set_AutoSizeHeight(true);
			val.set_Text(text);
			val.set_TextColor(textColor.GetValueOrDefault(Color.get_White()));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_WrapText(true);
		}

		public void PrintHeader(Item item)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_layout);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f));
			((Control)val).set_Width(((Control)_layout).get_Width());
			((Control)val).set_Height(50);
			FlowPanel header = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)header);
			val2.set_Texture(ViewModel.GetIcon(ViewModel.Item));
			((Control)val2).set_Size(new Point(50));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			val3.set_TextColor(ViewModel.ItemNameColor);
			((Control)val3).set_Width(((Control)_layout).get_Width() - 55);
			((Control)val3).set_Height(50);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_WrapText(true);
			Label name = val3;
			Binder.Bind(ViewModel, (ItemTooltipViewModel vm) => vm.ItemName, name);
			name.set_Text(name.get_Text().Replace(" ", "  "));
		}

		public void PrintDefense(int defense)
		{
			if (defense > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Defense", new object[1] { defense }]);
			}
		}

		public void PrintAttributes(IReadOnlyDictionary<string, int> attributes)
		{
			if (attributes.Count <= 0)
			{
				return;
			}
			StringBuilder builder = new StringBuilder();
			foreach (KeyValuePair<string, int> stat in attributes)
			{
				if (builder.Length > 0)
				{
					builder.AppendLine();
				}
				builder.Append(ViewModel.Localizer[stat.Key, new object[1] { stat.Value }]);
			}
			PrintPlainText(builder.ToString());
		}

		public void PrintUpgrades()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			using IEnumerator<UpgradeSlot> enumerator = ViewModel.UpgradesSlots.GetEnumerator();
			FormattedLabelBuilder builder;
			for (; enumerator.MoveNext(); ((Control)builder.Build()).set_Parent((Container)(object)_layout))
			{
				UpgradeSlot slot = enumerator.Current;
				builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight().Wrap();
				if ((object)slot.UpgradeComponent != null)
				{
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" " + slot.UpgradeComponent!.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_003c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0051: Unknown result type (might be due to invalid IL or missing references)
						if (!string.IsNullOrEmpty(slot.UpgradeComponent!.IconHref))
						{
							part.SetPrefixImage(ViewModel.GetIcon(slot.UpgradeComponent));
							part.SetPrefixImageSize(new Point(16));
						}
						part.SetFontSize((FontSize)16);
						part.SetTextColor(ActiveBuffColor);
					});
					Rune rune = slot.UpgradeComponent as Rune;
					if ((object)rune != null)
					{
						foreach (var (bonus, ordinal) in (rune.Bonuses ?? Array.Empty<string>()).Select((string value, int index) => (value, index + 1)))
						{
							builder.CreatePart($"\r\n({ordinal:0}): {bonus}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
							{
								//IL_000a: Unknown result type (might be due to invalid IL or missing references)
								part.SetFontSize((FontSize)16);
								part.SetTextColor(Gray);
							});
						}
						continue;
					}
					Buff buff = slot.UpgradeComponent!.Buff;
					if ((object)buff != null)
					{
						string description = buff.Description;
						if (description != null && description.Length > 0)
						{
							builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
							{
								part.SetFontSize((FontSize)16);
							});
							builder.AddMarkup(slot.UpgradeComponent!.Buff!.Description, ActiveBuffColor);
							continue;
						}
					}
					foreach (KeyValuePair<Extensible<AttributeName>, int> stat in slot.UpgradeComponent!.Attributes)
					{
						builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
						{
							part.SetFontSize((FontSize)16);
						});
						builder.CreatePart($"+{stat.Value:N0} {ViewModel.Localizer[stat.Key.ToString()]}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
						{
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							part.SetFontSize((FontSize)16);
							part.SetTextColor(ActiveBuffColor);
						});
					}
					continue;
				}
				switch (slot.Type)
				{
				case UpgradeSlotType.Infusion:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" " + ViewModel.Localizer["Unused infusion slot"], (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(EmbeddedResources.Texture("unused_infusion_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				case UpgradeSlotType.Enrichment:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" " + ViewModel.Localizer["Unused enrichment slot"], (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(EmbeddedResources.Texture("unused_enrichment_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				default:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" " + ViewModel.Localizer["Unused upgrade slot"], (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(EmbeddedResources.Texture("unused_upgrade_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				}
			}
		}

		public void PrintItemSkin()
		{
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			if (!ViewModel.DefaultLocked)
			{
				return;
			}
			if (ViewModel.Unlocked.HasValue)
			{
				if (ViewModel.Unlocked.Value)
				{
					PrintPlainText(string.Format("\r\n{0}\r\n{1}", ViewModel.Localizer["Skin Unlocked"], ViewModel.DefaultSkin?.Name));
				}
				else
				{
					PrintPlainText(string.Format("\r\n{0}\r\n{1}", ViewModel.Localizer["Skin Locked"], ViewModel.DefaultSkin?.Name), Gray);
				}
			}
			else
			{
				PrintPlainText("\r\n" + ViewModel.AuthorizationText + "\r\n" + ViewModel.DefaultSkin?.Name, Gray);
			}
		}

		public void PrintTransmutation()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			if (!ViewModel.DefaultLocked)
			{
				return;
			}
			if (ViewModel.Unlocked.HasValue)
			{
				if (ViewModel.Unlocked.Value)
				{
					PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Skin Unlocked"]));
				}
				else
				{
					PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Skin Locked"]), Gray);
				}
			}
			else
			{
				PrintPlainText("\r\n" + ViewModel.AuthorizationText, Gray);
			}
		}

		public void PrintItemRarity(Extensible<Rarity> rarity)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (rarity == Rarity.Basic)
			{
				PrintPlainText(" ");
			}
			else
			{
				PrintPlainText($"\r\n{rarity}", ItemColors.Rarity(rarity));
			}
		}

		public void PrintWeightClass(Extensible<WeightClass> weightClass)
		{
			PrintPlainText(weightClass.ToString());
		}

		public void PrintRequiredLevel(int level)
		{
			if (level > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Required Level", new object[1] { level }]);
			}
		}

		public void PrintDescription(string description, bool finalNewLine = false)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(description))
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_layout);
			((Control)val).set_Width(((Control)_layout).get_Width());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel container = val;
			FormattedLabelBuilder builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width() - 10).AutoSizeHeight().Wrap()
				.AddMarkup(description);
			if (finalNewLine)
			{
				builder.CreatePart("\r\n\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					part.SetFontSize((FontSize)16);
				});
			}
			((Control)builder.Build()).set_Parent((Container)(object)container);
		}

		public void PrintInBank()
		{
		}

		public void PrintItemBinding(Item item)
		{
			if ((!(item is Currency) && !(item is Service)) || 1 == 0)
			{
				if (item.Flags.AccountBound)
				{
					PrintPlainText((string)ViewModel.Localizer["Account Bound on Acquire"]);
				}
				else if (item.Flags.AccountBindOnUse)
				{
					PrintPlainText((string)ViewModel.Localizer["Account Bound on Use"]);
				}
				if (item.Flags.Soulbound)
				{
					PrintPlainText((string)ViewModel.Localizer["Soulbound on Acquire"]);
				}
				else if (item.Flags.SoulbindOnUse)
				{
					PrintPlainText((string)ViewModel.Localizer["Soulbound on Use"]);
				}
			}
		}

		public void PrintVendorValue(Item _)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			Coin totalValue = ViewModel.TotalVendorValue;
			if (!(totalValue == Coin.Zero) && !ViewModel.Item.Flags.NoSell)
			{
				FormattedLabelBuilder builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight();
				if (totalValue.Amount >= 10000)
				{
					FormattedLabelPartBuilder gold = builder.CreatePart(totalValue.Gold.ToString("N0"));
					gold.SetTextColor(new Color(221, 187, 68));
					gold.SetFontSize((FontSize)16);
					gold.SetSuffixImage(AsyncTexture2D.FromAssetId(156904));
					gold.SetSuffixImageSize(new Point(20));
					builder.CreatePart(gold);
					builder.CreatePart("  ", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
				if (totalValue.Amount >= 100)
				{
					FormattedLabelPartBuilder silver = builder.CreatePart(totalValue.Silver.ToString("N0"));
					silver.SetTextColor(new Color(192, 192, 192));
					silver.SetFontSize((FontSize)16);
					silver.SetSuffixImage(AsyncTexture2D.FromAssetId(156907));
					silver.SetSuffixImageSize(new Point(20));
					builder.CreatePart(silver);
					builder.CreatePart("  ", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
				FormattedLabelPartBuilder copper = builder.CreatePart(totalValue.Copper.ToString("N0"));
				copper.SetTextColor(new Color(205, 127, 50));
				copper.SetFontSize((FontSize)16);
				copper.SetSuffixImage(AsyncTexture2D.FromAssetId(156902));
				copper.SetSuffixImageSize(new Point(20));
				builder.CreatePart(copper);
				FormattedLabel obj = builder.Build();
				((Control)obj).set_Parent((Container)(object)_layout);
				((Control)obj).set_Width(((Control)_layout).get_Width());
			}
		}

		public void PrintEffect(Effect effect)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_layout);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(((Control)_layout).get_Width());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(5f));
			FlowPanel panel = val;
			if (!string.IsNullOrEmpty(effect.IconHref))
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)panel);
				val2.set_Texture(GameService.Content.GetRenderServiceTexture(effect.IconHref));
				((Control)val2).set_Size(new Point(32));
			}
			StringBuilder builder = new StringBuilder();
			builder.Append(effect.Name);
			if (effect.Duration > TimeSpan.Zero)
			{
				string arg = ((effect.Duration.Hours < 1) ? $"{effect.Duration.TotalMinutes} m" : $"{effect.Duration.TotalHours} h");
				builder.AppendFormat(" ({0})", arg);
			}
			builder.Append(": ");
			builder.Append(effect.Description);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Width(((Control)panel).get_Width() - 30);
			val3.set_AutoSizeHeight(true);
			val3.set_WrapText(true);
			val3.set_Text(builder.ToString());
			val3.set_TextColor(new Color(170, 170, 170));
			val3.set_Font(GameService.Content.get_DefaultFont16());
		}

		public void PrintBuff(Buff buff)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			((Control)new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight().Wrap()
				.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					part.SetFontSize((FontSize)16);
				})
				.AddMarkup(buff.Description, ActiveBuffColor)
				.Build()).set_Parent((Container)(object)_layout);
		}

		public void PrintBonuses(IReadOnlyList<string> bonuses)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder text = new StringBuilder();
			foreach (var (bonus, ordinal) in bonuses.Select((string value, int index) => (value, index + 1)))
			{
				text.Append($"\r\n({ordinal:0}): {bonus}");
			}
			PrintPlainText(text.ToString(), Gray);
		}

		public void PrintWeaponStrength(Weapon weapon)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabelBuilder builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight().Wrap();
			builder.CreatePart((string)ViewModel.Localizer["Weapon Strength", new object[2] { weapon.MinPower, weapon.MaxPower }], (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
			{
				part.SetFontSize((FontSize)16);
			});
			Extensible<DamageType> damageType = weapon.DamageType;
			WeaponSkin defaultSkin = ViewModel.DefaultSkin as WeaponSkin;
			if ((object)defaultSkin != null)
			{
				damageType = defaultSkin.DamageType;
			}
			if (damageType != DamageType.Physical)
			{
				builder.CreatePart($" ({ViewModel.Localizer[damageType.ToString()]})", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					part.SetFontSize((FontSize)16);
					part.SetTextColor(Gray);
				});
			}
			((Control)builder.Build()).set_Parent((Container)(object)_layout);
		}

		public void PrintStatChoices(ICombatEquipment equipment)
		{
			if (equipment.StatChoices.Count > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Double-click to select stats."]);
			}
		}

		public void PrintUniqueness(Item item)
		{
			if (item.Flags.Unique)
			{
				PrintPlainText((string)ViewModel.Localizer["Unique"]);
			}
		}

		protected override void Build(Container buildPanel)
		{
			Item item = ViewModel.Item;
			Armor armor = item as Armor;
			if ((object)armor == null)
			{
				Backpack back = item as Backpack;
				if ((object)back == null)
				{
					Bag bag = item as Bag;
					if ((object)bag == null)
					{
						Consumable consumable = item as Consumable;
						if ((object)consumable == null)
						{
							Container container = item as Container;
							if ((object)container == null)
							{
								CraftingMaterial craftingMaterial = item as CraftingMaterial;
								if ((object)craftingMaterial == null)
								{
									GatheringTool gatheringTool = item as GatheringTool;
									if ((object)gatheringTool == null)
									{
										Trinket trinket = item as Trinket;
										if ((object)trinket == null)
										{
											Gizmo gizmo = item as Gizmo;
											if ((object)gizmo == null)
											{
												JadeTechModule jadeTechModule = item as JadeTechModule;
												if ((object)jadeTechModule == null)
												{
													Miniature miniature = item as Miniature;
													if ((object)miniature == null)
													{
														PowerCore powerCore = item as PowerCore;
														if ((object)powerCore == null)
														{
															Relic relic = item as Relic;
															if ((object)relic == null)
															{
																SalvageTool salvageTool = item as SalvageTool;
																if ((object)salvageTool == null)
																{
																	Trophy trophy = item as Trophy;
																	if ((object)trophy == null)
																	{
																		UpgradeComponent upgradeComponent = item as UpgradeComponent;
																		if ((object)upgradeComponent == null)
																		{
																			Weapon weapon = item as Weapon;
																			if ((object)weapon != null)
																			{
																				PrintWeapon(weapon);
																			}
																			else
																			{
																				Print(ViewModel.Item);
																			}
																		}
																		else
																		{
																			PrintUpgradeComponent(upgradeComponent);
																		}
																	}
																	else
																	{
																		PrintTrophy(trophy);
																	}
																}
																else
																{
																	PrintSalvageTool(salvageTool);
																}
															}
															else
															{
																PrintRelic(relic);
															}
														}
														else
														{
															PrintPowerCore(powerCore);
														}
													}
													else
													{
														PrintMiniature(miniature);
													}
												}
												else
												{
													PrintJadeTechModule(jadeTechModule);
												}
											}
											else
											{
												PrintGizmo(gizmo);
											}
										}
										else
										{
											PrintTrinket(trinket);
										}
									}
									else
									{
										PrintGatheringTool(gatheringTool);
									}
								}
								else
								{
									PrintCraftingMaterial(craftingMaterial);
								}
							}
							else
							{
								PrintContainer(container);
							}
						}
						else
						{
							PrintConsumable(consumable);
						}
					}
					else
					{
						PrintBag(bag);
					}
				}
				else
				{
					PrintBackpack(back);
				}
			}
			else
			{
				PrintArmor(armor);
			}
			((Control)_layout).set_Parent(buildPanel);
		}
	}
}
