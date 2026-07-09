using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace rp.spark.UI.Controls
{
	internal class AssetIcon : Panel
	{
		private AsyncTexture2D _texture;

		private EventHandler<ValueChangedEventArgs<Texture2D>> _textureSwappedHandler;

		public int AssetId { get; private set; }

		public void SetAssetId(int assetId)
		{
			ClearTexture();
			AssetId = assetId;
			if (assetId <= 0)
			{
				return;
			}
			_texture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(assetId);
			if (_texture != null)
			{
				((Panel)this).set_BackgroundTexture(AsyncTexture2D.op_Implicit(_texture.get_Texture()));
				_textureSwappedHandler = delegate(object s, ValueChangedEventArgs<Texture2D> e)
				{
					((Panel)this).set_BackgroundTexture(AsyncTexture2D.op_Implicit(e.get_NewValue()));
				};
				_texture.add_TextureSwapped(_textureSwappedHandler);
			}
		}

		private void ClearTexture()
		{
			if (_texture != null && _textureSwappedHandler != null)
			{
				_texture.remove_TextureSwapped(_textureSwappedHandler);
			}
			_texture = null;
			_textureSwappedHandler = null;
			((Panel)this).set_BackgroundTexture((AsyncTexture2D)null);
		}

		protected override void DisposeControl()
		{
			ClearTexture();
			((Panel)this).DisposeControl();
		}

		public AssetIcon()
			: this()
		{
		}
	}
}
