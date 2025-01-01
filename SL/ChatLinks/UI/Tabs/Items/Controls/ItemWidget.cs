using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GuildWars2.Chat;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SL.Common;
using SL.Common.Controls;
using SL.Common.Controls.Items;
using SL.Common.Controls.Items.Upgrades;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class ItemWidget : FlowPanel
	{
		private readonly TextBox _chatLink;

		private readonly Item _item;

		private readonly IReadOnlyDictionary<int, UpgradeComponent> _upgrades;

		private readonly ItemImage _itemIcon;

		private readonly ItemName _itemName;

		private readonly NumberPicker _numberPicker;

		private readonly UpgradeSlots? _upgradeComponents;

		private readonly Label? _infusionWarning;

		public ItemWidget(Item item, IReadOnlyDictionary<int, UpgradeComponent> upgrades)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Expected O, but got Unknown
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Expected O, but got Unknown
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Expected O, but got Unknown
			Item item2 = item;
			IReadOnlyDictionary<int, UpgradeComponent> upgrades2 = upgrades;
			((FlowPanel)this)._002Ector();
			ItemWidget itemWidget = this;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_ShowBorder(true);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 15f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(10f));
			((Container)this).set_AutoSizePadding(new Point(10));
			((Control)this).set_Width(350);
			((Container)this).set_HeightSizingMode((SizingMode)2);
			((Container)this).set_ContentRegion(new Rectangle(5, 5, 290, 520));
			((Panel)this).set_CanScroll(true);
			_item = item2;
			_upgrades = upgrades2;
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f));
			((Control)val).set_Width(((Control)this).get_Width() - 5);
			((Control)val).set_Height(50);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel header = val;
			ItemImage itemImage = new ItemImage(item2);
			((Control)itemImage).set_Parent((Container)(object)header);
			_itemIcon = itemImage;
			((Control)_itemIcon).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Expected O, but got Unknown
				//IL_0032: Expected O, but got Unknown
				ItemImage itemIcon = itemWidget._itemIcon;
				if (((Control)itemIcon).get_Tooltip() == null)
				{
					Tooltip val9 = new Tooltip((ITooltipView)(object)new ItemTooltipView(item2, upgrades2));
					Tooltip val10 = val9;
					((Control)itemIcon).set_Tooltip(val9);
				}
			});
			((Control)header).set_Menu((ContextMenuStrip)(object)new ItemContextMenu(item2));
			ItemName itemName = new ItemName(item2, upgrades2);
			((Control)itemName).set_Parent((Container)(object)header);
			((Control)itemName).set_Width(((Control)header).get_Width() - 50);
			((Control)itemName).set_Height(50);
			((Label)itemName).set_VerticalAlignment((VerticalAlignment)1);
			((Label)itemName).set_Font(GameService.Content.get_DefaultFont18());
			((Label)itemName).set_WrapText(true);
			_itemName = itemName;
			((Label)_itemName).set_Text(((Label)_itemName).get_Text().Replace(" ", "  "));
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Control)val2).set_Width(((Control)this).get_Width());
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			FlowPanel quantityGroup = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)quantityGroup);
			val3.set_Text("Quantity:");
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			NumberPicker numberPicker = new NumberPicker();
			((Control)numberPicker).set_Parent((Container)(object)quantityGroup);
			((Control)numberPicker).set_Width(80);
			numberPicker.Value = 1;
			numberPicker.MinValue = 1;
			numberPicker.MaxValue = 250;
			_numberPicker = numberPicker;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)quantityGroup);
			val4.set_Text("Min");
			((Control)val4).set_Width(40);
			((Control)val4).set_Height(((Control)this).get_Height());
			StandardButton minQuantity = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)quantityGroup);
			val5.set_Text("Max");
			((Control)val5).set_Width(40);
			((Control)val5).set_Height(((Control)this).get_Height());
			if (item2 is IUpgradable)
			{
				UpgradeSlots upgradeSlots = new UpgradeSlots(item2, upgrades2);
				((Control)upgradeSlots).set_Parent((Container)(object)this);
				((Control)upgradeSlots).set_Width(((Control)this).get_Width() - 40);
				((Container)upgradeSlots).set_HeightSizingMode((SizingMode)1);
				_upgradeComponents = upgradeSlots;
				_upgradeComponents!.ViewModel.UpgradesChanged += delegate
				{
					itemWidget.UpdateHeader();
					itemWidget.UpdateChatLink();
					itemWidget.ToggleWarnings();
				};
			}
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Chat Link:");
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			TextBox val7 = new TextBox();
			((Control)val7).set_Parent((Container)(object)this);
			((TextInputBase)val7).set_Text(item2.ChatLink);
			((Control)val7).set_Width(200);
			_chatLink = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Width(((Control)this).get_Width() - 20);
			val8.set_AutoSizeHeight(true);
			val8.set_WrapText(true);
			val8.set_TextColor(Color.get_Yellow());
			val8.set_Text("Due to technical restrictions, the game only\r\nshows the item's default infusion(s) instead of\r\nthe selected infusion(s).");
			((Control)val8).set_Visible(false);
			_infusionWarning = val8;
			((Control)_itemIcon).add_Click((EventHandler<MouseEventArgs>)HeaderClicked);
			((TextInputBase)_numberPicker).add_TextChanged((EventHandler<EventArgs>)NumberPickerChanged);
			((Control)_chatLink).add_Click((EventHandler<MouseEventArgs>)ChatLinkClicked);
			((Control)minQuantity).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				itemWidget._numberPicker.Value = 1;
			});
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				itemWidget._numberPicker.Value = 250;
			});
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			if (!(((IEnumerable<Control>)(object)_upgradeComponents)?.Any((Control slot) => slot.get_MouseOver()) ?? false))
			{
				((Control)this).OnMouseWheelScrolled(e);
			}
		}

		private void NumberPickerChanged(object sender, EventArgs e)
		{
			_itemName.Quantity = _numberPicker.Value;
			UpdateChatLink();
		}

		private void HeaderClicked(object sender, MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			ModifierKeys activeModifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
			if ((int)activeModifiers != 1)
			{
				if ((int)activeModifiers != 4)
				{
					GameService.GameIntegration.get_Chat().Paste(((TextInputBase)_chatLink).get_Text());
				}
			}
			else
			{
				GameService.GameIntegration.get_Chat().Send(((TextInputBase)_chatLink).get_Text());
			}
		}

		private void UpdateHeader()
		{
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Expected O, but got Unknown
			_itemName.SuffixItem = _upgradeComponents?.ViewModel.EffectiveSuffixItem();
			ItemImage itemIcon = _itemIcon;
			Item item = _item;
			Armor armor = item as Armor;
			Item item2;
			if ((object)armor == null)
			{
				Weapon weapon = item as Weapon;
				if ((object)weapon == null)
				{
					Backpack back = item as Backpack;
					if ((object)back == null)
					{
						Trinket trinket = item as Trinket;
						if ((object)trinket != null)
						{
							Trinket obj = (Trinket)trinket._003CClone_003E_0024();
							obj.Name = trinket.NameWithoutSuffix(_upgrades);
							obj.SuffixItemId = _upgradeComponents?.ViewModel.UpgradeSlot1?.EffectiveUpgrade?.Id;
							obj.InfusionSlots = _upgradeComponents?.ViewModel.Infusions() ?? Array.Empty<InfusionSlot>();
							item2 = obj;
						}
						else
						{
							item2 = _item;
						}
					}
					else
					{
						Backpack obj2 = (Backpack)back._003CClone_003E_0024();
						obj2.Name = back.NameWithoutSuffix(_upgrades);
						obj2.SuffixItemId = _upgradeComponents?.ViewModel.UpgradeSlot1?.EffectiveUpgrade?.Id;
						obj2.InfusionSlots = _upgradeComponents?.ViewModel.Infusions() ?? Array.Empty<InfusionSlot>();
						item2 = obj2;
					}
				}
				else
				{
					Weapon obj3 = (Weapon)weapon._003CClone_003E_0024();
					obj3.Name = weapon.NameWithoutSuffix(_upgrades);
					obj3.SuffixItemId = _upgradeComponents?.ViewModel.UpgradeSlot1?.EffectiveUpgrade?.Id;
					obj3.SecondarySuffixItemId = _upgradeComponents?.ViewModel.UpgradeSlot2?.EffectiveUpgrade?.Id;
					obj3.InfusionSlots = _upgradeComponents?.ViewModel.Infusions() ?? Array.Empty<InfusionSlot>();
					item2 = obj3;
				}
			}
			else
			{
				Armor obj4 = (Armor)armor._003CClone_003E_0024();
				obj4.Name = armor.NameWithoutSuffix(_upgrades);
				obj4.SuffixItemId = _upgradeComponents?.ViewModel.UpgradeSlot1?.EffectiveUpgrade?.Id;
				obj4.InfusionSlots = _upgradeComponents?.ViewModel.Infusions() ?? Array.Empty<InfusionSlot>();
				item2 = obj4;
			}
			((Control)itemIcon).set_Tooltip(new Tooltip((ITooltipView)(object)new ItemTooltipView(item2, _upgrades)));
		}

		private void UpdateChatLink()
		{
			int quantity = _numberPicker.Value;
			TextBox chatLink = _chatLink;
			ItemLink obj = (ItemLink)_item.GetChatLink()._003CClone_003E_0024();
			obj.Count = quantity;
			obj.SuffixItemId = _upgradeComponents?.ViewModel.SuffixItemId;
			obj.SecondarySuffixItemId = _upgradeComponents?.ViewModel.SecondarySuffixItemId;
			((TextInputBase)chatLink).set_Text(obj.ToString());
		}

		private void ToggleWarnings()
		{
			((Control)_infusionWarning).set_Visible((_upgradeComponents?.ViewModel.InfusionSlots?.Any((UpgradeSlotModel s) => s.SelectedUpgradeComponent != null)).GetValueOrDefault());
		}

		private void ChatLinkClicked(object sender, MouseEventArgs e)
		{
			((TextInputBase)_chatLink).set_SelectionStart(0);
			((TextInputBase)_chatLink).set_SelectionEnd(((TextInputBase)_chatLink).get_Text().Length);
		}
	}
}
