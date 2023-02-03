using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class RaceRunnerLoader : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<RaceRunnerLoader>();

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly RaceRunner _runner;

		private bool _lockGhost;

		private Dictionary<string, FullGhost> _localGhosts = new Dictionary<string, FullGhost>();

		private readonly object _lock = new object();

		private CancellationTokenSource? _loadRace;

		public FullRace? FullRace => _runner.FullRace;

		public int MaxGhostData { get; private set; }

		public bool AutoLocalGhost { get; private set; }

		public event Action<FullRace?> RaceLoaded
		{
			add
			{
				_runner.RaceLoaded += value;
			}
			remove
			{
				_runner.RaceLoaded -= value;
			}
		}

		public event Action<FullGhost?> GhostLoaded
		{
			add
			{
				_runner.GhostLoaded += value;
			}
			remove
			{
				_runner.GhostLoaded -= value;
			}
		}

		public RaceRunnerLoader(RaceRunner runner)
		{
			_runner = runner;
			_dc.Add(RacingModule.Settings.MaxGhostData.OnChangedAndNow(delegate(int v)
			{
				MaxGhostData = v;
			}));
			_dc.Add(RacingModule.Settings.AutoLocalGhost.OnChangedAndNow(delegate(bool v)
			{
				AutoLocalGhost = v;
			}));
			_runner.RaceFinished += new Action<Race, Ghost>(RaceFinished);
		}

		private void RaceFinished(Race _, Ghost ghost)
		{
			if (SaveGhost(ghost))
			{
				LoadBestLocalGhost();
			}
		}

		public void LoadRace(FullRace? fullRace, FullGhost? fullGhost)
		{
			FullRace fullRace2 = fullRace;
			FullGhost fullGhost2 = fullGhost;
			lock (_lock)
			{
				TaskUtils.Cancel(ref _loadRace);
				_runner.SetGhost(null);
				_runner.SetRace(null);
				_lockGhost = false;
				CancellationToken ct = TaskUtils.New(out _loadRace);
				Task.Run(delegate
				{
					_localGhosts = ((fullRace2 != null) ? LocalData.GetGhosts(fullRace2) : new Dictionary<string, FullGhost>());
					ct.ThrowIfCancellationRequested();
					_runner.SetRace(fullRace2);
					ct.ThrowIfCancellationRequested();
					if (fullGhost2 != null)
					{
						SetGhost(fullGhost2, lockGhost: true);
					}
					LoadBestLocalGhost();
				}, ct).Done(Logger, Strings.ErrorRaceLoad, _loadRace);
			}
		}

		public void SetGhost(FullGhost? fullGhost, bool lockGhost = false)
		{
			_runner.SetGhost(fullGhost);
			_lockGhost = lockGhost;
		}

		private void LoadBestLocalGhost()
		{
			if (AutoLocalGhost && !_lockGhost)
			{
				SetGhost(_localGhosts.Any() ? _localGhosts.Values.MinBy((FullGhost g) => g.Meta.Time) : null);
			}
		}

		private bool SaveGhost(Ghost ghost)
		{
			lock (_lock)
			{
				if (!_runner.IsTesting && ghost != null && ghost.RaceId != null)
				{
					FullRace fullRace = _runner.FullRace;
					if (fullRace != null)
					{
						int count = _localGhosts.Count;
						return RacingModule.LocalData.SaveGhost(fullRace, ref _localGhosts, ghost, MaxGhostData) || _localGhosts.Count != count;
					}
				}
				return false;
			}
		}

		public void Dispose()
		{
			_runner.RaceFinished -= new Action<Race, Ghost>(RaceFinished);
			_dc.Dispose();
		}
	}
}
