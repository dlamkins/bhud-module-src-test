using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class PresenceLoop : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<PresenceLoop>();

		private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(15.0);

		private static readonly TimeSpan FirstRetryDelay = TimeSpan.FromSeconds(30.0);

		private readonly PresenceService _presenceService;

		private readonly SemaphoreSlim _refreshGate = new SemaphoreSlim(1, 1);

		private CancellationTokenSource _cancellation;

		private Task _worker;

		private bool _isDisposed;

		public PlayerPresence CurrentPresence { get; private set; }

		public DateTime LastRefreshedAt { get; private set; }

		public string LastStatus { get; private set; } = string.Empty;


		public bool IsRunning
		{
			get
			{
				if (_worker != null)
				{
					return !_worker.IsCompleted;
				}
				return false;
			}
		}

		public event Action<PlayerPresence> PresenceUpdated;

		public event Action<string> StatusChanged;

		public PresenceLoop(PresenceService presenceService)
		{
			_presenceService = presenceService;
		}

		public void Start()
		{
			if (!IsRunning)
			{
				Stop();
				_cancellation = new CancellationTokenSource();
				_worker = RunAsync(_cancellation.Token);
			}
		}

		public void Stop()
		{
			CancellationTokenSource cancellation = _cancellation;
			Task worker = _worker;
			_cancellation = null;
			_worker = null;
			if (cancellation != null)
			{
				cancellation.Cancel();
				TaskCleanup.DisposeWhenComplete(worker, cancellation);
			}
		}

		public async Task<PlayerPresence> RefreshAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await _refreshGate.WaitAsync(cancellationToken);
			try
			{
				PlayerPresence presence = (CurrentPresence = await _presenceService.GetCurrentPresenceAsync(cancellationToken));
				LastRefreshedAt = DateTime.UtcNow;
				SetStatus((presence != null && presence.CanShare) ? "Presence ready." : (presence?.ShareBlockReason ?? "Presence refreshed."));
				this.PresenceUpdated?.Invoke(presence);
				return presence;
			}
			finally
			{
				_refreshGate.Release();
			}
		}

		private async Task RunAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				TimeSpan delay = RefreshInterval;
				try
				{
					await RefreshAsync(cancellationToken);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					return;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "SPARK presence refresh failed.");
					SetStatus("Presence refresh failed. Retrying later.");
					delay = FirstRetryDelay;
				}
				try
				{
					await Task.Delay(delay, cancellationToken);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					return;
				}
			}
		}

		private void SetStatus(string status)
		{
			LastStatus = status ?? string.Empty;
			this.StatusChanged?.Invoke(LastStatus);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Stop();
			}
		}
	}
}
