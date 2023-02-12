using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class MainSettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel2 = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-95, 0), new Point(0, 5)).AddSetting((SettingEntry)(object)Service.Settings.SettingsPanelKeyBind).AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.ApiPollingPeriod)
				.AddSpace();
			StandardButton val = new StandardButton();
			val.set_Text(Strings.Settings_RefreshNow);
			Control refreshButton;
			FlowPanel panel = panel2.AddFlowControl((Control)val, out refreshButton).AddSpace().AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.GlobalCornerIconEnabled);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val2).set_Parent((Container)(object)panel);
			((Panel)val2).set_ShowTint(false);
			((Panel)val2).set_ShowBorder(false);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Control)panel).get_Width() - 40);
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val2, Strings.Setting_cornerIconHelpText).AddSetting((SettingEntry)(object)Service.Settings.RaidSettings.Generic.ToolbarIcon).AddSetting((SettingEntry)(object)Service.Settings.DungeonSettings.Generic.ToolbarIcon)
				.AddSetting((SettingEntry)(object)Service.Settings.StrikeSettings.Generic.ToolbarIcon));
			refreshButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.ApiPollingService?.Invoke();
				refreshButton.set_Enabled(false);
			});
		}

		public MainSettingsView()
			: this()
		{
		}
	}
}
