using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics;
using CinemaModule.Models;
using CinemaModule.Models.Twitch;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.Services
{
	public class TextureService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<TextureService>();

		private readonly ImageCacheService _imageCache;

		private readonly HttpClient _httpClient;

		private Texture2D _whitePixel;

		private AsyncTexture2D _fallbackTexture;

		private TwitchStreamInfo _cachedTwitchStreamInfo;

		private const string CornerIconTexture = "logo_64.png";

		private const string EmblemTexture = "logo_90.png";

		private const string LogoTexture = "logo_highres.png";

		private const string LogoTextTexture = "logo_text.png";

		private const string SmallWindowBackgroundTexture = "window_background.png";

		private const string TwitchIconTextureName = "icon_twitch.png";

		private const string TwitchBigTexture = "icon_twitch_large.png";

		private const string PauseIconTexture = "icon_pause.png";

		private const string WaypointIconTexture = "icon_waypoint.png";

		private const string DeleteIconTexture = "icon_delete.png";

		private const string ExportIconTexture = "icon_export.png";

		private const string ImportIconTexture = "icon_import.png";

		private const string YoutubeIconTexture = "icon_youtube.png";

		private const string VlcIconTexture = "vlc-icon.png";

		private const string TvSideTexture = "tv_frame_side.png";

		private const string TvTopBottomTexture = "tv_frame_topbottom.png";

		private const string TvBackTexture = "tv_frame_back.png";

		private const string TvScreenOffTexture = "tv_screen_off.png";

		private const string SeekBarBackgroundTexture = "155208_background.png";

		private const string ChatBackgroundTexture = "window_background_chat.png";

		public TextureService(string cacheDirectory)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(10.0));
			_httpClient = val;
			string imageCacheDir = Path.Combine(cacheDirectory, "images");
			_imageCache = new ImageCacheService(imageCacheDir, _httpClient);
		}

		public AsyncTexture2D GetCornerIcon()
		{
			return GetTexture("logo_64.png");
		}

		public AsyncTexture2D GetEmblem()
		{
			return GetTexture("logo_90.png");
		}

		public AsyncTexture2D GetLogo()
		{
			return GetTexture("logo_highres.png");
		}

		public AsyncTexture2D GetLogoText()
		{
			return GetTexture("logo_text.png");
		}

		public AsyncTexture2D GetSmallWindowBackground()
		{
			return GetTexture("window_background.png");
		}

		public AsyncTexture2D GetTwitchIcon()
		{
			return GetTexture("icon_twitch.png");
		}

		public AsyncTexture2D GetTwitchBigIcon()
		{
			return GetTexture("icon_twitch_large.png");
		}

		public AsyncTexture2D GetPauseIcon()
		{
			return GetTexture("icon_pause.png");
		}

		public AsyncTexture2D GetDeleteIcon()
		{
			return GetTexture("icon_delete.png");
		}

		public AsyncTexture2D GetExportIcon()
		{
			return GetTexture("icon_export.png");
		}

		public AsyncTexture2D GetImportIcon()
		{
			return GetTexture("icon_import.png");
		}

		public AsyncTexture2D GetYoutubeIcon()
		{
			return GetTexture("icon_youtube.png");
		}

		public AsyncTexture2D GetVlcIcon()
		{
			return GetTexture("vlc-icon.png");
		}

		public AsyncTexture2D GetDefaultAvatar()
		{
			return GetTexture("logo_64.png");
		}

		public AsyncTexture2D GetTvSide()
		{
			return GetTexture("tv_frame_side.png");
		}

		public AsyncTexture2D GetTvTopBottom()
		{
			return GetTexture("tv_frame_topbottom.png");
		}

		public AsyncTexture2D GetTvBack()
		{
			return GetTexture("tv_frame_back.png");
		}

		public AsyncTexture2D GetTvScreenOff()
		{
			return GetTexture("tv_screen_off.png");
		}

		public AsyncTexture2D GetChatBackground()
		{
			return GetTexture("window_background_chat.png");
		}

		public AsyncTexture2D GetPlayIcon()
		{
			return GetAssetTexture(156998);
		}

		public AsyncTexture2D GetVolumeNotMutedIcon()
		{
			return GetAssetTexture(156738);
		}

		public AsyncTexture2D GetVolumeMutedIcon()
		{
			return GetAssetTexture(156739);
		}

		public AsyncTexture2D GetSettingsIcon()
		{
			return GetAssetTexture(155052);
		}

		public AsyncTexture2D GetSettingsBackground()
		{
			return GetAssetTexture(965776);
		}

		public AsyncTexture2D GetTwitchChatIcon()
		{
			return GetAssetTexture(155156);
		}

		public AsyncTexture2D GetCloseIcon()
		{
			return GetAssetTexture(255443);
		}

		public AsyncTexture2D GetQualityIcon()
		{
			return GetAssetTexture(440023);
		}

		public AsyncTexture2D GetVolumeBackground()
		{
			return GetAssetTexture(155208);
		}

		public AsyncTexture2D GetSeekBarBackground()
		{
			return GetTexture("155208_background.png");
		}

		public AsyncTexture2D GetResizeCorner()
		{
			return GetAssetTexture(156009);
		}

		public AsyncTexture2D GetResizeCornerActive()
		{
			return GetAssetTexture(156010);
		}

		public AsyncTexture2D GetLockIcon()
		{
			return GetAssetTexture(733265);
		}

		public AsyncTexture2D GetLockActiveIcon()
		{
			return GetAssetTexture(733266);
		}

		public AsyncTexture2D GetDisplayIcon()
		{
			return GetAssetTexture(358406);
		}

		public AsyncTexture2D GetSourceIcon()
		{
			return GetAssetTexture(156909);
		}

		public AsyncTexture2D GetCopyIcon()
		{
			return GetAssetTexture(2208347);
		}

		public AsyncTexture2D GetCardBackground()
		{
			return GetAssetTexture(154960);
		}

		public AsyncTexture2D GetWindowTexture()
		{
			return GetAssetTexture(155997);
		}

		public AsyncTexture2D GetSetScreenIcon()
		{
			return GetAssetTexture(528726);
		}

		public AsyncTexture2D GetWaypointIcon()
		{
			return GetAssetTexture(156628);
		}

		public AsyncTexture2D GetInfoIcon()
		{
			return GetAssetTexture(1508665);
		}

		public AsyncTexture2D GetRefreshIcon()
		{
			return GetAssetTexture(156749);
		}

		public AsyncTexture2D GetWatchPartyIcon()
		{
			return GetAssetTexture(156694);
		}

		public AsyncTexture2D GetArrowUpIcon()
		{
			return GetAssetTexture(102617);
		}

		public AsyncTexture2D GetArrowDownIcon()
		{
			return GetAssetTexture(102618);
		}

		public AsyncTexture2D GetTabbedWindowBackground()
		{
			return GetAssetTexture(155985);
		}

		public AsyncTexture2D GetMenuItemFade()
		{
			return GetAssetTexture(156044);
		}

		public async Task<AsyncTexture2D> GetImageFromUrlAsync(string cacheKey, string imageUrl)
		{
			return await _imageCache.GetImageAsync(cacheKey, imageUrl);
		}

		public async Task<AsyncTexture2D> GetYouTubeThumbnailAsync(string videoIdOrUrl)
		{
			if (string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return null;
			}
			string videoId = YouTubeService.ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
			string thumbnailUrl = "https://img.youtube.com/vi/" + videoId + "/hqdefault.jpg";
			return await GetImageFromUrlAsync("youtube_thumb_" + videoId, thumbnailUrl);
		}

		public async Task<AsyncTexture2D> GetTwitchAvatarAsync(string channelName, string avatarUrl)
		{
			if (string.IsNullOrWhiteSpace(channelName) || string.IsNullOrWhiteSpace(avatarUrl))
			{
				return null;
			}
			return await GetImageFromUrlAsync("twitch_avatar_" + channelName, avatarUrl);
		}

		public async Task<AsyncTexture2D> GetPresetImageAsync(string cacheKey, string imageUrl)
		{
			if (string.IsNullOrWhiteSpace(cacheKey) || string.IsNullOrWhiteSpace(imageUrl))
			{
				return null;
			}
			return await GetImageFromUrlAsync("preset_" + cacheKey, imageUrl);
		}

		public void UpdateCachedStreamInfo(TwitchStreamInfo streamInfo)
		{
			_cachedTwitchStreamInfo = streamInfo;
		}

		public void ClearCachedStreamInfo()
		{
			_cachedTwitchStreamInfo = null;
		}

		public async Task<Texture2D> LoadOfflineTextureAsync(CinemaUserSettings userSettings, TwitchService twitchService)
		{
			if (userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				return await LoadTwitchAvatarTextureAsync(userSettings, twitchService);
			}
			if (userSettings.CurrentStreamSourceType == StreamSourceType.YouTubeVideo)
			{
				return await LoadYouTubeThumbnailTextureAsync(userSettings);
			}
			return await LoadStaticImageTextureAsync(userSettings);
		}

		private async Task<Texture2D> LoadTwitchAvatarTextureAsync(CinemaUserSettings userSettings, TwitchService twitchService)
		{
			string channelName = userSettings.CurrentTwitchChannel;
			if (string.IsNullOrEmpty(channelName))
			{
				return null;
			}
			TwitchStreamInfo twitchStreamInfo = _cachedTwitchStreamInfo;
			if (twitchStreamInfo == null)
			{
				twitchStreamInfo = await twitchService.GetStreamInfoAsync(channelName);
			}
			TwitchStreamInfo streamInfo = twitchStreamInfo;
			if (streamInfo == null || string.IsNullOrEmpty(streamInfo.AvatarUrl))
			{
				return null;
			}
			AsyncTexture2D obj = await GetTwitchAvatarAsync(channelName, streamInfo.AvatarUrl);
			return (obj != null) ? obj.get_Texture() : null;
		}

		private async Task<Texture2D> LoadYouTubeThumbnailTextureAsync(CinemaUserSettings userSettings)
		{
			string videoId = userSettings.CurrentYouTubeVideo;
			if (string.IsNullOrEmpty(videoId))
			{
				return null;
			}
			AsyncTexture2D obj = await GetYouTubeThumbnailAsync(videoId);
			return (obj != null) ? obj.get_Texture() : null;
		}

		private async Task<Texture2D> LoadStaticImageTextureAsync(CinemaUserSettings userSettings)
		{
			StreamPresetData preset = userSettings.CurrentStreamPreset;
			if (preset == null || string.IsNullOrEmpty(preset.StaticImage))
			{
				return null;
			}
			AsyncTexture2D asyncTexture = await GetImageFromUrlAsync("offline_static_" + preset.Id, preset.StaticImage);
			if (asyncTexture != null)
			{
				preset.StaticImageTexture = asyncTexture;
			}
			return (asyncTexture != null) ? asyncTexture.get_Texture() : null;
		}

		public Texture2D GetWhitePixel()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (_whitePixel == null || ((GraphicsResource)_whitePixel).get_IsDisposed())
			{
				GraphicsDeviceContext graphicsContext = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_whitePixel = new Texture2D(((GraphicsDeviceContext)(ref graphicsContext)).get_GraphicsDevice(), 1, 1);
					_whitePixel.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
				}
				finally
				{
					((GraphicsDeviceContext)(ref graphicsContext)).Dispose();
				}
			}
			return _whitePixel;
		}

		public bool IsTextureReady(AsyncTexture2D texture)
		{
			if (texture != null)
			{
				return !texture.get_IsDisposed();
			}
			return false;
		}

		public bool IsTextureReady(Texture2D texture)
		{
			if (texture != null)
			{
				return !((GraphicsResource)texture).get_IsDisposed();
			}
			return false;
		}

		public void Dispose()
		{
			Texture2D whitePixel = _whitePixel;
			if (whitePixel != null)
			{
				((GraphicsResource)whitePixel).Dispose();
			}
			_whitePixel = null;
			_fallbackTexture = null;
			_imageCache?.Dispose();
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}

		private AsyncTexture2D GetFallbackTexture()
		{
			if (_fallbackTexture == null || _fallbackTexture.get_IsDisposed())
			{
				_fallbackTexture = AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			return _fallbackTexture;
		}

		private AsyncTexture2D GetTexture(string textureName)
		{
			try
			{
				return AsyncTexture2D.op_Implicit(CinemaModule.Instance.ContentsManager.GetTexture(textureName));
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to load texture '" + textureName + "': " + ex.Message);
				return GetFallbackTexture();
			}
		}

		private AsyncTexture2D GetAssetTexture(int assetId)
		{
			try
			{
				return AsyncTexture2D.FromAssetId(assetId);
			}
			catch (Exception ex)
			{
				Logger.Debug($"Failed to load asset texture '{assetId}': {ex.Message}");
				return GetFallbackTexture();
			}
		}
	}
}
