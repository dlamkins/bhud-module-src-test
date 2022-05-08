using System;
using Denrage.AchievementTrackerModule.Models.Achievement;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IAchievementDetailsWindowManager : IDisposable
	{
		event Action<int> WindowHidden;

		void CreateWindow(AchievementTableEntry achievement);

		void Update();

		bool WindowExists(int achievementId);
	}
}
