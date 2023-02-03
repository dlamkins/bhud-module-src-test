using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.LiveMap.Models;
using Estreya.BlishHUD.Shared.Settings;

namespace Estreya.BlishHUD.LiveMap
{
	public class ModuleSettings : BaseModuleSettings
	{
		public SettingEntry<PlayerFacingType> PlayerFacingType { get; private set; }

		public SettingEntry<PublishType> PublishType { get; private set; }

		public SettingEntry<bool> HideCommander { get; private set; }

		public SettingEntry<bool> StreamerModeEnabled { get; private set; }

		public ModuleSettings(SettingCollection settings)
			: base(settings, new KeyBinding())
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			base.RegisterCornerIcon.set_Value(false);
			PlayerFacingType = settings.DefineSetting<PlayerFacingType>("PlayerFacingType", Estreya.BlishHUD.LiveMap.Models.PlayerFacingType.Camera, (Func<string>)(() => "Player Facing Type"), (Func<string>)(() => "Defines the type with which your player facing gets displayed."));
			PublishType = settings.DefineSetting<PublishType>("PublishType", Estreya.BlishHUD.LiveMap.Models.PublishType.Both, (Func<string>)(() => "Publish Type"), (Func<string>)(() => "Defines the scope where your position should be published to."));
			HideCommander = settings.DefineSetting<bool>("HideCommander", false, (Func<string>)(() => "Hide Commander"), (Func<string>)(() => "Whether the commander tag should be hidden on the live map."));
			StreamerModeEnabled = settings.DefineSetting<bool>("StreamerModeEnabled", false, (Func<string>)(() => "Streamer Mode Enabled"), (Func<string>)(() => "Whether the module should stop sending the position when a streaming program is detected."));
		}
	}
}
