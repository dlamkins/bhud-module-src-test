using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Services
{
	public class TexturesService : IDisposable
	{
		private readonly Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();

		private readonly ContentsManager _contentsManager;

		private bool _disposed;

		public TexturesService(ContentsManager contentsManager)
		{
			_contentsManager = contentsManager;
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			if (_loadedTextures.Count > 0)
			{
				((IEnumerable<IDisposable>)_loadedTextures.Select((KeyValuePair<string, Texture2D> e) => e.Value)).DisposeAll();
			}
			_loadedTextures.Clear();
		}

		public Texture2D GetTexture(string path, string key)
		{
			if (_loadedTextures.ContainsKey(key))
			{
				return _loadedTextures[key];
			}
			Texture2D texture = _contentsManager.GetTexture(path);
			_loadedTextures.Add(key, texture);
			return texture;
		}

		public Texture2D GetTexture(Bitmap bitmap, string key)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (_loadedTextures.ContainsKey(key))
			{
				return _loadedTextures[key];
			}
			GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				using MemoryStream s = new MemoryStream();
				bitmap.Save(s, ImageFormat.Png);
				Texture2D texture = Texture2D.FromStream(((GraphicsDeviceContext)(ref device)).get_GraphicsDevice(), (Stream)s);
				_loadedTextures.Add(key, texture);
				return texture;
			}
			finally
			{
				((GraphicsDeviceContext)(ref device)).Dispose();
			}
		}
	}
}
