using System;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListEntry : Control
	{
		private readonly ScrollingHighlightEffect _highlightEffect;

		private readonly AsyncTexture2D? _icon;

		private readonly Rectangle _iconBounds = new Rectangle(0, 0, 35, 35);

		private readonly ItemsListViewModel _viewModel;

		private Rectangle _textBounds = Rectangle.get_Empty();

		public ItemsListEntry(ItemsListViewModel viewModel)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			ThrowHelper.ThrowIfNull(viewModel, "viewModel");
			_viewModel = viewModel;
			_icon = viewModel.GetIcon();
			_highlightEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_highlightEffect);
			Binder.Bind(viewModel, (ItemsListViewModel viewModel) => viewModel.IsSelected, _highlightEffect);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_002d: Expected O, but got Unknown
			if (((Control)this).get_MouseOver())
			{
				if (((Control)this).get_Tooltip() == null)
				{
					Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(_viewModel.CreateTooltipViewModel()));
					Tooltip val2 = val;
					((Control)this).set_Tooltip(val);
				}
			}
			else
			{
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)this).set_Width(parent.get_ContentRegion().Width);
				_textBounds = new Rectangle(40, 0, ((Control)this).get_Width() - 40, ((Control)this).get_Height());
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			if (_icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _iconBounds, Color.get_White());
			}
			if (((Control)this).get_MouseOver() || _viewModel.IsSelected)
			{
				ReadOnlySpan<(int, int)> readOnlySpan = new ReadOnlySpan<(int, int)>(new(int, int)[4]
				{
					(1, 1),
					(-1, 1),
					(-1, -1),
					(1, -1)
				});
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					var (x, y) = readOnlySpan[i];
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _viewModel.ItemName, Control.get_Content().get_DefaultFont14(), RectangleExtension.OffsetBy(_textBounds, x, y), new Color(Color.get_Black(), 0.4f), true, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _viewModel.ItemName, Control.get_Content().get_DefaultFont14(), _textBounds, _viewModel.Color, true, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
