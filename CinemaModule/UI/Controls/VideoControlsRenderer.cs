using Blish_HUD.Content;
using CinemaModule.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Controls
{
	public class VideoControlsRenderer
	{
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

		private readonly AsyncTexture2D _qualityIconTexture;

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
			_qualityIconTexture = _textureService.GetQualityIcon();
			_volumeBgTexture = _textureService.GetVolumeBackground();
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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			Rectangle iconBounds = default(Rectangle);
			if (drawBackground)
			{
				if (_textureService.IsTextureReady(_settingsBgTexture))
				{
					Color bgColor = ApplyOpacity(Color.get_White(), opacity);
					spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsBgTexture), bounds, bgColor);
				}
				int iconPadding = 4;
				((Rectangle)(ref iconBounds))._002Ector(bounds.X + iconPadding, bounds.Y + iconPadding, bounds.Width - iconPadding * 2, bounds.Height - iconPadding * 2);
			}
			else
			{
				iconBounds = bounds;
			}
			Color iconColor = GetIconColor(isHovering, opacity);
			if (isPaused)
			{
				if (_textureService.IsTextureReady(_playTexture))
				{
					spriteBatch.Draw(AsyncTexture2D.op_Implicit(_playTexture), iconBounds, iconColor);
				}
			}
			else if (_textureService.IsTextureReady(_pauseTexture))
			{
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_pauseTexture), iconBounds, iconColor);
			}
		}

		public void DrawVolumeIcon(SpriteBatch spriteBatch, Rectangle bounds, int volume, bool isHovering, float opacity)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = GetVolumeTexture(volume);
			if (_textureService.IsTextureReady(texture))
			{
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), bounds, iconColor);
			}
		}

		public void DrawVolumeIconWithBackground(SpriteBatch spriteBatch, Rectangle iconBounds, Rectangle backgroundBounds, int volume, bool isHovering, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_volumeBgTexture))
			{
				Color bgColor = ApplyOpacity(Color.get_White(), opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_volumeBgTexture), backgroundBounds, bgColor);
			}
			DrawVolumeIcon(spriteBatch, iconBounds, volume, isHovering, opacity);
		}

		public void DrawSettingsButtonWithBackground(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_settingsBgTexture))
			{
				Color bgColor = ApplyOpacity(Color.get_White(), opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsBgTexture), bounds, bgColor);
			}
			if (_textureService.IsTextureReady(_settingsIconTexture))
			{
				int iconPadding = 4;
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(bounds.X + iconPadding, bounds.Y + iconPadding, bounds.Width - iconPadding * 2, bounds.Height - iconPadding * 2);
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsIconTexture), iconBounds, iconColor);
			}
		}

		public void DrawSettingsIconOnly(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_settingsIconTexture))
			{
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsIconTexture), bounds, iconColor);
			}
		}

		public void DrawTwitchChatIconOnly(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_twitchChatIconTexture))
			{
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_twitchChatIconTexture), bounds, iconColor);
			}
		}

		public void DrawTwitchChatButton(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_settingsBgTexture))
			{
				Color bgColor = ApplyOpacity(Color.get_White(), opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsBgTexture), bounds, bgColor);
			}
			if (_textureService.IsTextureReady(_twitchChatIconTexture))
			{
				int iconPadding = 4;
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(bounds.X + iconPadding, bounds.Y + iconPadding, bounds.Width - iconPadding * 2, bounds.Height - iconPadding * 2);
				Color iconColor = GetIconColor(isHovering, opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_twitchChatIconTexture), iconBounds, iconColor);
			}
		}

		public void DrawCloseButton(SpriteBatch spriteBatch, Rectangle bounds, bool isHovering, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			if (_textureService.IsTextureReady(_settingsBgTexture))
			{
				Color bgColor = ApplyOpacity(Color.get_White(), opacity);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_settingsBgTexture), bounds, bgColor);
			}
			if (_textureService.IsTextureReady(_closeIconTexture))
			{
				int iconPadding = 4;
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(bounds.X + iconPadding - 3, bounds.Y + iconPadding - 3, bounds.Width - (iconPadding - 3) * 2, bounds.Height - (iconPadding - 3) * 2);
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
