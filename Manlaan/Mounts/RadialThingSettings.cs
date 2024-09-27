using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.Mounts.Things;
using Mounts.Settings;

namespace Manlaan.Mounts
{
	public abstract class RadialThingSettings : ThingsSettings
	{
		public SettingEntry<bool> IsEnabled;

		public SettingEntry<CenterBehavior> CenterThingBehavior;

		public SettingEntry<bool> RemoveCenterThing;

		public SettingEntry<string> DefaultThingChoice;

		public abstract string Name { get; }

		public event EventHandler<SettingsUpdatedEvent> RadialSettingsUpdated;

		public RadialThingSettings(SettingCollection settingCollection, string settingsPrefix, bool defaultIsEnabled, IList<Thing> defaultThings)
			: base(settingCollection, defaultThings, settingsPrefix + "Things")
		{
			IsEnabled = settingCollection.DefineSetting<bool>(settingsPrefix + "IsEnabled", defaultIsEnabled, (Func<string>)null, (Func<string>)null);
			CenterThingBehavior = settingCollection.DefineSetting<CenterBehavior>(settingsPrefix + "CenterThingBehavior", CenterBehavior.None, (Func<string>)null, (Func<string>)null);
			RemoveCenterThing = settingCollection.DefineSetting<bool>(settingsPrefix + "RemoveCenterThingFromRadial", true, (Func<string>)null, (Func<string>)null);
			DefaultThingChoice = settingCollection.DefineSetting<string>(settingsPrefix + "DefaultMountChoice", "Disabled", (Func<string>)null, (Func<string>)null);
			ThingsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<IList<string>>>)ThingsSetting_SettingChanged);
		}

		private void ThingsSetting_SettingChanged(object sender, ValueChangedEventArgs<IList<string>> e)
		{
			if (GetDefaultThing() == null)
			{
				DefaultThingChoice.set_Value("Disabled");
			}
			SettingsUpdatedEvent myevent = new SettingsUpdatedEvent();
			if (this.RadialSettingsUpdated != null)
			{
				this.RadialSettingsUpdated(this, myevent);
			}
		}

		internal Thing GetCenterThing()
		{
			if (CenterThingBehavior.get_Value() == CenterBehavior.Default)
			{
				return GetDefaultThing();
			}
			if (CenterThingBehavior.get_Value() == CenterBehavior.LastUsed)
			{
				return GetLastUsedThing();
			}
			return null;
		}

		internal Thing GetDefaultThing()
		{
			return base.Things.SingleOrDefault((Thing m) => m.Name == DefaultThingChoice.get_Value());
		}

		internal Thing GetLastUsedThing()
		{
			return (from m in base.Things
				where m.LastUsedTimestamp.HasValue
				orderby m.LastUsedTimestamp descending
				select m).FirstOrDefault();
		}

		public abstract SettingEntry<KeyBinding> GetKeybind();

		public abstract bool GetIsApplicable();
	}
}
