using System;
using System.Collections.Generic;
using Blish_HUD;
using Denrage.AchievementTrackerModule.Interfaces;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementTrackerService : IAchievementTrackerService
	{
		private readonly List<int> activeAchievements;

		private readonly Logger logger;

		public IReadOnlyList<int> ActiveAchievements => activeAchievements.AsReadOnly();

		public event Action<int> AchievementTracked;

		public event Action<int> AchievementUntracked;

		public AchievementTrackerService(Logger logger)
		{
			activeAchievements = new List<int>();
			this.logger = logger;
		}

		public void TrackAchievement(int achievement)
		{
			activeAchievements.Add(achievement);
			this.AchievementTracked?.Invoke(achievement);
		}

		public void RemoveAchievement(int achievement)
		{
			activeAchievements.Remove(achievement);
			this.AchievementUntracked?.Invoke(achievement);
		}

		public void Load(IPersistanceService persistanceService)
		{
			try
			{
				foreach (int item in persistanceService.Get().TrackedAchievements)
				{
					activeAchievements.Add(item);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on restoring tracked achievements");
			}
		}
	}
}
