using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.Mounts;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework.Input;

namespace Mounts.Settings
{
	internal class UserDefinedRadialThingSettings : RadialThingSettings
	{
		public SettingEntry<string> NameSetting;

		public SettingEntry<KeyBinding> Keybind;

		public int Id { get; }

		public Func<Task> _callback { get; }

		public override string Name => NameSetting.get_Value();

		public UserDefinedRadialThingSettings(SettingCollection settingCollection, int id, Func<Task> callback)
			: base(settingCollection, $"RadialThingSettings{id}", defaultIsEnabled: true, new List<Thing>())
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			Id = id;
			_callback = callback;
			NameSetting = settingCollection.DefineSetting<string>($"RadialThingSettings{Id}Name", "", (Func<string>)null, (Func<string>)null);
			Keybind = settingCollection.DefineSetting<KeyBinding>($"RadialThingSettings{Id}Keybind", new KeyBinding((Keys)0), (Func<string>)null, (Func<string>)null);
			Keybind.get_Value().set_Enabled(true);
			Keybind.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await _callback();
			});
		}

		public override void DeleteFromSettings(SettingCollection settingCollection)
		{
			settingCollection.UndefineSetting($"RadialThingSettings{Id}Name");
			settingCollection.UndefineSetting($"RadialThingSettings{Id}Keybind");
			Keybind.get_Value().set_Enabled(false);
			base.DeleteFromSettings(settingCollection);
		}
	}
}
