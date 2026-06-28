using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class DownloadTextureService : IDisposable
	{
		private readonly Dictionary<string, (AsyncTexture2D Texture, int RefCount)> _textureCache = new Dictionary<string, (AsyncTexture2D, int)>();

		private static HttpClient HttpClient => ModuleHttpClient.Instance;

		public void ResetDownloadCache()
		{
			new DirectoryInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) ?? "").EnumerateFiles().ToList().ForEach(delegate(FileInfo file)
			{
				file.Delete();
			});
			ScreenNotification.ShowNotification("Cache purge complete", (NotificationType)4, (Texture2D)null, 4);
		}

		public AsyncTexture2D GetDynamicTexture(string path)
		{
			if (_textureCache.TryGetValue(path, out var cached))
			{
				cached.Item2++;
				return cached.Item1;
			}
			AsyncTexture2D asyncTex = LoadTexture(path, Textures.get_Pixel());
			_textureCache[path] = (asyncTex, 1);
			return asyncTex;
		}

		public AsyncTexture2D GetDynamicTextureFromUrl(string url, string fileName)
		{
			if (_textureCache.TryGetValue(fileName, out var cached))
			{
				cached.Item2++;
				return cached.Item1;
			}
			AsyncTexture2D asyncTex = LoadTextureFromUrl(url, fileName, Textures.get_TransparentPixel());
			_textureCache[fileName] = (asyncTex, 1);
			return asyncTex;
		}

		public void ReleaseTexture(string key)
		{
			if (_textureCache.TryGetValue(key, out var cached))
			{
				cached.Item2--;
				if (cached.Item2 <= 0)
				{
					cached.Item1.Dispose();
					_textureCache.Remove(key);
				}
			}
		}

		public void ForceRefreshTexture(string fileName)
		{
			if (_textureCache.TryGetValue(fileName, out var cached))
			{
				cached.Item1.Dispose();
				_textureCache.Remove(fileName);
			}
			try
			{
				FileInfo fileInfo = GetFileInfo(fileName);
				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
			}
			catch (Exception)
			{
			}
		}

		public bool ValidateTextureCache(string fileName)
		{
			return false;
		}

		protected AsyncTexture2D LoadTexture(string fileName, Texture2D fallbackTexture)
		{
			FileInfo configFileInfo = GetFileInfo(fileName);
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (FileStream stream = new FileStream(configFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					return AsyncTexture2D.op_Implicit(TextureUtil.FromStreamPremultiplied((Stream)stream));
				}
			}
			return LoadTextureFromUrl(Module.STATIC_HOST_URL + "/" + fileName, fileName, fallbackTexture);
		}

		protected (bool, Texture2D?) TryGetFileFromDisk(string fileName)
		{
			try
			{
				FileInfo fileInfo = GetFileInfo(fileName);
				if (fileInfo.Exists)
				{
					using (FileStream stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						return (true, TextureUtil.FromStreamPremultiplied((Stream)stream));
					}
				}
			}
			catch (Exception)
			{
			}
			return (false, null);
		}

		protected AsyncTexture2D LoadTextureFromUrl(string url, string fileName, Texture2D fallbackTexture)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			string fileName2 = fileName;
			string url2 = url;
			AsyncTexture2D asyncTex = new AsyncTexture2D(fallbackTexture);
			Task.Run(async delegate
			{
				try
				{
					var (loaded, texture) = TryGetFileFromDisk(fileName2);
					if (loaded && texture != null)
					{
						asyncTex.SwapTexture(texture);
					}
					else if (await DownloadFileAsync(url2, fileName2))
					{
						var (loadedAfterDownload, textureAfterDownload) = TryGetFileFromDisk(fileName2);
						if (loadedAfterDownload && textureAfterDownload != null)
						{
							asyncTex.SwapTexture(textureAfterDownload);
						}
					}
				}
				catch (Exception)
				{
				}
			});
			return asyncTex;
		}

		private FileInfo GetFileInfo(string fileName)
		{
			try
			{
				return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + fileName);
			}
			catch (Exception)
			{
				return new FileInfo(fileName);
			}
		}

		public static string GetImageSavePath(string fileName)
		{
			return Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + fileName;
		}

		private async Task<bool> DownloadFileAsync(string url, string fileName)
		{
			_ = 1;
			try
			{
				Logger.GetLogger<Module>().Info("Downloading image file " + fileName + " from " + url);
				string savePath = GetImageSavePath(fileName);
				HttpResponseMessage response = await HttpClient.GetAsync(url, (HttpCompletionOption)1);
				try
				{
					response.EnsureSuccessStatusCode();
					using FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read);
					await response.get_Content().CopyToAsync((Stream)fileStream);
					Logger.GetLogger<Module>().Info("Downloading image file " + fileName + " Complete");
					return true;
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Could not download " + fileName + " from " + url);
			}
			return false;
		}

		public void Dispose()
		{
			foreach (var value in _textureCache.Values)
			{
				value.Texture.Dispose();
			}
			_textureCache.Clear();
		}
	}
}
