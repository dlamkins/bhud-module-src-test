using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

		private Color _rarityColor = Color.White;

		private Color _fontColor;

		public SelectableType Type
		{
			[CompilerGenerated]
			get
			{
				return _003CType_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CType_003Ek__BackingField, value, new PropertyChangedEventHandler(OnTypeChanged), triggerOnUpdate: true, "Type");
			}
		}

		public Action OnClickAction { get; set; }

		public TemplateSlotType TemplateSlot { get; set; }

		public BaseItem Item
		{
			[CompilerGenerated]
			get
			{
				return _003CItem_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CItem_003Ek__BackingField, value, delegate(BaseItem v)
				{
					_003CItem_003Ek__BackingField = v;
				}, new Action(SetItem));
			}
		}

		public TemplateSlotType ActiveSlot { get; set; }

		public GearSubSlotType SubSlotType { get; set; }

		public SelectionPanelSelectable()
		{
			base.Height = 64;
			base.BackgroundColor = Color.Black * 0.2f;
			base.BorderColor = Color.Black;
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
			base.RecalculateLayout();
			_itemControl.Size = new Point(base.Height - base.BorderWidth.Vertical);
			_iconBounds = new Rectangle(3, 3, base.Height - 6, base.Height - 6);
			_nameBounds = new Rectangle(_itemControl.Right + 5, 0, base.Width - _itemControl.Right, base.Height);
			_descriptionBounds = new Rectangle(_nameBounds.Left, _nameBounds.Bottom + 3, _nameBounds.Width, base.Height - 3 - _nameBounds.Bottom);
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
			_itemControl.Item = Item;
			_fontColor = Item?.Rarity.GetColor() ?? Color.White;
			ItemTooltip itemTooltip = base.Tooltip as ItemTooltip;
			if (itemTooltip != null)
			{
				itemTooltip.Item = Item;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
