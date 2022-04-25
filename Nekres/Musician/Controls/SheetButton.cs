using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;

namespace Nekres.Musician.Controls
{
	internal class SheetButton : DetailsButton
	{
		private const int SHEETBUTTON_WIDTH = 345;

		private const int SHEETBUTTON_HEIGHT = 100;

		private const int USER_WIDTH = 75;

		private const int BOTTOMSECTION_HEIGHT = 35;

		private static Texture2D _trashCanClosed = MusicianModule.ModuleInstance.ContentsManager.GetTexture("trashcanClosed_icon_64x64.png");

		private static Texture2D _trashCanOpen = MusicianModule.ModuleInstance.ContentsManager.GetTexture("trashcanOpen_icon_64x64.png");

		private static Texture2D _beatManiaSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("beatmania.png");

		private static Texture2D _glowBeatManiaSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_beatmania.png");

		private static Texture2D _autoplaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("autoplay.png");

		private static Texture2D _glowAutoplaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_autoplay.png");

		private static Texture2D _playSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("stop.png");

		private static Texture2D _glowPlaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_stop.png");

		private static Texture2D _stopSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("play.png");

		private static Texture2D _glowStopSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_play.png");

		private static Texture2D _dividerSprite = GameService.Content.GetTexture("157218");

		private static Texture2D _iconBoxSprite = GameService.Content.GetTexture("controls/detailsbutton/605003");

		public readonly Guid Id;

		private string _artist;

		private string _user;

		private Instrument _instrument;

		private bool _isPreviewing;

		private Rectangle _practiceButtonBounds;

		private bool _mouseOverPractice;

		private Rectangle _emulateButtonBounds;

		private bool _mouseOverEmulate;

		private Rectangle _previewButtonBounds;

		private bool _mouseOverPreview;

		private Rectangle _deleteButtonBounds;

		private bool _mouseOverDelete;

		public string Artist
		{
			get
			{
				return _artist;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _artist, value, false, "Artist");
			}
		}

		public string User
		{
			get
			{
				return _user;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _user, value, false, "User");
			}
		}

		public Instrument Instrument
		{
			get
			{
				return _instrument;
			}
			set
			{
				((Control)this).SetProperty<Instrument>(ref _instrument, value, false, "Instrument");
			}
		}

		public event EventHandler<EventArgs> OnPracticeClick;

		public event EventHandler<EventArgs> OnEmulateClick;

		public event EventHandler<ValueEventArgs<bool>> OnPreviewClick;

		public event EventHandler<ValueEventArgs<Guid>> OnDelete;

		public SheetButton(MusicSheetModel sheet)
			: this()
		{
			Id = sheet.Id;
			Artist = sheet.Artist;
			User = sheet.User;
			((Panel)this).set_Title(sheet.Title);
			((DetailsButton)this).set_Icon(AsyncTexture2D.op_Implicit(sheet.Instrument.GetIcon()));
			Instrument = sheet.Instrument;
			((Control)this).set_Size(new Point(345, 100));
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			InvalidateMousePosition();
			((DetailsButton)this).OnMouseMoved(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Panel)this).OnClick(e);
			if (_mouseOverPractice)
			{
				this.OnPracticeClick?.Invoke(this, EventArgs.Empty);
				GameService.Content.PlaySoundEffectByName("error");
				ScreenNotification.ShowNotification("Not yet implemented!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverEmulate)
			{
				this.OnEmulateClick?.Invoke(this, EventArgs.Empty);
				GameService.Content.PlaySoundEffectByName("button-click");
			}
			else if (_mouseOverPreview)
			{
				this.OnPreviewClick?.Invoke(this, new ValueEventArgs<bool>(!MusicianModule.ModuleInstance.MusicPlayer.IsMySongPlaying(Id)));
				GameService.Content.PlaySoundEffectByName("button-click");
			}
			else if (_mouseOverDelete)
			{
				((Control)this).Dispose();
			}
		}

		protected override void DisposeControl()
		{
			this.OnDelete?.Invoke(this, new ValueEventArgs<Guid>(Id));
			((Panel)this).DisposeControl();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)5;
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			InvalidateMousePosition();
			((Control)this).OnMoved(e);
		}

		private void InvalidateMousePosition()
		{
			Point relPos = ((Control)this).get_RelativeMousePosition();
			_mouseOverPractice = _practiceButtonBounds.Contains(relPos);
			_mouseOverEmulate = _emulateButtonBounds.Contains(relPos);
			_mouseOverPreview = _previewButtonBounds.Contains(relPos);
			_mouseOverDelete = _deleteButtonBounds.Contains(relPos);
			if (_mouseOverPractice)
			{
				((Control)((Control)this).get_Parent()).set_BasicTooltipText("Practice mode (Synthesia)");
			}
			else if (_mouseOverEmulate)
			{
				((Control)((Control)this).get_Parent()).set_BasicTooltipText("Emulate keys (Autoplay)");
			}
			else if (_mouseOverPreview)
			{
				((Control)((Control)this).get_Parent()).set_BasicTooltipText("Preview");
			}
			else
			{
				((Control)((Control)this).get_Parent()).set_BasicTooltipText(string.Empty);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			_isPreviewing = MusicianModule.ModuleInstance.MusicPlayer.IsMySongPlaying(Id);
			int iconSize = (((int)((DetailsButton)this).get_IconSize() == 1) ? 100 : 65);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.Black * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, bounds.Height - 35, bounds.Width - 35, 35), Color.Black * 0.1f);
			_previewButtonBounds = new Rectangle(309, bounds.Height - 35 + 1, 32, 32);
			if (_isPreviewing)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _playSprite, _previewButtonBounds, Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _stopSprite, _previewButtonBounds, Color.White);
			}
			_practiceButtonBounds = new Rectangle(_previewButtonBounds.Left - 4 - 32, bounds.Height - 35 + 1, 32, 32);
			if (_mouseOverPractice)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _glowBeatManiaSprite, _practiceButtonBounds, Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _beatManiaSprite, _practiceButtonBounds, Color.White);
			}
			_emulateButtonBounds = new Rectangle(_practiceButtonBounds.Left - 4 - 32, bounds.Height - 35 + 1, 32, 32);
			if (_mouseOverEmulate)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _glowAutoplaySprite, _emulateButtonBounds, Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _autoplaySprite, _emulateButtonBounds, Color.White);
			}
			_deleteButtonBounds = new Rectangle(_emulateButtonBounds.Left - 4 - 32, bounds.Height - 35 + 1, 32, 32);
			if (_mouseOverDelete)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _trashCanOpen, _deleteButtonBounds, Color.White);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _trashCanClosed, _deleteButtonBounds, Color.White);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _dividerSprite, new Rectangle(0, bounds.Height - 40, bounds.Width, 8), Color.White);
			if (((DetailsButton)this).get_Icon() != null)
			{
				Rectangle iconBounds = new Rectangle((bounds.Height - 35) / 2 - 32, (bounds.Height - 35) / 2 - 32, 64, 64);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((DetailsButton)this).get_Icon()), iconBounds, Color.White);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _iconBoxSprite, iconBounds, Color.White);
			}
			string track = ((Panel)this).get_Title() + " - " + Artist;
			string wrappedText = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), track, (float)(305 - iconSize - 20));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, wrappedText, Control.get_Content().get_DefaultFont14(), new Rectangle(89, 0, 216, ((Control)this).get_Height() - 35), Color.White, false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, User, Control.get_Content().get_DefaultFont14(), new Rectangle(5, bounds.Height - 35, 75, 35), Color.White, false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
