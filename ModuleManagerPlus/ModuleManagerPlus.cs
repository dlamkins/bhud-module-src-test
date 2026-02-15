using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModuleManagerPlus.UI;

namespace ModuleManagerPlus
{
	[Export(typeof(Module))]
	public class ModuleManagerPlus : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleManagerPlus>();

		private MenuItem _moduleManagerPlusSettingEntry;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public static Effect MaskEffect { get; private set; }

		[ImportingConstructor]
		public ModuleManagerPlus([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override async Task LoadAsync()
		{
			MaskEffect = ContentsManager.GetEffect("effects/alphashader.mgfx");
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			_moduleManagerPlusSettingEntry = new MenuItem("ModuleManager+", AsyncTexture2D.FromAssetId(156764));
			GameService.Overlay.get_SettingsTab().RegisterSettingMenu(_moduleManagerPlusSettingEntry, (Func<MenuItem, IView>)((MenuItem m) => (IView)(object)new ModuleRepoView(ContentsManager)), 0);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			if (_moduleManagerPlusSettingEntry != null)
			{
				GameService.Overlay.get_SettingsTab().RemoveSettingMenu(_moduleManagerPlusSettingEntry);
			}
		}
	}
}
