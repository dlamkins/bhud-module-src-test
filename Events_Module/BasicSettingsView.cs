using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Events_Module
{
	public class BasicSettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("Set Notification Position");
			((Control)val).set_Width(196);
			((Control)val).set_Location(new Point(32, 32));
			((Control)val).set_Parent(buildPanel);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)SetPosition_Click);
		}

		private void SetPosition_Click(object sender, MouseEventArgs e)
		{
			EventsModule.ModuleInstance.ShowSetNotificationPositions();
		}

		public BasicSettingsView()
			: this()
		{
		}
	}
}
