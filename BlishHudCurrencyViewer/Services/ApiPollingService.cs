using System;
using Microsoft.Xna.Framework;

namespace BlishHudCurrencyViewer.Services
{
	public class ApiPollingService : IDisposable
	{
		private int _refreshIntervalMilliseconds;

		private double _runningTimer;

		public event EventHandler<bool> ApiPollingTrigger;

		public ApiPollingService(int refreshIntervalMilliseconds = 300000)
		{
			_refreshIntervalMilliseconds = refreshIntervalMilliseconds;
		}

		public void Dispose()
		{
		}

		public void Update(GameTime gameTime)
		{
			_runningTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_runningTimer >= (double)_refreshIntervalMilliseconds)
			{
				this.ApiPollingTrigger?.Invoke(this, e: true);
				_runningTimer = 0.0;
			}
		}

		public void Invoke()
		{
			_runningTimer = 0.0;
			this.ApiPollingTrigger?.Invoke(this, e: true);
		}
	}
}
