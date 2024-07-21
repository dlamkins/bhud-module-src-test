using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Extensions;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class ItemButton : FlowPanel
	{
		private readonly AsyncTexture2D _textureBottomSectionSeparator = AsyncTexture2D.FromAssetId(157218);

		private readonly AsyncTexture2D _textureVignette = AsyncTexture2D.FromAssetId(605003);

		private DetailsHighlightType _highlightType = (DetailsHighlightType)1;

		private Tween _animFill;

		private readonly ScrollingHighlightEffect _scrollEffect;

		public string Text { get; set; }

		public string Rarity { get; set; }

		public string Type { get; set; }

		public CommercePrices Prices { get; set; }

		public string IconDetails { get; set; }

		public AsyncTexture2D Icon { get; set; }

		public bool Favorite { get; set; }

		public bool ShowToggleButton { get; set; }

		public bool ToggleState { get; set; }

		public Item Item { get; set; }

		public DetailsHighlightType HighlightType
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _highlightType;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Invalid comparison between Unknown and I4
				if (((Control)this).SetProperty<DetailsHighlightType>(ref _highlightType, value, false, "HighlightType") && ((Control)this).get_EffectBehind() != null)
				{
					((Control)this).get_EffectBehind().set_Enabled((int)_highlightType == 1);
				}
			}
		}

		public ItemButton()
			: this()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Invalid comparison between Unknown and I4
			//IL_007a: Expected O, but got Unknown
			((FlowPanel)this).set_ControlPadding(new Vector2(6f, 1f));
			((FlowPanel)this).set_PadLeftBeforeControl(true);
			((FlowPanel)this).set_PadTopBeforeControl(true);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(65);
			ScrollingHighlightEffect val = new ScrollingHighlightEffect((Control)(object)this);
			((ControlEffect)val).set_Enabled((int)_highlightType == 1);
			_scrollEffect = val;
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			ScrollingHighlightEffect scrollEffect = _scrollEffect;
			int enableState;
			if ((int)_highlightType == 1)
			{
				int y = ((Control)this).get_RelativeMousePosition().Y;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				if (y >= ((Rectangle)(ref contentRegion)).get_Top())
				{
					int x = ((Control)this).get_RelativeMousePosition().X;
					contentRegion = ((Container)this).get_ContentRegion();
					enableState = ((x < ((Rectangle)(ref contentRegion)).get_Left()) ? 1 : 0);
				}
				else
				{
					enableState = 1;
				}
			}
			else
			{
				enableState = 0;
			}
			((ControlEffect)scrollEffect).SetEnableState((byte)enableState != 0);
			((Control)this).OnMouseMoved(e);
		}

		public override void RecalculateLayout()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			int num = ((Control)this)._size.Y;
			((Container)this).set_ContentRegion(new Rectangle(num, ((Control)this).get_Height(), ((Control)this).get_Width() - num, 0));
			if (ShowToggleButton && ((IEnumerable<Control>)((Container)this)._children).Any((Control c) => c.get_Visible()))
			{
				((IEnumerable<Control>)((Container)this)._children).Last((Control c) => c.get_Visible()).set_Right(((Container)this).get_ContentRegion().Width - 4);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			float num = ((((Control)this).get_MouseOver() && (int)_highlightType == 2) ? 0.1f : 0.3f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * num);
			int num2 = ((Control)this)._size.Y;
			int num3 = 10;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Container)this).get_ContentRegion().X - num3, ((Container)this).get_ContentRegion().Y, ((Container)this).get_ContentRegion().Width + num3, ((Container)this).get_ContentRegion().Height), Color.get_Black() * 0.1f);
			if (Icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Icon), new Rectangle(1, 1, 62, 62));
			}
			Texture2D obj = AsyncTexture2D.op_Implicit(_textureBottomSectionSeparator);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, new Rectangle(((Rectangle)(ref contentRegion)).get_Left(), ((Control)this)._size.Y, ((Container)this).get_ContentRegion().Width, _textureBottomSectionSeparator.get_Height()));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, Control.get_Content().get_DefaultFont16(), new Rectangle(num2 + 15, 0, ((Control)this)._size.X - num2 - 35, ((Control)this).get_Height()), ColorHelper.FromRarity(Rarity), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (Prices != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Prices.get_Sells().get_UnitPrice().ToString(), Control.get_Content().get_DefaultFont16(), new Rectangle(num2 + 450, 0, ((Control)this)._size.X - num2 - 35, ((Control)this).get_Height()), Color.get_White(), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
