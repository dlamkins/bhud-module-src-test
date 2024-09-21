using System.Collections.Generic;
using Blish_HUD.Content;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public interface ITextureRepository
	{
		TextureResources Textures { get; }

		void ClearTextures();

		AsyncTexture2D GetTexture(string url);

		AsyncTexture2D GetRefTexture(string fileName);

		AsyncTexture2D GetVendorIconTexture(string icon);

		AsyncTexture2D GetItemSourceBackgroundTexture(IList<IItemSource> itemSource);

		AsyncTexture2D GetRecipeSourceIcon(IItemSource itemSource);
	}
}
