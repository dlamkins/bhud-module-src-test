using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views
{
	public class ModuleMainSettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			val.set_Text(Strings.ModuleSettings_OpenSettings);
			((Control)val).set_Size(((Control)buildPanel).get_Size().Scale(0.2f));
			((Control)val).set_Location(((Control)buildPanel).get_Size().Half() - ((Control)buildPanel).get_Size().Scale(0.2f).Half());
			StandardButton _openSettingsButton = val;
			buildPanel.AddControl((Control)(object)_openSettingsButton);
			((Control)_openSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)Service.SettingsWindow).Show();
			});
		}

		public ModuleMainSettingsView()
			: this()
		{
		}
	}
}
