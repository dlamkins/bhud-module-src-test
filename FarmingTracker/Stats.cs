using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class Stats
	{
		private readonly Dictionary<int, Stat> _statById = new Dictionary<int, Stat>();

		private readonly object _statsLock = new object();

		public Stat GetStat(int apiId, StatType statType)
		{
			int key = CreateKey(apiId, statType);
			lock (_statsLock)
			{
				return _statById[key];
			}
		}

		public List<Stat> GetStats()
		{
			lock (_statsLock)
			{
				return _statById.Values.ToList();
			}
		}

		public void ResetCounts()
		{
			foreach (Stat stat in GetStats())
			{
				stat.Signed_Count.Value = 0L;
			}
		}

		public void AddStat(Stat stat)
		{
			int key = CreateKey(stat);
			lock (_statsLock)
			{
				if (_statById.ContainsKey(key))
				{
					Module.Logger.Error("Cannot add stat to model because a stat with that key already exists");
				}
				else
				{
					_statById[key] = stat;
				}
			}
		}

		public void UpdateCountOrAddNewStat(Stat newStat)
		{
			int key = CreateKey(newStat);
			lock (_statsLock)
			{
				if (_statById.TryGetValue(key, out var stat))
				{
					stat.Signed_Count.Add(newStat.Signed_Count);
				}
				else
				{
					_statById[key] = newStat;
				}
			}
		}

		private static int CreateKey(Stat stat)
		{
			return CreateKey(stat.ApiId, stat.StatType);
		}

		private static int CreateKey(int apiId, StatType statType)
		{
			if (statType != 0)
			{
				return apiId;
			}
			return -apiId;
		}
	}
}
