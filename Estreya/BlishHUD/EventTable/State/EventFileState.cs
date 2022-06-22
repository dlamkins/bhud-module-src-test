using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models.Settings;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.State
{
	public class EventFileState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<EventFileState>();

		private const string WEB_SOURCE_URL = "https://files.blishhud.estreya.de/event-table/events.json";

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

		protected override async Task InternalReload()
		{
			await CheckAndNotifyOrUpdate(null);
		}

		protected override async Task Initialize()
		{
			if (!ExternalFileExists())
			{
				await ExportFile();
			}
			timeSinceUpdate = updateInterval.TotalMilliseconds;
		}

		protected override void InternalUnload()
		{
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(CheckAndNotifyOrUpdate, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		private async Task CheckAndNotifyOrUpdate(GameTime gameTime)
		{
			lock (_lockObject)
			{
				if (_notified)
				{
					return;
				}
			}
			if (!(await IsNewFileVersionAvaiable()))
			{
				return;
			}
			if (EventTableModule.ModuleInstance.ModuleSettings.AutomaticallyUpdateEventFile.get_Value())
			{
				await ExportFile();
				await EventTableModule.ModuleInstance.LoadEvents();
				return;
			}
			ScreenNotification.ShowNotification(new string[2] { "A new version of the event file is available.", "Please update it from the settings window." }, (NotificationType)0, null, 10);
			lock (_lockObject)
			{
				_notified = true;
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

		public async Task<EventSettingsFile> GetInternalFile()
		{
			_ = 1;
			try
			{
				try
				{
					Logger.Debug("Loading json from web source.");
					string webJson = await EventTableModule.ModuleInstance.GetWebClient().DownloadStringTaskAsync(new Uri("https://files.blishhud.estreya.de/event-table/events.json"));
					Logger.Debug($"Got content (length): {webJson?.Length ?? 0}");
					if (!string.IsNullOrWhiteSpace(webJson))
					{
						EventSettingsFile webEventSettingFile = JsonConvert.DeserializeObject<EventSettingsFile>(webJson);
						if (webEventSettingFile.MinimumModuleVersion.IsSatisfied(((Module)EventTableModule.ModuleInstance).get_Version().BaseVersion()))
						{
							Logger.Debug("Module statisfies min web file version");
							return webEventSettingFile;
						}
						Logger.Debug("Module does not statisfy min web file version");
					}
				}
				catch (Exception ex2)
				{
					Logger.Error(ex2, "Could not read json from web source.");
				}
				Logger.Debug("Load json from internal ref.");
				using Stream stream = ContentsManager.GetFileStream("events.json");
				EventSettingsFile internalEventSettingFile = JsonConvert.DeserializeObject<EventSettingsFile>(await FileUtil.ReadStringAsync(stream));
				if (internalEventSettingFile.MinimumModuleVersion.IsSatisfied(((Module)EventTableModule.ModuleInstance).get_Version().BaseVersion()))
				{
					Logger.Debug("Module statisfies min internal file version");
					return internalEventSettingFile;
				}
				Logger.Debug("Module does not statisfy min internal file version");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Could not load internal file.");
			}
			return null;
		}

		public async Task<EventSettingsFile> GetExternalFile()
		{
			try
			{
				return JsonConvert.DeserializeObject<EventSettingsFile>(await FileUtil.ReadStringAsync(FilePath));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Could not load external file.");
			}
			return null;
		}

		private async Task<bool> IsNewFileVersionAvaiable()
		{
			_ = 1;
			try
			{
				EventSettingsFile internalEventFile = await GetInternalFile();
				EventSettingsFile externalEventFile = await GetExternalFile();
				return internalEventFile?.Version > externalEventFile?.Version;
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to check for new file version: " + ex.Message);
				return false;
			}
		}

		internal async Task ExportFile(EventSettingsFile eventSettingsFile)
		{
			if (eventSettingsFile == null)
			{
				eventSettingsFile = new EventSettingsFile();
			}
			string content = JsonConvert.SerializeObject((object)eventSettingsFile, (Formatting)1);
			await FileUtil.WriteStringAsync(FilePath, content);
			await Clear();
		}

		public async Task ExportFile()
		{
			await ExportFile(await GetInternalFile());
		}

		public override Task Clear()
		{
			lock (_lockObject)
			{
				_notified = false;
			}
			return Task.CompletedTask;
		}
	}
}
