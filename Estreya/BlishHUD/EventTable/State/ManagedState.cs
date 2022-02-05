using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	public abstract class ManagedState : IDisposable
	{
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
				Task.Run(async delegate
				{
					await Save();
					TimeSinceSave = TimeSpan.Zero;
				});
			}
			InternalUpdate(gameTime);
		}

		public abstract Task Reload();

		public async Task Unload()
		{
			await Save();
		}

		protected abstract Task InternalUnload();

		protected abstract Task Initialize();

		protected abstract void InternalUpdate(GameTime gameTime);

		protected abstract Task Save();

		protected abstract Task Load();

		public void Dispose()
		{
			Stop();
		}
	}
}
