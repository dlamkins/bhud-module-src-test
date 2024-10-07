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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public abstract class BaseModule<ModuleType, ModuleWindow, ModuleSettings, ModulePaths> : Module where ModuleType : Module where ModuleWindow : Container where ModuleSettings : BaseSettingsModel where ModulePaths : PathCollection, new()
	{
		public static Logger Logger = Blish_HUD.Logger.GetLogger<ModuleType>();

		protected bool HasGUI;

		protected bool AutoLoadGUI = true;

		protected bool IsGUICreated;

		public StaticHosting StaticHosting { get; private set; }

		public static string ModuleName => ModuleInstance.Name;

		public static ModuleType ModuleInstance { get; private set; }

		public static VirtualKeyShort[] ModKeyMapping { get; private set; }

		public Version ModuleVersion { get; private set; }

		public ModulePaths Paths { get; private set; }

		public SettingCollection SettingCollection { get; private set; }

		public CoreServiceCollection CoreServices { get; private set; }

		public SharedSettingsView SharedSettingsView { get; private set; }

		public SettingsManager SettingsManager { get; private set; }

		public ContentsManager ContentsManager { get; private set; }

		public DirectoriesManager DirectoriesManager { get; private set; }

		public Gw2ApiManager Gw2ApiManager { get; private set; }

		public ModuleWindow MainWindow { get; protected set; }

		public BaseSettingsWindow SettingsWindow { get; protected set; }

		public ModuleSettings Settings { get; private set; }

		protected SettingEntry<KeyBinding> ReloadKey { get; set; }

		public IServiceProvider ServiceProvider { get; private set; }

		protected BaseModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this as ModuleType;
			GameService.Overlay.UserLocale.SettingChanged += OnLocaleChanged;
			JsonConvert.set_DefaultSettings((Func<JsonSerializerSettings>)delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected O, but got Unknown
				JsonSerializerSettings val = new JsonSerializerSettings();
				val.set_Formatting((Formatting)1);
				val.set_NullValueHandling((NullValueHandling)1);
				return val;
			});
			SetupServices();
		}

		private void SetupServices()
		{
			ServiceCollection services = new ServiceCollection();
			DefineServices(services);
			ServiceProvider = services.BuildServiceProvider();
			AssignServiceInstaces(ServiceProvider);
		}

		protected virtual ServiceCollection DefineServices(ServiceCollection services)
		{
			((IServiceCollection)services).AddSingleton((Module)this);
			services.AddSingleton<ModuleWindow>();
			services.AddSingleton<ModuleSettings>();
			services.AddSingleton<ModulePaths>();
			services.AddSingleton(ModuleParameters.ContentsManager);
			services.AddSingleton(ModuleParameters.SettingsManager);
			services.AddSingleton(ModuleParameters.Gw2ApiManager);
			services.AddSingleton(ModuleParameters.DirectoriesManager);
			services.AddSingleton(Logger);
			services.AddSingleton<StaticHosting>();
			services.AddSingleton<ClientWindowService>();
			services.AddSingleton<SharedSettings>();
			services.AddSingleton<InputDetectionService>();
			services.AddSingleton<GameStateDetectionService>();
			services.AddSingleton<SharedSettingsView>();
			services.AddSingleton<CoreServiceCollection>();
			return services;
		}

		protected virtual void AssignServiceInstaces(IServiceProvider serviceProvider)
		{
			CoreServices = ServiceProviderServiceExtensions.GetRequiredService<CoreServiceCollection>(serviceProvider);
			SharedSettingsView = ServiceProviderServiceExtensions.GetRequiredService<SharedSettingsView>(serviceProvider);
			ContentsManager = ServiceProviderServiceExtensions.GetRequiredService<ContentsManager>(serviceProvider);
			DirectoriesManager = ServiceProviderServiceExtensions.GetRequiredService<DirectoriesManager>(serviceProvider);
			Gw2ApiManager = ServiceProviderServiceExtensions.GetRequiredService<Gw2ApiManager>(serviceProvider);
			SettingsManager = ServiceProviderServiceExtensions.GetRequiredService<SettingsManager>(serviceProvider);
			Logger = ServiceProviderServiceExtensions.GetRequiredService<Logger>(serviceProvider);
			StaticHosting = ServiceProviderServiceExtensions.GetRequiredService<StaticHosting>(serviceProvider);
			Paths = ServiceProviderServiceExtensions.GetRequiredService<ModulePaths>(serviceProvider);
			Settings = ServiceProviderServiceExtensions.GetRequiredService<ModuleSettings>(serviceProvider);
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
			await CoreServices.SharedSettings.Load(Paths.SharedSettingsPath);
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
			CoreServices.GameStateDetectionService.Run(gameTime);
			CoreServices.ClientWindowService.Run(gameTime);
			CoreServices.InputDetectionService.Run(gameTime);
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
			CoreServices?.Dispose();
			GameService.Overlay.UserLocale.SettingChanged -= OnLocaleChanged;
			ModuleInstance = null;
			base.Unload();
		}

		protected virtual void ReloadKey_Activated(object sender, EventArgs e)
		{
		}
	}
}
