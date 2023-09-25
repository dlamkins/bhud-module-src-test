using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.GameIntegration;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public abstract class BaseModule<ModuleType, ModuleWindow, ModuleSettings, ModulePaths> : Module where ModuleType : Module where ModuleWindow : Container where ModuleSettings : BaseSettingsModel where ModulePaths : PathCollection, new()
	{
		public static readonly Logger Logger = Logger.GetLogger<ModuleType>();

		protected bool HasGUI;

		protected bool AutoLoadGUI = true;

		protected bool IsGUICreated;

		public static string ModuleName => ((Module)ModuleInstance).get_Name();

		public static ModuleType ModuleInstance { get; protected set; }

		public static VirtualKeyShort[] ModKeyMapping { get; private set; }

		public Version ModuleVersion { get; private set; }

		public ModulePaths Paths { get; protected set; }

		public SettingCollection SettingCollection { get; private set; }

		public ServiceCollection Services { get; private set; }

		public SharedSettingsView SharedSettingsView { get; private set; }

		public SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		public ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		public DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		public Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public ModuleWindow MainWindow { get; protected set; }

		public BaseSettingsWindow SettingsWindow { get; protected set; }

		public ModuleSettings Settings { get; protected set; }

		protected SettingEntry<KeyBinding> ReloadKey { get; set; }

		protected BaseModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ClientWindowService clientWindowService = new ClientWindowService();
			SharedSettings sharedSettings = new SharedSettings();
			InputDetectionService inputDetectionService = new InputDetectionService();
			GameStateDetectionService gameState = new GameStateDetectionService(clientWindowService, sharedSettings);
			Services = new ServiceCollection(gameState, clientWindowService, sharedSettings, inputDetectionService);
			SharedSettingsView = new SharedSettingsView(sharedSettings, clientWindowService);
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLocaleChanged);
			TexturesService.Initilize(ContentsManager);
		}

		protected override void Initialize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			((Module)this).Initialize();
			ModuleVersion = ((Module)this).get_Version();
			Logger.Info($"Initializing {((Module)this).get_Name()} {ModuleVersion}");
			ModKeyMapping = (VirtualKeyShort[])(object)new VirtualKeyShort[5];
			ModKeyMapping[1] = (VirtualKeyShort)17;
			ModKeyMapping[2] = (VirtualKeyShort)18;
			ModKeyMapping[4] = (VirtualKeyShort)160;
			if (Program.get_OverlayVersion() < new Version(1, 1, 0, (string)null, (string)null))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
		}

		protected override async Task LoadAsync()
		{
			await _003C_003En__0();
			await Services.SharedSettings.Load(Paths.SharedSettingsPath);
		}

		protected virtual void OnLocaleChanged(object sender, ValueChangedEventArgs<Locale> eventArgs)
		{
			LocalizingService.OnLocaleChanged(sender, eventArgs);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected virtual void LoadGUI()
		{
			UnloadGUI();
			IsGUICreated = true;
		}

		protected virtual void UnloadGUI()
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			((Module)this).DefineSettings(settings);
			SettingCollection = settings;
		}

		protected override void Update(GameTime gameTime)
		{
			((Module)this).Update(gameTime);
			Services.GameStateDetectionService.Run(gameTime);
			Services.ClientWindowService.Run(gameTime);
			Services.InputDetectionService.Run(gameTime);
			if (HasGUI && !IsGUICreated && AutoLoadGUI)
			{
				PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
				if (player != null && !string.IsNullOrEmpty(player.get_Name()))
				{
					LoadGUI();
				}
			}
		}

		protected override void Unload()
		{
			UnloadGUI();
			Services?.Dispose();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLocaleChanged);
			ModuleInstance = default(ModuleType);
			((Module)this).Unload();
		}

		protected virtual void ReloadKey_Activated(object sender, EventArgs e)
		{
		}
	}
}
