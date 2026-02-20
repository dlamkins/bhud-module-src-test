using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Controls
{
	public class WindowVideoControls : BaseVideoControls
	{
		private const int DefaultButtonSize = 48;

		private const int ControlMargin = 30;

		private const int IconSliderSpacing = 12;

		private const float HoverScale = 1.15f;

		private new const int SeekBarHeight = 16;

		private const int TimeDisplayWidth = 75;

		private const int BottomBarMargin = 40;

		private const int SeekBarPadding = 80;

		private const int StreamInfoSpacing = 8;

		private readonly int _buttonSize;

		private bool _isHoveringPlayPause;

		private bool _isHoveringVolume;

		private bool _isHoveringSettings;

		private bool _isHoveringTwitchChat;

		private bool _isHoveringClose;

		private bool _isHoveringPanel;

		private bool _wasHoveringPanel;

		private bool _isHoveringLock;

		private bool _isLocked;

		private Rectangle _panelBounds;

		private Rectangle _playPauseBounds;

		private Rectangle _volumeIconBounds;

		private Rectangle _volumeControlBounds;

		private Rectangle _settingsBounds;

		private Rectangle _twitchChatBounds;

		private Rectangle _closeBounds;

		private Rectangle _lockBounds;

		private Rectangle _streamInfoBounds;

		private Rectangle _seekBarBackgroundBounds;

		private Rectangle _timeDisplayBounds;

		private int _currentSeekBarWidth;

		private bool _isSeekable;

		public bool IsTwitchStream { get; set; }

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}
			set
			{
				_isLocked = value;
			}
		}

		public bool IsVisible
		{
			get
			{
				if (!_isHoveringPanel && !base.VolumeTrackBar.get_Dragging() && !((Control)base.QualityDropdown).get_MouseOver())
				{
					return base.IsSeekBarDragging;
				}
				return true;
			}
		}

		private bool ShouldDraw => Opacity > 0.01f;

		public string CurrentTooltip { get; private set; }

		public bool IsSeekable
		{
			get
			{
				return _isSeekable;
			}
			set
			{
				_isSeekable = value;
				UpdateSeekBarVisibility();
			}
		}

		public string StreamTitle { get; set; }

		public int? ViewerCount { get; set; }

		public string GameName { get; set; }

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public event EventHandler<bool> LockToggled;

		public WindowVideoControls(Container parent, int buttonSize = 48)
			: base(parent, 100, 16, 140, createSeekBar: true)
		{
			_buttonSize = buttonSize;
		}

		private void UpdateSeekBarVisibility()
		{
			((Control)base.SeekBar).set_Visible(ShouldDraw && _isSeekable && !IsTwitchStream);
		}

		public void Update(Rectangle panelBounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			_panelBounds = panelBounds;
			Vector2 center = default(Vector2);
			((Vector2)(ref center))._002Ector((float)panelBounds.X + (float)panelBounds.Width / 2f, (float)panelBounds.Y + (float)panelBounds.Height / 2f);
			Vector2 bottomRight = default(Vector2);
			((Vector2)(ref bottomRight))._002Ector((float)(panelBounds.X + panelBounds.Width), (float)(panelBounds.Y + panelBounds.Height));
			Vector2 bottomLeft = default(Vector2);
			((Vector2)(ref bottomLeft))._002Ector((float)panelBounds.X, (float)(panelBounds.Y + panelBounds.Height));
			Vector2 topRight = default(Vector2);
			((Vector2)(ref topRight))._002Ector((float)(panelBounds.X + panelBounds.Width), (float)panelBounds.Y);
			Vector2 topLeft = default(Vector2);
			((Vector2)(ref topLeft))._002Ector((float)panelBounds.X, (float)panelBounds.Y);
			UpdatePlayPauseBounds(center);
			UpdateVolumeBounds(bottomRight, bottomLeft);
			UpdateSeekBarBounds(bottomRight, bottomLeft);
			UpdateSettingsBounds(topRight);
			UpdateLockBounds(topLeft);
			UpdateStreamInfoBounds();
			if (IsTwitchStream)
			{
				UpdateTwitchChatBounds();
			}
			UpdateCloseBounds();
			UpdateTrackBarPosition();
			UpdateSeekBarPosition();
			UpdateSeekBarDragState();
			UpdateQualityDropdownPosition();
			UpdateHoverStates();
			UpdateFadeAnimation();
		}

		private void UpdatePlayPauseBounds(Vector2 center)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			_playPauseBounds = new Rectangle((int)(center.X - (float)_buttonSize / 2f), (int)(center.Y - (float)_buttonSize / 2f), _buttonSize, _buttonSize);
		}

		private void UpdateVolumeBounds(Vector2 bottomRight, Vector2 bottomLeft)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			int bottomY = (int)bottomLeft.Y - 40 - 32;
			_volumeIconBounds = new Rectangle((int)bottomRight.X - 40 - 100 - 12 - 32, bottomY, 32, 32);
			_volumeControlBounds = new Rectangle(_volumeIconBounds.X - 14, _volumeIconBounds.Y - 4, 178, 40);
		}

		private void UpdateTrackBarPosition()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			int trackBarX = _volumeIconBounds.X + 32 + 12 - _panelBounds.X;
			int trackBarY = _volumeIconBounds.Y + 8 - _panelBounds.Y;
			((Control)base.VolumeTrackBar).set_Location(new Point(trackBarX, trackBarY));
			((Control)base.VolumeTrackBar).set_Visible(ShouldDraw);
			((Control)base.VolumeTrackBar).set_Opacity(Opacity);
		}

		private void UpdateSeekBarBounds(Vector2 bottomRight, Vector2 bottomLeft)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			if (_isSeekable && !IsTwitchStream)
			{
				int bottomY = _volumeIconBounds.Y;
				int sliderLeft = _panelBounds.X + 80;
				int num = _volumeControlBounds.X - 80;
				int timeRight = num - 14;
				_timeDisplayBounds = new Rectangle(timeRight - 75, bottomY, 75, 32);
				int seekBarToTimeSpacing = 20;
				_currentSeekBarWidth = _timeDisplayBounds.X - seekBarToTimeSpacing - sliderLeft;
				int bgStartX = sliderLeft - 14;
				int bgWidth = num - bgStartX + 10;
				_seekBarBackgroundBounds = new Rectangle(bgStartX, bottomY - 4, bgWidth, 40);
			}
		}

		private void UpdateSeekBarPosition()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			bool shouldShow = ShouldDraw && _isSeekable && !IsTwitchStream;
			((Control)base.SeekBar).set_Visible(shouldShow);
			((Control)base.SeekBar).set_Opacity(Opacity);
			if (shouldShow)
			{
				int seekBarX = _seekBarBackgroundBounds.X + 14 - _panelBounds.X;
				int seekBarY = _seekBarBackgroundBounds.Y + 4 + 8 - _panelBounds.Y;
				((Control)base.SeekBar).set_Location(new Point(seekBarX, seekBarY));
				((Control)base.SeekBar).set_Size(new Point(_currentSeekBarWidth, 16));
			}
		}

		private void UpdateSettingsBounds(Vector2 topRight)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			_settingsBounds = new Rectangle((int)(topRight.X - 32f - 60f), (int)(topRight.Y + 30f), 32, 32);
		}

		private void UpdateTwitchChatBounds()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			_twitchChatBounds = new Rectangle(_settingsBounds.X - 32 - 8, _settingsBounds.Y + (_settingsBounds.Height - 32) / 2, 32, 32);
		}

		private void UpdateQualityDropdownPosition()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			int dropdownX = ((IsTwitchStream && _twitchChatBounds.Width > 0) ? _twitchChatBounds.X : _settingsBounds.X) - 140 - 8 - _panelBounds.X;
			int dropdownY = _settingsBounds.Y - _panelBounds.Y;
			((Control)base.QualityDropdown).set_Location(new Point(dropdownX, dropdownY));
			((Control)base.QualityDropdown).set_Opacity(Opacity);
			bool hasQualities = base.QualityDropdown.get_Items().Count > 0;
			((Control)base.QualityDropdown).set_Visible(ShouldDraw && hasQualities);
		}

		private void UpdateCloseBounds()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			_closeBounds = new Rectangle(_settingsBounds.X + _settingsBounds.Width + 8, _settingsBounds.Y + (_settingsBounds.Height - 32) / 2, 32, 32);
		}

		private void UpdateLockBounds(Vector2 topLeft)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			_lockBounds = new Rectangle((int)(topLeft.X + 30f), (int)(topLeft.Y + 30f), 32, 32);
		}

		private void UpdateStreamInfoBounds()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			int maxWidth = _settingsBounds.X - ((Rectangle)(ref _lockBounds)).get_Right() - 16 - 140 - 8;
			_streamInfoBounds = new Rectangle(((Rectangle)(ref _lockBounds)).get_Right() + 8, _lockBounds.Y, maxWidth, 32);
		}

		private void UpdateHoverStates()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			_isHoveringPanel = ((Rectangle)(ref _panelBounds)).Contains(mousePos);
			_isHoveringPlayPause = _isHoveringPanel && ((Rectangle)(ref _playPauseBounds)).Contains(mousePos);
			_isHoveringVolume = _isHoveringPanel && (((Rectangle)(ref _volumeControlBounds)).Contains(mousePos) || ((Control)base.VolumeTrackBar).get_MouseOver());
			_isHoveringSettings = _isHoveringPanel && ((Rectangle)(ref _settingsBounds)).Contains(mousePos);
			_isHoveringTwitchChat = IsTwitchStream && _isHoveringPanel && ((Rectangle)(ref _twitchChatBounds)).Contains(mousePos);
			_isHoveringClose = _isHoveringPanel && ((Rectangle)(ref _closeBounds)).Contains(mousePos);
			_isHoveringLock = _isHoveringPanel && ((Rectangle)(ref _lockBounds)).Contains(mousePos);
			UpdateTooltip();
		}

		private void UpdateFadeAnimation()
		{
			if (IsVisible && !_wasHoveringPanel)
			{
				StartFadeIn();
			}
			else if (!IsVisible && _wasHoveringPanel)
			{
				StartFadeOut();
			}
			_wasHoveringPanel = IsVisible;
		}

		private void UpdateTooltip()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (!_isHoveringPanel)
			{
				CurrentTooltip = null;
			}
			else if (_isHoveringPlayPause)
			{
				CurrentTooltip = (base.IsPaused ? "Play" : "Pause");
			}
			else if (_isHoveringLock)
			{
				CurrentTooltip = (_isLocked ? "Unlock Position" : "Lock Position");
			}
			else if (_isHoveringVolume && ((Rectangle)(ref _volumeIconBounds)).Contains(GameService.Input.get_Mouse().get_Position()))
			{
				CurrentTooltip = ((base.Volume == 0) ? "Unmute" : "Mute");
			}
			else if (_isHoveringSettings)
			{
				CurrentTooltip = "Settings";
			}
			else if (_isHoveringTwitchChat)
			{
				CurrentTooltip = "Toggle Twitch Chat";
			}
			else if (_isHoveringClose)
			{
				CurrentTooltip = "Close";
			}
			else
			{
				CurrentTooltip = null;
			}
		}

		public bool HandleMouseDown(Point mousePosition)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			if (!IsVisible)
			{
				return false;
			}
			Rectangle absoluteBounds = ((Control)base.VolumeTrackBar).get_AbsoluteBounds();
			if (((Rectangle)(ref absoluteBounds)).Contains(mousePosition))
			{
				return true;
			}
			TrackBar seekBar = base.SeekBar;
			int num;
			if (seekBar == null)
			{
				num = 0;
			}
			else
			{
				absoluteBounds = ((Control)seekBar).get_AbsoluteBounds();
				num = (((Rectangle)(ref absoluteBounds)).Contains(mousePosition) ? 1 : 0);
			}
			if (num != 0)
			{
				return true;
			}
			if (((Rectangle)(ref _playPauseBounds)).Contains(mousePosition))
			{
				RaisePlayPauseClicked();
				return true;
			}
			if (((Rectangle)(ref _settingsBounds)).Contains(mousePosition))
			{
				RaiseSettingsClicked();
				return true;
			}
			if (IsTwitchStream && ((Rectangle)(ref _twitchChatBounds)).Contains(mousePosition))
			{
				this.TwitchChatClicked?.Invoke(this, EventArgs.Empty);
				return true;
			}
			if (((Rectangle)(ref _closeBounds)).Contains(mousePosition))
			{
				this.CloseClicked?.Invoke(this, EventArgs.Empty);
				return true;
			}
			if (((Rectangle)(ref _lockBounds)).Contains(mousePosition))
			{
				_isLocked = !_isLocked;
				this.LockToggled?.Invoke(this, _isLocked);
				return true;
			}
			if (((Rectangle)(ref _volumeIconBounds)).Contains(mousePosition))
			{
				ToggleMuteAndNotify();
				return true;
			}
			if (((Rectangle)(ref _volumeControlBounds)).Contains(mousePosition))
			{
				return true;
			}
			if (_isSeekable && !IsTwitchStream && ((Rectangle)(ref _seekBarBackgroundBounds)).Contains(mousePosition))
			{
				return true;
			}
			return false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (ShouldDraw)
			{
				DrawLockButton(spriteBatch);
				DrawStreamInfo(spriteBatch);
				DrawPlayPauseButton(spriteBatch);
				DrawSeekBarControls(spriteBatch);
				DrawVolumeIcon(spriteBatch);
				if (IsTwitchStream)
				{
					DrawTwitchChatButton(spriteBatch);
				}
				DrawSettingsButton(spriteBatch);
				DrawCloseButton(spriteBatch);
			}
		}

		private void DrawSeekBarControls(SpriteBatch spriteBatch)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (_isSeekable && !IsTwitchStream)
			{
				base.Renderer.DrawSeekBarBackground(spriteBatch, _seekBarBackgroundBounds, Opacity);
				base.Renderer.DrawTimeText(spriteBatch, FormatTimeDisplay(), _timeDisplayBounds, Opacity);
			}
		}

		private void DrawPlayPauseButton(SpriteBatch spriteBatch)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			Rectangle drawBounds = _playPauseBounds;
			if (_isHoveringPlayPause)
			{
				int scaledSize = (int)((float)_playPauseBounds.Width * 1.15f);
				int offset = (scaledSize - _playPauseBounds.Width) / 2;
				((Rectangle)(ref drawBounds))._002Ector(_playPauseBounds.X - offset, _playPauseBounds.Y - offset, scaledSize, scaledSize);
			}
			base.Renderer.DrawPlayPauseButton(spriteBatch, drawBounds, base.IsPaused, _isHoveringPlayPause, Opacity);
		}

		private void DrawVolumeIcon(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawVolumeIconWithBackground(spriteBatch, _volumeIconBounds, _volumeControlBounds, base.Volume, _isHoveringVolume, Opacity);
		}

		private void DrawSettingsButton(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawSettingsButtonWithBackground(spriteBatch, _settingsBounds, _isHoveringSettings, Opacity);
		}

		private void DrawTwitchChatButton(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawTwitchChatButton(spriteBatch, _twitchChatBounds, _isHoveringTwitchChat, Opacity);
		}

		private void DrawCloseButton(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawCloseButton(spriteBatch, _closeBounds, _isHoveringClose, Opacity);
		}

		private void DrawLockButton(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawLockButton(spriteBatch, _lockBounds, _isLocked, _isHoveringLock, Opacity);
		}

		private void DrawStreamInfo(SpriteBatch spriteBatch)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(StreamTitle))
			{
				base.Renderer.DrawStreamInfo(spriteBatch, _streamInfoBounds, StreamTitle, ViewerCount, GameName, Opacity);
			}
		}
	}
}
