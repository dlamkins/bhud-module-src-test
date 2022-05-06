using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	public class EventState : ManagedState
	{
		public enum EventStates
		{
			Completed,
			Hidden
		}

		public struct VisibleStateInfo
		{
			public string Key;

			public EventStates State;

			public DateTime Until;
		}

		private static readonly Logger Logger = Logger.GetLogger<EventState>();

		private const string FILE_NAME = "event_states.txt";

		private const string LINE_SPLIT = "<-->";

		private bool dirty;

		private string _path;

		private string BasePath { get; set; }

		private string Path
		{
			get
			{
				if (_path == null)
				{
					_path = System.IO.Path.Combine(BasePath, "event_states.txt");
				}
				return _path;
			}
		}

		private List<VisibleStateInfo> Instances { get; set; } = new List<VisibleStateInfo>();


		public event EventHandler<ValueEventArgs<VisibleStateInfo>> StateAdded;

		public event EventHandler<ValueEventArgs<string>> StateRemoved;

		public EventState(string basePath)
			: base(awaitLoad: true, 30000)
		{
			BasePath = basePath;
		}

		public override async Task InternalReload()
		{
			await Clear();
			await Load();
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			lock (Instances)
			{
				for (int i = Instances.Count - 1; i >= 0; i--)
				{
					VisibleStateInfo instance = Instances.ElementAt(i);
					if (now >= instance.Until)
					{
						Remove(instance.Key);
					}
				}
			}
		}

		public void Add(string name, DateTime until, EventStates state)
		{
			lock (Instances)
			{
				Remove(name);
				until = until.ToUniversalTime();
				Logger.Info($"Add event state for \"{name}\" with \"{state}\" until \"{until}\" UTC.");
				VisibleStateInfo visibleStateInfo = default(VisibleStateInfo);
				visibleStateInfo.Key = name;
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

		public void Remove(string name)
		{
			lock (Instances)
			{
				List<VisibleStateInfo> instancesToRemove = Instances.Where((VisibleStateInfo instance) => instance.Key == name).ToList();
				if (instancesToRemove.Count != 0)
				{
					Logger.Info("Remove event states for \"" + name + "\".");
					for (int i = instancesToRemove.Count - 1; i >= 0; i--)
					{
						Instances.Remove(instancesToRemove[i]);
					}
					try
					{
						this.StateRemoved?.Invoke(this, new ValueEventArgs<string>(name));
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "StateRemoved.Invoke failed.");
					}
					dirty = true;
				}
			}
		}

		public override Task Clear()
		{
			lock (Instances)
			{
				Logger.Info("Remove all event states.");
				for (int i = Instances.Count - 1; i >= 0; i--)
				{
					Remove(Instances[i].Key);
				}
				dirty = true;
			}
			return Task.CompletedTask;
		}

		public bool Contains(string name, EventStates state)
		{
			lock (Instances)
			{
				return Instances.Any((VisibleStateInfo instance) => instance.Key == name && instance.State == state);
			}
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override async Task Load()
		{
			if (!File.Exists(Path))
			{
				return;
			}
			try
			{
				string[] lines = await FileUtil.ReadLinesAsync(Path);
				if (lines == null || lines.Length == 0)
				{
					return;
				}
				lock (Instances)
				{
					string[] array = lines;
					for (int i = 0; i < array.Length; i++)
					{
						string[] parts = array[i].Split(new string[1] { "<-->" }, StringSplitOptions.None);
						if (parts.Length == 0)
						{
							Logger.Warn("Line is empty.");
							continue;
						}
						string name = parts[0];
						try
						{
							EventStates state = (EventStates)Enum.Parse(typeof(EventStates), parts[1]);
							DateTime until = DateTime.Parse(parts[2]);
							VisibleStateInfo visibleStateInfo = default(VisibleStateInfo);
							visibleStateInfo.Key = name;
							visibleStateInfo.Until = until;
							visibleStateInfo.State = state;
							Add(name, until, state);
						}
						catch (Exception ex2)
						{
							Logger.Error(ex2, "Loading line \"{0}\" failed.", new object[1] { name });
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Loading \"{0}\" failed.", new object[1] { GetType().Name });
			}
		}

		protected override async Task Save()
		{
			if (!dirty)
			{
				return;
			}
			Collection<string> lines = new Collection<string>();
			lock (Instances)
			{
				foreach (VisibleStateInfo instance in Instances)
				{
					lines.Add(string.Format("{0}{1}{2}{3}{4}", instance.Key, "<-->", instance.State, "<-->", instance.Until));
				}
			}
			await FileUtil.WriteLinesAsync(Path, lines.ToArray());
			dirty = false;
		}

		protected override void InternalUnload()
		{
			AsyncHelper.RunSync(Save);
			AsyncHelper.RunSync(Clear);
		}
	}
}
