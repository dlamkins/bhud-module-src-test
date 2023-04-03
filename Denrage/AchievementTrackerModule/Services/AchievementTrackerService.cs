using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using Denrage.AchievementTrackerModule.Interfaces;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementTrackerService : IAchievementTrackerService
	{
		private readonly List<int> activeAchievements;

		private readonly Logger logger;

		private readonly SettingEntry<bool> limitAchievement;

		public IReadOnlyList<int> ActiveAchievements => activeAchievements.AsReadOnly();

		public event Action<int> AchievementTracked;

		public event Action<int> AchievementUntracked;

		public AchievementTrackerService(Logger logger, SettingEntry<bool> limitAchievement)
		{
			activeAchievements = new List<int>();
			this.logger = logger;
			this.limitAchievement = limitAchievement;
		}

		public bool TrackAchievement(int achievement)
		{
			if (!limitAchievement.get_Value() || activeAchievements.Count <= 15)
			{
				if (!activeAchievements.Contains(achievement))
				{
					activeAchievements.Add(achievement);
					this.AchievementTracked?.Invoke(achievement);
				}
				return true;
			}
			return false;
		}

		public bool IsBeingTracked(int achievement)
		{
			if (!limitAchievement.get_Value() || activeAchievements.Count <= 15)
			{
				return activeAchievements.Contains(achievement);
			}
			return false;
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
