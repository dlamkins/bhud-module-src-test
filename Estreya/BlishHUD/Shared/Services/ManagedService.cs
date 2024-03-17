using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Services
{
	public abstract class ManagedService : IDisposable
	{
		private readonly AsyncRef<double> _lastSaved = new AsyncRef<double>(0.0);

		private CancellationTokenSource _cancellationTokenSource;

		protected Logger Logger;

		protected ServiceConfiguration Configuration { get; }

		protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

		public bool Running { get; private set; }

		public bool AwaitLoading => Configuration.AwaitLoading;

		protected ManagedService(ServiceConfiguration configuration)
		{
			Logger = Logger.GetLogger(GetType());
			Configuration = configuration;
		}

		public void Dispose()
		{
			Stop();
			Unload();
		}

		public async Task Start()
		{
			if (Running)
			{
				Logger.Warn("Trying to start, but already running.");
				return;
			}
			Logger.Debug("Starting state.");
			_cancellationTokenSource = new CancellationTokenSource();
			await Initialize();
			Running = true;
			await Load();
		}

		private void Stop()
		{
			if (!Running)
			{
				Logger.Warn("Trying to stop, but not running.");
				return;
			}
			Logger.Debug("Stopping state.");
			Running = false;
		}

		public void Update(GameTime gameTime)
		{
			if (Running)
			{
				if (Configuration.SaveInterval != Timeout.InfiniteTimeSpan)
				{
					UpdateUtil.UpdateAsync(Save, gameTime, Configuration.SaveInterval.TotalMilliseconds, _lastSaved);
				}
				try
				{
					InternalUpdate(gameTime);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to update:");
				}
			}
		}

		public virtual async Task Reload()
		{
			if (!Running)
			{
				Logger.Warn("Trying to reload, but not running.");
				return;
			}
			Logger.Debug("Reloading state.");
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			await Clear();
			await Load();
			await InternalReload();
		}

		private void Unload()
		{
			if (_cancellationTokenSource.IsCancellationRequested)
			{
				Logger.Warn("Already unloaded.");
				return;
			}
			Logger.Debug("Unloading state.");
			_cancellationTokenSource.Cancel();
			InternalUnload();
		}

		protected abstract Task Initialize();

		protected abstract Task Load();

		protected virtual Task Save()
		{
			return Task.CompletedTask;
		}

		protected abstract void InternalUpdate(GameTime gameTime);

		protected virtual Task InternalReload()
		{
			return Task.CompletedTask;
		}

		protected virtual Task Clear()
		{
			return Task.CompletedTask;
		}

		protected abstract void InternalUnload();
	}
}
