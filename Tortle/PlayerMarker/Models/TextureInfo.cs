using System;
using Blish_HUD;
using Microsoft.Xna.Framework.Graphics;
using Tortle.PlayerMarker.Util;

namespace Tortle.PlayerMarker.Models
{
	internal class TextureInfo : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TextureInfo));

		private readonly string _filePath;

		private Texture2D _texture;

		public Texture2D Texture
		{
			get
			{
				if (_texture != null)
				{
					return _texture;
				}
				Logger.Debug("Creating texture for {file}", new object[1] { _filePath });
				_texture = TextureUtil.FromPathPremultiplied(_filePath);
				return _texture;
			}
		}

		public TextureInfo(string filePath)
		{
			_filePath = filePath;
		}

		public void Dispose()
		{
			Logger.Debug("Disposing texture for {file}", new object[1] { _filePath });
			Texture2D texture = _texture;
			if (texture != null)
			{
				((GraphicsResource)texture).Dispose();
			}
		}
	}
}
