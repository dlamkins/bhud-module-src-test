using Blish_HUD;
using Blish_HUD.Content;
using CinemaModule.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace CinemaModule.UI.Controls
{
	public class VideoControlsRenderer
	{
		private const int IconPadding = 4;

		private const int CloseIconOffset = 3;

		private readonly TextureService _textureService;

		private readonly AsyncTexture2D _playTexture;

		private readonly AsyncTexture2D _pauseTexture;

		private readonly AsyncTexture2D _volumeNotMutedTexture;

		private readonly AsyncTexture2D _volumeMutedTexture;

		private readonly AsyncTexture2D _volumeBgTexture;

		private readonly AsyncTexture2D _settingsIconTexture;

		private readonly AsyncTexture2D _settingsBgTexture;

		private readonly AsyncTexture2D _twitchChatIconTexture;

		private readonly AsyncTexture2D _closeIconTexture;

		private readonly AsyncTexture2D _seekBarBgTexture;

		private readonly AsyncTexture2D _lockIconTexture;

		private readonly AsyncTexture2D _lockActiveIconTexture;

		public VideoControlsRenderer(TextureService textureService)
		{
			_textureService = textureService;
			_pauseTexture = _textureService.GetPauseIcon();
			_playTexture = _textureService.GetPlayIcon();
			_volumeNotMutedTexture = _textureService.GetVolumeNotMutedIcon();
			_volumeMutedTexture = _textureService.GetVolumeMutedIcon();
			_settingsIconTexture = _textureService.GetSettingsIcon();
			_settingsBgTexture = _textureService.GetSettingsBackground();
			_twitchChatIconTexture = _textureService.GetTwitchChatIcon();
			_closeIconTexture = _textureService.GetCloseIcon();
			_volumeBgTexture = _textureService.GetVolumeBackground();
			_seekBarBgTexture = _textureService.GetSeekBarBackground();
			_lockIconTexture = _textureService.GetLockIcon();
			_lockActiveIconTexture = _textureService.GetLockActiveIcon();
		}

		public AsyncTexture2D GetVolumeTexture(int volume)
		{
			if (volume != 0)
			{
				return _volumeNotMutedTexture;
			}
			return _volumeMutedTexture;
		}

		public void DrawPlayPauseButton(SpriteBatch spriteBatch, Rectangle bounds, bool isPaused, bool isHovering, float opacity, bool drawBackground = true)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Rectangle iconBounds;
			if (drawBackground)
			{
				DrawBackground(spriteBatch, _settingsBgTexture, bounds, opacity);
				iconBounds = GetPaddedIconBounds(bounds, 4);
			}
			else
			{
				iconBounds = bounds;
			}
			AsyncTexture2D texture = (isPaused ? _playTexture : _pauseTexture);
			DrawIcon(spriteBatch, texture, iconBounds, isHovering, opacity);
		}

		public void DrawVolumeIcon(SpriteBatch spriteBatch, Rectangle bounds, int volume, bool isHovering, float opacity)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			DrawIcon(spriteBatch, GetVolumeTexture(volume), bounds, isHovering, opacity);
		}

		public void DrawVolumeIconWithBackground(SpriteBatch spriteBatch, Rectangle iconBounds, Rectangle backgroundBounds, int volume, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			DrawBackground(spriteBatch, _volumeBgTexture, backgroundBounds, opacity);
			DrawVolumeIcon(spriteBatch, iconBounds, volume, isHovering, opacity);
		}

		public void DrawSettingsButtonWithBackground(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DrawIconWithBackground(spriteBatch, _settingsIconTexture, bounds, isHovering, opacity);
		}

		public void DrawSettingsIconOnly(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DrawIcon(spriteBatch, _settingsIconTexture, bounds, isHovering, opacity);
		}

		public void DrawTwitchChatIconOnly(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DrawIcon(spriteBatch, _twitchChatIconTexture, bounds, isHovering, opacity);
		}

		public void DrawTwitchChatButton(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DrawIconWithBackground(spriteBatch, _twitchChatIconTexture, bounds, isHovering, opacity);
		}

		public void DrawCloseButton(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			DrawBackground(spriteBatch, _settingsBgTexture, bounds, opacity);
			if (_textureService.IsTextureReady(_closeIconTexture))
			{
				int adjustedPadding = 1;
				Rectangle iconBounds = GetPaddedIconBounds(bounds, adjustedPadding);
				Color iconColor = GetIconColor(isHovering, opacity);
				float rotation = MathHelper.ToRadians(45f);
				Vector2 origin = default(Vector2);
				((Vector2)(ref origin))._002Ector((float)_closeIconTexture.get_Width() / 2f, (float)_closeIconTexture.get_Height() / 2f);
				Vector2 position = default(Vector2);
				((Vector2)(ref position))._002Ector((float)iconBounds.X + (float)iconBounds.Width / 2f, (float)iconBounds.Y + (float)iconBounds.Height / 2f);
				Vector2 scale = default(Vector2);
				((Vector2)(ref scale))._002Ector((float)iconBounds.Width / (float)_closeIconTexture.get_Width(), (float)iconBounds.Height / (float)_closeIconTexture.get_Height());
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_closeIconTexture), position, (Rectangle?)null, iconColor, rotation, origin, scale, (SpriteEffects)0, 0f);
			}
		}

		public void DrawTimeText(SpriteBatch spriteBatch, string timeText, Rectangle bounds, float opacity)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont14();
			if (font != null)
			{
				Color textColor = ApplyOpacity(Color.get_White(), opacity);
				Size2 textSize = font.MeasureString(timeText);
				Vector2 textPos = default(Vector2);
				((Vector2)(ref textPos))._002Ector((float)bounds.X + ((float)bounds.Width - textSize.Width) / 2f, (float)bounds.Y + ((float)bounds.Height - textSize.Height) / 2f);
				BitmapFontExtensions.DrawString(spriteBatch, font, timeText, textPos, textColor, (Rectangle?)null);
			}
		}

		public void DrawSeekBarBackground(SpriteBatch spriteBatch, Rectangle bounds, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DrawBackground(spriteBatch, _seekBarBgTexture, bounds, opacity);
		}

		public void DrawLockButton(SpriteBatch spriteBatch, Rectangle bounds, bool isLocked, bool isHovering, float opacity)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = (isLocked ? _lockActiveIconTexture : _lockIconTexture);
			DrawIconWithBackground(spriteBatch, texture, bounds, isHovering, opacity);
		}

		public void DrawStreamInfo(SpriteBatch spriteBatch, Rectangle bounds, string streamTitle, int? viewerCount, string gameName, float opacity)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont16();
			if (font != null && !string.IsNullOrEmpty(streamTitle))
			{
				string displayText = BuildStreamDisplayText(streamTitle, viewerCount, gameName);
				Color textColor = ApplyOpacity(new Color(220, 220, 220), opacity);
				Vector2 textPos = default(Vector2);
				((Vector2)(ref textPos))._002Ector((float)bounds.X, (float)bounds.Y + (float)(bounds.Height - font.get_LineHeight()) / 2f);
				BitmapFontExtensions.DrawString(spriteBatch, font, displayText, textPos, textColor, (Rectangle?)null);
			}
		}

		private void DrawBackground(SpriteBatch spriteBatch, AsyncTexture2D texture, Rectangle bounds, float opacity)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(texture))
			{
				Color bgColor = ApplyOpacity(Color.get_White(), opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), bounds, bgColor);
			}
		}

		private void DrawIcon(SpriteBatch spriteBatch, AsyncTexture2D texture, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(texture))
			{
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), bounds, iconColor);
			}
		}

		private void DrawIconWithBackground(SpriteBatch spriteBatch, AsyncTexture2D iconTexture, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			DrawBackground(spriteBatch, _settingsBgTexture, bounds, opacity);
			Rectangle iconBounds = GetPaddedIconBounds(bounds, 4);
			DrawIcon(spriteBatch, iconTexture, iconBounds, isHovering, opacity);
		}

		private Rectangle GetPaddedIconBounds(Rectangle bounds, int padding)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(bounds.X + padding, bounds.Y + padding, bounds.Width - padding * 2, bounds.Height - padding * 2);
		}

		private string BuildStreamDisplayText(string streamTitle, int? viewerCount, string gameName)
		{
			string displayText = streamTitle;
			if (!string.IsNullOrEmpty(gameName))
			{
				displayText = displayText + " • " + gameName;
			}
			if (viewerCount.HasValue)
			{
				displayText += $" • {viewerCount.Value:N0} viewers";
			}
			return displayText;
		}

		private Color ApplyOpacity(Color color, float opacity)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			return new Color((int)((Color)(ref color)).get_R(), (int)((Color)(ref color)).get_G(), (int)((Color)(ref color)).get_B(), (int)((float)(int)((Color)(ref color)).get_A() * opacity));
		}

		private Color GetIconColor(bool isHovering, float opacity)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Color baseColor = (Color)(isHovering ? Color.get_White() : new Color(220, 220, 220));
			return ApplyOpacity(baseColor, opacity);
		}
	}
}
