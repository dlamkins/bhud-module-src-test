using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Debug;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Tortle.PlayerMarker.Models;

namespace Tortle.PlayerMarker.Services
{
	internal class TextureCache : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TextureCache));

		private readonly Dictionary<string, TextureInfo> _textures = new Dictionary<string, TextureInfo>();

		private readonly DirectoriesManager _directoriesManager;

		private readonly ContentsManager _contentsManager;

		private readonly string[] _supportedFileFormats = new string[6] { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

		public TextureCache(DirectoriesManager directoriesManager, ContentsManager contentsManager)
		{
			_directoriesManager = directoriesManager;
			_contentsManager = contentsManager;
		}

		public async Task Load(IEnumerable<string> defaultImageFileNames)
		{
			string imageDirectory = Path.Combine(_directoriesManager.GetFullDirectoryPath("tortle"), "playermarkers");
			if (!Directory.Exists(imageDirectory))
			{
				try
				{
					Directory.CreateDirectory(imageDirectory);
				}
				catch (UnauthorizedAccessException e)
				{
					Logger.Error((Exception)e, "Unable to create {directory}", new object[1] { imageDirectory });
					Contingency.NotifyFileSaveAccessDenied(imageDirectory, "creating playermarkers directory", false);
					return;
				}
				catch (Exception e2)
				{
					Logger.Error(e2, "Unable to copy default markers into {directory}", new object[1] { imageDirectory });
					return;
				}
			}
			await CopyDefaultMarkerImages(defaultImageFileNames, imageDirectory);
			LoadMarkerImages(imageDirectory);
		}

		public Texture2D Get(string name)
		{
			if (!_textures.TryGetValue(name, out var textureInfo))
			{
				return null;
			}
			return textureInfo.Texture;
		}

		public bool ContainsKey(string name)
		{
			return _textures.ContainsKey(name);
		}

		public IEnumerable<string> GetNames()
		{
			return _textures.Keys;
		}

		public void Dispose()
		{
			Logger.Debug("Disposing {textureCount} entries", new object[1] { _textures.Count });
			foreach (KeyValuePair<string, TextureInfo> texture in _textures)
			{
				texture.Value.Dispose();
			}
		}

		private async Task CopyDefaultMarkerImages(IEnumerable<string> defaultImageFiles, string imageDirectory)
		{
			try
			{
				foreach (string fileName in defaultImageFiles)
				{
					using Stream fs = _contentsManager.GetFileStream(fileName);
					using FileStream wfs = File.Create(Path.Combine(imageDirectory, fileName));
					await fs.CopyToAsync(wfs);
				}
			}
			catch (UnauthorizedAccessException e2)
			{
				Logger.Error((Exception)e2, "Unable to access {directory}", new object[1] { imageDirectory });
				Contingency.NotifyFileSaveAccessDenied(imageDirectory, "copying default images", false);
			}
			catch (Exception e)
			{
				Logger.Error(e, "Unable to copy default markers into {directory}", new object[1] { imageDirectory });
			}
		}

		private void LoadMarkerImages(string imageDirectory)
		{
			try
			{
				string[] files = Directory.GetFiles(imageDirectory);
				foreach (string filePath in files)
				{
					if (_supportedFileFormats.Contains(Path.GetExtension(filePath)))
					{
						Add(Path.GetFileName(filePath), filePath);
					}
				}
			}
			catch (UnauthorizedAccessException e2)
			{
				Logger.Error((Exception)e2, "Unable to access markers in {directory}", new object[1] { imageDirectory });
				Contingency.NotifyFileSaveAccessDenied(imageDirectory, "listing images", false);
			}
			catch (Exception e)
			{
				Logger.Error(e, "Unable to load markers in {directory}", new object[1] { imageDirectory });
			}
		}

		private void Add(string name, string filePath)
		{
			if (!_textures.ContainsKey(name))
			{
				Logger.Debug("Adding {name} located at {filePath}", new object[2] { name, filePath });
				_textures.Add(name, new TextureInfo(filePath));
			}
		}
	}
}
