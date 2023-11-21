using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Settings;
using Microsoft.Xna.Framework.Input;

namespace Estreya.BlishHUD.PortalDistance
{
	public class ModuleSettings : BaseModuleSettings
	{
		public SettingEntry<KeyBinding> ManualKeyBinding { get; private set; }

		public SettingEntry<bool> UseArcDPS { get; private set; }

		public ModuleSettings(SettingCollection settings)
			: base(settings, new KeyBinding())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown


		protected override void DoInitializeGlobalSettings(SettingCollection globalSettingCollection)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			ManualKeyBinding = globalSettingCollection.DefineSetting<KeyBinding>("ManualKeyBinding", new KeyBinding((ModifierKeys)2, (Keys)80), (Func<string>)(() => "Manual KeyBinding"), (Func<string>)(() => "Defines the key for manual activation of the distance measurement."));
			ManualKeyBinding.get_Value().set_Enabled(true);
			ManualKeyBinding.get_Value().set_BlockSequenceFromGw2(true);
			ManualKeyBinding.get_Value().set_IgnoreWhenInTextField(true);
			UseArcDPS = globalSettingCollection.DefineSetting<bool>("UseArcDPS", true, (Func<string>)(() => "Use ArcDPS"), (Func<string>)(() => "Whether the module tries to auto detect portal usage. Requires a restart. (YOU NEED TO STAND STILL UNTIL BUFF IS REGISTERED)"));
		}
	}
}
