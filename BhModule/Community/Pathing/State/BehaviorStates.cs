using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class BehaviorStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<BehaviorStates>();

		private const string STATE_FILE = "timers.txt";

		private const double INTERVAL_CHECKTIMERS = 5000.0;

		private const double INTERVAL_SAVESTATE = 10000.0;

		private readonly HashSet<Guid> _hiddenUntilMapChange = new HashSet<Guid>();

		private readonly HashSet<Guid> _hiddenUntilTimer = new HashSet<Guid>();

		private readonly SafeList<(DateTime timerExpiration, Guid guid)> _timerMetadata = new SafeList<(DateTime, Guid)>();

		private readonly ConcurrentDictionary<ulong, HashSet<Guid>> _hiddenInShard = new ConcurrentDictionary<ulong, HashSet<Guid>>();

		private double _lastTimerCheck = 5000.0;

		private double _lastSaveState;

		private bool _stateDirty;

		public BehaviorStates(IRootPackState packState)
			: base(packState)
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapOnMapChanged);
		}

		private bool SyncedCollectionContainsGuid(HashSet<Guid> collection, Guid guid)
		{
			if (collection == null)
			{
				return false;
			}
			lock (collection)
			{
				return collection.Contains(guid);
			}
		}

		private ulong GetMapInstanceKey()
		{
			return (ulong)(((long)_rootPackState.CurrentMapId << 32) + GameService.Gw2Mumble.get_Info().get_ShardId());
		}

		public bool IsBehaviorHidden(StandardPathableBehavior behavior, Guid guid)
		{
			HashSet<Guid> guidList;
			return behavior switch
			{
				StandardPathableBehavior.AlwaysVisible => false, 
				StandardPathableBehavior.ReappearOnMapChange => SyncedCollectionContainsGuid(_hiddenUntilMapChange, guid), 
				StandardPathableBehavior.ReappearOnDailyReset => SyncedCollectionContainsGuid(_hiddenUntilTimer, guid), 
				StandardPathableBehavior.OnlyVisibleBeforeActivation => SyncedCollectionContainsGuid(_hiddenUntilTimer, guid), 
				StandardPathableBehavior.ReappearAfterTimer => SyncedCollectionContainsGuid(_hiddenUntilTimer, guid), 
				StandardPathableBehavior.OncePerInstance => SyncedCollectionContainsGuid(_hiddenInShard.TryGetValue(GetMapInstanceKey(), out guidList) ? guidList : null, guid), 
				StandardPathableBehavior.OnceDailyPerCharacter => SyncedCollectionContainsGuid(_hiddenUntilTimer, guid), 
				StandardPathableBehavior.ReappearOnWeeklyReset => SyncedCollectionContainsGuid(_hiddenUntilTimer, guid), 
				StandardPathableBehavior.ReappearOnMapReset => false, 
				_ => UnsupportedBehavior(behavior), 
			};
		}

		private bool UnsupportedBehavior(StandardPathableBehavior behavior)
		{
			return false;
		}

		private void CurrentMapOnMapChanged(object sender, ValueEventArgs<int> e)
		{
			lock (_hiddenUntilMapChange)
			{
				_hiddenUntilMapChange.Clear();
			}
			_stateDirty = true;
		}

		public void AddFilteredBehavior(StandardPathableBehavior behavior, Guid guid)
		{
			switch (behavior)
			{
			case StandardPathableBehavior.ReappearOnMapChange:
				lock (_hiddenUntilMapChange)
				{
					_hiddenUntilMapChange.Add(guid);
				}
				break;
			case StandardPathableBehavior.OnlyVisibleBeforeActivation:
				lock (_hiddenUntilTimer)
				{
					_hiddenUntilTimer.Add(guid);
					_timerMetadata.Add((DateTime.MaxValue, guid));
				}
				break;
			case StandardPathableBehavior.OncePerInstance:
			{
				ulong instanceKey = GetMapInstanceKey();
				lock (_hiddenInShard)
				{
					if (!_hiddenInShard.ContainsKey(instanceKey))
					{
						_hiddenInShard[instanceKey] = new HashSet<Guid>();
					}
					lock (_hiddenInShard[instanceKey])
					{
						_hiddenInShard[instanceKey].Add(guid);
					}
				}
				break;
			}
			default:
				Logger.Warn($"TacO behavior {behavior} can not have its filtering handled this way.");
				break;
			}
			_stateDirty = true;
		}

		public void AddFilteredBehavior(Guid guid, DateTime expire)
		{
			lock (_hiddenUntilTimer)
			{
				_hiddenUntilTimer.Add(guid);
				_timerMetadata.Add((expire, guid));
			}
			_stateDirty = true;
		}

		protected override async Task<bool> Initialize()
		{
			await LoadState();
			return true;
		}

		public override Task Reload()
		{
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateWithCadence(UpdateTimers, gameTime, 5000.0, ref _lastTimerCheck);
			UpdateCadenceUtil.UpdateAsyncWithCadence(SaveState, gameTime, 10000.0, ref _lastSaveState);
		}

		public override async Task Unload()
		{
			await SaveState(null);
		}

		private async Task LoadState()
		{
			string timerStatesPath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), "timers.txt");
			if (!File.Exists(timerStatesPath))
			{
				return;
			}
			string[] recordedTimerMetadata = Array.Empty<string>();
			try
			{
				recordedTimerMetadata = await FileUtil.ReadLinesAsync(timerStatesPath);
			}
			catch (Exception e)
			{
				Logger.Error(e, "Failed to read timers.txt (" + timerStatesPath + ").");
			}
			lock (_hiddenUntilTimer)
			{
				string[] array = recordedTimerMetadata;
				foreach (string line in array)
				{
					string[] lineParts = line.Split(',');
					try
					{
						Guid markerGuid = Guid.ParseExact(lineParts[0], "D");
						DateTime markerExpire = DateTime.Parse(lineParts[1]);
						_hiddenUntilTimer.Add(markerGuid);
						_timerMetadata.Add((markerExpire, markerGuid));
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Failed to parse behavior state '" + line + "' from " + timerStatesPath);
					}
				}
			}
		}

		private async Task SaveState(GameTime gameTime)
		{
			if (!_stateDirty)
			{
				return;
			}
			Logger.Debug("Saving BehaviorStates state.");
			(DateTime, Guid)[] timerMetadata = _timerMetadata.ToArray();
			string timerStatesPath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), "timers.txt");
			try
			{
				await FileUtil.WriteLinesAsync(timerStatesPath, timerMetadata.Select(((DateTime timerExpiration, Guid guid) metadata) => $"{metadata.guid},{metadata.timerExpiration}"));
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to write timers.txt (" + timerStatesPath + ").");
			}
			_stateDirty = false;
		}

		private void UpdateTimers(GameTime gameTime)
		{
			lock (_hiddenUntilTimer)
			{
				(DateTime, Guid)[] array = _timerMetadata.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					(DateTime, Guid) guidDetails = array[i];
					if (guidDetails.Item1 < DateTime.UtcNow)
					{
						_timerMetadata.Remove(guidDetails);
						_hiddenUntilTimer.Remove(guidDetails.Item2);
						_stateDirty = true;
					}
				}
			}
		}
	}
}
