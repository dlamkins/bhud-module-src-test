using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics;
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

		private const string CornerIconTexture = "cinemahudx64.png";

		private const string EmblemTexture = "cinemahudx90.png";

		private const string LogoTexture = "quaggantv_highres.png";

		private const string LogoTextTexture = "cinemahudtext.png";

		private const string SmallWindowBackgroundTexture = "bgwindow3.png";

		private const string TwitchIconTextureName = "twitchicon.png";

		private const string PauseIconTexture = "pause.png";

		private const string TvSideTexture = "tv_side.png";

		private const string TvTopBottomTexture = "tv_topbottom.png";

		private const string TvBackTexture = "tv_back.png";

		private const string TvScreenOffTexture = "tv_screenoff.png";

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
			return GetTexture("cinemahudx64.png");
		}

		public AsyncTexture2D GetEmblem()
		{
			return GetTexture("cinemahudx90.png");
		}

		public AsyncTexture2D GetLogo()
		{
			return GetTexture("quaggantv_highres.png");
		}

		public AsyncTexture2D GetLogoText()
		{
			return GetTexture("cinemahudtext.png");
		}

		public AsyncTexture2D GetSmallWindowBackground()
		{
			return GetTexture("bgwindow3.png");
		}

		public AsyncTexture2D GetTwitchIcon()
		{
			return GetTexture("twitchicon.png");
		}

		public AsyncTexture2D GetPauseIcon()
		{
			return GetTexture("pause.png");
		}

		public AsyncTexture2D GetDefaultAvatar()
		{
			return GetTexture("cinemahudx64.png");
		}

		public AsyncTexture2D GetTvSide()
		{
			return GetTexture("tv_side.png");
		}

		public AsyncTexture2D GetTvTopBottom()
		{
			return GetTexture("tv_topbottom.png");
		}

		public AsyncTexture2D GetTvBack()
		{
			return GetTexture("tv_back.png");
		}

		public AsyncTexture2D GetTvScreenOff()
		{
			return GetTexture("tv_screenoff.png");
		}

		public AsyncTexture2D GetPlayIcon()
		{
			return AsyncTexture2D.FromAssetId(156998);
		}

		public AsyncTexture2D GetVolumeNotMutedIcon()
		{
			return AsyncTexture2D.FromAssetId(156738);
		}

		public AsyncTexture2D GetVolumeMutedIcon()
		{
			return AsyncTexture2D.FromAssetId(156739);
		}

		public AsyncTexture2D GetSettingsIcon()
		{
			return AsyncTexture2D.FromAssetId(155052);
		}

		public AsyncTexture2D GetSettingsBackground()
		{
			return AsyncTexture2D.FromAssetId(965776);
		}

		public AsyncTexture2D GetTwitchChatIcon()
		{
			return AsyncTexture2D.FromAssetId(155156);
		}

		public AsyncTexture2D GetCloseIcon()
		{
			return AsyncTexture2D.FromAssetId(255443);
		}

		public AsyncTexture2D GetQualityIcon()
		{
			return AsyncTexture2D.FromAssetId(440023);
		}

		public AsyncTexture2D GetVolumeBackground()
		{
			return AsyncTexture2D.FromAssetId(155208);
		}

		public AsyncTexture2D GetResizeCorner()
		{
			return AsyncTexture2D.FromAssetId(156009);
		}

		public AsyncTexture2D GetResizeCornerActive()
		{
			return AsyncTexture2D.FromAssetId(156010);
		}

		public AsyncTexture2D GetDisplayIcon()
		{
			return AsyncTexture2D.FromAssetId(358406);
		}

		public AsyncTexture2D GetSourceIcon()
		{
			return AsyncTexture2D.FromAssetId(156909);
		}

		public AsyncTexture2D GetCopyIcon()
		{
			return AsyncTexture2D.FromAssetId(2208347);
		}

		public AsyncTexture2D GetImportIcon()
		{
			return AsyncTexture2D.FromAssetId(2208351);
		}

		public AsyncTexture2D GetCardBackground()
		{
			return AsyncTexture2D.FromAssetId(154960);
		}

		public AsyncTexture2D GetWindowTexture()
		{
			return AsyncTexture2D.FromAssetId(155997);
		}

		public async Task<AsyncTexture2D> GetImageFromUrlAsync(string cacheKey, string imageUrl)
		{
			return await _imageCache.GetImageAsync(cacheKey, imageUrl);
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
			_imageCache?.Dispose();
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
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
				return null;
			}
		}
	}
}
