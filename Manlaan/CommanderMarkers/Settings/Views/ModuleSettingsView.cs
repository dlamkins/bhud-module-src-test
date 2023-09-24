using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;

namespace Manlaan.CommanderMarkers.Settings.Views
{
	internal class ModuleSettingsView : View
	{
		protected SettingService _settings;

		public ModuleSettingsView(SettingService settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Open Settings");
			((Control)val).set_Size(((Control)buildPanel).get_Size().Scale(0.2f));
			((Control)val).set_Location(((Control)buildPanel).get_Size().Half() - ((Control)buildPanel).get_Size().Scale(0.2f).Half());
			((Control)buildPanel.AddControl((Control)val, out var _)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SettingsPanel settingsWindow = Service.SettingsWindow;
				if (settingsWindow != null)
				{
					((Control)settingsWindow).Show();
				}
			});
		}

		protected void CreateDoubleKeybind(Container buildPanel, string label, SettingEntry<KeyBinding> groundSetting, SettingEntry<KeyBinding> objectSetting)
		{
		}
	}
}
