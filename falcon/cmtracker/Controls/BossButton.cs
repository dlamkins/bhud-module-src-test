using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using falcon.cmtracker.Persistance;

namespace falcon.cmtracker.Controls
{
	public class BossButton : DetailsButton
	{
		private const int DEFAULT_WIDTH = 327;

		private const int DEFAULT_HEIGHT = 100;

		private const int DEFAULT_BOTTOMSECTION_HEIGHT = 35;

		private readonly Texture2D BORDER_SPRITE;

		private readonly Texture2D ICON_TITLE;

		private readonly Texture2D PIXEL;

		private readonly Texture2D SEPARATOR;

		private BitmapFont _font;

		private Color _background;

		public Token Token;

		private bool _isTitleDisplay;

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value;
					OnPropertyChanged("Font");
				}
			}
		}

		public Color Background
		{
			get
			{
				return _background;
			}
			set
			{
				if (!(_background == value))
				{
					_background = value;
					OnPropertyChanged("Background");
				}
			}
		}

		public bool IsTitleDisplay
		{
			get
			{
				return _isTitleDisplay;
			}
			set
			{
				if (value != _isTitleDisplay)
				{
					_isTitleDisplay = value;
				}
			}
		}

		public BossButton()
		{
			ICON_TITLE = ICON_TITLE ?? Module.ModuleInstance._sortByRaidTexture;
			BORDER_SPRITE = BORDER_SPRITE ?? Control.Content.GetTexture("controls/detailsbutton/605003");
			SEPARATOR = SEPARATOR ?? Control.Content.GetTexture("157218");
			PIXEL = PIXEL ?? ContentService.Textures.Pixel;
			base.Size = new Point(327, 100);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, PIXEL, bounds, Background * 0.25f);
			spriteBatch.DrawOnCtrl(this, PIXEL, base.ContentRegion, Color.Black * 0.1f);
			int iconSize = ((base.IconSize == DetailsIconSize.Large) ? 100 : 65);
			if (base.Icon != null && base.Icon.HasTexture)
			{
				spriteBatch.DrawOnCtrl(this, base.Icon, new Rectangle(iconSize / 2 - 32 + ((base.IconSize == DetailsIconSize.Small) ? 10 : 0), iconSize / 2 - 32, 64, 64), Color.White);
				if (base.IconSize == DetailsIconSize.Large)
				{
					spriteBatch.DrawOnCtrl(this, BORDER_SPRITE, new Rectangle(0, 0, iconSize, iconSize), Color.White);
				}
			}
			string wrappedText = DrawUtil.WrapText(Font, base.Text, 287 - iconSize - 20);
			spriteBatch.DrawStringOnCtrl(this, wrappedText, Font, new Rectangle(iconSize + 20, 0, 327 - iconSize - 20, base.Height - 35), Color.White, wrap: false, stroke: true, 2);
		}
	}
}
