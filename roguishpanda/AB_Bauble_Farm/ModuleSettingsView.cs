using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace roguishpanda.AB_Bauble_Farm
{
	public class ModuleSettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Open Settings");
			((Control)val).set_Size(new Point(100, 30));
			StandardButton btnSettings = val;
			((Control)btnSettings).set_Location(new Point((((Control)buildPanel).get_Size().X - ((Control)btnSettings).get_Size().X) / 2, (((Control)buildPanel).get_Size().Y - ((Control)btnSettings).get_Size().Y) / 2));
			((Control)btnSettings).add_Click((EventHandler<MouseEventArgs>)OpenSettings_Click);
		}

		private void OpenSettings_Click(object sender, MouseEventArgs e)
		{
			MainWindowModule module = MainWindowModule.ModuleInstance;
			if (!((Control)module._SettingsWindow).get_Visible())
			{
				((Control)module._SettingsWindow).Show();
			}
			else
			{
				((Control)module._SettingsWindow).Hide();
			}
		}

		public ModuleSettingsView()
			: this()
		{
		}
	}
}
