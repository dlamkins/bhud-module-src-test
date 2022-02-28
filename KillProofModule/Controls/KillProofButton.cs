using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace KillProofModule.Controls
{
	public class KillProofButton : DetailsButton
	{
		private const int DEFAULT_WIDTH = 327;

		private const int DEFAULT_HEIGHT = 100;

		private const int DEFAULT_BOTTOMSECTION_HEIGHT = 35;

		private readonly Texture2D BORDER_SPRITE;

		private readonly Texture2D ICON_TITLE;

		private readonly Texture2D PIXEL;

		private readonly Texture2D SEPARATOR;

		private BitmapFont _font;

		private string _bottomText = "z";

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

		public string BottomText
		{
			get
			{
				return _bottomText;
			}
			set
			{
				if (!(_bottomText == value))
				{
					_bottomText = value;
					((Control)this).OnPropertyChanged("BottomText");
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
					((Control)this).OnPropertyChanged("IsTitleDisplay");
				}
			}
		}

		public KillProofButton()
			: this()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			ICON_TITLE = ICON_TITLE ?? KillProofModule.ModuleInstance.ContentsManager.GetTexture("icon_title.png");
			BORDER_SPRITE = BORDER_SPRITE ?? Control.get_Content().GetTexture("controls/detailsbutton/605003");
			SEPARATOR = SEPARATOR ?? Control.get_Content().GetTexture("157218");
			PIXEL = PIXEL ?? Textures.get_Pixel();
			((Control)this).set_Size(new Point(327, 100));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Invalid comparison between Unknown and I4
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Invalid comparison between Unknown and I4
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, bounds, Color.get_Black() * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, ((Container)this).get_ContentRegion(), Color.get_Black() * 0.1f);
			int iconSize = (((int)((DetailsButton)this).get_IconSize() == 1) ? 100 : 65);
			if (IsTitleDisplay)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ICON_TITLE, new Rectangle(291, bounds.Height - 35 + 1, 32, 32), Color.get_White());
			}
			else
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, BottomText, Control.get_Content().get_DefaultFont14(), new Rectangle(iconSize + 20, iconSize - 35, 287, 35), Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (((DetailsButton)this).get_Icon() != null && ((DetailsButton)this).get_Icon().get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((DetailsButton)this).get_Icon()), new Rectangle(iconSize / 2 - 32 + (((int)((DetailsButton)this).get_IconSize() == 0) ? 10 : 0), iconSize / 2 - 32, 64, 64), Color.get_White());
				if ((int)((DetailsButton)this).get_IconSize() == 1)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BORDER_SPRITE, new Rectangle(0, 0, iconSize, iconSize), Color.get_White());
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, SEPARATOR, new Rectangle(((Container)this).get_ContentRegion().X, bounds.Height - 40, bounds.Width, 8), Color.get_White());
			string wrappedText = DrawUtil.WrapText(Font, ((DetailsButton)this).get_Text(), (float)(287 - iconSize - 20));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, wrappedText, Font, new Rectangle(iconSize + 20, 0, 327 - iconSize - 20, ((Control)this).get_Height() - 35), Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
