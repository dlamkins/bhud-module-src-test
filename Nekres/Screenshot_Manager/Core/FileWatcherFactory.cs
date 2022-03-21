using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Screenshot_Manager.Properties;
using Nekres.Screenshot_Manager.UI.Controls;
using Nekres.Screenshot_Manager.UI.Models;
using Nekres.Screenshot_Manager.UI.Views;
using Nekres.Screenshot_Manager_Module.Controls;

namespace Nekres.Screenshot_Manager.Core
{
	public class FileWatcherFactory : IDisposable
	{
		public const int NewFileNotificationDelay = 300;

		private string[] _imageFilters;

		private List<FileSystemWatcher> _screensPathWatchers;

		private readonly List<string> _index;

		public IReadOnlyList<string> Index => new List<string>(_index);

		public event EventHandler<ValueEventArgs<string>> FileAdded;

		public event EventHandler<ValueEventArgs<string>> FileDeleted;

		public event EventHandler<ValueChangedEventArgs<string>> FileRenamed;

		public FileWatcherFactory()
		{
			_index = new List<string>();
			_screensPathWatchers = new List<FileSystemWatcher>();
			_imageFilters = new string[3] { "*.bmp", "*.jpg", "*.png" };
			string[] imageFilters = _imageFilters;
			foreach (string filter2 in imageFilters)
			{
				FileSystemWatcher watcher = new FileSystemWatcher
				{
					Path = DirectoryUtil.get_ScreensPath(),
					NotifyFilter = (NotifyFilters.FileName | NotifyFilters.LastWrite),
					Filter = filter2,
					EnableRaisingEvents = true
				};
				watcher.Created += OnScreenShotCreated;
				watcher.Deleted += OnScreenShotDeleted;
				watcher.Renamed += OnScreenShotRenamed;
				watcher.EnableRaisingEvents = true;
				_screensPathWatchers.Add(watcher);
			}
			IEnumerable<string> initialFiles = from s in Directory.EnumerateFiles(DirectoryUtil.get_ScreensPath())
				where Array.Exists(_imageFilters, (string filter) => filter.Equals("*" + Path.GetExtension(s), StringComparison.InvariantCultureIgnoreCase))
				select s into x
				select Path.Combine(DirectoryUtil.get_ScreensPath(), x);
			_index.AddRange(initialFiles);
		}

		private async void OnScreenShotCreated(object sender, FileSystemEventArgs e)
		{
			_index.Add(e.FullPath);
			await ScreenShotNotify(e.FullPath);
			this.FileAdded?.Invoke(this, new ValueEventArgs<string>(e.FullPath));
		}

		private void OnScreenShotDeleted(object sender, FileSystemEventArgs e)
		{
			_index.Remove(e.FullPath);
			this.FileDeleted?.Invoke(this, new ValueEventArgs<string>(e.FullPath));
		}

		private void OnScreenShotRenamed(object sender, RenamedEventArgs e)
		{
			_index.Remove(e.OldFullPath);
			_index.Add(e.FullPath);
			this.FileRenamed?.Invoke(this, new ValueChangedEventArgs<string>(e.OldFullPath, e.FullPath));
		}

		private async Task ScreenShotNotify(string filePath)
		{
			if (!ScreenshotManagerModule.ModuleInstance.MuteSound.get_Value())
			{
				ScreenshotManagerModule.ModuleInstance.ScreenShotSfx.Play();
			}
			if (ScreenshotManagerModule.ModuleInstance.DisableNotification.get_Value())
			{
				return;
			}
			AsyncTexture2D texture;
			await Task.Delay(300).ContinueWith((Func<Task, Task>)async delegate
			{
				DateTime timeout = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < timeout)
				{
					try
					{
						texture = new AsyncTexture2D();
						ScreenshotNotification.ShowNotification(texture, filePath, Resources.Screenshot_Created_, 5f, delegate
						{
							OpenInspectionPanel(filePath);
						});
						await TextureUtil.GetThumbnail(filePath).ContinueWith(delegate(Task<Texture2D> t)
						{
							texture.SwapTexture(t.Result);
						});
						return;
					}
					catch (InvalidOperationException ex)
					{
						if (!(DateTime.UtcNow < timeout))
						{
							ScreenshotManagerModule.Logger.Error(ex.Message);
							return;
						}
					}
				}
			});
		}

		private async void OpenInspectionPanel(string filePath)
		{
			await CreateInspectionPanel(filePath);
			((Control)GameService.Overlay.get_BlishHudWindow()).Show();
			GameService.Overlay.get_BlishHudWindow().Navigate((IView)(object)new ScreenshotManagerView(new ScreenshotManagerModel(this)), true);
		}

		public async Task CreateInspectionPanel(string fileName)
		{
			AsyncTexture2D texture = new AsyncTexture2D();
			new InspectPanel(texture, Path.GetFileNameWithoutExtension(fileName));
			await TextureUtil.GetScreenShot(fileName).ContinueWith(delegate(Task<Texture2D> t)
			{
				texture.SwapTexture(t.Result);
			});
		}

		public void Dispose()
		{
			foreach (FileSystemWatcher screensPathWatcher in _screensPathWatchers)
			{
				screensPathWatcher.Created -= OnScreenShotCreated;
				screensPathWatcher.Deleted -= OnScreenShotDeleted;
				screensPathWatcher.Renamed -= OnScreenShotRenamed;
				screensPathWatcher.Dispose();
			}
		}
	}
}
