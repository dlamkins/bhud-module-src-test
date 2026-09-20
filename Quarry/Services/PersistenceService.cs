using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Debug;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.Models.Persistence;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class PersistenceService : IPersistenceService, IDisposable
	{
		private const string SAVE_FILE_NAME = "persistanceStorage.json";

		private readonly DirectoriesManager directoriesManager;

		private readonly AchievementTrackerService achievementTrackerService;

		private readonly Logger logger;

		private readonly AchievementService achievementService;

		private readonly IHuntService huntService;

		private readonly IHereExclusionService hereExclusionService;

		private Storage storage;

		private Task autoSaveTask;

		private CancellationTokenSource autoSaveCancellationTokenSource;

		private DateTime? lastKnownFileWriteTimeUtc;

		private readonly SettingEntry<bool> autoSaveSetting;

		public event Action AutoSave;

		public PersistenceService(DirectoriesManager directoriesManager, AchievementTrackerService achievementTrackerService, Logger logger, AchievementService achievementService, IHuntService huntService, IHereExclusionService hereExclusionService, SettingEntry<bool> autoSave)
		{
			this.directoriesManager = directoriesManager;
			this.achievementTrackerService = achievementTrackerService;
			this.logger = logger;
			this.achievementService = achievementService;
			this.huntService = huntService;
			this.hereExclusionService = hereExclusionService;
			autoSaveSetting = autoSave;
			autoSaveSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoSaveSetting_SettingChanged);
			if (autoSave.get_Value())
			{
				InitializeAutoSaveTask();
			}
		}

		private void AutoSaveSetting_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				InitializeAutoSaveTask();
			}
			else
			{
				ResetAutoSaveTask();
			}
		}

		public void Dispose()
		{
			autoSaveSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoSaveSetting_SettingChanged);
			ResetAutoSaveTask();
		}

		private void ResetAutoSaveTask()
		{
			if (autoSaveTask != null)
			{
				autoSaveCancellationTokenSource.Cancel();
				autoSaveCancellationTokenSource.Dispose();
				autoSaveCancellationTokenSource = null;
				autoSaveTask = null;
			}
		}

		private void InitializeAutoSaveTask()
		{
			ResetAutoSaveTask();
			autoSaveCancellationTokenSource = new CancellationTokenSource();
			autoSaveTask = Task.Run(async delegate
			{
				while (true)
				{
					try
					{
						await Task.Delay(TimeSpan.FromMinutes(5.0), autoSaveCancellationTokenSource.Token);
					}
					catch (TaskCanceledException)
					{
						return;
					}
					try
					{
						this.AutoSave?.Invoke();
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Exception occured during autosave; will retry next cycle");
					}
				}
			}, autoSaveCancellationTokenSource.Token);
		}

		private string GetSaveFilePath()
		{
			return Path.Combine(directoriesManager.GetFullDirectoryPath("quarry"), "persistanceStorage.json");
		}

		public void Save(int achievementTrackWindowLocationX, int achievementTrackWindowLocationY, bool showTrackWindow, int trackWindowCompactWidth, int trackWindowCompactHeight, int overviewWindowWidth = -1, int overviewWindowHeight = -1)
		{
			string file = GetSaveFilePath();
			if (File.Exists(file) && lastKnownFileWriteTimeUtc.HasValue && File.GetLastWriteTimeUtc(file) > lastKnownFileWriteTimeUtc.Value)
			{
				logger.Info("persistanceStorage.json changed on disk since our last save (likely an external edit) -- merging before writing.");
				Reload();
			}
			try
			{
				Storage storage = new Storage();
				List<int> trackedAchievements = achievementTrackerService.ActiveAchievements.ToList();
				lock (achievementService.ManualCompletedSync)
				{
					storage.ManualCompletedAchievements = achievementService.ManualCompletedAchievements.ToDictionary((KeyValuePair<int, List<int>> kv) => kv.Key, (KeyValuePair<int, List<int>> kv) => new List<int>(kv.Value));
				}
				storage.TrackedAchievements.AddRange(trackedAchievements);
				storage.HuntEnabledNamespaces = huntService.HuntEnabledNamespaces.ToDictionary((KeyValuePair<int, List<string>> kv) => kv.Key, (KeyValuePair<int, List<string>> kv) => new List<string>(kv.Value));
				storage.HiddenAchievements = hereExclusionService.HiddenAchievementIds.ToList();
				storage.SnoozedAchievements = hereExclusionService.SnoozedUntilUtc.ToDictionary((KeyValuePair<int, DateTime> kv) => kv.Key, (KeyValuePair<int, DateTime> kv) => kv.Value);
				storage.TrackWindowLocationX = achievementTrackWindowLocationX;
				storage.TrackWindowLocationY = achievementTrackWindowLocationY;
				storage.ShowTrackWindow = showTrackWindow;
				storage.TrackWindowCompactWidth = trackWindowCompactWidth;
				storage.TrackWindowCompactHeight = trackWindowCompactHeight;
				storage.OverviewWindowWidth = ((overviewWindowWidth > 0) ? overviewWindowWidth : Get().OverviewWindowWidth);
				storage.OverviewWindowHeight = ((overviewWindowHeight > 0) ? overviewWindowHeight : Get().OverviewWindowHeight);
				Directory.CreateDirectory(Path.GetDirectoryName(file));
				File.WriteAllText(file, JsonSerializer.Serialize(storage));
				this.storage = storage;
				lastKnownFileWriteTimeUtc = File.GetLastWriteTimeUtc(file);
			}
			catch (UnauthorizedAccessException ex2)
			{
				logger.Error((Exception)ex2, "Access denied writing persistanceStorage.json; nothing tracked this session will survive a restart.");
				Contingency.NotifyFileSaveAccessDenied(file, "save your tracked achievements", false);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on saving persistent information");
			}
		}

		public Storage Get()
		{
			try
			{
				if (storage == null)
				{
					storage = ReadStorageFromDisk();
				}
				return storage;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on reading persistent information");
				return new Storage();
			}
		}

		public void Reload()
		{
			try
			{
				storage = ReadStorageFromDisk();
				int mergedCount = 0;
				foreach (int achievementId in storage.TrackedAchievements)
				{
					if (achievementService.Achievements != null && !achievementService.Achievements.Any((AchievementTableEntry x) => x.Id == achievementId))
					{
						logger.Warn(string.Format("{0}: rejecting tracked id {1} -- not found in wiki achievement data.", "persistanceStorage.json", achievementId));
						continue;
					}
					achievementTrackerService.TrackAchievement(achievementId);
					mergedCount++;
				}
				logger.Info(string.Format("Reloaded {0}: merged {1} tracked achievement(s) from disk.", "persistanceStorage.json", mergedCount));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on reloading persistent information");
			}
		}

		private Storage ReadStorageFromDisk()
		{
			string file = GetSaveFilePath();
			if (!File.Exists(file))
			{
				return new Storage();
			}
			try
			{
				Storage? result = JsonSerializer.Deserialize<Storage>(File.ReadAllText(file));
				lastKnownFileWriteTimeUtc = File.GetLastWriteTimeUtc(file);
				return result;
			}
			catch (Exception ex)
			{
				string badFile = file + ".bad";
				try
				{
					File.Copy(file, badFile, overwrite: true);
				}
				catch (Exception copyEx)
				{
					logger.Warn(copyEx, "Failed to back up corrupt persistanceStorage.json to \"" + badFile + "\"");
				}
				logger.Warn(ex, "persistanceStorage.json is corrupt; backed up to \"" + badFile + "\" and continuing with an empty tracked set.");
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					ScreenNotification.ShowNotification("Quarry: saved data was unreadable and was reset. A backup was kept as persistanceStorage.json.bad", (NotificationType)1, (Texture2D)null, 8);
				});
				lastKnownFileWriteTimeUtc = File.GetLastWriteTimeUtc(file);
				return new Storage();
			}
		}
	}
}
