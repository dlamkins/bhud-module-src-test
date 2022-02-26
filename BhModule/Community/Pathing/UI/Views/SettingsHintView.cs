using System;
using System.Diagnostics;
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

		private StandardButton _bttnOpenSetupGuide;

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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("Open Settings");
			((Control)val).set_Width(192);
			((Control)val).set_Parent(buildPanel);
			_bttnOpenSettings = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Open Setup Guide");
			((Control)val2).set_Width(((Control)_bttnOpenSettings).get_Width());
			((Control)val2).set_Parent(buildPanel);
			_bttnOpenSetupGuide = val2;
			((Control)_bttnOpenSettings).set_Location(new Point(Math.Max(((Control)buildPanel).get_Width() / 2 - ((Control)_bttnOpenSettings).get_Width() / 2, 20), Math.Max(((Control)buildPanel).get_Height() / 2 - ((Control)_bttnOpenSettings).get_Height(), 20) - ((Control)_bttnOpenSettings).get_Height() - 10));
			((Control)_bttnOpenSetupGuide).set_Location(new Point(((Control)_bttnOpenSettings).get_Left(), ((Control)_bttnOpenSettings).get_Bottom() + 10));
			((Control)_bttnOpenSettings).add_Click((EventHandler<MouseEventArgs>)_bttnOpenSettings_Click);
			((Control)_bttnOpenSetupGuide).add_Click((EventHandler<MouseEventArgs>)BttnOpenSetupGuideClick);
		}

		private void _bttnOpenSettings_Click(object sender, MouseEventArgs e)
		{
			this.OpenSettingsClicked?.Invoke(this, (EventArgs)(object)e);
		}

		private void BttnOpenSetupGuideClick(object sender, MouseEventArgs e)
		{
			Process.Start("https://link.blishhud.com/pathingsetup");
		}

		protected override void Unload()
		{
			if (_bttnOpenSettings != null)
			{
				((Control)_bttnOpenSettings).remove_Click((EventHandler<MouseEventArgs>)_bttnOpenSettings_Click);
			}
			if (_bttnOpenSetupGuide != null)
			{
				((Control)_bttnOpenSetupGuide).remove_Click((EventHandler<MouseEventArgs>)BttnOpenSetupGuideClick);
			}
		}
	}
}
