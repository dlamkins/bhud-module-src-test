using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public abstract class ManagedState : IDisposable
	{
		protected readonly IRootPackState _rootPackState;

		public bool Running { get; private set; }

		protected ManagedState(IRootPackState rootPackState)
		{
			_rootPackState = rootPackState;
		}

		public async Task<ManagedState> Start()
		{
			if (Running)
			{
				return this;
			}
			await Initialize();
			Running = true;
			return this;
		}

		private void Stop()
		{
			if (Running)
			{
				Running = false;
			}
		}

		public abstract Task Reload();

		public abstract void Update(GameTime gameTime);

		protected abstract Task<bool> Initialize();

		public abstract Task Unload();

		public void Dispose()
		{
			Stop();
		}
	}
}
