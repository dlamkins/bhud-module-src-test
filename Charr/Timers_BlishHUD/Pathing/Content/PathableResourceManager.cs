using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Content
{
	public class PathableResourceManager : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<PathableResourceManager>();

		private readonly Dictionary<string, Texture2D> _textureCache;

		private readonly HashSet<string> _pendingTextureUse;

		private readonly HashSet<string> _pendingTextureRemoval;

		public IDataReader DataReader { get; }

		public PathableResourceManager(IDataReader dataReader)
		{
			DataReader = dataReader;
			_textureCache = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
			_pendingTextureUse = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_pendingTextureRemoval = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public void RunTextureDisposal()
		{
			if (_pendingTextureUse.Count == 0 && _pendingTextureRemoval.Count == 0)
			{
				return;
			}
			Logger.Debug("Running texture swap for pathables. {addCount} will be added and {removeCount} will be removed.", new object[2] { _pendingTextureUse.Count, _pendingTextureRemoval.Count });
			_pendingTextureRemoval.RemoveWhere((string t) => _pendingTextureUse.Contains(t));
			foreach (string textureKey in _pendingTextureRemoval)
			{
				if (_textureCache.TryGetValue(textureKey, out var texture) && texture != null)
				{
					((GraphicsResource)texture).Dispose();
				}
				_textureCache.Remove(textureKey);
			}
			_pendingTextureUse.Clear();
			_pendingTextureRemoval.Clear();
		}

		public void MarkTextureForDisposal(string texturePath)
		{
			if (texturePath != null)
			{
				_pendingTextureRemoval.Add(texturePath);
			}
		}

		public Texture2D LoadTexture(string texturePath)
		{
			return LoadTexture(texturePath, Textures.get_Error());
		}

		public Texture2D LoadTexture(string texturePath, Texture2D fallbackTexture)
		{
			_pendingTextureUse.Add(texturePath);
			if (!_textureCache.ContainsKey(texturePath))
			{
				using Stream textureStream = DataReader.GetFileStream(texturePath);
				if (textureStream == null)
				{
					Logger.Warn("Failed to load texture {dataReaderPath}.", new object[1] { DataReader.GetPathRepresentation(texturePath) });
					return fallbackTexture;
				}
				_textureCache.Add(texturePath, TextureUtil.FromStreamPremultiplied(GameService.Graphics.get_GraphicsDevice(), textureStream));
				Logger.Debug("Successfully loaded texture {dataReaderPath}.", new object[1] { DataReader.GetPathRepresentation(texturePath) });
			}
			return _textureCache[texturePath];
		}

		public void Dispose()
		{
			((IDisposable)DataReader)?.Dispose();
			foreach (KeyValuePair<string, Texture2D> item in _textureCache)
			{
				Texture2D value = item.Value;
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			_textureCache.Clear();
			_pendingTextureUse.Clear();
			_pendingTextureRemoval.Clear();
		}
	}
}
