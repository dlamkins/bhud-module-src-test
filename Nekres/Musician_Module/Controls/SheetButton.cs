using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician_Module.Notation.Persistance;

namespace Nekres.Musician_Module.Controls
{
	public class SheetButton : DetailsButton
	{
		private const int SHEETBUTTON_WIDTH = 327;

		private const int SHEETBUTTON_HEIGHT = 100;

		private const int USER_WIDTH = 75;

		private const int BOTTOMSECTION_HEIGHT = 35;

		private readonly Texture2D BeatmaniaSprite;

		private readonly Texture2D GlowBeatmaniaSprite;

		private readonly Texture2D AutoplaySprite;

		private readonly Texture2D GlowAutoplaySprite;

		private readonly Texture2D PlaySprite;

		private readonly Texture2D GlowPlaySprite;

		private readonly Texture2D StopSprite;

		private readonly Texture2D GlowStopSprite;

		private readonly Texture2D BackgroundSprite;

		private readonly Texture2D DividerSprite;

		private readonly Texture2D IconBoxSprite;

		private bool _isPreviewing;

		private RawMusicSheet _musicSheet;

		private bool _mouseOverPlay;

		private bool _mouseOverEmulate;

		private bool _mouseOverPreview;

		public string Artist { get; set; }

		public string User { get; set; }

		public bool IsPreviewing
		{
			get
			{
				return _isPreviewing;
			}
			set
			{
				if (value != _isPreviewing)
				{
					_isPreviewing = value;
					((Control)this).Invalidate();
				}
			}
		}

		public RawMusicSheet MusicSheet
		{
			get
			{
				return _musicSheet;
			}
			set
			{
				if (_musicSheet != value)
				{
					_musicSheet = value;
					((Control)this).OnPropertyChanged("MusicSheet");
				}
			}
		}

		public bool MouseOverPlay
		{
			get
			{
				return _mouseOverPlay;
			}
			set
			{
				if (_mouseOverPlay != value)
				{
					_mouseOverPlay = value;
					((Control)this).Invalidate();
				}
			}
		}

		public bool MouseOverEmulate
		{
			get
			{
				return _mouseOverEmulate;
			}
			set
			{
				if (_mouseOverEmulate != value)
				{
					_mouseOverEmulate = value;
					((Control)this).Invalidate();
				}
			}
		}

		public bool MouseOverPreview
		{
			get
			{
				return _mouseOverPreview;
			}
			set
			{
				if (_mouseOverPreview != value)
				{
					_mouseOverPreview = value;
					((Control)this).Invalidate();
				}
			}
		}

		public SheetButton()
			: this()
		{
			BeatmaniaSprite = BeatmaniaSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("beatmania.png");
			GlowBeatmaniaSprite = GlowBeatmaniaSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_beatmania.png");
			AutoplaySprite = AutoplaySprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("autoplay.png");
			GlowAutoplaySprite = GlowAutoplaySprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_autoplay.png");
			StopSprite = StopSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("stop.png");
			GlowStopSprite = GlowStopSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_stop.png");
			PlaySprite = PlaySprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("play.png");
			GlowPlaySprite = GlowPlaySprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_play.png");
			BackgroundSprite = BackgroundSprite ?? Textures.get_Pixel();
			DividerSprite = DividerSprite ?? GameService.Content.GetTexture("157218");
			IconBoxSprite = IconBoxSprite ?? GameService.Content.GetTexture("controls/detailsbutton/605003");
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)SheetButton_MouseMoved);
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)SheetButton_MouseLeft);
			((Control)this).set_Size(new Point(327, 100));
		}

		private void SheetButton_MouseLeft(object sender, MouseEventArgs e)
		{
			MouseOverPlay = false;
			MouseOverEmulate = false;
		}

		private void SheetButton_MouseMoved(object sender, MouseEventArgs e)
		{
			Point relPos = e.get_MouseState().Position - ((Control)this).get_AbsoluteBounds().Location;
			if (((Control)this).get_MouseOver() && relPos.Y > ((Control)this).get_Height() - 35)
			{
				MouseOverPreview = relPos.X < 323 && relPos.X > 291;
				MouseOverPlay = relPos.X < 286 && relPos.X > 254;
				MouseOverEmulate = relPos.X < 250 && relPos.X > 218;
			}
			else
			{
				MouseOverPlay = false;
				MouseOverEmulate = false;
				MouseOverPreview = false;
			}
			if (MouseOverPlay)
			{
				((Control)this).set_BasicTooltipText("Practice mode (Synthesia)");
			}
			else if (MouseOverEmulate)
			{
				((Control)this).set_BasicTooltipText("Emulate keys (Autoplay)");
			}
			else if (MouseOverPreview)
			{
				((Control)this).set_BasicTooltipText("Preview");
			}
			else
			{
				((Control)this).set_BasicTooltipText(((Panel)this).get_Title());
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)5;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			int iconSize = (((int)((DetailsButton)this).get_IconSize() == 1) ? 100 : 65);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BackgroundSprite, bounds, Color.Black * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BackgroundSprite, new Rectangle(0, bounds.Height - 35, bounds.Width - 35, 35), Color.Black * 0.1f);
			if (_mouseOverPreview)
			{
				if (IsPreviewing)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, GlowStopSprite, new Rectangle(291, bounds.Height - 35 + 1, 32, 32), Color.White);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, GlowPlaySprite, new Rectangle(291, bounds.Height - 35 + 1, 32, 32), Color.White);
				}
			}
			else if (IsPreviewing)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, StopSprite, new Rectangle(291, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PlaySprite, new Rectangle(291, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			if (_mouseOverPlay)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, GlowBeatmaniaSprite, new Rectangle(254, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BeatmaniaSprite, new Rectangle(254, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			if (_mouseOverEmulate)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, GlowAutoplaySprite, new Rectangle(218, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AutoplaySprite, new Rectangle(218, bounds.Height - 35 + 1, 32, 32), Color.White);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, DividerSprite, new Rectangle(0, bounds.Height - 40, bounds.Width, 8), Color.White);
			if (((DetailsButton)this).get_Icon() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((DetailsButton)this).get_Icon()), new Rectangle((bounds.Height - 35) / 2 - 32, (bounds.Height - 35) / 2 - 32, 64, 64), Color.White);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, IconBoxSprite, new Rectangle(0, 0, iconSize, iconSize), Color.White);
			}
			string track = ((Panel)this).get_Title() + " - " + Artist;
			string wrappedText = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), track, (float)(287 - iconSize - 20));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, wrappedText, Control.get_Content().get_DefaultFont14(), new Rectangle(89, 0, 216, ((Control)this).get_Height() - 35), Color.White, false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, User, Control.get_Content().get_DefaultFont14(), new Rectangle(5, bounds.Height - 35, 75, 35), Color.White, false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
