using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.EventTable.Services
{
	public class EventStateService : ManagedService
	{
		public enum EventStates
		{
			Completed,
			Hidden
		}

		public struct VisibleStateInfo
		{
			public string AreaName;

			public string EventKey;

			[JsonConverter(typeof(StringEnumConverter))]
			public EventStates State;

			public DateTime Until;
		}

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private const string FILE_NAME = "event_states.json";

		private readonly Func<DateTime> _getNowAction;

		private string _path;

		private bool dirty;

		private string _basePath { get; }

		private string Path
		{
			get
			{
				if (_path == null)
				{
					_path = System.IO.Path.Combine(_basePath, "event_states.json");
				}
				return _path;
			}
		}

		public List<VisibleStateInfo> Instances { get; } = new List<VisibleStateInfo>();


		public event EventHandler<ValueEventArgs<VisibleStateInfo>> StateAdded;

		public event EventHandler<ValueEventArgs<VisibleStateInfo>> StateRemoved;

		public EventStateService(ServiceConfiguration configuration, string basePath, Func<DateTime> getNowAction)
			: base(configuration)
		{
			_basePath = basePath;
			_getNowAction = getNowAction;
		}

		protected override Task InternalReload()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			DateTime now = _getNowAction().ToUniversalTime();
			lock (Instances)
			{
				for (int i = Instances.Count - 1; i >= 0; i--)
				{
					VisibleStateInfo instance = Instances.ElementAt(i);
					if (now >= instance.Until)
					{
						Remove(instance.AreaName, instance.EventKey);
					}
				}
			}
		}

		public void Add(string areaName, string eventKey, DateTime until, EventStates state)
		{
			lock (Instances)
			{
				Remove(areaName, eventKey);
				until = until.ToUniversalTime();
				string name = GetName(areaName, eventKey);
				Logger.Info(string.Format("Add event state for \"{0}\" with \"{1}\" until \"{2}\" UTC.", name, state, until.ToString("yyyy-MM-ddTHH:mm:ss")));
				VisibleStateInfo visibleStateInfo = default(VisibleStateInfo);
				visibleStateInfo.AreaName = areaName;
				visibleStateInfo.EventKey = eventKey;
				visibleStateInfo.State = state;
				visibleStateInfo.Until = until;
				VisibleStateInfo newInstance = visibleStateInfo;
				Instances.Add(newInstance);
				try
				{
					this.StateAdded?.Invoke(this, new ValueEventArgs<VisibleStateInfo>(newInstance));
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "StateAdded.Invoke failed.");
				}
				dirty = true;
			}
		}

		public void Remove(string areaName, EventStates? state)
		{
			lock (Instances)
			{
				Instances.Where((VisibleStateInfo instance) => instance.AreaName == areaName && (!state.HasValue || instance.State == state.Value)).ToList().ForEach(delegate(VisibleStateInfo i)
				{
					Remove(areaName, i.EventKey);
				});
			}
		}

		public void Remove(string areaName, string eventKey)
		{
			lock (Instances)
			{
				List<VisibleStateInfo> instancesToRemove = Instances.Where((VisibleStateInfo instance) => instance.AreaName == areaName && instance.EventKey == eventKey).ToList();
				if (instancesToRemove.Count == 0)
				{
					return;
				}
				string name = GetName(areaName, eventKey);
				Logger.Info("Remove event states for \"" + name + "\".");
				for (int i = instancesToRemove.Count - 1; i >= 0; i--)
				{
					VisibleStateInfo instance2 = instancesToRemove[i];
					Instances.Remove(instance2);
					try
					{
						this.StateRemoved?.Invoke(this, new ValueEventArgs<VisibleStateInfo>(instance2));
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "StateRemoved.Invoke failed.");
					}
				}
				dirty = true;
			}
		}

		private string GetName(string areaName, string eventKey)
		{
			return areaName + "-" + eventKey;
		}

		protected override Task Clear()
		{
			lock (Instances)
			{
				Logger.Info("Remove all event states.");
				for (int i = Instances.Count - 1; i >= 0; i--)
				{
					Remove(Instances[i].AreaName, Instances[i].EventKey);
				}
				dirty = true;
			}
			return Task.CompletedTask;
		}

		public bool Contains(string areaName, string eventKey, EventStates state)
		{
			lock (Instances)
			{
				return Instances.Any((VisibleStateInfo instance) => instance.AreaName == areaName && instance.EventKey == eventKey && instance.State == state);
			}
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override async Task Load()
		{
			Logger.Info("Load saved event states from filesystem.");
			if (!File.Exists(Path))
			{
				Logger.Info("File does not exist.");
				return;
			}
			try
			{
				string json = await FileUtil.ReadStringAsync(Path);
				if (string.IsNullOrWhiteSpace(json))
				{
					return;
				}
				foreach (VisibleStateInfo instance in JsonConvert.DeserializeObject<List<VisibleStateInfo>>(json)!)
				{
					Add(instance.AreaName, instance.EventKey, instance.Until, instance.State);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Loading \"{0}\" failed.", new object[1] { GetType().Name });
			}
		}

		protected override async Task Save()
		{
			if (dirty)
			{
				string json = null;
				lock (Instances)
				{
					json = JsonConvert.SerializeObject(Instances, Formatting.Indented);
				}
				if (!string.IsNullOrWhiteSpace(json))
				{
					await FileUtil.WriteStringAsync(Path, json);
				}
				dirty = false;
			}
		}

		protected override void InternalUnload()
		{
			AsyncHelper.RunSync(Save);
		}
	}
}
