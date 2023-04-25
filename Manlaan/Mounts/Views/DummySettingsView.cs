using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Manlaan.Mounts.Views
{
	internal class DummySettingsView : View
	{
		public EventHandler OnSettingsButtonClicked { get; internal set; }

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(100, 100));
			val.set_Text(Strings.Settings_Button_Label);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate(object args, MouseEventArgs sender)
			{
				OnSettingsButtonClicked(args, (EventArgs)(object)sender);
			});
		}

		public DummySettingsView()
			: this()
		{
		}
	}
}
