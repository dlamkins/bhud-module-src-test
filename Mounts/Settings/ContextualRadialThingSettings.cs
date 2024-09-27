using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Input;
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

		public SettingEntry<string> ApplyInstantlyOnTap;

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
			ApplyInstantlyOnTap = settingCollection.DefineSetting<string>("RadialThingSettings" + _name + "ApplyInstantlyOnTap", "Disabled", (Func<string>)null, (Func<string>)null);
			UnconditionallyDoAction = settingCollection.DefineSetting<bool>("RadialThingSettings" + _name + "UnconditionallyDoAction", defaultUnconditionallyDoAction, (Func<string>)null, (Func<string>)null);
		}

		public override SettingEntry<KeyBinding> GetKeybind()
		{
			return Module._settingDefaultMountBinding;
		}

		public override bool GetIsApplicable()
		{
			return IsApplicable();
		}

		public bool IsTapApplicable()
		{
			if (ApplyInstantlyOnTap.get_Value() != "Disabled")
			{
				return Module._settingTapThresholdInMilliseconds.get_Value() != 0;
			}
			return false;
		}

		internal Thing GetApplyInstantlyOnTapThing()
		{
			return base.Things.SingleOrDefault((Thing m) => m.Name == ApplyInstantlyOnTap.get_Value());
		}
	}
}
