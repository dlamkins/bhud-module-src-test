using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static readonly Logger Logger = Logger.GetLogger<Module>();

		private static bool _isDebugConfiguration = false;

		private readonly ModuleLoadError _moduleLoadError = new ModuleLoadError();

		private TrackerCornerIcon _trackerCornerIcon;

		private FarmingTrackerWindow _farmingTrackerWindow;

		private SettingService _settingService;

		private DateTimeService _dateTimeService;

		private Services _services;

		private Model _model;

		public static bool DebugEnabled
		{
			get
			{
				if (!_isDebugConfiguration)
				{
					return GameService.Debug.get_EnableDebugLogging().get_Value();
				}
				return true;
			}
		}

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingService = new SettingService(settings);
			_dateTimeService = new DateTimeService(settings);
		}

		public override IView GetSettingsView()
		{
			if (!_moduleLoadError.HasModuleLoadFailed)
			{
				return (IView)(object)new ModuleSettingsView(_farmingTrackerWindow);
			}
			return _moduleLoadError.CreateErrorSettingsView();
		}

		protected override async Task LoadAsync()
		{
			Services services = new Services(ContentsManager, DirectoriesManager, Gw2ApiManager, _settingService, _dateTimeService);
			Model model = (_model = await services.FileLoadService.LoadModelFromFile());
			_services = services;
			if (_services.Drf.WindowsVersionIsTooLowToSupportWebSockets)
			{
				_moduleLoadError.InitializeErrorSettingsViewAndShowErrorWindow(((Module)this).get_Name() + ": Module does not work :-(", "Your Windows version is too old. This module requires at least Windows 8\nbecause DRF relies on the WebSocket technology.");
				return;
			}
			_farmingTrackerWindow = new FarmingTrackerWindow(570, 650, model, services);
			_trackerCornerIcon = new TrackerCornerIcon(services, CornerIconClickEventHandler);
			_services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnWindowVisibilityKeyBindingActivated);
			_services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().set_Enabled(true);
		}

		protected override void Update(GameTime gameTime)
		{
			if (!_moduleLoadError.HasModuleLoadFailed)
			{
				_farmingTrackerWindow.Update2(gameTime);
			}
		}

		protected override void Unload()
		{
			_moduleLoadError?.Dispose();
			TrackerCornerIcon trackerCornerIcon = _trackerCornerIcon;
			if (trackerCornerIcon != null)
			{
				((Control)trackerCornerIcon).Dispose();
			}
			FarmingTrackerWindow farmingTrackerWindow = _farmingTrackerWindow;
			if (farmingTrackerWindow != null)
			{
				((Control)farmingTrackerWindow).Dispose();
			}
			_services?.Dispose();
			_services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().set_Enabled(false);
			_services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnWindowVisibilityKeyBindingActivated);
			if (_model != null)
			{
				_services.FarmingDuration.SaveFarmingTime();
				_services.FileSaveService.SaveModelToFileSync(_model);
			}
		}

		private void OnWindowVisibilityKeyBindingActivated(object sender, EventArgs e)
		{
			_farmingTrackerWindow.ToggleWindowAndSelectSummaryTab();
		}

		private void CornerIconClickEventHandler(object s, MouseEventArgs e)
		{
			_farmingTrackerWindow.ToggleWindowAndSelectSummaryTab();
		}
	}
}
