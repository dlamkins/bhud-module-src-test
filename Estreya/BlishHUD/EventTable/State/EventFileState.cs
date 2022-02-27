using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models.Settings;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.State
{
	public class EventFileState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<EventFileState>();

		private TimeSpan updateInterval = TimeSpan.FromHours(1.0);

		private double timeSinceUpdate;

		private static object _lockObject = new object();

		private bool _notified;

		private string Directory { get; set; }

		private string FileName { get; set; }

		private string FilePath => Path.Combine(Directory, FileName);

		private ContentsManager ContentsManager { get; set; }

		public EventFileState(ContentsManager contentsManager, string directory, string fileName)
		{
			ContentsManager = contentsManager;
			Directory = directory;
			FileName = fileName;
		}

		public override async Task Reload()
		{
			await CheckAndNotify(null);
		}

		protected override async Task Initialize()
		{
			bool flag = !ExternalFileExists();
			if (!flag)
			{
				bool flag2 = EventTableModule.ModuleInstance.ModuleSettings.AutomaticallyUpdateEventFile.get_Value();
				if (flag2)
				{
					flag2 = await IsNewFileVersionAvaiable();
				}
				flag = flag2;
			}
			if (flag)
			{
				await ExportFile();
			}
			timeSinceUpdate = updateInterval.TotalMilliseconds;
		}

		protected override Task InternalUnload()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(CheckAndNotify, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		private async Task CheckAndNotify(GameTime gameTime)
		{
			lock (_lockObject)
			{
				if (_notified)
				{
					return;
				}
			}
			if (await IsNewFileVersionAvaiable())
			{
				ScreenNotification.ShowNotification("Please update it from the settings window.", (NotificationType)0, (Texture2D)null, 10);
				ScreenNotification.ShowNotification("A new version of the event file is available.", (NotificationType)0, (Texture2D)null, 10);
				lock (_lockObject)
				{
					_notified = true;
				}
			}
		}

		private bool ExternalFileExists()
		{
			try
			{
				return File.Exists(FilePath);
			}
			catch (Exception ex)
			{
				Logger.Error("Check for existing external file failed: " + ex.Message);
				throw ex;
			}
		}

		private async Task<string> GetInternalFileContent()
		{
			using StreamReader eventsReader = new StreamReader(ContentsManager.GetFileStream("events.json"));
			return await eventsReader.ReadToEndAsync();
		}

		public async Task<string> GetExternalFileContent()
		{
			using StreamReader eventsReader = new StreamReader(FilePath);
			return await eventsReader.ReadToEndAsync();
		}

		private async Task<bool> IsNewFileVersionAvaiable()
		{
			_ = 1;
			try
			{
				EventSettingsFile internalEventFile = JsonConvert.DeserializeObject<EventSettingsFile>(await GetInternalFileContent());
				EventSettingsFile externalEventFile = JsonConvert.DeserializeObject<EventSettingsFile>(await GetExternalFileContent());
				return internalEventFile.Version > externalEventFile.Version;
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to check for new file version: " + ex.Message);
				return false;
			}
		}

		public async Task ExportFile()
		{
			string json = await GetInternalFileContent();
			File.WriteAllText(FilePath, json);
		}
	}
}
