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
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
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
			if (DateTime.UtcNow.Date < new DateTime(2023, 8, 25, 0, 0, 0, DateTimeKind.Utc))
			{
				Label val3 = new Label();
				val3.set_Text("The Guild Wars 2 API is unavailable until some time on August 24th.\nUntil that time, some features such as minimap markers/trails will not work.");
				((Control)val3).set_Width(((Control)buildPanel).get_Width() - 40);
				((Control)val3).set_Height(120);
				val3.set_HorizontalAlignment((HorizontalAlignment)1);
				val3.set_VerticalAlignment((VerticalAlignment)0);
				((Control)val3).set_Parent(buildPanel);
				val3.set_TextColor(Color.get_Yellow());
				((Control)val3).set_Left(20);
				((Control)val3).set_Top(150);
			}
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
