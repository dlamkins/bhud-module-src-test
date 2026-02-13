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

		private readonly int _buttonSize;

		private bool _isHoveringTwitchChat;

		private bool _isHoveringClose;

		private bool _isHoveringPanel;

		private bool _wasHoveringPanel;

		private Rectangle _panelBounds;

		private Rectangle _playPauseBounds;

		private Rectangle _volumeIconBounds;

		private Rectangle _volumeControlBounds;

		private Rectangle _settingsBounds;

		private Rectangle _twitchChatBounds;

		private Rectangle _closeBounds;

		public bool IsTwitchStream { get; set; }

		public bool IsVisible
		{
			get
			{
				if (!_isHoveringPanel)
				{
					TrackBar volumeTrackBar = base.VolumeTrackBar;
					if (volumeTrackBar == null || !volumeTrackBar.get_Dragging())
					{
						Dropdown qualityDropdown = base.QualityDropdown;
						if (qualityDropdown == null)
						{
							return false;
						}
						return ((Control)qualityDropdown).get_MouseOver();
					}
				}
				return true;
			}
		}

		private bool ShouldDraw => Opacity > 0.01f;

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public WindowVideoControls(Container parent, int buttonSize = 48)
			: base(parent, 100, 16, 140)
		{
			_buttonSize = buttonSize;
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
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			_panelBounds = panelBounds;
			Vector2 center = default(Vector2);
			((Vector2)(ref center))._002Ector((float)panelBounds.X + (float)panelBounds.Width / 2f, (float)panelBounds.Y + (float)panelBounds.Height / 2f);
			Vector2 bottomRight = default(Vector2);
			((Vector2)(ref bottomRight))._002Ector((float)(panelBounds.X + panelBounds.Width), (float)(panelBounds.Y + panelBounds.Height));
			Vector2 bottomLeft = default(Vector2);
			((Vector2)(ref bottomLeft))._002Ector((float)panelBounds.X, (float)(panelBounds.Y + panelBounds.Height));
			Vector2 topRight = default(Vector2);
			((Vector2)(ref topRight))._002Ector((float)(panelBounds.X + panelBounds.Width), (float)panelBounds.Y);
			UpdatePlayPauseBounds(center);
			UpdateVolumeBounds(bottomRight, bottomLeft);
			UpdateSettingsBounds(topRight);
			if (IsTwitchStream)
			{
				UpdateTwitchChatBounds();
			}
			UpdateCloseBounds();
			UpdateTrackBarPosition();
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			Vector2 bottomEdgeDir = Vector2.Normalize(bottomRight - bottomLeft);
			Vector2 volumePos = bottomRight - bottomEdgeDir * 174f;
			volumePos.Y -= 46f;
			_volumeIconBounds = new Rectangle((int)volumePos.X, (int)(volumePos.Y - 16f), 32, 32);
			_volumeControlBounds = new Rectangle(_volumeIconBounds.X - 14, _volumeIconBounds.Y - 4, 178, 40);
		}

		private void UpdateTrackBarPosition()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (base.VolumeTrackBar != null)
			{
				int trackBarX = _volumeIconBounds.X + 32 + 12 - _panelBounds.X;
				int trackBarY = _volumeIconBounds.Y + 8 - _panelBounds.Y;
				((Control)base.VolumeTrackBar).set_Location(new Point(trackBarX, trackBarY));
				((Control)base.VolumeTrackBar).set_Visible(ShouldDraw);
				((Control)base.VolumeTrackBar).set_Opacity(Opacity);
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
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			if (base.QualityDropdown != null)
			{
				int dropdownX = ((IsTwitchStream && _twitchChatBounds.Width > 0) ? _twitchChatBounds.X : _settingsBounds.X) - 140 - 8 - _panelBounds.X;
				int dropdownY = _settingsBounds.Y - _panelBounds.Y;
				((Control)base.QualityDropdown).set_Location(new Point(dropdownX, dropdownY));
				((Control)base.QualityDropdown).set_Opacity(Opacity);
				bool hasQualities = base.QualityDropdown.get_Items().Count > 0;
				((Control)base.QualityDropdown).set_Visible(ShouldDraw && hasQualities);
			}
		}

		private void UpdateCloseBounds()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			_closeBounds = new Rectangle(_settingsBounds.X + _settingsBounds.Width + 8, _settingsBounds.Y + (_settingsBounds.Height - 32) / 2, 32, 32);
		}

		private void UpdateHoverStates()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			_isHoveringPanel = ((Rectangle)(ref _panelBounds)).Contains(mousePos);
			IsHoveringPlayPause = _isHoveringPanel && ((Rectangle)(ref _playPauseBounds)).Contains(mousePos);
			int isHoveringVolume;
			if (_isHoveringPanel)
			{
				if (!((Rectangle)(ref _volumeControlBounds)).Contains(mousePos))
				{
					TrackBar volumeTrackBar = base.VolumeTrackBar;
					isHoveringVolume = ((volumeTrackBar != null && ((Control)volumeTrackBar).get_MouseOver()) ? 1 : 0);
				}
				else
				{
					isHoveringVolume = 1;
				}
			}
			else
			{
				isHoveringVolume = 0;
			}
			IsHoveringVolume = (byte)isHoveringVolume != 0;
			IsHoveringSettings = _isHoveringPanel && ((Rectangle)(ref _settingsBounds)).Contains(mousePos);
			_isHoveringTwitchChat = IsTwitchStream && _isHoveringPanel && ((Rectangle)(ref _twitchChatBounds)).Contains(mousePos);
			_isHoveringClose = _isHoveringPanel && ((Rectangle)(ref _closeBounds)).Contains(mousePos);
		}

		private void UpdateFadeAnimation()
		{
			int num;
			if (!_isHoveringPanel)
			{
				TrackBar volumeTrackBar = base.VolumeTrackBar;
				if (volumeTrackBar == null || !volumeTrackBar.get_Dragging())
				{
					Dropdown qualityDropdown = base.QualityDropdown;
					num = ((qualityDropdown != null && ((Control)qualityDropdown).get_MouseOver()) ? 1 : 0);
					goto IL_0031;
				}
			}
			num = 1;
			goto IL_0031;
			IL_0031:
			bool shouldBeVisible = (byte)num != 0;
			if (shouldBeVisible && !_wasHoveringPanel)
			{
				StartFadeIn();
			}
			else if (!shouldBeVisible && _wasHoveringPanel)
			{
				StartFadeOut();
			}
			_wasHoveringPanel = shouldBeVisible;
		}

		public bool HandleMouseDown(Point mousePosition)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (!IsVisible)
			{
				return false;
			}
			TrackBar volumeTrackBar = base.VolumeTrackBar;
			int num;
			if (volumeTrackBar == null)
			{
				num = 0;
			}
			else
			{
				Rectangle absoluteBounds = ((Control)volumeTrackBar).get_AbsoluteBounds();
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
			if (((Rectangle)(ref _volumeIconBounds)).Contains(mousePosition))
			{
				ToggleMuteAndNotify();
				return true;
			}
			if (((Rectangle)(ref _volumeControlBounds)).Contains(mousePosition))
			{
				return true;
			}
			return false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (ShouldDraw)
			{
				DrawPlayPauseButton(spriteBatch);
				DrawVolumeIcon(spriteBatch);
				if (IsTwitchStream)
				{
					DrawTwitchChatButton(spriteBatch);
				}
				DrawSettingsButton(spriteBatch);
				DrawCloseButton(spriteBatch);
			}
		}

		private void DrawPlayPauseButton(SpriteBatch spriteBatch)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			Rectangle drawBounds = _playPauseBounds;
			if (IsHoveringPlayPause)
			{
				int scaledSize = (int)((float)_playPauseBounds.Width * 1.15f);
				int offset = (scaledSize - _playPauseBounds.Width) / 2;
				((Rectangle)(ref drawBounds))._002Ector(_playPauseBounds.X - offset, _playPauseBounds.Y - offset, scaledSize, scaledSize);
			}
			base.Renderer.DrawPlayPauseButton(spriteBatch, drawBounds, base.IsPaused, IsHoveringPlayPause, Opacity);
		}

		private void DrawVolumeIcon(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawVolumeIconWithBackground(spriteBatch, _volumeIconBounds, _volumeControlBounds, base.Volume, IsHoveringVolume, Opacity);
		}

		private void DrawSettingsButton(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.Renderer.DrawSettingsButtonWithBackground(spriteBatch, _settingsBounds, IsHoveringSettings, Opacity);
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

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
