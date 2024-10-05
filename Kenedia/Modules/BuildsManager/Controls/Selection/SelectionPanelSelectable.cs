using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls_Old.GearPage;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class SelectionPanelSelectable : Kenedia.Modules.Core.Controls.Panel
	{
		public enum TargetType
		{
			Single,
			Group,
			GroupEmpty,
			All,
			AllEmpty
		}

		public enum SelectableType
		{
			None,
			Rune,
			Sigil,
			Infusion,
			Stat
		}

		private readonly ItemControl _itemControl = new ItemControl
		{
			CaptureInput = false
		};

		private Rectangle _iconBounds;

		private Rectangle _nameBounds;

		private Rectangle _descriptionBounds;

		private Color _rarityColor = Color.get_White();

		private BaseItem _item;

		private SelectableType _type;

		private Color _fontColor;

		public SelectableType Type
		{
			get
			{
				return _type;
			}
			set
			{
				Common.SetProperty(ref _type, value, new PropertyChangedEventHandler(OnTypeChanged), triggerOnUpdate: true, "Type");
			}
		}

		public Action OnClickAction { get; set; }

		public TemplateSlotType TemplateSlot { get; set; }

		public BaseItem Item
		{
			get
			{
				return _item;
			}
			set
			{
				Common.SetProperty(ref _item, value, new Action(SetItem));
			}
		}

		public TemplateSlotType ActiveSlot { get; set; }

		public GearSubSlotType SubSlotType { get; set; }

		public SelectionPanelSelectable()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			base.Height = 64;
			base.BackgroundColor = Color.get_Black() * 0.2f;
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			_itemControl.Parent = this;
			_itemControl.Location = new Point(base.BorderWidth.Left, base.BorderWidth.Top);
			_itemControl.Size = new Point(base.Height - base.BorderWidth.Vertical);
			base.Tooltip = new ItemTooltip
			{
				SetLocalizedComment = () => Environment.NewLine + strings.ItemControlClickToCopyItem
			};
		}

		public override void RecalculateLayout()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_itemControl.Size = new Point(base.Height - base.BorderWidth.Vertical);
			_iconBounds = new Rectangle(3, 3, base.Height - 6, base.Height - 6);
			_nameBounds = new Rectangle(_itemControl.Right + 5, 0, base.Width - _itemControl.Right, base.Height);
			_descriptionBounds = new Rectangle(((Rectangle)(ref _nameBounds)).get_Left(), ((Rectangle)(ref _nameBounds)).get_Bottom() + 3, _nameBounds.Width, base.Height - 3 - ((Rectangle)(ref _nameBounds)).get_Bottom());
		}

		protected virtual void OnTypeChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			OnClickAction?.Invoke();
		}

		private void SetItem()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			_itemControl.Item = Item;
			_fontColor = Item?.Rarity.GetColor() ?? Color.get_White();
			ItemTooltip itemTooltip = base.Tooltip as ItemTooltip;
			if (itemTooltip != null)
			{
				itemTooltip.Item = Item;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, Item?.Name, Control.Content.DefaultFont14, _nameBounds, _fontColor);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Item = null;
		}
	}
}
