using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Tooltips
{
	public sealed class ItemTooltipView : View, ITooltipView, IView
	{
		private readonly FlowPanel _layout;

		public ItemTooltipViewModel ViewModel { get; }

		public ItemTooltipView(ItemTooltipViewModel viewModel)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			ViewModel = viewModel;
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(350);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			_layout = val;
			Item item = viewModel.Item;
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
																				Print(viewModel.Item);
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
		}

		private void PrintArmor(Armor armor)
		{
			PrintHeader(armor);
			PrintAttributes(armor.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.AttributeName(stat.Key), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin(armor.DefaultSkinId);
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
									PrintPlainText("Shoulder Armor");
								}
							}
							else
							{
								PrintPlainText("Leg Armor");
							}
						}
						else
						{
							PrintPlainText("Head Armor");
						}
					}
					else
					{
						PrintPlainText("Hand Armor");
					}
				}
				else
				{
					PrintPlainText("Chest Armor");
				}
			}
			else
			{
				PrintPlainText("Foot Armor");
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
			PrintAttributes(back.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.AttributeName(stat.Key), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin(back.DefaultSkinId);
			PrintItemRarity(back.Rarity);
			PrintPlainText("Back Item");
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
			PrintHeader(consumable);
			if (consumable is Currency || consumable is Service)
			{
				PrintPlainText("Takes effect immediately upon receipt.");
			}
			else
			{
				PrintPlainText("Double-click to consume.");
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
				Transmutation transmutation = consumable as Transmutation;
				if ((object)transmutation == null)
				{
					if (consumable is Booze)
					{
						PrintPlainText("\r\nExcessive alcohol consumption will result in intoxication.\r\n\r\nConsumable          ");
					}
					else
					{
						PrintPlainText(string.IsNullOrEmpty(consumable.Description) ? "Consumable" : "\r\nConsumable");
					}
				}
				else
				{
					PrintItemSkin(transmutation.SkinIds.First());
					PrintPlainText("\r\nConsumable");
				}
			}
			else
			{
				PrintPlainText(string.IsNullOrEmpty(consumable.Description) ? "Service" : "\r\nService");
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
			PrintPlainText(string.IsNullOrEmpty(container.Description) ? "Consumable" : "\r\nConsumable");
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
			PrintAttributes(trinket.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.AttributeName(stat.Key), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemRarity(trinket.Rarity);
			if (!(trinket is Accessory))
			{
				if (!(trinket is Amulet))
				{
					if (trinket is Ring)
					{
						PrintPlainText("Ring");
					}
				}
				else
				{
					PrintPlainText("Amulet");
				}
			}
			else
			{
				PrintPlainText("Accessory");
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
			PrintHeader(gizmo);
			PrintDescription(gizmo.Description, gizmo.Level > 0);
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
			PrintPlainText("Module");
			PrintRequiredLevel(jadeTechModule.Level);
			PrintPlainText("Required Mastery: Jade Bots");
			PrintInBank();
			PrintUniqueness(jadeTechModule);
			PrintItemBinding(jadeTechModule);
			PrintVendorValue(jadeTechModule);
		}

		private void PrintMiniature(Miniature miniature)
		{
			PrintHeader(miniature);
			PrintDescription(miniature.Description);
			PrintMini(miniature.MiniatureId);
			PrintPlainText("Mini");
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
			PrintPlainText("Power Core");
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
			PrintPlainText("Relic");
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
			PrintPlainText("Consumable");
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
			PrintPlainText("Trophy");
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
				PrintAttributes(upgradeComponent.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.AttributeName(stat.Key), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
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
			PrintAttributes(weapon.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.AttributeName(stat.Key), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin(weapon.DefaultSkinId);
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
																									PrintPlainText("Warhorn");
																								}
																							}
																							else
																							{
																								PrintPlainText("Trident");
																							}
																						}
																						else
																						{
																							PrintPlainText("Toy");
																						}
																					}
																					else
																					{
																						PrintPlainText("Torch");
																					}
																				}
																				else
																				{
																					PrintPlainText("Sword");
																				}
																			}
																			else
																			{
																				PrintPlainText("Staff");
																			}
																		}
																		else
																		{
																			PrintPlainText("Spear");
																		}
																	}
																	else
																	{
																		PrintPlainText("Small Bundle");
																	}
																}
																else
																{
																	PrintPlainText("Shortbow");
																}
															}
															else
															{
																PrintPlainText("Shield");
															}
														}
														else
														{
															PrintPlainText("Scepter");
														}
													}
													else
													{
														PrintPlainText("Rifle");
													}
												}
												else
												{
													PrintPlainText("Pistol");
												}
											}
											else
											{
												PrintPlainText("Mace");
											}
										}
										else
										{
											PrintPlainText("Longbow");
										}
									}
									else
									{
										PrintPlainText("Large Bundle");
									}
								}
								else
								{
									PrintPlainText("Harpoon Gun");
								}
							}
							else
							{
								PrintPlainText("Hammer");
							}
						}
						else
						{
							PrintPlainText("Greatsword");
						}
					}
					else
					{
						PrintPlainText("Focus");
					}
				}
				else
				{
					PrintPlainText("Dagger");
				}
			}
			else
			{
				PrintPlainText("Axe");
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
				PrintPlainText($"Defense: {defense:N0}");
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
				builder.AppendFormat($"+{stat.Value:N0} {stat.Key}");
			}
			PrintPlainText(builder.ToString());
		}

		public void PrintUpgrades()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
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
						//IL_005d: Unknown result type (might be due to invalid IL or missing references)
						if (!string.IsNullOrEmpty(slot.UpgradeComponent!.IconHref))
						{
							part.SetPrefixImage(ViewModel.GetIcon(slot.UpgradeComponent));
							part.SetPrefixImageSize(new Point(16));
						}
						part.SetFontSize((FontSize)16);
						part.SetTextColor(new Color(85, 153, 255));
					});
					Rune rune = slot.UpgradeComponent as Rune;
					if ((object)rune != null)
					{
						foreach (var (bonus, ordinal) in (rune.Bonuses ?? Array.Empty<string>()).Select((string value, int index) => (value, index + 1)))
						{
							builder.CreatePart($"\r\n({ordinal:0}): {bonus}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
							{
								//IL_0019: Unknown result type (might be due to invalid IL or missing references)
								part.SetFontSize((FontSize)16);
								part.SetTextColor(new Color(153, 153, 153));
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
							builder.AddMarkup(slot.UpgradeComponent!.Buff!.Description, (Color?)new Color(85, 153, 255));
							continue;
						}
					}
					foreach (KeyValuePair<Extensible<AttributeName>, int> stat in slot.UpgradeComponent!.Attributes)
					{
						builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
						{
							part.SetFontSize((FontSize)16);
						});
						builder.CreatePart($"+{stat.Value:N0} {ViewModel.AttributeName(stat.Key)}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
						{
							//IL_0016: Unknown result type (might be due to invalid IL or missing references)
							part.SetFontSize((FontSize)16);
							part.SetTextColor(new Color(85, 153, 255));
						});
					}
					continue;
				}
				switch (slot.Type)
				{
				case UpgradeSlotType.Infusion:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" Unused Infusion Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_infusion_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				case UpgradeSlotType.Enrichment:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" Unused Enrichment Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_enrichment_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				default:
					builder.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart(" Unused Upgrade Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_upgrade_slot.png")));
						part.SetPrefixImageSize(new Point(16));
						part.SetFontSize((FontSize)16);
					});
					break;
				}
			}
		}

		public void PrintItemSkin(int skinId)
		{
		}

		public void PrintItemRarity(Extensible<Rarity> rarity)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (rarity != Rarity.Basic)
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
				PrintPlainText($"Required Level: {level}");
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
					PrintPlainText("Account Bound on Acquire");
				}
				else if (item.Flags.AccountBindOnUse)
				{
					PrintPlainText("Account Bound on Use");
				}
				if (item.Flags.Soulbound)
				{
					PrintPlainText("Soulbound on Acquire");
				}
				else if (item.Flags.SoulbindOnUse)
				{
					PrintPlainText("Soulbound on Use");
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

		public void PrintMini(int miniatureId)
		{
		}

		public void PrintBuff(Buff buff)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			((Control)new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight().Wrap()
				.CreatePart("\r\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					part.SetFontSize((FontSize)16);
				})
				.AddMarkup(buff.Description, (Color?)new Color(85, 153, 255))
				.Build()).set_Parent((Container)(object)_layout);
		}

		public void PrintBonuses(IReadOnlyList<string> bonuses)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder text = new StringBuilder();
			foreach (var (bonus, ordinal) in bonuses.Select((string value, int index) => (value, index + 1)))
			{
				text.Append($"\r\n({ordinal:0}): {bonus}");
			}
			PrintPlainText(text.ToString(), (Color?)new Color(153, 153, 153));
		}

		public void PrintWeaponStrength(Weapon weapon)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabelBuilder builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight().Wrap();
			builder.CreatePart($"Weapon Strength: {weapon.MinPower:N0} - {weapon.MaxPower:N0}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
			{
				part.SetFontSize((FontSize)16);
			});
			if (weapon.DamageType.IsDefined())
			{
				switch (weapon.DamageType.ToEnum())
				{
				case DamageType.Choking:
					builder.CreatePart(" (Choking)", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetFontSize((FontSize)16);
						part.SetTextColor(new Color(153, 153, 153));
					});
					break;
				case DamageType.Fire:
					builder.CreatePart(" (Fire)", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetFontSize((FontSize)16);
						part.SetTextColor(new Color(153, 153, 153));
					});
					break;
				case DamageType.Ice:
					builder.CreatePart(" (Ice)", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetFontSize((FontSize)16);
						part.SetTextColor(new Color(153, 153, 153));
					});
					break;
				case DamageType.Lightning:
					builder.CreatePart(" (Lightning)", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
					{
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						part.SetFontSize((FontSize)16);
						part.SetTextColor(new Color(153, 153, 153));
					});
					break;
				}
			}
			((Control)builder.Build()).set_Parent((Container)(object)_layout);
		}

		public void PrintStatChoices(ICombatEquipment equipment)
		{
			if (equipment.StatChoices.Count > 0)
			{
				PrintPlainText("Double-click to select stats.");
			}
		}

		public void PrintUniqueness(Item item)
		{
			if (item.Flags.Unique)
			{
				PrintPlainText("Unique");
			}
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_layout).set_Parent(buildPanel);
		}
	}
}
