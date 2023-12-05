using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using Manlaan.Mounts;
using Manlaan.Mounts.Things;

namespace Mounts.Settings
{
	internal class ContextualRadialThingSettings : RadialThingSettings
	{
		private readonly string _name;

		public readonly int Order;

		public readonly Func<bool> IsApplicable;

		public SettingEntry<bool> ApplyInstantlyIfSingle;

		public SettingEntry<bool> UnconditionallyDoAction;

		public bool IsDefault => Order == 99;

		public override string Name => _name;

		public ContextualRadialThingSettings(SettingCollection settingCollection, string name, int order, Func<bool> isApplicable, bool defaultIsEnabled, bool defaultApplyInstantlyIfSingle, bool defaultUnconditionallyDoAction, IList<Thing> defaultThings)
			: base(settingCollection, "RadialThingSettings" + name, defaultIsEnabled, defaultThings)
		{
			_name = name;
			Order = order;
			IsApplicable = isApplicable;
			ApplyInstantlyIfSingle = settingCollection.DefineSetting<bool>("RadialThingSettings" + _name + "ApplyInstantlyIfSingle", defaultApplyInstantlyIfSingle, (Func<string>)null, (Func<string>)null);
			UnconditionallyDoAction = settingCollection.DefineSetting<bool>("RadialThingSettings" + _name + "UnconditionallyDoAction", defaultUnconditionallyDoAction, (Func<string>)null, (Func<string>)null);
			ThingsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<IList<string>>>)ThingsSetting_SettingChanged);
		}

		private void ThingsSetting_SettingChanged(object sender, ValueChangedEventArgs<IList<string>> e)
		{
			ApplyInstantlyIfSingle.set_Value(ThingsSetting.get_Value().Count == 1);
		}
	}
}
