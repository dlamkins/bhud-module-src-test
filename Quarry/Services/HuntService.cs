using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Settings;
using Quarry.Interfaces;
using Quarry.Models.Markers;

namespace Quarry.Services
{
	public class HuntService : IHuntService, IDisposable
	{
		private readonly IPathingBridge pathingBridge;

		private readonly IMarkerPackIndexService markerPackIndexService;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly ICurrentMapService currentMapService;

		private readonly SettingEntry<bool> huntMode;

		private readonly Logger logger;

		private readonly object stateLock = new object();

		private readonly Dictionary<int, List<string>> enabledNamespacesByAchievementId = new Dictionary<int, List<string>>();

		private readonly Dictionary<int, List<string>> peekNamespacesByAchievementId = new Dictionary<int, List<string>>();

		public IReadOnlyDictionary<int, List<string>> HuntEnabledNamespaces
		{
			get
			{
				lock (stateLock)
				{
					return enabledNamespacesByAchievementId.ToDictionary((KeyValuePair<int, List<string>> kv) => kv.Key, (KeyValuePair<int, List<string>> kv) => new List<string>(kv.Value));
				}
			}
		}

		public bool CanPeek
		{
			get
			{
				if (huntMode.get_Value())
				{
					return pathingBridge.IsAvailable;
				}
				return false;
			}
		}

		public HuntService(IPathingBridge pathingBridge, IMarkerPackIndexService markerPackIndexService, IAchievementTrackerService achievementTrackerService, ICurrentMapService currentMapService, SettingEntry<bool> huntMode, Logger logger)
		{
			this.pathingBridge = pathingBridge;
			this.markerPackIndexService = markerPackIndexService;
			this.achievementTrackerService = achievementTrackerService;
			this.currentMapService = currentMapService;
			this.huntMode = huntMode;
			this.logger = logger;
			this.achievementTrackerService.AchievementTracked += OnAchievementTracked;
			this.achievementTrackerService.AchievementUntracked += OnAchievementUntracked;
			this.currentMapService.Changed += OnMapChanged;
			this.huntMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHuntModeChanged);
		}

		public void Load(IPersistenceService persistenceService)
		{
			lock (stateLock)
			{
				foreach (KeyValuePair<int, List<string>> entry in persistenceService.Get().HuntEnabledNamespaces)
				{
					enabledNamespacesByAchievementId[entry.Key] = new List<string>(entry.Value);
				}
			}
		}

		public void Peek(int achievementId)
		{
			if (!CanPeek)
			{
				return;
			}
			lock (stateLock)
			{
				if (!enabledNamespacesByAchievementId.ContainsKey(achievementId))
				{
					ApplyNamespaces(achievementId, peekNamespacesByAchievementId);
				}
			}
		}

		public void RevertForCompletion(int achievementId)
		{
			lock (stateLock)
			{
				RevertForAchievement(achievementId, enabledNamespacesByAchievementId);
				RevertForAchievement(achievementId, peekNamespacesByAchievementId);
			}
		}

		public void RevertAllForUnload()
		{
			lock (stateLock)
			{
				RevertAll();
			}
		}

		private void RevertAll()
		{
			foreach (int id2 in enabledNamespacesByAchievementId.Keys.ToList())
			{
				RevertForAchievement(id2, enabledNamespacesByAchievementId);
			}
			foreach (int id in peekNamespacesByAchievementId.Keys.ToList())
			{
				RevertForAchievement(id, peekNamespacesByAchievementId);
			}
		}

		private void OnAchievementTracked(int achievementId)
		{
			if (huntMode.get_Value())
			{
				lock (stateLock)
				{
					ApplyForAchievement(achievementId);
				}
			}
		}

		private void OnAchievementUntracked(int achievementId)
		{
			lock (stateLock)
			{
				RevertForAchievement(achievementId, enabledNamespacesByAchievementId);
			}
		}

		private void OnHuntModeChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			lock (stateLock)
			{
				RetryPendingReverts();
				if (e.get_NewValue())
				{
					foreach (int id in achievementTrackerService.ActiveAchievements.ToList())
					{
						ApplyForAchievement(id);
					}
				}
				else
				{
					RevertAll();
				}
			}
		}

		private void OnMapChanged()
		{
			lock (stateLock)
			{
				RetryPendingReverts();
				foreach (int id in peekNamespacesByAchievementId.Keys.ToList())
				{
					RevertForAchievement(id, peekNamespacesByAchievementId);
				}
			}
		}

		private void ApplyForAchievement(int achievementId)
		{
			if (pathingBridge.IsAvailable)
			{
				if (peekNamespacesByAchievementId.TryGetValue(achievementId, out var peeked))
				{
					enabledNamespacesByAchievementId[achievementId] = peeked;
					peekNamespacesByAchievementId.Remove(achievementId);
				}
				else
				{
					ApplyNamespaces(achievementId, enabledNamespacesByAchievementId);
				}
			}
		}

		private void ApplyNamespaces(int achievementId, Dictionary<int, List<string>> target)
		{
			if (!markerPackIndexService.TryGet(achievementId, out var route))
			{
				return;
			}
			List<string> flipped = new List<string>();
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string item in (from o in route.Objectives
				select o.Namespace into ns
				where !string.IsNullOrEmpty(ns)
				select ns).Distinct(StringComparer.OrdinalIgnoreCase))
			{
				foreach (string ns2 in WalkAncestors(item))
				{
					if (seen.Add(ns2) && pathingBridge.TryGetInactive(ns2, out var inactive) && inactive && pathingBridge.TrySetInactive(ns2, inactive: false))
					{
						flipped.Add(ns2);
					}
				}
			}
			if (flipped.Count != 0)
			{
				target[achievementId] = flipped;
				logger.Debug(string.Format("Hunt: on {0} — enabled {1} categories ({2}{3}).", achievementId, flipped.Count, string.Join(", ", flipped.Take(3)), (flipped.Count > 3) ? "…" : ""));
			}
		}

		private void RevertForAchievement(int achievementId, Dictionary<int, List<string>> source)
		{
			if (!source.TryGetValue(achievementId, out var namespaces))
			{
				return;
			}
			List<string> stillPending = new List<string>();
			foreach (string ns in namespaces)
			{
				if (!IsNamespaceStillNeeded(ns, achievementId) && !pathingBridge.TrySetInactive(ns, inactive: true))
				{
					stillPending.Add(ns);
				}
			}
			if (stillPending.Count == 0)
			{
				source.Remove(achievementId);
				return;
			}
			source[achievementId] = stillPending;
			logger.Debug($"Hunt: {stillPending.Count} categorie(s) for {achievementId} couldn't be turned off (Pathing unavailable); will retry.");
		}

		private void RetryPendingReverts()
		{
			if (!pathingBridge.IsAvailable)
			{
				return;
			}
			foreach (int id in enabledNamespacesByAchievementId.Keys.ToList())
			{
				if (!achievementTrackerService.IsBeingTracked(id))
				{
					RevertForAchievement(id, enabledNamespacesByAchievementId);
				}
			}
		}

		private bool IsNamespaceStillNeeded(string ns, int excludingAchievementId)
		{
			if (!ContainsNamespace(enabledNamespacesByAchievementId))
			{
				return ContainsNamespace(peekNamespacesByAchievementId);
			}
			return true;
			bool ContainsNamespace(Dictionary<int, List<string>> dict)
			{
				return dict.Any((KeyValuePair<int, List<string>> kv) => kv.Key != excludingAchievementId && kv.Value.Any((string x) => string.Equals(x, ns, StringComparison.OrdinalIgnoreCase)));
			}
		}

		[IteratorStateMachine(typeof(_003CWalkAncestors_003Ed__28))]
		private static IEnumerable<string> WalkAncestors(string ns)
		{
			return new _003CWalkAncestors_003Ed__28(-2)
			{
				_003C_003E3__ns = ns
			};
		}

		public void Dispose()
		{
			achievementTrackerService.AchievementTracked -= OnAchievementTracked;
			achievementTrackerService.AchievementUntracked -= OnAchievementUntracked;
			currentMapService.Changed -= OnMapChanged;
			huntMode.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHuntModeChanged);
		}
	}
}
