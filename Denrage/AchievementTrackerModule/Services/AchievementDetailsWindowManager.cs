using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.Models.Persistance;
using Denrage.AchievementTrackerModule.UserInterface.Windows;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementDetailsWindowManager : IAchievementDetailsWindowManager, IDisposable
	{
		private readonly IAchievementDetailsWindowFactory achievementDetailsWindowFactory;

		private readonly IAchievementControlManager achievementControlManager;

		private readonly IAchievementService achievementService;

		private readonly Logger logger;

		private readonly List<AchievementDetailsWindow> hiddenWindows = new List<AchievementDetailsWindow>();

		internal Dictionary<int, AchievementDetailsWindow> Windows { get; } = new Dictionary<int, AchievementDetailsWindow>();


		public event Action<int> WindowHidden;

		public AchievementDetailsWindowManager(IAchievementDetailsWindowFactory achievementDetailsWindowFactory, IAchievementControlManager achievementControlManager, IAchievementService achievementService, Logger logger)
		{
			this.achievementDetailsWindowFactory = achievementDetailsWindowFactory;
			this.achievementControlManager = achievementControlManager;
			this.achievementService = achievementService;
			this.logger = logger;
		}

		public void Load(IPersistanceService persistanceService)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				foreach (KeyValuePair<int, AchievementWindowInformation> item in persistanceService.Get().AchievementInformation)
				{
					AchievementTableEntry achievement = achievementService.Achievements.FirstOrDefault((AchievementTableEntry x) => x.Id == item.Key);
					CreateWindow(achievement);
					((Control)Windows[item.Key]).set_Location(new Point(item.Value.PositionX, item.Value.PositionY));
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on restoring window positions");
			}
		}

		public void CreateWindow(AchievementTableEntry achievement)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			AchievementDetailsWindow window = achievementDetailsWindowFactory.Create(achievement);
			((Control)window).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)window).set_Location(((Control)GameService.Graphics.get_SpriteScreen()).get_Size() / new Point(2) - new Point(256, 178) / new Point(2));
			((Control)window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				if (!hiddenWindows.Any())
				{
					Windows.Remove(achievement.Id);
					achievementControlManager.RemoveParent(achievement.Id);
					((Control)window).Dispose();
					this.WindowHidden?.Invoke(achievement.Id);
				}
			});
			Windows[achievement.Id] = window;
			if (GameService.Gw2Mumble.get_IsAvailable() && (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen()) && !hiddenWindows.Contains(window))
			{
				hiddenWindows.Add(window);
			}
			else
			{
				((Control)window).Show();
			}
		}

		public bool WindowExists(int achievementId)
		{
			return Windows.ContainsKey(achievementId);
		}

		public void Update()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				foreach (KeyValuePair<int, AchievementDetailsWindow> item in Windows)
				{
					hiddenWindows.Add(item.Value);
					((Control)item.Value).Hide();
				}
			}
			else
			{
				if (!hiddenWindows.Any())
				{
					return;
				}
				foreach (AchievementDetailsWindow hiddenWindow in hiddenWindows)
				{
					((Control)hiddenWindow).Show();
				}
				hiddenWindows.Clear();
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<int, AchievementDetailsWindow> window in Windows)
			{
				((Control)window.Value).Dispose();
			}
			Windows.Clear();
		}
	}
}
