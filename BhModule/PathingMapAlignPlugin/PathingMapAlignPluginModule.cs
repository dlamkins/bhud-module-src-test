using System.ComponentModel.Composition;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace BhModule.PathingMapAlignPlugin
{
	[Export(typeof(Module))]
	public class PathingMapAlignPluginModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<PathingMapAlignPluginModule>();

		public static PathingMapAlignPluginModule Instance;

		public static ModuleManager InstanceManager;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public PluginService PluginService { get; private set; }

		public ModuleSettings Settings { get; private set; }

		[ImportingConstructor]
		public PathingMapAlignPluginModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new PathingMapAlignPluginSettingsView(SettingsManager.get_ModuleSettings());
		}

		protected override void Initialize()
		{
			PluginService = new PluginService();
			InstanceManager = GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => m.get_ModuleInstance() == this);
		}

		protected override void Update(GameTime gameTime)
		{
			PluginService.Upadate();
		}

		protected override void Unload()
		{
			PluginService.Unload();
			Settings.Unload();
		}
	}
}
