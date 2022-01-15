using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Estreya.BlishHUD.EventTable.Helpers;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	internal class HiddenState : ManagedState
	{
		private const string FILE_NAME = "hidden.txt";

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
					_path = System.IO.Path.Combine(BasePath, "hidden.txt");
				}
				return _path;
			}
		}

		private Dictionary<string, DateTime> Instances { get; set; } = new Dictionary<string, DateTime>();


		public HiddenState(string basePath)
			: base(30000)
		{
			BasePath = basePath;
		}

		public override async Task Reload()
		{
			lock (Instances)
			{
				Instances.Clear();
			}
			await Load();
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			lock (Instances)
			{
				for (int i = Instances.Count - 1; i >= 0; i--)
				{
					KeyValuePair<string, DateTime> instance = Instances.ElementAt(i);
					string name = instance.Key;
					DateTime hiddenUntil = instance.Value;
					if (now >= hiddenUntil)
					{
						Remove(name);
					}
				}
			}
		}

		public void Add(string name, DateTime hideUntil, bool isUTC)
		{
			lock (Instances)
			{
				if (Instances.ContainsKey(name))
				{
					Instances.Remove(name);
				}
				if (!isUTC)
				{
					hideUntil = hideUntil.ToUniversalTime();
				}
				Instances.Add(name, hideUntil);
				dirty = true;
			}
		}

		public void Remove(string name)
		{
			lock (Instances)
			{
				if (Instances.ContainsKey(name))
				{
					Instances.Remove(name);
					dirty = true;
				}
			}
		}

		public bool IsHidden(string name)
		{
			lock (Instances)
			{
				return Instances.ContainsKey(name);
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
					string[] array2 = array[i].Split(new string[1] { "<-->" }, StringSplitOptions.None);
					string name = array2[0];
					DateTime hiddenUntil = DateTime.Parse(array2[1]);
					Instances.Add(name, hiddenUntil);
				}
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
				foreach (KeyValuePair<string, DateTime> instance in Instances)
				{
					lines.Add(string.Format("{0}{1}{2}", instance.Key, "<-->", instance.Value));
				}
			}
			await FileUtil.WriteLinesAsync(Path, lines.ToArray());
			dirty = false;
		}

		protected override Task InternalUnload()
		{
			return Task.CompletedTask;
		}
	}
}
