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

		private readonly ModuleLoadError _moduleLoadError = new ModuleLoadError();

		private TrackerCornerIcon? _trackerCornerIcon;

		private FarmingTrackerWindow? _farmingTrackerWindow;

		private SettingService? _settingService;

		private DateTimeService? _dateTimeService;

		private Services? _services;

		private Model? _model;

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
			if (_services == null)
			{
				Logger.Error("Cannot create module settings view without services.");
				return (IView)(object)new ErrorView();
			}
			if (!_moduleLoadError.HasModuleLoadFailed)
			{
				return (IView)(object)new ModuleSettingsView(_services!.WindowTabSelector);
			}
			return _moduleLoadError.CreateErrorSettingsView();
		}

		protected override async Task LoadAsync()
		{
			if (_settingService == null || _dateTimeService == null)
			{
				Logger.Error("Cannot load module without settingsService and dateTimeService from Module.DefineSettings().");
				return;
			}
			Services services = new Services(ContentsManager, DirectoriesManager, Gw2ApiManager, _settingService, _dateTimeService);
			Model model = (_model = await services.FileLoader.LoadModelFromFile());
			_services = services;
			if (services.Drf.WindowsVersionIsTooLowToSupportWebSockets)
			{
				_moduleLoadError.InitializeErrorSettingsViewAndShowErrorWindow(((Module)this).get_Name() + ": Module does not work :-(", "Your Windows version is too old. This module requires at least Windows 8\nbecause DRF relies on the WebSocket technology.");
				return;
			}
			FarmingTrackerWindow farmingTrackerWindow = new FarmingTrackerWindow(570, 650, model, services);
			services.WindowTabSelector.Init(farmingTrackerWindow);
			_farmingTrackerWindow = farmingTrackerWindow;
			services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnWindowVisibilityKeyBindingActivated);
			services.SettingService.WindowVisibilityKeyBindingSetting.get_Value().set_Enabled(true);
			_trackerCornerIcon = new TrackerCornerIcon(services, new EventHandler<MouseEventArgs>(CornerIconClickEventHandler));
		}

		protected override void Update(GameTime gameTime)
		{
			if (!_moduleLoadError.HasModuleLoadFailed)
			{
				_farmingTrackerWindow?.Update2(gameTime);
			}
		}

		protected override void Unload()
		{
			_moduleLoadError?.Dispose();
			TrackerCornerIcon? trackerCornerIcon = _trackerCornerIcon;
			if (trackerCornerIcon != null)
			{
				((Control)trackerCornerIcon).Dispose();
			}
			FarmingTrackerWindow? farmingTrackerWindow = _farmingTrackerWindow;
			if (farmingTrackerWindow != null)
			{
				((Control)farmingTrackerWindow).Dispose();
			}
			if (_services != null)
			{
				_services!.Dispose();
				_services!.SettingService.WindowVisibilityKeyBindingSetting.get_Value().set_Enabled(false);
				_services!.SettingService.WindowVisibilityKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnWindowVisibilityKeyBindingActivated);
				_services!.FarmingDuration.SaveFarmingTime();
				if (_model != null)
				{
					_services!.FileSaver.SaveModelToFileSync(_model);
				}
			}
		}

		private void OnWindowVisibilityKeyBindingActivated(object sender, EventArgs e)
		{
			_services?.WindowTabSelector.SelectWindowTab(WindowTab.Summary, WindowVisibility.Toggle);
		}

		private void CornerIconClickEventHandler(object s, MouseEventArgs e)
		{
			_services?.WindowTabSelector.SelectWindowTab(WindowTab.Summary, WindowVisibility.Toggle);
		}
	}
}
