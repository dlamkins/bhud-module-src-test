using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListEntry : Control
	{
		[CompilerGenerated]
		private ItemsListViewModel _003CviewModel_003EP;

		private static readonly Color ActiveColor = new Color(109, 100, 69, 0);

		private static readonly Color HoverColor = new Color(109, 100, 69, 127);

		private readonly AsyncTexture2D? _icon;

		private readonly Rectangle _iconBounds;

		private Rectangle _textBounds;

		public ItemsListEntry(ItemsListViewModel viewModel)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			_003CviewModel_003EP = viewModel;
			_icon = _003CviewModel_003EP.GetIcon();
			_iconBounds = new Rectangle(0, 0, 35, 35);
			_textBounds = Rectangle.get_Empty();
			((Control)this)._002Ector();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005f: Expected O, but got Unknown
			Color backgroundColor = (_003CviewModel_003EP.IsSelected ? ActiveColor : ((!((Control)this).get_MouseOver()) ? Color.get_Transparent() : HoverColor));
			((Control)this).set_BackgroundColor(backgroundColor);
			if (((Control)this).get_MouseOver() && ((Control)this).get_Tooltip() == null)
			{
				Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(_003CviewModel_003EP.CreateTooltipViewModel()));
				Tooltip val2 = val;
				((Control)this).set_Tooltip(val);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)this).set_Width(parent.get_ContentRegion().Width);
				((Control)this).set_Height(35);
				_textBounds = new Rectangle(40, 0, ((Control)this).get_Width() - 40, 35);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			if (_icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _iconBounds, Color.get_White());
			}
			if (((Control)this).get_MouseOver() || _003CviewModel_003EP.IsSelected)
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
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _003CviewModel_003EP.Item.Name, Control.get_Content().get_DefaultFont14(), RectangleExtension.OffsetBy(_textBounds, x, y), new Color(Color.get_Black(), 0.4f), true, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _003CviewModel_003EP.Item.Name, Control.get_Content().get_DefaultFont14(), _textBounds, _003CviewModel_003EP.Color, true, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
