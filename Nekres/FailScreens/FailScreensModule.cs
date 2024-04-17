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

		internal SettingEntry<float> Volume;

		internal SettingEntry<bool> Muted;

		internal float SoundVolume = 1f;

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
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			SettingCollection visualsCol = settings.AddSubCollection("visuals", true, (Func<string>)(() => "Defeated Screen"));
			FailScreen = visualsCol.DefineSetting<DefeatedService.FailScreens>("fail_screen", DefeatedService.FailScreens.DarkSouls, (Func<string>)(() => "Appearance"), (Func<string>)(() => "Visual to display upon defeat."));
			Random = visualsCol.DefineSetting<bool>("random", true, (Func<string>)(() => "Randomize"), (Func<string>)(() => "Ignores selection if set."));
			SettingCollection soundCol = settings.AddSubCollection("sound", true, (Func<string>)(() => "Sound Options"));
			Volume = soundCol.DefineSetting<float>("volume", 0.05f, (Func<string>)(() => "Volume"), (Func<string>)(() => "Adjusts the audio volume."));
			Muted = soundCol.DefineSetting<bool>("mute", false, (Func<string>)(() => "Mute"), (Func<string>)(() => "Mutes the audio."));
			SettingComplianceExtensions.SetRange(Volume, 0f, 0.1f);
			SettingComplianceExtensions.SetValidation<float>(Volume, (Func<float, SettingValidationResult>)ValidateVolume);
			SettingValidationResult val = ValidateVolume(Volume.get_Value());
			if (!((SettingValidationResult)(ref val)).get_Valid())
			{
				Volume.set_Value(0.05f);
			}
		}

		private SettingValidationResult ValidateVolume(float vol)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new SettingValidationResult(vol >= 0f && vol <= 0.1f, (string)null);
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
			SoundVolume = (Muted.get_Value() ? 0f : Math.Min(GameService.GameIntegration.get_Audio().get_Volume(), Volume.get_Value()));
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
