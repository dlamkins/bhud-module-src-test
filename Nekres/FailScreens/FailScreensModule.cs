using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Nekres.FailScreens.Core.Services;

namespace Nekres.FailScreens
{
	[Export(typeof(Module))]
	public class FailScreensModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<FailScreensModule>();

		internal StateService State;

		internal DefeatedService Defeated;

		internal SettingEntry<DefeatedService.FailScreens> FailScreen;

		internal SettingEntry<bool> Random;

		internal static FailScreensModule Instance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public FailScreensModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection visualsCol = settings.AddSubCollection("visuals", true, (Func<string>)(() => "Defeated Screen"));
			FailScreen = visualsCol.DefineSetting<DefeatedService.FailScreens>("fail_screen", DefeatedService.FailScreens.DarkSouls, (Func<string>)(() => "Appearance"), (Func<string>)(() => "Visual to display upon defeat."));
			Random = visualsCol.DefineSetting<bool>("random", true, (Func<string>)(() => "Randomize"), (Func<string>)(() => "Ignores selection if set."));
		}

		protected override void Initialize()
		{
			State = new StateService();
			Defeated = new DefeatedService();
		}

		protected override async Task LoadAsync()
		{
			await State.SetupLockFiles(StateService.State.Defeated);
		}

		protected override void Update(GameTime gameTime)
		{
			State?.Update();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Unload()
		{
			Defeated?.Dispose();
			State?.Dispose();
			Instance = null;
		}
	}
}
