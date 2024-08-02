using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;

namespace MysticCrafting.Module.Services.API
{
	public abstract class ApiService : IApiService
	{
		private static readonly Logger Logger = Logger.GetLogger<IApiService>();

		private Timer _updateTimer;

		private bool _loading;

		private readonly Gw2ApiManager _apiManager;

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

		public int ExecutionIntervalMinutes { get; set; } = 5;


		public abstract List<TokenPermission> Permissions { get; }

		public event EventHandler<EventArgs> LoadingStarted;

		public event EventHandler<EventArgs> LoadingFinished;

		public abstract Task<string> LoadAsync();

		protected ApiService(Gw2ApiManager manager)
		{
			_apiManager = manager;
		}

		public async Task StartTimedLoadingAsync(int minutes)
		{
			int interval = minutes * 60000;
			_updateTimer?.Dispose();
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
				if (MysticCraftingModule.WindowIsOpen())
				{
					await LoadSafeAsync();
					_updateTimer.Start();
				}
			}
		}

		public async Task LoadSafeAsync()
		{
			if (_apiManager == null)
			{
				throw new Exception("Gw2ApiManager object is null.");
			}
			if (Loading || !_apiManager.HasPermissions((IEnumerable<TokenPermission>)Permissions))
			{
				await Task.CompletedTask;
				return;
			}
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
	}
}
