using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;

namespace MysticCrafting.Module.Items.Controls
{
	public class ItemButton : FlowPanel
	{
		private readonly AsyncTexture2D _textureBottomSectionSeparator = AsyncTexture2D.FromAssetId(157218);

		private readonly AsyncTexture2D _textureVignette = AsyncTexture2D.FromAssetId(605003);

		private DetailsHighlightType _highlightType = DetailsHighlightType.ScrollingHighlight;

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

		public MysticItem Item { get; set; }

		public DetailsHighlightType HighlightType
		{
			get
			{
				return _highlightType;
			}
			set
			{
				if (SetProperty(ref _highlightType, value, invalidateLayout: false, "HighlightType") && base.EffectBehind != null)
				{
					base.EffectBehind.Enabled = _highlightType == DetailsHighlightType.ScrollingHighlight;
				}
			}
		}

		public ItemButton()
		{
			base.Size = new Point(354, 100);
			base.ControlPadding = new Vector2(6f, 1f);
			base.PadLeftBeforeControl = true;
			base.PadTopBeforeControl = true;
			WidthSizingMode = SizingMode.Fill;
			base.Height = 65;
			_scrollEffect = new ScrollingHighlightEffect(this)
			{
				Enabled = (_highlightType == DetailsHighlightType.ScrollingHighlight)
			};
			base.EffectBehind = _scrollEffect;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			_scrollEffect.SetEnableState(_highlightType == DetailsHighlightType.ScrollingHighlight && (base.RelativeMousePosition.Y < base.ContentRegion.Top || base.RelativeMousePosition.X < base.ContentRegion.Left));
			base.OnMouseMoved(e);
		}

		public override void RecalculateLayout()
		{
			int num = _size.Y;
			base.ContentRegion = new Microsoft.Xna.Framework.Rectangle(num, base.Height, base.Width - num, 0);
			if (ShowToggleButton && _children.Any((Control c) => c.Visible))
			{
				_children.Last((Control c) => c.Visible).Right = base.ContentRegion.Width - 4;
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			float num = ((base.MouseOver && _highlightType == DetailsHighlightType.LightHighlight) ? 0.1f : 0.3f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Microsoft.Xna.Framework.Color.Black * num);
			int num2 = _size.Y;
			int num3 = 10;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Microsoft.Xna.Framework.Rectangle(base.ContentRegion.X - num3, base.ContentRegion.Y, base.ContentRegion.Width + num3, base.ContentRegion.Height), Microsoft.Xna.Framework.Color.Black * 0.1f);
			if (Icon != null)
			{
				spriteBatch.DrawOnCtrl(this, Icon, new Microsoft.Xna.Framework.Rectangle(1, 1, 62, 62));
			}
			spriteBatch.DrawOnCtrl(this, _textureVignette, new Microsoft.Xna.Framework.Rectangle(0, 0, num2, num2));
			spriteBatch.DrawOnCtrl(this, _textureBottomSectionSeparator, new Microsoft.Xna.Framework.Rectangle(base.ContentRegion.Left, _size.Y, base.ContentRegion.Width, _textureBottomSectionSeparator.Height));
			spriteBatch.DrawStringOnCtrl(this, Text, Control.Content.DefaultFont16, new Microsoft.Xna.Framework.Rectangle(num2 + 15, 0, _size.X - num2 - 35, base.Height), ColorHelper.FromRarity(Rarity), wrap: true, stroke: true);
			if (Prices != null)
			{
				spriteBatch.DrawStringOnCtrl(this, Prices.Sells.UnitPrice.ToString(), Control.Content.DefaultFont16, new Microsoft.Xna.Framework.Rectangle(num2 + 450, 0, _size.X - num2 - 35, base.Height), Microsoft.Xna.Framework.Color.White, wrap: true, stroke: true);
			}
			base.PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
