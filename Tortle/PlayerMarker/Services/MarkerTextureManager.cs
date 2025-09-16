using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Debug;
using Blish_HUD.Modules.Managers;
using Tortle.PlayerMarker.Models;

namespace Tortle.PlayerMarker.Services
{
	internal sealed class MarkerTextureManager : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(MarkerTextureManager));

		private readonly Dictionary<string, ITexture> _textures;

		private readonly List<string> _duplicateFilePaths;

		private readonly string _imageDirectory;

		private readonly string[] _supportedFileFormats = new string[6] { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

		public IEnumerable<ITexture> DefaultTextures { get; }

		public MarkerTextureManager(DirectoriesManager directoriesManager, ContentsManager contentsManager)
		{
			_imageDirectory = Path.Combine(directoriesManager.GetFullDirectoryPath("tortle"), "playermarkers");
			DefaultTextures = new ITexture[10]
			{
				new RefTexture(contentsManager, "gw2PersonalTarget.png"),
				new RefTexture(contentsManager, "circleFill.png"),
				new Gw2DatTexture(1335145, "gw2CommanderArrow.png"),
				new Gw2DatTexture(1335146, "gw2CommanderCircle.png"),
				new Gw2DatTexture(1335147, "gw2CommanderHeart.png"),
				new Gw2DatTexture(1335148, "gw2CommanderSquare.png"),
				new Gw2DatTexture(1335149, "gw2CommanderStar.png"),
				new Gw2DatTexture(1335150, "gw2CommanderSpiral.png"),
				new Gw2DatTexture(1335151, "gw2CommanderTriangle.png"),
				new Gw2DatTexture(1335152, "gw2CommanderX.png")
			};
			_textures = DefaultTextures.ToDictionary((ITexture a) => a.Id);
			_duplicateFilePaths = new List<string>();
		}

		public void Load()
		{
			if (!Directory.Exists(_imageDirectory))
			{
				try
				{
					Directory.CreateDirectory(_imageDirectory);
				}
				catch (UnauthorizedAccessException e2)
				{
					Logger.Error((Exception)e2, "Unable to create {directory}", new object[1] { _imageDirectory });
					Contingency.NotifyFileSaveAccessDenied(_imageDirectory, "creating playermarkers directory", false);
					return;
				}
				catch (Exception e)
				{
					Logger.Error(e, "Unable to copy default markers into {directory}", new object[1] { _imageDirectory });
					return;
				}
			}
			LoadCustomMarkers(_imageDirectory);
		}

		public ITexture Get(string key)
		{
			if (!_textures.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}

		public bool ContainsKey(string key)
		{
			return _textures.ContainsKey(key);
		}

		public IEnumerable<string> GetNames()
		{
			return _textures.Keys;
		}

		public int GetDuplicateCount()
		{
			return _duplicateFilePaths.Count;
		}

		public void CleanupDuplicates()
		{
			Logger.Info("Cleaning up {numberOfDuplicates} duplicates", new object[1] { _duplicateFilePaths.Count });
			for (int i = _duplicateFilePaths.Count - 1; i >= 0; i--)
			{
				if (TryDelete(_duplicateFilePaths[i]))
				{
					_duplicateFilePaths.RemoveAt(i);
				}
			}
		}

		public void Dispose()
		{
			Logger.Debug("Disposing {textureCount} entries", new object[1] { _textures.Count });
			foreach (KeyValuePair<string, ITexture> texture in _textures)
			{
				texture.Value.Dispose();
			}
		}

		private void LoadCustomMarkers(string imageDirectory)
		{
			try
			{
				string[] files = Directory.GetFiles(imageDirectory);
				foreach (string filePath in files)
				{
					if (_supportedFileFormats.Contains(Path.GetExtension(filePath)))
					{
						string fileName = Path.GetFileName(filePath);
						if (ContainsKey(fileName))
						{
							Logger.Info("Marking {filename} as a duplicate because it is already loaded as a Gw2DatTexture", new object[1] { fileName });
							_duplicateFilePaths.Add(filePath);
						}
						else
						{
							Logger.Debug("Adding {filePath}", new object[1] { filePath });
							_textures.Add(fileName, new CustomTexture(filePath, fileName));
						}
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

		private static bool TryDelete(string filePath)
		{
			try
			{
				Logger.Debug("Deleting {filePath}", new object[1] { filePath });
				File.Delete(filePath);
				return true;
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to delete {filePath}", new object[1] { filePath });
				return false;
			}
		}
	}
}
