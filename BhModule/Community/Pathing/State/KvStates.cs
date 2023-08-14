using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using FASTER.core;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class KvStates : ManagedState
	{
		private const double INTERVAL_CHECKPOINT = 5000.0;

		private string _kvDir;

		private FasterKV<string, string> _kvStore;

		private bool _dirty;

		private double _lastCheckpointCheck = 5000.0;

		public KvStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		protected override async Task<bool> Initialize()
		{
			_kvDir = DataDirUtil.GetSafeDataDir("kv");
			IDevice defLog = Devices.CreateLogDevice(Path.Combine(_kvDir, "bkv.db"), preallocateFile: false, deleteOnClose: false, -1L);
			IDevice objLog = Devices.CreateLogDevice(Path.Combine(_kvDir, "okv.db"), preallocateFile: false, deleteOnClose: false, -1L);
			_kvStore = new FasterKV<string, string>(new FasterKVSettings<string, string>(_kvDir)
			{
				LogDevice = defLog,
				ObjectLogDevice = objLog
			});
			try
			{
				await _kvStore.RecoverAsync(-1, undoNextVersion: true, -1L);
			}
			catch (Exception)
			{
			}
			return true;
		}

		public ClientSession<string, string, string, string, Empty, IFunctions<string, string, string, string, Empty>> GetSession()
		{
			return _kvStore.NewSession(new SimpleFunctions<string, string>());
		}

		public void Invalidate()
		{
			_dirty = true;
		}

		public override async Task Reload()
		{
			await Unload();
			await Initialize();
		}

		private async Task FlushCheckpoint(GameTime gameTime)
		{
			if (_dirty)
			{
				_dirty = false;
				await _kvStore.TakeHybridLogCheckpointAsync(CheckpointType.FoldOver, tryIncremental: true, default(CancellationToken), -1L);
			}
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(FlushCheckpoint, gameTime, 5000.0, ref _lastCheckpointCheck);
		}

		public override async Task Unload()
		{
			await _kvStore.CompleteCheckpointAsync();
			_kvStore.Dispose();
		}
	}
}
