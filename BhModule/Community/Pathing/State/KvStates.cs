using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace BhModule.Community.Pathing.State
{
	public class KvStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<KvStates>();

		private const string KVFILE = "kv.json";

		private const double INTERVAL_CHECKPOINT = 5000.0;

		private string _kvDir;

		private string _kvFile;

		private bool _dirty;

		private double _lastCheckpointCheck = 5000.0;

		private ConcurrentDictionary<string, string> _kvStore;

		public KvStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		protected override async Task<bool> Initialize()
		{
			_kvDir = DataDirUtil.GetSafeDataDir("kv");
			_kvFile = Path.Combine(_kvDir, "kv.json");
			LoadKv();
			return true;
		}

		private void LoadKv()
		{
			try
			{
				if (File.Exists(_kvFile))
				{
					_kvStore = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(File.ReadAllText(_kvFile)) ?? new ConcurrentDictionary<string, string>();
				}
				else
				{
					_kvStore = new ConcurrentDictionary<string, string>();
				}
			}
			catch (Exception ex)
			{
				_kvStore = new ConcurrentDictionary<string, string>();
				Logger.Warn(ex, "Failed to load kv.json.  Settings will not be restored!");
			}
		}

		private async Task FlushKv(GameTime gameTime)
		{
			if (_dirty)
			{
				_dirty = false;
				File.WriteAllText(_kvFile, JsonConvert.SerializeObject((object)_kvStore, (Formatting)1));
			}
		}

		public string UpsertValue(string key, string value)
		{
			bool updated = !_kvStore.ContainsKey(key);
			_kvStore.AddOrUpdate(key, value, delegate(string _, string existingVal)
			{
				if (existingVal != value)
				{
					updated = true;
					return value;
				}
				return existingVal;
			});
			if (updated)
			{
				Invalidate();
			}
			return value;
		}

		public string ReadValue(string name)
		{
			_kvStore.TryGetValue(name, out var value);
			return value;
		}

		public void DeleteValue(string key)
		{
			if (_kvStore.TryRemove(key, out var _))
			{
				Invalidate();
			}
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

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(FlushKv, gameTime, 5000.0, ref _lastCheckpointCheck);
		}

		public override async Task Unload()
		{
			await FlushKv(null);
		}
	}
}
