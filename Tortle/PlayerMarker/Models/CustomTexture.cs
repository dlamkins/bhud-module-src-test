using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;
using Tortle.PlayerMarker.Util;

namespace Tortle.PlayerMarker.Models
{
	internal class CustomTexture : ModuleManagedTextureBase, ITexture, IDisposable
	{
		private readonly string _filePath;

		public CustomTexture(string filePath, string fileName)
		{
			base.Id = fileName;
			_filePath = filePath;
		}

		public AsyncTexture2D Get()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			if (_texture != null)
			{
				return _texture;
			}
			_texture = new AsyncTexture2D();
			TextureUtil.FromPathPremultipliedAsync(_filePath).ContinueWith(delegate(Task<Texture2D> textureResponse)
			{
				Texture2D val = Textures.get_Error();
				if (textureResponse.Exception == null)
				{
					val = textureResponse.Result;
				}
				_texture.SwapTexture(val);
			});
			return _texture;
		}
	}
}
