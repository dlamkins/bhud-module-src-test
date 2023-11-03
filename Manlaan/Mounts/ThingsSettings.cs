using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using Manlaan.Mounts.Things;

namespace Manlaan.Mounts
{
	public abstract class ThingsSettings
	{
		protected SettingEntry<IList<string>> ThingsSetting;

		private string ThingSettingsName;

		public ICollection<Thing> Things => (from typeOfThingInSettings in ThingsSetting.get_Value()
			select Module._things.Single((Thing t) => typeOfThingInSettings == t.GetType().FullName)).ToList();

		public ICollection<Thing> AvailableThings => Things.Where((Thing t) => t.IsAvailable).ToList();

		protected ThingsSettings(SettingCollection settingCollection, IEnumerable<Thing> things, string thingSettingsName)
		{
			ThingSettingsName = thingSettingsName;
			if (things == null)
			{
				things = new List<Thing>();
			}
			ThingsSetting = settingCollection.DefineSetting<IList<string>>(ThingSettingsName, (IList<string>)things.Select((Thing t) => t.GetType().FullName).ToList(), (Func<string>)null, (Func<string>)null);
		}

		public void SetThings(IEnumerable<Thing> things)
		{
			ThingsSetting.set_Value((IList<string>)things.Select((Thing t) => t.GetType().FullName).ToList());
		}

		public void AddThing(Thing thingToAdd)
		{
			ThingsSetting.set_Value((IList<string>)ThingsSetting.get_Value().Append(thingToAdd.GetType().FullName).ToList());
		}

		public void RemoveThing(Thing thingToRemove)
		{
			ThingsSetting.set_Value((IList<string>)(from t in ThingsSetting.get_Value()
				where t != thingToRemove.GetType().FullName
				select t).ToList());
		}

		public virtual void DeleteFromSettings(SettingCollection settingCollection)
		{
			settingCollection.UndefineSetting(ThingSettingsName);
		}
	}
}
