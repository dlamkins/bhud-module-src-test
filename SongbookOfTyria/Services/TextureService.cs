using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using SongbookOfTyria.Models;

namespace SongbookOfTyria.Services
{
	public sealed class TextureService : IDisposable
	{
		private class PendingTexture
		{
			public byte[] ImageData { get; }

			public AsyncTexture2D AsyncTexture { get; }

			public string CacheFileName { get; }

			public PendingTexture(byte[] imageData, AsyncTexture2D asyncTexture, string cacheFileName)
			{
				ImageData = imageData;
				AsyncTexture = asyncTexture;
				CacheFileName = cacheFileName;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<TextureService>();

		private const int MaxBatchSize = 10;

		private const int HttpTimeoutSeconds = 30;

		private readonly ContentsManager _contentsManager;

		private readonly HttpClient _httpClient;

		private readonly string _cacheDirectory;

		private readonly ConcurrentDictionary<string, AsyncTexture2D> _remoteTextureCache;

		private readonly ConcurrentQueue<PendingTexture> _pendingTextures;

		private const string CornerIconTexture = "songbook64x64_icon.png";

		private const string EmblemTexture = "songbook100x.png";

		private const string LogoSmallTexture = "songbook64x64.png";

		private const string PauseIconTexture = "icon_pause.png";

		private const int WindowBackgroundAssetId = 155985;

		private const int AboutIconAssetId = 440023;

		private const int SongLibraryIconAssetId = 102357;

		private const int OpenWindowIconAssetId = 155910;

		private const int PlayIconAssetId = 156998;

		private const int VolumeNotMutedIconAssetId = 156738;

		private const int VolumeMutedIconAssetId = 156739;

		private const int FavoriteFilledAssetId = 102439;

		private const int FavoriteEmptyAssetId = 102440;

		public TextureService(ContentsManager contentsManager, string cacheDirectory)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			_contentsManager = contentsManager;
			_cacheDirectory = cacheDirectory;
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(30.0));
			_httpClient = val;
			_remoteTextureCache = new ConcurrentDictionary<string, AsyncTexture2D>();
			_pendingTextures = new ConcurrentQueue<PendingTexture>();
			Directory.CreateDirectory(_cacheDirectory);
		}

		public AsyncTexture2D GetCornerIcon()
		{
			return GetBundledTexture("songbook64x64_icon.png");
		}

		public AsyncTexture2D GetEmblem()
		{
			return GetBundledTexture("songbook100x.png");
		}

		public AsyncTexture2D GetLogoSmall()
		{
			return GetBundledTexture("songbook64x64.png");
		}

		private AsyncTexture2D GetBundledTexture(string textureName)
		{
			try
			{
				return AsyncTexture2D.op_Implicit(_contentsManager.GetTexture(textureName));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load bundled texture {TextureName}", new object[1] { textureName });
				return null;
			}
		}

		public AsyncTexture2D GetWindowBackground()
		{
			return GetAssetTexture(155985);
		}

		public AsyncTexture2D GetAboutIcon()
		{
			return GetAssetTexture(440023);
		}

		public AsyncTexture2D GetSongLibraryIcon()
		{
			return GetAssetTexture(102357);
		}

		public AsyncTexture2D GetOpenWindowIcon()
		{
			return GetAssetTexture(155910);
		}

		public AsyncTexture2D GetPlayIcon()
		{
			return GetAssetTexture(156998);
		}

		public AsyncTexture2D GetPauseIcon()
		{
			return GetBundledTexture("icon_pause.png");
		}

		public AsyncTexture2D GetVolumeIcon()
		{
			return GetAssetTexture(156738);
		}

		public AsyncTexture2D GetVolumeMutedIcon()
		{
			return GetAssetTexture(156739);
		}

		public AsyncTexture2D GetFavoriteFilledIcon()
		{
			return GetAssetTexture(102439);
		}

		public AsyncTexture2D GetFavoriteEmptyIcon()
		{
			return GetAssetTexture(102440);
		}

		private static AsyncTexture2D GetAssetTexture(int assetId)
		{
			try
			{
				return AsyncTexture2D.FromAssetId(assetId);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load asset texture {AssetId}", new object[1] { assetId });
				return null;
			}
		}

		public void PreloadThumbnails(IEnumerable<MusicTab> tabs)
		{
			if (tabs == null)
			{
				return;
			}
			Task.Run(async delegate
			{
				foreach (MusicTab tab in tabs)
				{
					if (!string.IsNullOrEmpty(tab.Thumbnail) && !tab.Thumbnail.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
					{
						await PreloadThumbnailToCacheAsync(tab.Thumbnail).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			});
		}

		private async Task PreloadThumbnailToCacheAsync(string url)
		{
			_ = 1;
			try
			{
				string fileName = GetCacheFileName(url);
				string filePath = Path.Combine(_cacheDirectory, fileName);
				if (!File.Exists(filePath))
				{
					byte[] imageData = await _httpClient.GetByteArrayAsync(url).ConfigureAwait(continueOnCapturedContext: false);
					Directory.CreateDirectory(_cacheDirectory);
					await Task.Run(delegate
					{
						File.WriteAllBytes(filePath, imageData);
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to preload thumbnail {Url}", new object[1] { url });
			}
		}

		public AsyncTexture2D GetRemoteTexture(string url)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			if (url.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			if (_remoteTextureCache.TryGetValue(url, out var cachedTexture))
			{
				return cachedTexture;
			}
			AsyncTexture2D asyncTexture = new AsyncTexture2D();
			_remoteTextureCache[url] = asyncTexture;
			Task.Run(() => DownloadTextureDataAsync(url, asyncTexture));
			return asyncTexture;
		}

		private async Task DownloadTextureDataAsync(string url, AsyncTexture2D asyncTexture)
		{
			_ = 2;
			try
			{
				string fileName = GetCacheFileName(url);
				string filePath = Path.Combine(_cacheDirectory, fileName);
				byte[] imageData;
				if (File.Exists(filePath))
				{
					imageData = await Task.Run(() => File.ReadAllBytes(filePath)).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					imageData = await _httpClient.GetByteArrayAsync(url).ConfigureAwait(continueOnCapturedContext: false);
					Directory.CreateDirectory(_cacheDirectory);
					await Task.Run(delegate
					{
						File.WriteAllBytes(filePath, imageData);
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				_pendingTextures.Enqueue(new PendingTexture(imageData, asyncTexture, fileName));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load remote texture {CacheFileName}", new object[1] { GetCacheFileName(url) });
			}
		}

		public void ProcessPendingTextures()
		{
			int processedCount = 0;
			PendingTexture pending;
			while (processedCount < 10 && _pendingTextures.TryDequeue(out pending))
			{
				try
				{
					using MemoryStream stream = new MemoryStream(pending.ImageData);
					Texture2D texture = TextureUtil.FromStreamPremultiplied((Stream)stream);
					pending.AsyncTexture.SwapTexture(texture);
					processedCount++;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to create texture {CacheFileName}", new object[1] { pending.CacheFileName });
				}
			}
		}

		private static string GetCacheFileName(string url)
		{
			string fileName = Path.GetFileName(new Uri(url).LocalPath);
			return url.GetHashCode().ToString("X8") + "_" + fileName;
		}

		public async Task DownloadFileAsync(string url, string targetPath)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			try
			{
				if (!File.Exists(targetPath))
				{
					byte[] data = await _httpClient.GetByteArrayAsync(url).ConfigureAwait(continueOnCapturedContext: false);
					string directory = Path.GetDirectoryName(targetPath);
					if (!string.IsNullOrEmpty(directory))
					{
						Directory.CreateDirectory(directory);
					}
					await Task.Run(delegate
					{
						File.WriteAllBytes(targetPath, data);
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to download file from {Url}", new object[1] { url });
			}
		}

		public void Dispose()
		{
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
			_remoteTextureCache.Clear();
		}
	}
}
