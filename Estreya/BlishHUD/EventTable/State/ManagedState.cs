using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
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

		protected ManagedState(int saveInterval = 60000)
		{
			SaveInternal = saveInterval;
		}

		public async Task Start()
		{
			if (!Running)
			{
				await Initialize();
				await Load();
				Running = true;
			}
		}

		public void Stop()
		{
			if (Running)
			{
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

		public abstract Task Reload();

		public async Task Unload()
		{
			await Save();
		}

		public abstract Task Clear();

		protected abstract void InternalUnload();

		protected abstract Task Initialize();

		protected abstract void InternalUpdate(GameTime gameTime);

		protected abstract Task Save();

		protected abstract Task Load();

		public void Dispose()
		{
			Stop();
			InternalUnload();
		}
	}
}
