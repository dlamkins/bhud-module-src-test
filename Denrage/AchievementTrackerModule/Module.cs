using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Persistance;
using Denrage.AchievementTrackerModule.Services;
using Denrage.AchievementTrackerModule.UserInterface.Views;
using Denrage.AchievementTrackerModule.UserInterface.Windows;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private readonly DependencyInjectionContainer dependencyInjectionContainer;

		private readonly Logger logger;

		private Func<IView> achievementOverviewView;

		private AchievementTrackWindow window;

		private CornerIcon cornerIcon;

		private bool purposelyHidden;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			logger = Logger;
			dependencyInjectionContainer = new DependencyInjectionContainer(Gw2ApiManager, ContentsManager, GameService.Content, DirectoriesManager, logger);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)async delegate
			{
				logger.Info("Subtoken updated");
				if (dependencyInjectionContainer != null && dependencyInjectionContainer.AchievementService != null)
				{
					await (dependencyInjectionContainer?.AchievementService?.LoadPlayerAchievements());
				}
			});
		}

		protected override async Task LoadAsync()
		{
			Task.Run(async delegate
			{
				achievementOverviewView = () => (IView)(object)new AchievementTrackerView(dependencyInjectionContainer.AchievementItemOverviewFactory, dependencyInjectionContainer.AchievementService);
				await Task.Delay(TimeSpan.FromSeconds(3.0));
				await dependencyInjectionContainer.InitializeAsync();
				dependencyInjectionContainer.AchievementTrackerService.AchievementTracked += AchievementTrackerService_AchievementTracked;
				if (dependencyInjectionContainer.PersistanceService.Get().ShowTrackWindow)
				{
					InitializeWindow();
					((Control)window).Show();
				}
				GameService.Overlay.get_BlishHudWindow().AddTab("Achievement Tracker", AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("achievement_icon.png")), achievementOverviewView, 0);
				CornerIcon val = new CornerIcon();
				val.set_IconName("Open Achievement Panel");
				val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon_inactive.png")));
				val.set_HoverIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon_active.png")));
				((Control)val).set_Width(64);
				((Control)val).set_Height(64);
				cornerIcon = val;
				((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					InitializeWindow();
					((WindowBase2)window).ToggleWindow();
				});
			});
			await _003C_003En__0();
		}

		private void InitializeWindow()
		{
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			if (window == null)
			{
				AchievementTrackWindow achievementTrackWindow = new AchievementTrackWindow(ContentsManager, dependencyInjectionContainer.AchievementTrackerService, dependencyInjectionContainer.AchievementControlProvider, dependencyInjectionContainer.AchievementService, dependencyInjectionContainer.AchievementDetailsWindowManager, dependencyInjectionContainer.AchievementControlManager, dependencyInjectionContainer.SubPageInformationWindowManager, GameService.Overlay, dependencyInjectionContainer.FormattedLabelHtmlService, achievementOverviewView);
				((Control)achievementTrackWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				window = achievementTrackWindow;
				Storage savedWindowLocation = dependencyInjectionContainer.PersistanceService.Get();
				logger.Info($"SavedWindowLocation -  X:{savedWindowLocation.TrackWindowLocationX} Y:{savedWindowLocation.TrackWindowLocationY}");
				((Control)window).set_Location((Point)((savedWindowLocation.TrackWindowLocationX == -1 || savedWindowLocation.TrackWindowLocationY == -1) ? (((Control)GameService.Graphics.get_SpriteScreen()).get_Size() / new Point(2) - new Point(256, 178) / new Point(2)) : new Point(savedWindowLocation.TrackWindowLocationX, savedWindowLocation.TrackWindowLocationY)));
				logger.Info($"AchievementTrackWindowLocation -  X:{((Control)window).get_Location().X} Y:{((Control)window).get_Location().Y}");
			}
		}

		private void AchievementTrackerService_AchievementTracked(int achievement)
		{
			InitializeWindow();
			if (!((Control)window).get_Visible())
			{
				((Control)window).Show();
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			dependencyInjectionContainer?.ItemDetailWindowManager?.Update();
			dependencyInjectionContainer?.AchievementDetailsWindowManager?.Update();
			if (!GameService.Gw2Mumble.get_IsAvailable() || window == null)
			{
				return;
			}
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				if (((Control)window).get_Visible())
				{
					purposelyHidden = true;
					((Control)window).Hide();
				}
			}
			else if (purposelyHidden)
			{
				((Control)window).Show();
				purposelyHidden = false;
			}
		}

		protected override void Unload()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			AchievementTrackWindow achievementTrackWindow = window;
			Point location = (Point)((achievementTrackWindow != null) ? ((Control)achievementTrackWindow).get_Location() : new Point(-1, -1));
			IPersistanceService persistanceService = dependencyInjectionContainer.PersistanceService;
			if (persistanceService != null)
			{
				int x = location.X;
				int y = location.Y;
				AchievementTrackWindow achievementTrackWindow2 = window;
				persistanceService.Save(x, y, achievementTrackWindow2 != null && ((Control)achievementTrackWindow2).get_Visible());
			}
			CornerIcon obj = cornerIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			AchievementTrackWindow achievementTrackWindow3 = window;
			if (achievementTrackWindow3 != null)
			{
				((Control)achievementTrackWindow3).Dispose();
			}
		}
	}
}
