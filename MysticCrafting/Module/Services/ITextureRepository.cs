using Blish_HUD.Content;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public interface ITextureRepository
	{
		TextureResources Textures { get; }

		AsyncTexture2D GetTexture(string url);

		AsyncTexture2D GetRefTexture(string fileName);
	}
}
