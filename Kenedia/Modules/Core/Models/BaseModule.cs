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
		public static readonly Logger Logger = Blish_HUD.Logger.GetLogger<ModuleType>();

		protected bool HasGUI;

		protected bool AutoLoadGUI = true;

		protected bool IsGUICreated;

		public static string ModuleName => ModuleInstance.Name;

		public static ModuleType ModuleInstance { get; protected set; }

		public static VirtualKeyShort[] ModKeyMapping { get; private set; }

		public Version ModuleVersion { get; private set; }

		public ModulePaths Paths { get; protected set; }

		public SettingCollection SettingCollection { get; private set; }

		public ServiceCollection Services { get; private set; }

		public SharedSettingsView SharedSettingsView { get; private set; }

		public SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		public ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		public DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		public Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		public ModuleWindow MainWindow { get; protected set; }

		public BaseSettingsWindow SettingsWindow { get; protected set; }

		public ModuleSettings Settings { get; protected set; }

		protected SettingEntry<KeyBinding> ReloadKey { get; set; }

		protected BaseModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ClientWindowService clientWindowService = new ClientWindowService();
			SharedSettings sharedSettings = new SharedSettings();
			InputDetectionService inputDetectionService = new InputDetectionService();
			GameStateDetectionService gameState = new GameStateDetectionService(clientWindowService, sharedSettings);
			Services = new ServiceCollection(gameState, clientWindowService, sharedSettings, inputDetectionService);
			SharedSettingsView = new SharedSettingsView(sharedSettings, clientWindowService);
			GameService.Overlay.UserLocale.SettingChanged += OnLocaleChanged;
			TexturesService.Initilize(ContentsManager);
		}

		protected override void Initialize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			base.Initialize();
			ModuleVersion = base.Version;
			Logger.Info($"Initializing {base.Name} {ModuleVersion}");
			ModKeyMapping = new VirtualKeyShort[5];
			ModKeyMapping[1] = VirtualKeyShort.CONTROL;
			ModKeyMapping[2] = VirtualKeyShort.MENU;
			ModKeyMapping[4] = VirtualKeyShort.LSHIFT;
			if (Program.OverlayVersion < new Version(1, 1, 0, (string)null, (string)null))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.TacO, new object[1] { true });
				}
				catch
				{
				}
			}
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			await Services.SharedSettings.Load(Paths.SharedSettingsPath);
		}

		protected virtual void OnLocaleChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> eventArgs)
		{
			LocalizingService.OnLocaleChanged(sender, eventArgs);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
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
			base.DefineSettings(settings);
			SettingCollection = settings;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Services.GameStateDetectionService.Run(gameTime);
			Services.ClientWindowService.Run(gameTime);
			Services.InputDetectionService.Run(gameTime);
			if (HasGUI && !IsGUICreated && AutoLoadGUI)
			{
				PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
				if (player != null && !string.IsNullOrEmpty(player.Name))
				{
					LoadGUI();
				}
			}
		}

		protected override void Unload()
		{
			UnloadGUI();
			Services?.Dispose();
			GameService.Overlay.UserLocale.SettingChanged -= OnLocaleChanged;
			ModuleInstance = null;
			base.Unload();
		}

		protected virtual void ReloadKey_Activated(object sender, EventArgs e)
		{
		}
	}
}
