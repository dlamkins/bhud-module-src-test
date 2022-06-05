using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IExternalImageService
	{
		Task<string> GetDirectImageLink(string imagePath, CancellationToken cancellationToken = default(CancellationToken));

		AsyncTexture2D GetImage(string imageUrl);

		AsyncTexture2D GetImageFromIndirectLink(string imagePath);
	}
}
