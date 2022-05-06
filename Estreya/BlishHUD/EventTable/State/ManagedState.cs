using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.EventTable.Helpers;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	public abstract class ManagedState : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ManagedState>();

		private SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);

		private int SaveInternal { get; set; }

		private TimeSpan TimeSinceSave { get; set; } = TimeSpan.Zero;


		public bool Running { get; private set; }

		public bool AwaitLoad { get; }

		protected ManagedState(bool awaitLoad = true, int saveInterval = 60000)
		{
			AwaitLoad = awaitLoad;
			SaveInternal = saveInterval;
		}

		public async Task Start()
		{
			if (Running)
			{
				Logger.Warn("Trying to start state \"{0}\" which is already running.", new object[1] { GetType().Name });
				return;
			}
			Logger.Debug("Starting managed state: {0}", new object[1] { GetType().Name });
			await Initialize();
			await Load();
			Running = true;
		}

		public void Stop()
		{
			if (!Running)
			{
				Logger.Warn("Trying to stop state \"{0}\" which is not running.", new object[1] { GetType().Name });
			}
			else
			{
				Logger.Debug("Stopping managed state: {0}", new object[1] { GetType().Name });
				Running = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (!Running)
			{
				return;
			}
			TimeSinceSave += gameTime.get_ElapsedGameTime();
			if (TimeSinceSave.TotalMilliseconds >= (double)SaveInternal)
			{
				if (_saveSemaphore.CurrentCount > 0)
				{
					Task.Run(async delegate
					{
						_ = 1;
						try
						{
							await _saveSemaphore.WaitAsync();
							await Save();
							TimeSinceSave = TimeSpan.Zero;
						}
						finally
						{
							_saveSemaphore.Release();
						}
					});
				}
				else
				{
					Logger.Debug("Another thread is already running Save()");
				}
			}
			InternalUpdate(gameTime);
		}

		public async Task Reload()
		{
			if (!Running)
			{
				Logger.Warn("Trying to reload state \"{0}\" which is not running.", new object[1] { GetType().Name });
			}
			else
			{
				Logger.Debug("Reloading state: {0}", new object[1] { GetType().Name });
				await InternalReload();
			}
		}

		public abstract Task InternalReload();

		private async Task Unload()
		{
			if (!Running)
			{
				Logger.Warn("Trying to unload state \"{0}\" which is not running.", new object[1] { GetType().Name });
			}
			else
			{
				Logger.Debug("Unloading state: {0}", new object[1] { GetType().Name });
				await Save();
			}
		}

		public abstract Task Clear();

		protected abstract void InternalUnload();

		protected abstract Task Initialize();

		protected abstract void InternalUpdate(GameTime gameTime);

		protected abstract Task Save();

		protected abstract Task Load();

		public void Dispose()
		{
			AsyncHelper.RunSync(Unload);
			Stop();
		}
	}
}
