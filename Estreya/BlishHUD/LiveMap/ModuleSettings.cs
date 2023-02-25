using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Settings;

namespace Estreya.BlishHUD.LiveMap
{
	public class ModuleSettings : BaseModuleSettings
	{
		public SettingEntry<bool> HideCommander { get; private set; }

		public SettingEntry<bool> StreamerModeEnabled { get; private set; }

		public SettingEntry<bool> FollowOnMap { get; private set; }

		public SettingEntry<bool> SendGroupInformation { get; private set; }

		public ModuleSettings(SettingCollection settings)
			: base(settings, new KeyBinding())
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			base.RegisterCornerIcon.set_Value(false);
			HideCommander = settings.DefineSetting<bool>("HideCommander", false, (Func<string>)(() => "Hide Commander"), (Func<string>)(() => "Whether the commander tag should be hidden on the live map."));
			StreamerModeEnabled = settings.DefineSetting<bool>("StreamerModeEnabled", true, (Func<string>)(() => "Streamer Mode Enabled"), (Func<string>)(() => "Whether the module should stop sending the position when a streaming program is detected."));
			FollowOnMap = settings.DefineSetting<bool>("FollowOnMap", true, (Func<string>)(() => "Follow on Map"), (Func<string>)(() => "Whether the map should follow the player if opened via the module."));
			SendGroupInformation = settings.DefineSetting<bool>("SendGroupInformation", true, (Func<string>)(() => "Send Group Information"), (Func<string>)(() => "Whether the module should publish your current group informations."));
		}
	}
}
