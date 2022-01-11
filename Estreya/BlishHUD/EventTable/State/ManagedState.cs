using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	internal abstract class ManagedState : IDisposable
	{
		private string _path;

		protected string BasePath { get; private set; }

		protected string FileName { get; private set; }

		private int SaveInternal { get; set; }

		private TimeSpan TimeSinceSave { get; set; } = TimeSpan.Zero;


		protected string Path
		{
			get
			{
				if (_path == null)
				{
					_path = System.IO.Path.Combine(BasePath, FileName);
				}
				return _path;
			}
		}

		public bool Running { get; private set; }

		protected ManagedState(string basePath, string fileName, int saveInterval = 60000)
		{
			BasePath = basePath;
			FileName = fileName;
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
