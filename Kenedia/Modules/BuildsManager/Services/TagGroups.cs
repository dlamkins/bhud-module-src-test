using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class TagGroups : IEnumerable<TagGroup>, IEnumerable
	{
		private readonly System.Timers.Timer _timer;

		private readonly ContentsManager _contentsManager;

		private readonly Paths _paths;

		private CancellationTokenSource _tokenSource;

		private List<TagGroup> _groups = new List<TagGroup>();

		private bool _saveRequested;

		public event PropertyAndValueChangedEventHandler GroupChanged;

		public event EventHandler<TagGroup> GroupAdded;

		public event EventHandler<TagGroup> GroupRemoved;

		public TagGroups(ContentsManager contentsManager, Paths paths)
		{
			_contentsManager = contentsManager;
			_paths = paths;
			_timer = new System.Timers.Timer(1000.0);
			_timer.Elapsed += OnTimerElapsed;
		}

		private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (_saveRequested)
			{
				_timer.Stop();
				await Save();
			}
		}

		public async Task Load()
		{
			if (File.Exists(_paths.ModulePath + "TagGroups.json"))
			{
				try
				{
					if (File.Exists(_paths.ModulePath + "TagGroups.json"))
					{
						string json2 = File.ReadAllText(_paths.ModulePath + "TagGroups.json");
						_groups = JsonConvert.DeserializeObject<List<TagGroup>>(json2, SerializerSettings.Default);
					}
				}
				catch (Exception ex)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to load TagGroups.json");
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
				}
			}
			else
			{
				string json = await new StreamReader(_contentsManager.GetFileStream("data\\TagGroups.json")).ReadToEndAsync();
				_groups = JsonConvert.DeserializeObject<List<TagGroup>>(json, SerializerSettings.Default);
				File.WriteAllText(_paths.ModulePath + "TagGroups.json", json);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Loaded default TagGroups.json");
			}
			foreach (TagGroup group in _groups)
			{
				group.Icon = new DetailedTexture(group.AssetId);
				group.PropertyChanged += new PropertyAndValueChangedEventHandler(Tag_PropertyChanged);
			}
		}

		private void Tag_PropertyChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			TagGroup tag = sender as TagGroup;
			if (tag != null)
			{
				OnTagChanged(tag, e);
			}
		}

		private void Tag_TagChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			TagGroup tag = sender as TagGroup;
			if (tag != null)
			{
				OnTagChanged(tag, e);
			}
		}

		private void OnTagChanged(TagGroup tag, PropertyAndValueChangedEventArgs e)
		{
			this.GroupChanged?.Invoke(tag, e);
			RequestSave();
		}

		public void Add(TagGroup tag)
		{
			TagGroup tag2 = tag;
			if (!_groups.Any((TagGroup t) => t.Name == tag2.Name))
			{
				_groups.Add(tag2);
				tag2.PropertyChanged += new PropertyAndValueChangedEventHandler(Tag_TagChanged);
				OnTagAdded(tag2);
			}
		}

		private void OnTagAdded(TagGroup tag)
		{
			this.GroupAdded?.Invoke(this, tag);
			RequestSave();
		}

		private void OnTagRemoved(TagGroup tag)
		{
			this.GroupRemoved?.Invoke(this, tag);
			RequestSave();
		}

		public bool Remove(TagGroup tag)
		{
			if (_groups.Remove(tag))
			{
				tag.PropertyChanged -= new PropertyAndValueChangedEventHandler(Tag_TagChanged);
				OnTagRemoved(tag);
				return true;
			}
			return false;
		}

		private void RequestSave()
		{
			_saveRequested = true;
			if (_saveRequested)
			{
				_timer.Stop();
				_timer.Start();
			}
		}

		public async Task Save()
		{
			try
			{
				_tokenSource?.Cancel();
				_tokenSource = new CancellationTokenSource();
				await Task.Delay(1000, _tokenSource.Token);
				string json = JsonConvert.SerializeObject((object)_groups, SerializerSettings.Default);
				if (!_tokenSource.IsCancellationRequested)
				{
					File.WriteAllText(_paths.ModulePath + "TagGroups.json", json);
				}
			}
			catch (Exception ex)
			{
				if (!(ex is TaskCanceledException))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to save TagGroups.json");
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
				}
			}
		}

		public IEnumerator<TagGroup> GetEnumerator()
		{
			_groups.Sort(TemplateTagComparer.CompareGroups);
			return _groups.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
