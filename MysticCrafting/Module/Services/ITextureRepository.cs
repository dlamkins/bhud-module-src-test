using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public interface ITextureRepository : IDisposable
	{
		TextureResources Textures { get; }

		AsyncTexture2D GetTexture(string url);

		AsyncTexture2D GetTexture(int id);

		AsyncTexture2D GetRefTexture(string fileName);

		AsyncTexture2D GetVendorIconTexture(string icon);

		AsyncTexture2D GetItemSourceBackgroundTexture(IList<IItemSource> itemSource);

		AsyncTexture2D GetRecipeSourceIcon(IItemSource itemSource);
	}
}
