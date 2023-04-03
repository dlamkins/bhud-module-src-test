using System;
using Blish_HUD.Content;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface ITextureService : IDisposable
	{
		AsyncTexture2D GetTexture(string url);

		AsyncTexture2D GetRefTexture(string fileName);
	}
}
