using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models;
using Denrage.AchievementTrackerModule.Models.Persistance;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Services
{
	public class PersistanceService : IPersistanceService
	{
		private const string SAVE_FILE_NAME = "persistanceStorage.json";

		private Storage storage;

		private readonly DirectoriesManager directoriesManager;

		private readonly AchievementDetailsWindowManager achievementDetailsWindowManager;

		private readonly ItemDetailWindowManager itemDetailWindowManager;

		private readonly AchievementTrackerService achievementTrackerService;

		private readonly Logger logger;

		public PersistanceService(DirectoriesManager directoriesManager, AchievementDetailsWindowManager achievementDetailsWindowManager, ItemDetailWindowManager itemDetailWindowManager, AchievementTrackerService achievementTrackerService, Logger logger)
		{
			this.directoriesManager = directoriesManager;
			this.achievementDetailsWindowManager = achievementDetailsWindowManager;
			this.itemDetailWindowManager = itemDetailWindowManager;
			this.achievementTrackerService = achievementTrackerService;
			this.logger = logger;
		}

		public void Save(int achievementTrackWindowLocationX, int achievementTrackWindowLocationY, bool showTrackWindow)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			Storage storage = new Storage();
			foreach (KeyValuePair<int, AchievementDetailsWindow> item2 in achievementDetailsWindowManager.Windows.Where((KeyValuePair<int, AchievementDetailsWindow> x) => ((Control)x.Value).get_Visible()))
			{
				storage.AchievementInformation[item2.Key] = new AchievementWindowInformation
				{
					AchievementId = item2.Value.AchievementId,
					PositionX = ((Control)item2.Value).get_Location().X,
					PositionY = ((Control)item2.Value).get_Location().Y
				};
			}
			foreach (KeyValuePair<string, ItemDetailWindowInformation> item in itemDetailWindowManager.Windows.Where((KeyValuePair<string, ItemDetailWindowInformation> x) => ((Control)x.Value.Window).get_Visible()))
			{
				if (!storage.ItemInformation.TryGetValue(item.Value.AchievementId, out var itemWindows))
				{
					itemWindows = new Dictionary<int, ItemInformation>();
					storage.ItemInformation[item.Value.AchievementId] = itemWindows;
				}
				itemWindows[item.Value.ItemIndex] = new ItemInformation
				{
					AchievementId = item.Value.AchievementId,
					Index = item.Value.ItemIndex,
					Name = item.Value.Name,
					PositionX = ((Control)item.Value.Window).get_Location().X,
					PositionY = ((Control)item.Value.Window).get_Location().Y
				};
			}
			storage.TrackedAchievements.AddRange(achievementTrackerService.ActiveAchievements);
			storage.TrackWindowLocationX = achievementTrackWindowLocationX;
			storage.TrackWindowLocationY = achievementTrackWindowLocationY;
			storage.ShowTrackWindow = showTrackWindow;
			try
			{
				string fullDirectoryPath = directoriesManager.GetFullDirectoryPath("achievement_module");
				Directory.CreateDirectory(fullDirectoryPath);
				File.WriteAllText(Path.Combine(fullDirectoryPath, "persistanceStorage.json"), JsonSerializer.Serialize<Storage>(storage, (JsonSerializerOptions)null));
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
				string file = Path.Combine(directoriesManager.GetFullDirectoryPath("achievement_module"), "persistanceStorage.json");
				if (storage == null)
				{
					storage = (File.Exists(file) ? JsonSerializer.Deserialize<Storage>(File.ReadAllText(file), (JsonSerializerOptions)null) : new Storage());
				}
				return storage;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on reading persistent information");
				return new Storage();
			}
		}
	}
}
