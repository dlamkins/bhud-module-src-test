using System;
using BhModule.Community.Pathing.UI.Presenters;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Views
{
	public class SettingsHintView : View
	{
		private StandardButton _bttnOpenSettings;

		public event EventHandler<EventArgs> OpenSettingsClicked;

		public SettingsHintView()
			: this()
		{
		}

		public SettingsHintView((Action OpenSettings, PackInitiator packInitiator) model)
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new SettingsHintPresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("Open Settings");
			((Control)val).set_Parent(buildPanel);
			_bttnOpenSettings = val;
			((Control)_bttnOpenSettings).set_Location(new Point(Math.Max(((Control)buildPanel).get_Width() / 2 - ((Control)_bttnOpenSettings).get_Width() / 2, 20), Math.Max(((Control)buildPanel).get_Height() / 2 - ((Control)_bttnOpenSettings).get_Height(), 20)));
			((Control)_bttnOpenSettings).add_Click((EventHandler<MouseEventArgs>)_bttnOpenSettings_Click);
		}

		private void _bttnOpenSettings_Click(object sender, MouseEventArgs e)
		{
			this.OpenSettingsClicked?.Invoke(this, (EventArgs)(object)e);
		}

		protected override void Unload()
		{
			((Control)_bttnOpenSettings).remove_Click((EventHandler<MouseEventArgs>)_bttnOpenSettings_Click);
		}
	}
}
