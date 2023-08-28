using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Res;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL.Views
{
	public class SettingsView : View
	{
		private StandardButton _openSettingsButton;

		private readonly Action _toggleWindow;

		public SettingsView(Action? toggleWindow)
			: this()
		{
			_toggleWindow = toggleWindow;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text(strings_common.OpenSettings);
			((Control)val).set_Width(192);
			((Control)val).set_Parent(buildPanel);
			_openSettingsButton = val;
			((Control)_openSettingsButton).set_Location(new Point(Math.Max(((Control)buildPanel).get_Width() / 2 - ((Control)_openSettingsButton).get_Width() / 2, 20), Math.Max(((Control)buildPanel).get_Height() / 2 - ((Control)_openSettingsButton).get_Height(), 20) - ((Control)_openSettingsButton).get_Height() - 10));
			((Control)_openSettingsButton).add_Click((EventHandler<MouseEventArgs>)OpenSettingsButton_Click);
		}

		private void OpenSettingsButton_Click(object sender, MouseEventArgs e)
		{
			_toggleWindow?.Invoke();
		}

		protected override void Unload()
		{
			if (_openSettingsButton != null)
			{
				((Control)_openSettingsButton).remove_Click((EventHandler<MouseEventArgs>)OpenSettingsButton_Click);
			}
		}
	}
}
