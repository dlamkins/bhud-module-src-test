using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using LoreBridge.Models;
using LoreBridge.Utils;

namespace LoreBridge.Views.SettingsView.Controls
{
	public class Policy
	{
		private readonly StandardButton _button;

		public Policy(Panel mainPanel, Settings settings)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			Policy policy = this;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)mainPanel);
			val.set_Text("Accept the policy for using the module");
			((Control)val).set_Width(260);
			_button = val;
			((Control)_button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				((Control)policy._button).set_Enabled(false);
				if (await PolicyConfirmation.ShowPolicyConfirmationAsync())
				{
					settings.ContentUsePolicyConfirmation.set_Value(true);
				}
				((Control)policy._button).set_Enabled(true);
			});
		}

		public void Dispose()
		{
			((Control)_button).Dispose();
		}
	}
}
