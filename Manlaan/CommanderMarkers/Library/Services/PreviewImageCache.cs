using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Blish_HUD;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class PreviewImageCache : IDisposable
	{
		private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

		private readonly HashSet<string> _thumbDownloadsInFlight = new HashSet<string>();

		private readonly HashSet<string> _previewDownloadsInFlight = new HashSet<string>();

		private readonly object _downloadLock = new object();

		private string _moduleDirectory = "";

		private string _serverUrl = "";

		public void SetModuleDirectory(string moduleDirectory)
		{
			_moduleDirectory = moduleDirectory;
		}

		public void SetServerUrl(string serverUrl)
		{
			_serverUrl = serverUrl;
		}

		public string? ThumbPathForSet(string communitySetId)
		{
			if (string.IsNullOrEmpty(communitySetId))
			{
				return null;
			}
			string path = GetThumbFilePath(communitySetId);
			if (!File.Exists(path))
			{
				return null;
			}
			return path;
		}

		public Texture2D? GetThumbTexture(string communitySetId, Texture2D fallback)
		{
			string path = ThumbPathForSet(communitySetId);
			if (path == null)
			{
				return null;
			}
			return LoadTexture(communitySetId, path);
		}

		public string? PreviewPathForSet(string communitySetId)
		{
			if (string.IsNullOrEmpty(communitySetId))
			{
				return null;
			}
			string path = GetPreviewFilePath(communitySetId);
			if (!File.Exists(path))
			{
				return null;
			}
			return path;
		}

		public Texture2D? GetPreviewTexture(string communitySetId)
		{
			string path = PreviewPathForSet(communitySetId);
			if (path == null)
			{
				return null;
			}
			return LoadTexture("preview:" + communitySetId, path);
		}

		private Texture2D? LoadTexture(string cacheKey, string path)
		{
			if (_textures.TryGetValue(cacheKey, out var cached))
			{
				return cached;
			}
			try
			{
				using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				Texture2D texture = TextureUtil.FromStreamPremultiplied((Stream)stream);
				_textures[cacheKey] = texture;
				return texture;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void RequestPreview(string communitySetId, string previewLargeUrl, Action<string>? onReady = null)
		{
			string communitySetId2 = communitySetId;
			Action<string> onReady2 = onReady;
			if (string.IsNullOrEmpty(communitySetId2))
			{
				return;
			}
			string existing = PreviewPathForSet(communitySetId2);
			if (existing != null)
			{
				NotifyReady(onReady2, existing);
				return;
			}
			lock (_downloadLock)
			{
				if (!_previewDownloadsInFlight.Add(communitySetId2))
				{
					return;
				}
			}
			string url = ResolvePreviewDownloadUrl(communitySetId2, previewLargeUrl);
			Task.Run(delegate
			{
				try
				{
					if (DownloadPreview(communitySetId2, url))
					{
						string text = PreviewPathForSet(communitySetId2);
						if (text != null)
						{
							NotifyReady(onReady2, text);
						}
					}
				}
				finally
				{
					lock (_downloadLock)
					{
						_previewDownloadsInFlight.Remove(communitySetId2);
					}
				}
			});
		}

		public void RequestThumb(string communitySetId, string previewThumbUrl, Action<string>? onReady = null)
		{
			string communitySetId2 = communitySetId;
			Action<string> onReady2 = onReady;
			if (string.IsNullOrEmpty(communitySetId2))
			{
				return;
			}
			string existing = ThumbPathForSet(communitySetId2);
			if (existing != null)
			{
				NotifyReady(onReady2, existing);
				return;
			}
			lock (_downloadLock)
			{
				if (!_thumbDownloadsInFlight.Add(communitySetId2))
				{
					return;
				}
			}
			string url = ResolveDownloadUrl(communitySetId2, previewThumbUrl);
			Task.Run(delegate
			{
				try
				{
					if (DownloadThumb(communitySetId2, url))
					{
						string text = ThumbPathForSet(communitySetId2);
						if (text != null)
						{
							NotifyReady(onReady2, text);
						}
					}
				}
				finally
				{
					lock (_downloadLock)
					{
						_thumbDownloadsInFlight.Remove(communitySetId2);
					}
				}
			});
		}

		private static void NotifyReady(Action<string>? onReady, string path)
		{
			Action<string> onReady2 = onReady;
			string path2 = path;
			if (onReady2 != null)
			{
				GameThreadUtil.Enqueue(delegate
				{
					onReady2(path2);
				});
			}
		}

		private string GetThumbFilePath(string communitySetId)
		{
			return Path.Combine(_moduleDirectory, "thumbs", communitySetId + ".png");
		}

		private string GetPreviewFilePath(string communitySetId)
		{
			return Path.Combine(_moduleDirectory, "previews", communitySetId + ".png");
		}

		private string ResolveDownloadUrl(string communitySetId, string previewThumbUrl)
		{
			if (!string.IsNullOrEmpty(previewThumbUrl))
			{
				if (previewThumbUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || previewThumbUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				{
					return previewThumbUrl;
				}
				string baseUrl = _serverUrl.TrimEnd('/');
				if (!previewThumbUrl.StartsWith("/"))
				{
					return baseUrl + "/" + previewThumbUrl;
				}
				return baseUrl + previewThumbUrl;
			}
			return _serverUrl.TrimEnd('/') + "/commander-markers/v1/sets/" + communitySetId + "/thumb.png";
		}

		private string ResolvePreviewDownloadUrl(string communitySetId, string previewLargeUrl)
		{
			if (!string.IsNullOrEmpty(previewLargeUrl))
			{
				if (previewLargeUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || previewLargeUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				{
					return previewLargeUrl;
				}
				string baseUrl = _serverUrl.TrimEnd('/');
				if (!previewLargeUrl.StartsWith("/"))
				{
					return baseUrl + "/" + previewLargeUrl;
				}
				return baseUrl + previewLargeUrl;
			}
			return _serverUrl.TrimEnd('/') + "/commander-markers/v1/sets/" + communitySetId + "/preview.png";
		}

		private bool DownloadThumb(string communitySetId, string url)
		{
			try
			{
				using WebClient client = ModuleHttp.CreateClient();
				byte[] bytes = client.DownloadData(url);
				if (bytes.Length == 0)
				{
					return false;
				}
				Directory.CreateDirectory(Path.Combine(_moduleDirectory, "thumbs"));
				File.WriteAllBytes(GetThumbFilePath(communitySetId), bytes);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool DownloadPreview(string communitySetId, string url)
		{
			try
			{
				using WebClient client = ModuleHttp.CreateClient();
				byte[] bytes = client.DownloadData(url);
				if (bytes.Length == 0)
				{
					return false;
				}
				Directory.CreateDirectory(Path.Combine(_moduleDirectory, "previews"));
				File.WriteAllBytes(GetPreviewFilePath(communitySetId), bytes);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Dispose()
		{
			foreach (Texture2D value in _textures.Values)
			{
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			_textures.Clear();
		}
	}
}
