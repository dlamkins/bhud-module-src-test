using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;

namespace Nekres.Mumble_Info.Core.UI
{
	internal class CustomSettingsView : View
	{
		private StandardButton _settingsBttn;

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(200);
			((Control)val).set_Height(40);
			((Control)val).set_Left((buildPanel.get_ContentRegion().Width - 200) / 2);
			((Control)val).set_Top(buildPanel.get_ContentRegion().Height / 2 - 40);
			val.set_Text("Open Mumble Info Panel");
			_settingsBttn = val;
			((Control)_settingsBttn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				MumbleInfoModule.Instance.ToggleWindow();
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		public CustomSettingsView()
			: this()
		{
		}
	}
}
