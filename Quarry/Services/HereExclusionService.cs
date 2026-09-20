using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Quarry.Interfaces;
using Quarry.Models.Persistence;

namespace Quarry.Services
{
	public class HereExclusionService : IHereExclusionService
	{
		private readonly Logger logger;

		private readonly object stateLock = new object();

		private readonly HashSet<int> hidden = new HashSet<int>();

		private readonly Dictionary<int, DateTime> snoozedUntilUtc = new Dictionary<int, DateTime>();

		public IReadOnlyCollection<int> HiddenAchievementIds
		{
			get
			{
				lock (stateLock)
				{
					return hidden.ToList();
				}
			}
		}

		public IReadOnlyDictionary<int, DateTime> SnoozedUntilUtc
		{
			get
			{
				lock (stateLock)
				{
					PruneExpired(DateTime.UtcNow);
					return snoozedUntilUtc.ToDictionary((KeyValuePair<int, DateTime> kv) => kv.Key, (KeyValuePair<int, DateTime> kv) => kv.Value);
				}
			}
		}

		public int TotalExcludedCount
		{
			get
			{
				lock (stateLock)
				{
					PruneExpired(DateTime.UtcNow);
					return hidden.Count + snoozedUntilUtc.Count;
				}
			}
		}

		public event Action Changed;

		public HereExclusionService(Logger logger)
		{
			this.logger = logger;
		}

		public void Hide(int achievementId)
		{
			lock (stateLock)
			{
				snoozedUntilUtc.Remove(achievementId);
				if (!hidden.Add(achievementId))
				{
					return;
				}
			}
			logger.Debug($"Here: hiding achievement {achievementId} (Not interested).");
			this.Changed?.Invoke();
		}

		public void Snooze(int achievementId)
		{
			DateTime until = DateTime.UtcNow.Date.AddDays(1.0);
			lock (stateLock)
			{
				hidden.Remove(achievementId);
				snoozedUntilUtc[achievementId] = until;
			}
			logger.Debug($"Here: snoozing achievement {achievementId} until {until:o} (Not today).");
			this.Changed?.Invoke();
		}

		public void Unhide(int achievementId)
		{
			lock (stateLock)
			{
				if (!(hidden.Remove(achievementId) | snoozedUntilUtc.Remove(achievementId)))
				{
					return;
				}
			}
			logger.Debug($"Here: un-hiding achievement {achievementId}.");
			this.Changed?.Invoke();
		}

		public bool IsExcluded(int achievementId, DateTime nowUtc)
		{
			lock (stateLock)
			{
				if (hidden.Contains(achievementId))
				{
					return true;
				}
				if (snoozedUntilUtc.TryGetValue(achievementId, out var until))
				{
					if (until > nowUtc)
					{
						return true;
					}
					snoozedUntilUtc.Remove(achievementId);
				}
				return false;
			}
		}

		public bool IsSnoozed(int achievementId, DateTime nowUtc)
		{
			lock (stateLock)
			{
				DateTime until;
				return snoozedUntilUtc.TryGetValue(achievementId, out until) && until > nowUtc;
			}
		}

		public void Load(IPersistenceService persistenceService)
		{
			Storage storage = persistenceService.Get();
			lock (stateLock)
			{
				foreach (int achievementId in storage.HiddenAchievements)
				{
					hidden.Add(achievementId);
				}
				foreach (KeyValuePair<int, DateTime> entry in storage.SnoozedAchievements)
				{
					snoozedUntilUtc[entry.Key] = entry.Value;
				}
				PruneExpired(DateTime.UtcNow);
				logger.Info($"HereExclusionService: loaded {hidden.Count} hidden and {snoozedUntilUtc.Count} snoozed achievement(s).");
			}
		}

		private void PruneExpired(DateTime nowUtc)
		{
			foreach (int achievementId in (from kv in snoozedUntilUtc
				where kv.Value <= nowUtc
				select kv.Key).ToList())
			{
				snoozedUntilUtc.Remove(achievementId);
			}
		}
	}
}
