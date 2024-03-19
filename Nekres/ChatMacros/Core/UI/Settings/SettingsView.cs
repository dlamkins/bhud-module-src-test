using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Nekres.ChatMacros.Core.UI.Credits;

namespace Nekres.ChatMacros.Core.UI.Settings
{
	internal class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width / 2);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height / 2);
			ViewContainer voiceRecognitionSettings = val;
			voiceRecognitionSettings.Show((IView)(object)new VoiceRecognitionSettings(ChatMacros.Instance.InputConfig.get_Value()));
			ViewContainer val2 = new ViewContainer();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Top(((Control)voiceRecognitionSettings).get_Bottom());
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width / 2);
			((Control)val2).set_Height(buildPanel.get_ContentRegion().Height / 2);
			val2.Show((IView)(object)new ControlSettings(ChatMacros.Instance.ControlsConfig.get_Value()));
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Width(buildPanel.get_ContentRegion().Width / 2);
			((Control)val3).set_Height(buildPanel.get_ContentRegion().Height);
			((Control)val3).set_Left(((Control)voiceRecognitionSettings).get_Right());
			val3.Show((IView)(object)new CreditsView());
			((View<IPresenter>)this).Build(buildPanel);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
