using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;

namespace BhModule.WebPeeper
{
	[Export(typeof(Module))]
	public class WebPeeperModule : Module
	{
		public static readonly Logger Logger = Logger.GetLogger<WebPeeperModule>();

		public static BlishHud BlishHudInstance;

		public static MenuItem InstanceSettingsMenuItem;

		public static ModuleManager InstanceModuleManager;

		public static WebPeeperModule Instance;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public CefService CefService { get; private set; }

		public UIService UIService { get; private set; }

		public ModuleSettings Settings { get; private set; }

		[ImportingConstructor]
		public WebPeeperModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			FieldInfo field = typeof(BlishHud).GetField("Instance", BindingFlags.Static | BindingFlags.NonPublic);
			object value = field.GetValue(field.ReflectedType);
			BlishHudInstance = (BlishHud)((value is BlishHud) ? value : null);
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new WebPeeperSettingsView(SettingsManager.get_ModuleSettings());
		}

		protected override void Initialize()
		{
			InstanceModuleManager = GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => m.get_ModuleInstance() == this);
			Control obj = ((IEnumerable<Control>)((Container)GameService.Overlay.get_SettingsTab().GetSettingMenus().Last()).get_Children()).FirstOrDefault((Control i) => ((MenuItem)i).get_Text() == InstanceModuleManager.get_Manifest().get_Name());
			InstanceSettingsMenuItem = (MenuItem)(object)((obj is MenuItem) ? obj : null);
			CefService = new CefService();
			UIService = new UIService();
		}

		protected override async Task LoadAsync()
		{
			await Task.Run(delegate
			{
				CefService.Load();
				UIService.Load();
			});
		}

		protected override void Unload()
		{
			Settings?.Unload();
			CefService?.Unload();
			UIService?.Unload();
		}
	}
}
