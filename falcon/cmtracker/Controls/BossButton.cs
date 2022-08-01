using Blish_HUD;
using Blish_HUD.Content;
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
					((Control)this).OnPropertyChanged("Font");
				}
			}
		}

		public Color Background
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _background;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				if (!(_background == value))
				{
					_background = value;
					((Control)this).OnPropertyChanged("Background");
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
			: this()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			ICON_TITLE = ICON_TITLE ?? Module.ModuleInstance._sortByRaidTexture;
			BORDER_SPRITE = BORDER_SPRITE ?? Control.get_Content().GetTexture("controls/detailsbutton/605003");
			SEPARATOR = SEPARATOR ?? Control.get_Content().GetTexture("157218");
			PIXEL = PIXEL ?? Textures.get_Pixel();
			((Control)this).set_Size(new Point(327, 100));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Invalid comparison between Unknown and I4
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, bounds, Background * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, ((Container)this).get_ContentRegion(), Color.get_Black() * 0.1f);
			int iconSize = (((int)((DetailsButton)this).get_IconSize() == 1) ? 100 : 65);
			if (((DetailsButton)this).get_Icon() != null && ((DetailsButton)this).get_Icon().get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((DetailsButton)this).get_Icon()), new Rectangle(iconSize / 2 - 32 + (((int)((DetailsButton)this).get_IconSize() == 0) ? 10 : 0), iconSize / 2 - 32, 64, 64), Color.get_White());
				if ((int)((DetailsButton)this).get_IconSize() == 1)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BORDER_SPRITE, new Rectangle(0, 0, iconSize, iconSize), Color.get_White());
				}
			}
			string wrappedText = DrawUtil.WrapText(Font, ((DetailsButton)this).get_Text(), (float)(287 - iconSize - 20));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, wrappedText, Font, new Rectangle(iconSize + 20, 0, 327 - iconSize - 20, ((Control)this).get_Height() - 35), Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
