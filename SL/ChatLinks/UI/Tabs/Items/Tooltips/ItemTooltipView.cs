using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using GuildWars2;
using GuildWars2.Collections;
using GuildWars2.Hero;
using GuildWars2.Hero.Equipment.Wardrobe;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Items.Tooltips
{
	public sealed class ItemTooltipView : View, ITooltipView, IView, IDisposable
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
			await ViewModel.Load(progress).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		protected override void Unload()
		{
			Dispose();
		}

		private void PrintArmor(Armor armor)
		{
			PrintHeader();
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
			PrintUniqueness();
			PrintItemBinding(armor);
			PrintVendorValue();
		}

		private void PrintBackItem(BackItem back)
		{
			PrintHeader();
			PrintAttributes(back.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			PrintUpgrades();
			PrintItemSkin();
			PrintItemRarity(back.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Back Item"]);
			PrintRequiredLevel(back.Level);
			PrintDescription(back.Description);
			PrintInBank();
			PrintStatChoices(back);
			PrintUniqueness();
			PrintItemBinding(back);
			PrintVendorValue();
		}

		private void PrintBag(Bag bag)
		{
			PrintHeader();
			PrintDescription(bag.Description);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(bag);
			PrintVendorValue();
		}

		private void PrintConsumable(Consumable consumable)
		{
			PrintHeader();
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
												if ((object)unlocker7 == null)
												{
													ConjuredDoorwayUnlocker unlocker8 = consumable as ConjuredDoorwayUnlocker;
													if ((object)unlocker8 == null)
													{
														MountSkinUnlocker unlocker9 = consumable as MountSkinUnlocker;
														if ((object)unlocker9 != null)
														{
															if (!string.IsNullOrEmpty(unlocker9.Description))
															{
																PrintPlainText(" ");
															}
															PrintUnlocked();
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
														if (!string.IsNullOrEmpty(unlocker8.Description))
														{
															PrintPlainText(" ");
														}
														PrintUnlocked();
														PrintPlainText((string)ViewModel.Localizer["Consumable"]);
													}
												}
												else
												{
													if (!string.IsNullOrEmpty(unlocker7.Description))
													{
														PrintPlainText(" ");
													}
													PrintUnlocked(ViewModel.UnlockedTextColor);
													PrintPlainText((string)ViewModel.Localizer["Consumable"]);
												}
											}
											else
											{
												if (!string.IsNullOrEmpty(unlocker6.Description))
												{
													PrintPlainText(" ");
												}
												PrintUnlocked();
												PrintPlainText((string)ViewModel.Localizer["Consumable"]);
											}
										}
										else
										{
											if (!string.IsNullOrEmpty(unlocker5.Description))
											{
												PrintPlainText(" ");
											}
											PrintUnlocked();
											PrintPlainText((string)ViewModel.Localizer["Consumable"]);
										}
									}
									else
									{
										if (!string.IsNullOrEmpty(unlocker4.Description))
										{
											PrintPlainText(" ");
										}
										PrintUnlocked();
										PrintPlainText((string)ViewModel.Localizer["Consumable"]);
									}
								}
								else
								{
									if (!string.IsNullOrEmpty(unlocker3.Description))
									{
										PrintPlainText(" ");
									}
									PrintUnlocked();
									PrintPlainText((string)ViewModel.Localizer["Consumable"]);
								}
							}
							else
							{
								if (!string.IsNullOrEmpty(unlocker2.Description))
								{
									PrintPlainText(" ");
								}
								PrintUnlocked();
								PrintPlainText((string)ViewModel.Localizer["Consumable"]);
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(unlocker.Description))
							{
								PrintPlainText(" ");
							}
							PrintUnlocked();
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
			PrintUniqueness();
			PrintItemBinding(consumable);
			PrintVendorValue();
		}

		private void PrintContainer(Container container)
		{
			PrintHeader();
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
			PrintUniqueness();
			PrintItemBinding(container);
			PrintVendorValue();
		}

		private void PrintCraftingMaterial(CraftingMaterial craftingMaterial)
		{
			PrintHeader();
			PrintDescription(craftingMaterial.Description);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(craftingMaterial);
			PrintVendorValue();
		}

		private void PrintGatheringTool(GatheringTool gatheringTool)
		{
			PrintHeader();
			PrintDescription(gatheringTool.Description);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(gatheringTool);
			PrintVendorValue();
		}

		private void PrintTrinket(Trinket trinket)
		{
			PrintHeader();
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
			PrintUniqueness();
			PrintItemBinding(trinket);
			PrintVendorValue();
		}

		private void PrintGizmo(Gizmo gizmo)
		{
			PrintHeader();
			PrintDescription(gizmo.Description, gizmo.Level > 0);
			if (ViewModel.DefaultLocked)
			{
				PrintLockedUnlocked(string.Format("\r\n{0}", ViewModel.Localizer["Novelty Locked"]), string.Format("\r\n{0}", ViewModel.Localizer["Novelty Unlocked"]), "\r\n" + ViewModel.LockedOtherText);
				PrintPlainText(string.Format("\r\n{0}", ViewModel.Localizer["Consumable"]));
			}
			PrintRequiredLevel(gizmo.Level);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(gizmo);
			PrintVendorValue();
		}

		private void PrintJadeTechModule(JadeTechModule jadeTechModule)
		{
			PrintHeader();
			PrintDescription(jadeTechModule.Description);
			PrintItemRarity(jadeTechModule.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Module"]);
			PrintRequiredLevel(jadeTechModule.Level);
			PrintPlainText((string)ViewModel.Localizer["Required Mastery: Jade Bots"]);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(jadeTechModule);
			PrintVendorValue();
		}

		private void PrintMiniature(MiniatureItem miniature)
		{
			PrintHeader();
			PrintDescription(miniature.Description);
			PrintLockedUnlocked(string.Format("\r\n{0}\r\n", ViewModel.Localizer["Mini Locked"]), string.Format("\r\n{0}\r\n", ViewModel.Localizer["Mini Unlocked"]), "\r\n" + ViewModel.LockedOtherText + "\r\n");
			PrintPlainText((string)ViewModel.Localizer["Mini"]);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(miniature);
			PrintVendorValue();
		}

		private void PrintPowerCore(PowerCore powerCore)
		{
			PrintHeader();
			PrintDescription(powerCore.Description, finalNewLine: true);
			PrintItemRarity(powerCore.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Power Core"]);
			PrintRequiredLevel(powerCore.Level);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(powerCore);
			PrintVendorValue();
		}

		private void PrintRelic(Relic relic)
		{
			PrintHeader();
			PrintDescription(relic.Description, finalNewLine: true);
			PrintItemRarity(relic.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Relic"]);
			PrintRequiredLevel(relic.Level);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(relic);
			PrintVendorValue();
		}

		private void PrintSalvageTool(SalvageTool salvageTool)
		{
			PrintHeader();
			PrintPlainText(" ");
			PrintItemRarity(salvageTool.Rarity);
			PrintPlainText((string)ViewModel.Localizer["Consumable"]);
			PrintDescription(salvageTool.Description);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(salvageTool);
			PrintVendorValue();
		}

		private void PrintTrophy(Trophy trophy)
		{
			PrintHeader();
			PrintDescription(trophy.Description);
			PrintPlainText((string)ViewModel.Localizer["Trophy"]);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(trophy);
			PrintVendorValue();
		}

		private void PrintUpgradeComponent(UpgradeComponent upgradeComponent)
		{
			PrintHeader();
			Rune rune = upgradeComponent as Rune;
			if ((object)rune != null)
			{
				PrintBonuses(rune.Bonuses ?? ImmutableValueList.Create(default(ReadOnlySpan<string>)));
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
						goto IL_0097;
					}
				}
				PrintAttributes(upgradeComponent.Attributes.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> stat) => ViewModel.Localizer[stat.Key.ToString()].ToString(), (KeyValuePair<Extensible<AttributeName>, int> stat) => stat.Value));
			}
			goto IL_0097;
			IL_0097:
			PrintDescription(upgradeComponent.Description);
			PrintRequiredLevel(upgradeComponent.Level);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(upgradeComponent);
			PrintVendorValue();
		}

		private void PrintWeapon(Weapon weapon)
		{
			PrintHeader();
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
																if (!(weapon is ShortBow))
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
																	PrintPlainText((string)ViewModel.Localizer["Short Bow"]);
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
			PrintUniqueness();
			PrintItemBinding(weapon);
			PrintVendorValue();
		}

		private void Print(Item item)
		{
			PrintHeader();
			PrintDescription(item.Description);
			PrintInBank();
			PrintUniqueness();
			PrintItemBinding(item);
			PrintVendorValue();
		}

		private void PrintPlainText(string text, Color? textColor = null)
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

		private void PrintHeader()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_layout);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f));
			((Control)val).set_Width(((Control)_layout).get_Width());
			((Control)val).set_Height(50);
			FlowPanel header = val;
			if ((object)ViewModel.DyeColor != null)
			{
				Color rgb = default(Color);
				((Color)(ref rgb))._002Ector((int)ViewModel.DyeColor!.Metal.Rgb.R, (int)ViewModel.DyeColor!.Metal.Rgb.G, (int)ViewModel.DyeColor!.Metal.Rgb.B);
				StackedImage stackedImage = new StackedImage();
				((Control)stackedImage).set_Parent((Container)(object)header);
				((Control)stackedImage).set_Size(new Point(50));
				stackedImage.Textures.Add((AsyncTexture2D.FromAssetId(156885), rgb));
			}
			else
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)header);
				val2.set_Texture(ViewModel.GetIcon(ViewModel.Item));
				((Control)val2).set_Size(new Point(50));
			}
			((Control)new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width() - 55).SetHeight(50).Wrap()
				.SetVerticalAlignment((VerticalAlignment)1)
				.AddMarkup(ViewModel.ItemName.Replace(" ", "  "), delegate(FormattedLabelPartBuilder part)
				{
					part.SetFontSize((FontSize)18);
				}, ViewModel.ItemNameColor)
				.Build()).set_Parent((Container)(object)header);
		}

		private void PrintDefense(int defense)
		{
			if (defense > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Defense", new object[1] { defense }]);
			}
		}

		private void PrintAttributes(Dictionary<string, int> attributes)
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

		private void PrintUpgrades()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
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
						//IL_0037: Unknown result type (might be due to invalid IL or missing references)
						//IL_004c: Unknown result type (might be due to invalid IL or missing references)
						if ((object)slot.UpgradeComponent!.IconUrl != null)
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
						foreach (var (bonus, ordinal) in (rune.Bonuses ?? ImmutableValueList.Create(default(ReadOnlySpan<string>))).Select((string value, int index) => (value, index + 1)))
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

		private void PrintItemSkin()
		{
			string skinName = ViewModel.DefaultSkin?.Name;
			if (!string.IsNullOrEmpty(skinName))
			{
				PrintLockedUnlocked(string.Format("\r\n{0}\r\n{1}", ViewModel.Localizer["Skin Locked"], skinName), string.Format("\r\n{0}\r\n{1}", ViewModel.Localizer["Skin Unlocked"], skinName), "\r\n" + ViewModel.LockedOtherText + "\r\n" + skinName);
			}
		}

		private void PrintTransmutation()
		{
			PrintLockedUnlocked(string.Format("\r\n{0}", ViewModel.Localizer["Skin Locked"]), string.Format("\r\n{0}", ViewModel.Localizer["Skin Unlocked"]), "\r\n" + ViewModel.LockedOtherText);
		}

		private void PrintUnlocked(Color? unlockedTextColor = null)
		{
			PrintUnlocked(ViewModel.UnlockedText + "\r\n", ViewModel.LockedOtherText + "\r\n", unlockedTextColor);
		}

		private void PrintUnlocked(string unlockedText, string? other = null, Color? unlockedTextColor = null)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (ViewModel.DefaultLocked)
			{
				if (ViewModel.Unlocked.HasValue)
				{
					if (ViewModel.Unlocked.Value)
					{
						PrintPlainText(unlockedText, (Color)(((_003F?)unlockedTextColor) ?? Color.get_Red()));
					}
				}
				else if (!string.IsNullOrEmpty(other))
				{
					PrintPlainText(other, Gray);
				}
			}
			else if (!string.IsNullOrEmpty(other))
			{
				PrintPlainText(other, Gray);
			}
		}

		private void PrintLockedUnlocked(string lockedText, string unlockedText, string? other = null)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (ViewModel.DefaultLocked)
			{
				if (ViewModel.Unlocked.HasValue)
				{
					if (ViewModel.Unlocked.Value)
					{
						PrintPlainText(unlockedText);
					}
					else
					{
						PrintPlainText(lockedText, Gray);
					}
				}
				else if (!string.IsNullOrEmpty(other))
				{
					PrintPlainText(other, Gray);
				}
			}
			else if (!string.IsNullOrEmpty(other))
			{
				PrintPlainText(other, Gray);
			}
		}

		private void PrintItemRarity(Extensible<Rarity> rarity)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (rarity == Rarity.Basic)
			{
				PrintPlainText(" ");
			}
			else
			{
				PrintPlainText($"\r\n{ViewModel.Localizer[rarity.ToString()]}", ItemColors.Rarity(rarity));
			}
		}

		private void PrintWeightClass(Extensible<WeightClass> weightClass)
		{
			if (weightClass != WeightClass.Clothing)
			{
				PrintPlainText((string)ViewModel.Localizer[weightClass.ToString()]);
			}
		}

		private void PrintRequiredLevel(int level)
		{
			if (level > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Required Level", new object[1] { level }]);
			}
		}

		private void PrintDescription(string description, bool finalNewLine = false)
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

		private void PrintInBank()
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			ItemTooltipViewModel viewModel = ViewModel;
			if (viewModel == null || viewModel.InBank != 0 || viewModel.InMaterialStorage != 0)
			{
				StringBuilder text = new StringBuilder("\r\n");
				if (ViewModel.InBank > 0)
				{
					text = text.AppendLine(ViewModel.Localizer["Count in bank", new object[1] { ViewModel.InBank }]);
				}
				if (ViewModel.InMaterialStorage > 0)
				{
					text = text.AppendLine(ViewModel.Localizer["Count in material storage", new object[1] { ViewModel.InMaterialStorage }]);
				}
				PrintPlainText(text.ToString(), Gray);
			}
		}

		private void PrintItemBinding(Item item)
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

		private void PrintVendorValue()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			Coin totalValue = ViewModel.TotalVendorValue;
			if (!(totalValue == Coin.Zero) && !ViewModel.Item.Flags.NoSell)
			{
				FormattedLabelBuilder builder = new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width()).AutoSizeHeight();
				if (totalValue.Amount >= 10000)
				{
					FormattedLabelPartBuilder gold = builder.CreatePart(totalValue.Gold.ToString("N0", CultureInfo.CurrentCulture));
					gold.SetTextColor(new Color(221, 187, 68));
					gold.SetFontSize((FontSize)16);
					gold.SetSuffixImage(ViewModel.GetIcon(156904));
					gold.SetSuffixImageSize(new Point(20));
					builder.CreatePart(gold);
					builder.CreatePart("  ", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
				if (totalValue.Amount >= 100)
				{
					FormattedLabelPartBuilder silver = builder.CreatePart(totalValue.Silver.ToString("N0", CultureInfo.CurrentCulture));
					silver.SetTextColor(new Color(192, 192, 192));
					silver.SetFontSize((FontSize)16);
					silver.SetSuffixImage(ViewModel.GetIcon(156907));
					silver.SetSuffixImageSize(new Point(20));
					builder.CreatePart(silver);
					builder.CreatePart("  ", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
				FormattedLabelPartBuilder copper = builder.CreatePart(totalValue.Copper.ToString("N0", CultureInfo.CurrentCulture));
				copper.SetTextColor(new Color(205, 127, 50));
				copper.SetFontSize((FontSize)16);
				copper.SetSuffixImage(ViewModel.GetIcon(156902));
				copper.SetSuffixImageSize(new Point(20));
				builder.CreatePart(copper);
				FormattedLabel obj = builder.Build();
				((Control)obj).set_Parent((Container)(object)_layout);
				((Control)obj).set_Width(((Control)_layout).get_Width());
			}
		}

		private void PrintEffect(Effect effect)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_layout);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(((Control)_layout).get_Width());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(5f));
			FlowPanel panel = val;
			if ((object)effect.IconUrl != null)
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)panel);
				val2.set_Texture(ViewModel.GetIcon(effect.IconUrl));
				((Control)val2).set_Size(new Point(32));
			}
			StringBuilder builder = new StringBuilder();
			builder.Append(effect.Name);
			if (effect.Duration > TimeSpan.Zero)
			{
				IFormatProvider currentCulture = CultureInfo.CurrentCulture;
				TimeSpan duration = effect.Duration;
				string arg = ((duration.TotalDays >= 1.0) ? $"{effect.Duration.Days} d" : ((duration.TotalHours >= 1.0) ? $"{effect.Duration.Hours} h" : ((!(duration.TotalMinutes >= 1.0)) ? $"{effect.Duration.Seconds}s" : $"{effect.Duration.Minutes} m")));
				builder.AppendFormat(currentCulture, " ({0})", arg);
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

		private void PrintBuff(Buff buff)
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

		private void PrintBonuses(IReadOnlyList<string> bonuses)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder text = new StringBuilder();
			foreach (var (bonus, ordinal) in bonuses.Select((string value, int index) => (value, index + 1)))
			{
				text.Append($"\r\n({ordinal:0}): {bonus}");
			}
			PrintPlainText(text.ToString(), Gray);
		}

		private void PrintWeaponStrength(Weapon weapon)
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

		private void PrintStatChoices(ICombatEquipment equipment)
		{
			if (equipment.StatChoices.Count > 0)
			{
				PrintPlainText((string)ViewModel.Localizer["Double-click to select stats"]);
			}
		}

		private void PrintUniqueness()
		{
			if (ViewModel.Item.Flags.Unique)
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
				BackItem back = item as BackItem;
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
													MiniatureItem miniature = item as MiniatureItem;
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
					PrintBackItem(back);
				}
			}
			else
			{
				PrintArmor(armor);
			}
			((Control)_layout).set_Parent(buildPanel);
		}

		public void Dispose()
		{
			((Control)_layout).Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
