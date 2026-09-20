using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class SessionSummaryService : ISessionSummaryService
	{
		private readonly IAchievementService achievementService;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly Logger logger;

		private readonly HashSet<int> everSeenDoneIds = new HashSet<int>();

		private readonly Dictionary<int, int> maxBitCountById = new Dictionary<int, int>();

		private bool hasBaseline;

		private readonly object baselineLock = new object();

		private readonly SemaphoreSlim diffGate = new SemaphoreSlim(1, 1);

		private readonly HashSet<int> completionFiredIds = new HashSet<int>();

		public SessionSummary Summary { get; private set; } = new SessionSummary();


		public event Action Changed;

		public event Action<int> AchievementCompleted;

		public SessionSummaryService(IAchievementService achievementService, IBitAlignmentService bitAlignmentService, IAchievementTrackerService achievementTrackerService, Logger logger)
		{
			this.achievementService = achievementService;
			this.bitAlignmentService = bitAlignmentService;
			this.achievementTrackerService = achievementTrackerService;
			this.logger = logger;
			this.achievementService.PlayerAchievementsLoaded += OnPlayerAchievementsLoaded;
		}

		public string GetSummaryLine()
		{
			List<string> parts = new List<string>();
			if (Summary.ApGained > 0)
			{
				parts.Add($"+{Summary.ApGained} AP");
			}
			if (Summary.CompletedAchievementNames.Count > 0)
			{
				parts.Add($"{Summary.CompletedAchievementNames.Count} completed");
			}
			if (Summary.BitsTicked > 0)
			{
				parts.Add($"{Summary.BitsTicked} steps");
			}
			if (parts.Count != 0)
			{
				return "This session: " + string.Join(" · ", parts);
			}
			return null;
		}

		private void OnPlayerAchievementsLoaded()
		{
			Dictionary<int, (int Current, bool Done, int BitCount)> current = BuildSnapshot();
			List<int> fireCompleted = new List<int>();
			bool runDiff = false;
			lock (baselineLock)
			{
				if (!hasBaseline)
				{
					if (achievementService.PlayerAchievements == null)
					{
						return;
					}
					foreach (KeyValuePair<int, (int, bool, int)> entry in current)
					{
						if (entry.Value.Item2)
						{
							everSeenDoneIds.Add(entry.Key);
							if (achievementTrackerService.IsBeingTracked(entry.Key))
							{
								fireCompleted.Add(entry.Key);
							}
						}
						maxBitCountById[entry.Key] = entry.Value.Item3;
					}
					hasBaseline = true;
				}
				else
				{
					fireCompleted.AddRange(SweepFinishedTrackedAchievements());
					runDiff = true;
				}
			}
			foreach (int id in fireCompleted)
			{
				this.AchievementCompleted?.Invoke(id);
			}
			if (runDiff)
			{
				Task.Run(() => DiffAndUpdateAsync(current));
			}
		}

		private List<int> SweepFinishedTrackedAchievements()
		{
			List<int> finished = new List<int>();
			foreach (int id in achievementTrackerService.ActiveAchievements.ToList())
			{
				if (!completionFiredIds.Contains(id) && achievementService.HasFinishedAchievement(id))
				{
					completionFiredIds.Add(id);
					finished.Add(id);
				}
			}
			return finished;
		}

		private Dictionary<int, (int Current, bool Done, int BitCount)> BuildSnapshot()
		{
			Dictionary<int, (int, bool, int)> result = new Dictionary<int, (int, bool, int)>();
			if (achievementService.PlayerAchievements != null)
			{
				foreach (AccountAchievement achievement in achievementService.PlayerAchievements)
				{
					result[achievement.get_Id()] = (achievement.get_Current(), achievementService.HasFinishedAchievement(achievement.get_Id()), achievement.get_Bits()?.Count ?? 0);
				}
				return result;
			}
			return result;
		}

		private async Task DiffAndUpdateAsync(Dictionary<int, (int Current, bool Done, int BitCount)> current)
		{
			await diffGate.WaitAsync();
			try
			{
				List<int> newlyDoneIds = new List<int>();
				int bitsTicked = 0;
				foreach (KeyValuePair<int, (int, bool, int)> entry in current)
				{
					int p;
					int previousMaxBitCount = (maxBitCountById.TryGetValue(entry.Key, out p) ? p : 0);
					if (!everSeenDoneIds.Contains(entry.Key) && entry.Value.Item2)
					{
						newlyDoneIds.Add(entry.Key);
						everSeenDoneIds.Add(entry.Key);
					}
					if (entry.Value.Item3 > previousMaxBitCount)
					{
						bitsTicked += entry.Value.Item3 - previousMaxBitCount;
						maxBitCountById[entry.Key] = entry.Value.Item3;
					}
				}
				if (newlyDoneIds.Count == 0 && bitsTicked == 0)
				{
					return;
				}
				int apGained = 0;
				List<string> completedNames = new List<string>();
				if (newlyDoneIds.Count > 0)
				{
					Dictionary<int, int> apiAchievements = await FetchAchievementPointsAsync(newlyDoneIds);
					foreach (int id in newlyDoneIds)
					{
						if (apiAchievements.TryGetValue(id, out var points))
						{
							apGained += points;
						}
						string name = achievementService.Achievements?.FirstOrDefault((AchievementTableEntry a) => a.Id == id)?.Name;
						completedNames.Add(name ?? $"#{id}");
					}
				}
				Summary = new SessionSummary
				{
					ApGained = Summary.ApGained + apGained,
					CompletedAchievementNames = Summary.CompletedAchievementNames.Concat(completedNames).ToList(),
					BitsTicked = Summary.BitsTicked + bitsTicked
				};
				this.Changed?.Invoke();
				foreach (int id2 in newlyDoneIds)
				{
					this.AchievementCompleted?.Invoke(id2);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Session summary: failed to update.");
			}
			finally
			{
				diffGate.Release();
			}
		}

		private async Task<Dictionary<int, int>> FetchAchievementPointsAsync(IReadOnlyList<int> ids)
		{
			return (await bitAlignmentService.GetAchievementsAsync(ids)).ToDictionary((KeyValuePair<int, Achievement> kv) => kv.Key, (KeyValuePair<int, Achievement> kv) => kv.Value.get_Tiers()?.Sum((AchievementTier t) => t.get_Points()) ?? 0);
		}
	}
}
