using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Controls
{
	public class WorldVideoControls : Container
	{
		private const int PanelHeight = 50;

		private const int ScreenMargin = 60;

		private const int TimeDisplayWidth = 90;

		private const int SeekBarWidth = 150;

		private const int VideoControlsOffset = 30;

		private readonly BaseVideoControls _base;

		private Rectangle _playPauseBounds;

		private Rectangle _volumeIconBounds;

		private Rectangle _settingsBounds;

		private Rectangle _twitchChatBounds;

		private Rectangle _closeBounds;

		private Rectangle _timeDisplayBounds;

		private bool _isHoveringPlayPause;

		private bool _isHoveringVolume;

		private bool _isHoveringSettings;

		private bool _isHoveringTwitchChat;

		private bool _isHoveringClose;

		private Tween _fadeAnimation;

		private bool _shouldBeVisible;

		private bool _isSeekable;

		private bool _isTwitchStream;

		private bool _isWatchPartyViewer;

		public bool IsPaused
		{
			get
			{
				return _base.IsPaused;
			}
			set
			{
				_base.IsPaused = value;
			}
		}

		public int Volume
		{
			get
			{
				return _base.Volume;
			}
			set
			{
				_base.Volume = value;
			}
		}

		public bool IsTrackBarDragging
		{
			get
			{
				TrackBar volumeTrackBar = _base.VolumeTrackBar;
				if (volumeTrackBar == null)
				{
					return _base.IsSeekBarDragging;
				}
				return volumeTrackBar.get_Dragging();
			}
		}

		public bool IsDropdownOpen
		{
			get
			{
				Dropdown qualityDropdown = _base.QualityDropdown;
				if (qualityDropdown == null)
				{
					return false;
				}
				return qualityDropdown.get_PanelOpen();
			}
		}

		public bool IsSeekable
		{
			get
			{
				return _isSeekable;
			}
			set
			{
				if (_isSeekable != value)
				{
					_isSeekable = value;
					UpdateLayout();
				}
			}
		}

		public float CurrentPosition
		{
			get
			{
				return _base.CurrentPosition;
			}
			set
			{
				_base.CurrentPosition = value;
			}
		}

		public long Duration
		{
			get
			{
				return _base.Duration;
			}
			set
			{
				_base.Duration = value;
			}
		}

		private bool ShowSeekBar
		{
			get
			{
				if (_isSeekable)
				{
					return !_isTwitchStream;
				}
				return false;
			}
		}

		public bool IsTwitchStream
		{
			get
			{
				return _isTwitchStream;
			}
			set
			{
				if (_isTwitchStream != value)
				{
					_isTwitchStream = value;
					UpdateLayout();
				}
			}
		}

		public bool IsWatchPartyViewer
		{
			get
			{
				return _isWatchPartyViewer;
			}
			set
			{
				if (_isWatchPartyViewer != value)
				{
					_isWatchPartyViewer = value;
					_base.IsWatchPartyViewer = value;
					UpdateLayout();
				}
			}
		}

		public event EventHandler PlayPauseClicked
		{
			add
			{
				_base.PlayPauseClicked += value;
			}
			remove
			{
				_base.PlayPauseClicked -= value;
			}
		}

		public event EventHandler<int> VolumeChanged
		{
			add
			{
				_base.VolumeChanged += value;
			}
			remove
			{
				_base.VolumeChanged -= value;
			}
		}

		public event EventHandler SettingsClicked
		{
			add
			{
				_base.SettingsClicked += value;
			}
			remove
			{
				_base.SettingsClicked -= value;
			}
		}

		public event EventHandler<int> QualityChanged
		{
			add
			{
				_base.QualityChanged += value;
			}
			remove
			{
				_base.QualityChanged -= value;
			}
		}

		public event EventHandler<float> SeekRequested
		{
			add
			{
				_base.SeekRequested += value;
			}
			remove
			{
				_base.SeekRequested -= value;
			}
		}

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public WorldVideoControls()
			: this()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BackgroundColor(new Color(20, 20, 20, 200));
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(false);
			_base = new BaseVideoControls((Container)(object)this, 100, 16, 140, createSeekBar: true);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			UpdateControlBounds();
			UpdateSeekBarBounds();
			UpdateTrackBarBounds();
			UpdateQualityDropdownBounds();
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || !_shouldBeVisible || ((Control)this).get_Opacity() < 0.01f)
			{
				return (CaptureType)0;
			}
			return ((Container)this).CapturesInput();
		}

		private void UpdateControlBounds()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			int centerY = 25;
			if (_isWatchPartyViewer)
			{
				_playPauseBounds = Rectangle.get_Empty();
			}
			else
			{
				_playPauseBounds = new Rectangle(8, centerY - 16, 32, 32);
			}
		}

		private void UpdateSeekBarBounds()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			int centerY = 25;
			int nextX = (_isWatchPartyViewer ? 8 : (((Rectangle)(ref _playPauseBounds)).get_Right() + 8));
			if (ShowSeekBar)
			{
				((Control)_base.SeekBar).set_Visible(((Control)this).get_Visible());
				((Control)_base.SeekBar).set_Location(new Point(nextX, centerY - 8));
				((Control)_base.SeekBar).set_Size(new Point(150, 16));
				nextX = ((Control)_base.SeekBar).get_Location().X + 150 + 8;
				_timeDisplayBounds = new Rectangle(nextX, centerY - 16, 90, 32);
				nextX += 98;
			}
			else
			{
				((Control)_base.SeekBar).set_Visible(false);
			}
			_volumeIconBounds = new Rectangle(nextX, centerY - 16, 32, 32);
		}

		private void UpdateTrackBarBounds()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			int centerY = 25;
			int trackBarX = ((Rectangle)(ref _volumeIconBounds)).get_Right() + 8;
			((Control)_base.VolumeTrackBar).set_Location(new Point(trackBarX, centerY - 8));
			((Control)_base.VolumeTrackBar).set_Size(new Point(100, 16));
		}

		private void UpdateQualityDropdownBounds()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			int centerY = 25;
			int dropdownX = ((Control)_base.VolumeTrackBar).get_Location().X + 100 + 8;
			int dropdownY = centerY - ((Control)_base.QualityDropdown).get_Height() / 2;
			((Control)_base.QualityDropdown).set_Location(new Point(dropdownX, dropdownY));
			bool hasQualities = _base.QualityDropdown.get_Items().Count > 0;
			((Control)_base.QualityDropdown).set_Visible(hasQualities);
			int nextX = (hasQualities ? (dropdownX + 140 + 8) : dropdownX);
			_twitchChatBounds = new Rectangle(nextX, centerY - 16, 32, 32);
			int settingsX = (IsTwitchStream ? (((Rectangle)(ref _twitchChatBounds)).get_Right() + 8) : nextX);
			_settingsBounds = new Rectangle(settingsX, centerY - 16, 32, 32);
			int closeX = ((Rectangle)(ref _settingsBounds)).get_Right() + 8;
			_closeBounds = new Rectangle(closeX, centerY - 16, 32, 32);
			int calculatedWidth = ((Rectangle)(ref _closeBounds)).get_Right() + 8;
			((Control)this).set_Size(new Point(calculatedWidth, 50));
		}

		public void UpdatePosition(Rectangle videoBounds)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			int x = videoBounds.X + (videoBounds.Width - ((Control)this).get_Width()) / 2;
			int y = ((Rectangle)(ref videoBounds)).get_Bottom() + 30;
			if (y + 50 > screenHeight - 60)
			{
				y = videoBounds.Y - 50 - 30;
			}
			if (y < 60)
			{
				y = Math.Min(((Rectangle)(ref videoBounds)).get_Bottom() - 50 - 60, screenHeight - 50 - 60);
				y = Math.Max(60, y);
			}
			x = Math.Max(60, Math.Min(x, screenWidth - ((Control)this).get_Width() - 60));
			y = Math.Max(60, Math.Min(y, screenHeight - 50 - 60));
			((Control)this).set_Location(new Point(x, y));
		}

		public void Show()
		{
			if (!_shouldBeVisible)
			{
				_shouldBeVisible = true;
				((Control)this).set_Visible(true);
				if (ShowSeekBar)
				{
					((Control)_base.SeekBar).set_Visible(true);
				}
				Tween fadeAnimation = _fadeAnimation;
				if (fadeAnimation != null)
				{
					fadeAnimation.Cancel();
				}
				_fadeAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<WorldVideoControls>(this, (object)new
				{
					Opacity = 1f
				}, 0.2f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
			}
		}

		public void Hide()
		{
			if (_shouldBeVisible && !IsTrackBarDragging && !IsDropdownOpen)
			{
				_shouldBeVisible = false;
				Tween fadeAnimation = _fadeAnimation;
				if (fadeAnimation != null)
				{
					fadeAnimation.Cancel();
				}
				_fadeAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<WorldVideoControls>(this, (object)new
				{
					Opacity = 0f
				}, 0.2f, 0f, true).Ease((Func<float, float>)Ease.QuadIn).OnComplete((Action)delegate
				{
					((Control)this).set_Visible(false);
					((Control)_base.SeekBar).set_Visible(false);
				});
			}
		}

		public void Reset()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Tween fadeAnimation = _fadeAnimation;
			if (fadeAnimation != null)
			{
				fadeAnimation.Cancel();
			}
			_shouldBeVisible = false;
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(false);
			((Control)this).set_Location(Point.get_Zero());
			_isHoveringPlayPause = false;
			_isHoveringVolume = false;
			_isHoveringSettings = false;
			_isHoveringTwitchChat = false;
			_isHoveringClose = false;
			_base.CurrentPosition = 0f;
			_base.Duration = 0L;
			_isSeekable = false;
			_base.SeekBar.set_Value(0f);
			((Control)_base.SeekBar).set_Visible(false);
			UpdateLayout();
		}

		public void UpdateAvailableQualities(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			_base.UpdateAvailableQualities(qualityNames, selectedIndex);
			UpdateQualityDropdownBounds();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = ((Control)_base.VolumeTrackBar).get_AbsoluteBounds();
			if (((Rectangle)(ref absoluteBounds)).Contains(e.get_MousePosition()))
			{
				return;
			}
			if (ShowSeekBar)
			{
				absoluteBounds = ((Control)_base.SeekBar).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(e.get_MousePosition()))
				{
					return;
				}
			}
			((Control)this).OnLeftMouseButtonPressed(e);
			Point localPos = default(Point);
			((Point)(ref localPos))._002Ector(e.get_MousePosition().X - ((Control)this).get_AbsoluteBounds().X, e.get_MousePosition().Y - ((Control)this).get_AbsoluteBounds().Y);
			if (!_isWatchPartyViewer && ((Rectangle)(ref _playPauseBounds)).Contains(localPos))
			{
				_base.RaisePlayPauseClicked();
			}
			else if (((Rectangle)(ref _volumeIconBounds)).Contains(localPos))
			{
				_base.ToggleMuteAndNotify();
			}
			else if (((Rectangle)(ref _settingsBounds)).Contains(localPos))
			{
				_base.RaiseSettingsClicked();
			}
			else if (IsTwitchStream && ((Rectangle)(ref _twitchChatBounds)).Contains(localPos))
			{
				this.TwitchChatClicked?.Invoke(this, EventArgs.Empty);
			}
			else if (((Rectangle)(ref _closeBounds)).Contains(localPos))
			{
				this.CloseClicked?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			Point localPos = default(Point);
			((Point)(ref localPos))._002Ector(mousePos.X - ((Control)this).get_AbsoluteBounds().X, mousePos.Y - ((Control)this).get_AbsoluteBounds().Y);
			_isHoveringPlayPause = !_isWatchPartyViewer && ((Rectangle)(ref _playPauseBounds)).Contains(localPos);
			_isHoveringVolume = ((Rectangle)(ref _volumeIconBounds)).Contains(localPos) || ((Control)_base.VolumeTrackBar).get_MouseOver();
			_isHoveringSettings = ((Rectangle)(ref _settingsBounds)).Contains(localPos);
			_isHoveringTwitchChat = IsTwitchStream && ((Rectangle)(ref _twitchChatBounds)).Contains(localPos);
			_isHoveringClose = ((Rectangle)(ref _closeBounds)).Contains(localPos);
			_base.UpdateSeekBarDragState();
			UpdateTooltip(localPos);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (!(((Control)this).get_Opacity() < 0.01f))
			{
				if (!_isWatchPartyViewer)
				{
					Rectangle playPauseRect = default(Rectangle);
					((Rectangle)(ref playPauseRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _playPauseBounds.X, ((Control)this).get_AbsoluteBounds().Y + _playPauseBounds.Y, _playPauseBounds.Width, _playPauseBounds.Height);
					_base.Renderer.DrawPlayPauseButton(spriteBatch, playPauseRect, IsPaused, _isHoveringPlayPause, ((Control)this).get_Opacity(), drawBackground: false);
				}
				if (ShowSeekBar)
				{
					DrawSeekBarSection(spriteBatch);
				}
				Rectangle volumeRect = default(Rectangle);
				((Rectangle)(ref volumeRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _volumeIconBounds.X, ((Control)this).get_AbsoluteBounds().Y + _volumeIconBounds.Y, _volumeIconBounds.Width, _volumeIconBounds.Height);
				_base.Renderer.DrawVolumeIcon(spriteBatch, volumeRect, Volume, _isHoveringVolume, ((Control)this).get_Opacity());
				if (IsTwitchStream)
				{
					Rectangle twitchChatRect = default(Rectangle);
					((Rectangle)(ref twitchChatRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _twitchChatBounds.X, ((Control)this).get_AbsoluteBounds().Y + _twitchChatBounds.Y, _twitchChatBounds.Width, _twitchChatBounds.Height);
					_base.Renderer.DrawTwitchChatIconOnly(spriteBatch, twitchChatRect, _isHoveringTwitchChat, ((Control)this).get_Opacity());
				}
				Rectangle settingsRect = default(Rectangle);
				((Rectangle)(ref settingsRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _settingsBounds.X, ((Control)this).get_AbsoluteBounds().Y + _settingsBounds.Y, _settingsBounds.Width, _settingsBounds.Height);
				_base.Renderer.DrawSettingsIconOnly(spriteBatch, settingsRect, _isHoveringSettings, ((Control)this).get_Opacity());
				Rectangle closeRect = default(Rectangle);
				((Rectangle)(ref closeRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _closeBounds.X, ((Control)this).get_AbsoluteBounds().Y + _closeBounds.Y, _closeBounds.Width, _closeBounds.Height);
				_base.Renderer.DrawCloseButton(spriteBatch, closeRect, _isHoveringClose, ((Control)this).get_Opacity());
			}
		}

		private void UpdateTooltip(Point localPos)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if (!_isWatchPartyViewer && ((Rectangle)(ref _playPauseBounds)).Contains(localPos))
			{
				((Control)this).set_BasicTooltipText(IsPaused ? "Play" : "Pause");
			}
			else if (((Rectangle)(ref _volumeIconBounds)).Contains(localPos))
			{
				((Control)this).set_BasicTooltipText((Volume == 0) ? "Unmute" : "Mute");
			}
			else if (((Rectangle)(ref _settingsBounds)).Contains(localPos))
			{
				((Control)this).set_BasicTooltipText("Settings");
			}
			else if (IsTwitchStream && ((Rectangle)(ref _twitchChatBounds)).Contains(localPos))
			{
				((Control)this).set_BasicTooltipText("Toggle Twitch Chat");
			}
			else if (((Rectangle)(ref _closeBounds)).Contains(localPos))
			{
				((Control)this).set_BasicTooltipText("Close");
			}
			else
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
		}

		private void DrawSeekBarSection(SpriteBatch spriteBatch)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Rectangle timeRect = default(Rectangle);
			((Rectangle)(ref timeRect))._002Ector(((Control)this).get_AbsoluteBounds().X + _timeDisplayBounds.X, ((Control)this).get_AbsoluteBounds().Y + _timeDisplayBounds.Y, _timeDisplayBounds.Width, _timeDisplayBounds.Height);
			_base.Renderer.DrawTimeText(spriteBatch, _base.FormatTimeDisplay(), timeRect, ((Control)this).get_Opacity());
		}

		protected override void DisposeControl()
		{
			_base.Dispose();
			((Container)this).DisposeControl();
		}
	}
}
