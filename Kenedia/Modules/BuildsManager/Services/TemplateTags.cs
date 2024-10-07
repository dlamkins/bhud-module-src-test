using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class TemplateTags : IEnumerable<TemplateTag>, IEnumerable
	{
		private readonly System.Timers.Timer _timer;

		private readonly ContentsManager _contentsManager;

		private readonly Paths _paths;

		private CancellationTokenSource _tokenSource;

		private List<TemplateTag> _tags;

		private bool _saveRequested;

		public TagGroups TagGroups { get; }

		public event PropertyChangedEventHandler TagChanged;

		public event EventHandler<TemplateTag> TagAdded;

		public event EventHandler<TemplateTag> TagRemoved;

		public TemplateTags(ContentsManager contentsManager, Paths paths, TagGroups tagGroups)
		{
			_contentsManager = contentsManager;
			_paths = paths;
			TagGroups = tagGroups;
			_timer = new System.Timers.Timer(1000.0);
			_timer.Elapsed += OnTimerElapsed;
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_TagRemoved);
			TagGroups.GroupChanged += new PropertyAndValueChangedEventHandler(TagGroups_GroupChanged);
		}

		private void TagGroups_GroupChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			if (!(sender is TagGroup) || !(e.PropertyName == "Name"))
			{
				return;
			}
			string old = e.OldValue as string;
			string oldGroup = ((old != null) ? old : string.Empty);
			string newgrp = e.NewValue as string;
			string newGroup = ((newgrp != null) ? newgrp : string.Empty);
			List<TemplateTag> tags = _tags;
			List<TemplateTag> list = new List<TemplateTag>(tags.Count);
			list.AddRange(tags);
			foreach (TemplateTag tag in list)
			{
				if (tag.Group == oldGroup)
				{
					tag.Group = newGroup;
				}
			}
		}

		private void TagGroups_TagRemoved(object sender, TagGroup e)
		{
			List<TemplateTag> tags = _tags;
			List<TemplateTag> list = new List<TemplateTag>(tags.Count);
			list.AddRange(tags);
			foreach (TemplateTag tag in list)
			{
				if (tag.Group == e.Name)
				{
					tag.Group = string.Empty;
				}
			}
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
			if (File.Exists(_paths.ModulePath + "TemplateTags.json"))
			{
				try
				{
					if (File.Exists(_paths.ModulePath + "TemplateTags.json"))
					{
						string json2 = File.ReadAllText(_paths.ModulePath + "TemplateTags.json");
						_tags = JsonConvert.DeserializeObject<List<TemplateTag>>(json2, SerializerSettings.Default);
					}
				}
				catch (Exception ex)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to load TemplateTags.json");
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
				}
			}
			else
			{
				string json = await new StreamReader(_contentsManager.GetFileStream("data\\TemplateTags.json")).ReadToEndAsync();
				_tags = JsonConvert.DeserializeObject<List<TemplateTag>>(json, SerializerSettings.Default);
				File.WriteAllText(_paths.ModulePath + "TemplateTags.json", json);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Loaded default TemplateTags.json");
			}
			foreach (TemplateTag tag in _tags)
			{
				tag.Icon = new DetailedTexture(tag.AssetId);
				tag.PropertyChanged += new PropertyChangedEventHandler(Tag_PropertyChanged);
			}
		}

		private void Tag_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag tag = sender as TemplateTag;
			if (tag != null)
			{
				OnTagChanged(tag, e);
			}
		}

		private void Tag_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag tag = sender as TemplateTag;
			if (tag != null)
			{
				OnTagChanged(tag, e);
			}
		}

		private void OnTagChanged(TemplateTag tag, PropertyChangedEventArgs e)
		{
			this.TagChanged?.Invoke(tag, e);
			RequestSave();
		}

		public void Add(TemplateTag tag)
		{
			TemplateTag tag2 = tag;
			if (!_tags.Any((TemplateTag t) => t.Name == tag2.Name))
			{
				_tags.Add(tag2);
				tag2.PropertyChanged += new PropertyChangedEventHandler(Tag_TagChanged);
				OnTagAdded(tag2);
			}
		}

		private void OnTagAdded(TemplateTag tag)
		{
			this.TagAdded?.Invoke(this, tag);
			RequestSave();
		}

		private void OnTagRemoved(TemplateTag tag)
		{
			this.TagRemoved?.Invoke(this, tag);
			RequestSave();
		}

		public bool Remove(TemplateTag tag)
		{
			if (_tags.Remove(tag))
			{
				tag.PropertyChanged -= new PropertyChangedEventHandler(Tag_TagChanged);
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
				string json = JsonConvert.SerializeObject((object)_tags, SerializerSettings.Default);
				if (!_tokenSource.IsCancellationRequested)
				{
					File.WriteAllText(_paths.ModulePath + "TemplateTags.json", json);
				}
			}
			catch (Exception ex)
			{
				if (!(ex is TaskCanceledException))
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to save TemplateTags.json");
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
				}
			}
		}

		public IEnumerator<TemplateTag> GetEnumerator()
		{
			_tags.Sort(new TemplateTagComparer(TagGroups));
			return _tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
