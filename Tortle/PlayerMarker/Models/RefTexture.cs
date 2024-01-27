using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Tortle.PlayerMarker.Models
{
	internal class RefTexture : ModuleManagedTextureBase, ITexture, IDisposable
	{
		private readonly ContentsManager _contentsManager;

		public RefTexture(ContentsManager contentsManager, string fileName)
		{
			_contentsManager = contentsManager;
			base.Id = fileName;
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
			Task.Run(() => _contentsManager.GetTexture(base.Id)).ContinueWith(delegate(Task<Texture2D> textureResponse)
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
