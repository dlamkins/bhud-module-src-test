using Blish_HUD;
using Blish_HUD.Content;

namespace Tortle.PlayerMarker.Models
{
	internal abstract class ModuleManagedTextureBase
	{
		protected AsyncTexture2D _texture;

		public string Id { get; protected set; }

		public void Dispose()
		{
			AsyncTexture2D texture = _texture;
			if (((texture != null) ? texture.get_Texture() : null) != Textures.get_Error())
			{
				AsyncTexture2D texture2 = _texture;
				if (texture2 != null)
				{
					texture2.Dispose();
				}
			}
			_texture = null;
		}
	}
}
