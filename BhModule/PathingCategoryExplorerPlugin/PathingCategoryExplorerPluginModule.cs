using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace BhModule.PathingCategoryExplorerPlugin
{
	[Export(typeof(Module))]
	public class PathingCategoryExplorerPluginModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<PathingCategoryExplorerPluginModule>();

		public static PathingCategoryExplorerPluginModule Instance;

		public static ModuleManager InstanceManager;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public PluginService PluginService { get; private set; }

		public ModuleSettings Settings { get; private set; }

		[ImportingConstructor]
		public PathingCategoryExplorerPluginModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(this, settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new PathingCategoryExplorerPluginSettingsView(SettingsManager.get_ModuleSettings());
		}

		protected override void Initialize()
		{
			PluginService = new PluginService();
			InstanceManager = GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => m.get_ModuleInstance() == this);
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			PluginService.Upadate();
		}

		protected override void Unload()
		{
			Settings.Unload();
			PluginService.Unload();
		}
	}
}
