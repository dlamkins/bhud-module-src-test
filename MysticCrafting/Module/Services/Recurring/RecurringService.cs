using System;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;

namespace MysticCrafting.Module.Services.Recurring
{
	public abstract class RecurringService : IRecurringService
	{
		private static readonly Logger Logger = Logger.GetLogger<IRecurringService>();

		private Timer _updateTimer;

		private bool _loading;

		public abstract string Name { get; }

		public DateTime LastLoaded { get; set; }

		public DateTime LastFailed { get; set; }

		public bool Loaded { get; set; }

		public bool Loading
		{
			get
			{
				return _loading;
			}
			set
			{
				if (_loading != value)
				{
					_loading = value;
					if (_loading)
					{
						this.LoadingStarted?.Invoke(this, EventArgs.Empty);
					}
					else
					{
						this.LoadingFinished?.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}

		public event EventHandler<EventArgs> LoadingStarted;

		public event EventHandler<EventArgs> LoadingFinished;

		public abstract Task<string> LoadAsync();

		public async Task StartTimedLoadingAsync(int interval)
		{
			_updateTimer = new Timer(interval);
			_updateTimer.Elapsed += UpdateTimerOnElapsed;
			await LoadSafeAsync();
			_updateTimer.Start();
		}

		private async void UpdateTimerOnElapsed(object sender, ElapsedEventArgs e)
		{
			if (_updateTimer != null)
			{
				_updateTimer.Stop();
				await LoadSafeAsync();
				_updateTimer.Start();
			}
		}

		private async Task LoadSafeAsync()
		{
			try
			{
				Loading = true;
				string result = await LoadAsync();
				LastLoaded = DateTime.Now;
				Logger.Info("API service '" + Name + "' loaded successfully with result: " + result);
			}
			catch (Exception e)
			{
				Logger.Warn("API service '" + Name + "' loading failed with exception: " + e.Message);
				LastFailed = DateTime.Now;
			}
			finally
			{
				Loading = false;
				Loaded = true;
			}
		}

		public void StopTimedLoading()
		{
			if (_updateTimer != null)
			{
				_updateTimer?.Stop();
				_updateTimer.Elapsed -= UpdateTimerOnElapsed;
				_updateTimer?.Dispose();
			}
		}
	}
}
