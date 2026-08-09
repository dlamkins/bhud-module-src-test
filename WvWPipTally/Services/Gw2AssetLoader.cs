using System.Threading.Tasks;
using Blish_HUD.Content;

namespace WvWPipTally.Services
{
	public static class Gw2AssetLoader
	{
		public static async Task<AsyncTexture2D> LoadAsync(int assetId, int minWidth = 64)
		{
			AsyncTexture2D tex = AsyncTexture2D.FromAssetId(assetId);
			for (int i = 0; i < 80; i++)
			{
				if (tex.get_HasTexture() && tex.get_Width() >= minWidth)
				{
					return tex;
				}
				await Task.Delay(50).ConfigureAwait(continueOnCapturedContext: true);
			}
			return tex;
		}
	}
}
