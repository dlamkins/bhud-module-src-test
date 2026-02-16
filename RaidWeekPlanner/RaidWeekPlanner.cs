using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidWeekPlanner.Services;
using RaidWeekPlanner.UI.Controls;
using RaidWeekPlanner.UI.Views;
using SemVer;

namespace RaidWeekPlanner
{
	[Export(typeof(Module))]
	public class RaidWeekPlanner : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<RaidWeekPlanner>();

		internal static RaidWeekPlanner RaidWeekPlannerInstance;

		private AsyncTexture2D _windowBackgroundTexture;

		private Texture2D _emblemTexture;

		private LoadingSpinner _apiSpinner;

		private RaidWeekPlannerWindow _mainWindow;

		public static ModuleSettings ModuleSettings { get; private set; }

		public static Gw2ApiService Gw2ApiService { get; private set; }

		public static BusinessService BusinessService { get; private set; }

		public static CornerIcon CornerIcon { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public RaidWeekPlanner([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			RaidWeekPlannerInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
			if (Program.get_OverlayVersion() < new SemVer.Version(1, 1, 0))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
			CornerIcon cornerIcon = new CornerIcon(ContentsManager);
			((Control)cornerIcon).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((CornerIcon)cornerIcon).set_Priority(683537109);
			CornerIcon = cornerIcon;
			Gw2ApiService = new Gw2ApiService(Gw2ApiManager, Logger);
			BusinessService = new BusinessService(ContentsManager, Gw2ApiService, () => _apiSpinner, CornerIcon, Logger);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLocaleChanged);
		}

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			await BusinessService.RefreshBaseData();
			List<string> userClears = await BusinessService.GetAccountClears();
			_mainWindow.InjectData(userClears);
		}

		private void OnLocaleChanged(object sender, ValueChangedEventArgs<Locale> eventArgs)
		{
			LocalizingService.OnLocaleChanged(sender, eventArgs);
		}

		protected override async Task LoadAsync()
		{
			BusinessService.LoadData();
			_emblemTexture = ContentsManager.GetTexture("emblem.png");
			_windowBackgroundTexture = AsyncTexture2D.FromAssetId(155985);
			RaidWeekPlannerWindow raidWeekPlannerWindow = new RaidWeekPlannerWindow(_windowBackgroundTexture, new Rectangle(40, 26, 913, 691), new Rectangle(60, 36, 893, 675), AsyncTexture2D.op_Implicit(_emblemTexture), BusinessService);
			((WindowBase2)raidWeekPlannerWindow).set_SavesPosition(true);
			((Control)raidWeekPlannerWindow).set_Width(1400);
			((Control)raidWeekPlannerWindow).set_Height(550);
			_mainWindow = raidWeekPlannerWindow;
			_mainWindow.BuildUi();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			HandleCornerIcon();
			((Module)this).OnModuleLoaded(e);
		}

		private void HandleCornerIcon()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_mainWindow).ToggleWindow();
			});
			LoadingSpinner loadingSpinner = new LoadingSpinner();
			((Control)loadingSpinner).set_Location(new Point(((Control)CornerIcon).get_Left(), ((Control)CornerIcon).get_Bottom() + 3));
			((Control)loadingSpinner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)loadingSpinner).set_Size(new Point(((Control)CornerIcon).get_Width(), ((Control)CornerIcon).get_Height()));
			loadingSpinner.SetLocalizedTooltip = () => strings.LoadingSpinner_Fetch;
			((Control)loadingSpinner).set_Visible(false);
			_apiSpinner = loadingSpinner;
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			LoadingSpinner apiSpinner = _apiSpinner;
			if (apiSpinner != null)
			{
				((Control)apiSpinner).Dispose();
			}
			RaidWeekPlannerWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			Texture2D emblemTexture = _emblemTexture;
			if (emblemTexture != null)
			{
				((GraphicsResource)emblemTexture).Dispose();
			}
			RaidWeekPlannerInstance = null;
		}
	}
}
