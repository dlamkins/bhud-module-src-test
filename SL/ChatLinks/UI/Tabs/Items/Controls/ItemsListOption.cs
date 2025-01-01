using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls.Items;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class ItemsListOption : Container
	{
		private readonly Lazy<Tooltip> _tooltip;

		private readonly ItemImage _icon;

		private readonly ItemName _name;

		public Item Item { get; }

		public IReadOnlyDictionary<int, UpgradeComponent> Upgrades { get; }

		public ItemsListOption(Item item, IReadOnlyDictionary<int, UpgradeComponent> upgrades)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Item item2 = item;
			IReadOnlyDictionary<int, UpgradeComponent> upgrades2 = upgrades;
			((Container)this)._002Ector();
			Item = item2;
			Upgrades = upgrades2;
			((Control)this).set_Width(425);
			((Control)this).set_Height(35);
			((Control)this).set_Menu((ContextMenuStrip)(object)new ItemContextMenu(item2));
			ItemImage itemImage = new ItemImage(item2);
			((Control)itemImage).set_Size(new Point(35, 35));
			((Control)itemImage).set_Parent((Container)(object)this);
			_icon = itemImage;
			ItemName itemName = new ItemName(item2, upgrades2);
			((Control)itemName).set_Left(40);
			((Control)itemName).set_Width(385);
			((Label)itemName).set_AutoSizeHeight(false);
			((Control)itemName).set_Height(35);
			((Label)itemName).set_VerticalAlignment((VerticalAlignment)1);
			((Control)itemName).set_Parent((Container)(object)this);
			_name = itemName;
			_tooltip = new Lazy<Tooltip>(() => new Tooltip(ServiceLocator.Resolve<IViewsFactory>().CreateItemTooltipView(item2, upgrades2)));
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BackgroundColor(Color.get_BurlyWood());
			((Label)_name).set_ShowShadow(true);
			ItemImage icon = _icon;
			if (((Control)icon).get_Tooltip() == null)
			{
				Tooltip value;
				((Control)icon).set_Tooltip(value = _tooltip.Value);
			}
			ItemName name = _name;
			if (((Control)name).get_Tooltip() == null)
			{
				Tooltip value;
				((Control)name).set_Tooltip(value = _tooltip.Value);
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			((Label)_name).set_ShowShadow(false);
		}

		protected override void DisposeControl()
		{
			((Control)_icon).Dispose();
			((Control)_name).Dispose();
			((Container)this).DisposeControl();
		}
	}
}
