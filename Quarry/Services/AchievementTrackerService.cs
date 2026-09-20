using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;

namespace Quarry.Services
{
	public class AchievementTrackerService : IAchievementTrackerService
	{
		private const int MaxTrackedAchievements = 15;

		private readonly List<int> activeAchievements;

		private readonly Logger logger;

		private readonly SettingEntry<bool> limitAchievement;

		public IReadOnlyList<int> ActiveAchievements => activeAchievements.AsReadOnly();

		public int FreeSlots
		{
			get
			{
				if (!limitAchievement.get_Value())
				{
					return int.MaxValue;
				}
				return Math.Max(0, 15 - activeAchievements.Count);
			}
		}

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
			if (!limitAchievement.get_Value() || activeAchievements.Count < 15)
			{
				if (!activeAchievements.Contains(achievement))
				{
					activeAchievements.Add(achievement);
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						this.AchievementTracked?.Invoke(achievement);
					});
				}
				return true;
			}
			return false;
		}

		public bool IsBeingTracked(int achievement)
		{
			return activeAchievements.Contains(achievement);
		}

		public void RemoveAchievement(int achievement)
		{
			activeAchievements.Remove(achievement);
			this.AchievementUntracked?.Invoke(achievement);
		}

		public void Load(IPersistenceService persistenceService)
		{
			try
			{
				foreach (int item in persistenceService.Get().TrackedAchievements.Distinct())
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
