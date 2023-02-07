using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Services
{
	public class TexturesService
	{
		private readonly Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();

		private readonly ContentsManager _contentsManager;

		public TexturesService(ContentsManager contentsManager)
		{
			_contentsManager = contentsManager;
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
