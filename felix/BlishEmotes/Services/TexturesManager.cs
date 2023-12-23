using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace felix.BlishEmotes.Services
{
	public class TexturesManager : IDisposable
	{
		public static string[] TextureExtensionMasks = new string[6] { "*.png", "*.jpg", "*.bmp", "*.gif", "*.tif", "*.dds" };

		private static readonly Logger Logger = Logger.GetLogger<PersistenceManager>();

		private bool _disposed;

		private Dictionary<string, Texture2D> _textureCache;

		public string ModuleDataTexturesDirectory { get; private set; }

		public TexturesManager(ContentsManager contentsManager, DirectoriesManager directoriesManager)
		{
			_textureCache = new Dictionary<string, Texture2D>
			{
				{
					Textures.ModuleIcon.ToString(),
					contentsManager.GetTexture("textures\\emotes_icon.png")
				},
				{
					Textures.Background.ToString(),
					contentsManager.GetTexture("textures\\156006.png")
				},
				{
					Textures.SettingsIcon.ToString(),
					contentsManager.GetTexture("textures\\102390.png")
				},
				{
					Textures.GlobalSettingsIcon.ToString(),
					contentsManager.GetTexture("textures\\155052.png")
				},
				{
					Textures.CategorySettingsIcon.ToString(),
					contentsManager.GetTexture("textures\\156909.png")
				},
				{
					Textures.HotkeySettingsIcon.ToString(),
					contentsManager.GetTexture("textures\\156734+155150.png")
				},
				{
					Textures.MissingTexture.ToString(),
					contentsManager.GetTexture("textures\\missing-texture.png")
				},
				{
					Textures.LockedTexture.ToString(),
					contentsManager.GetTexture("textures\\2107931.png")
				}
			};
			IReadOnlyList<string> registeredDirectories = directoriesManager.RegisteredDirectories;
			ModuleDataTexturesDirectory = Path.Combine(directoriesManager.GetFullDirectoryPath(registeredDirectories[0]), "textures");
			if (!Directory.Exists(ModuleDataTexturesDirectory))
			{
				Directory.CreateDirectory(ModuleDataTexturesDirectory);
			}
			foreach (string file in TextureExtensionMasks.SelectMany((string extension) => Directory.EnumerateFiles(ModuleDataTexturesDirectory, extension)))
			{
				FileStream fileStream = File.OpenRead(file);
				_textureCache.Add(Path.GetFileNameWithoutExtension(file), TextureUtil.FromStreamPremultiplied(fileStream));
			}
		}

		public void UpdateTexture(Textures textureRef, Texture2D newTexture)
		{
			UpdateTexture(textureRef.ToString(), newTexture);
		}

		public void UpdateTexture(string textureRef, Texture2D newTexture)
		{
			AssertAlive();
			if (!_textureCache.ContainsKey(textureRef))
			{
				Logger.Error("Failed to update texture - No texture found for " + textureRef);
				return;
			}
			_textureCache[textureRef]?.Dispose();
			_textureCache[textureRef] = newTexture;
		}

		public Texture2D GetTexture(Textures textureRef)
		{
			return GetTexture(textureRef.ToString());
		}

		public Texture2D GetTexture(string textureRef)
		{
			AssertAlive();
			return _textureCache[textureRef];
		}

		private void AssertAlive()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("TexturesManager was disposed!");
			}
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			foreach (Texture2D value in _textureCache.Values)
			{
				value.Dispose();
			}
			_textureCache.Clear();
		}
	}
}
