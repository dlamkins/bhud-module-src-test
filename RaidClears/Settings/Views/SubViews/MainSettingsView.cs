using System;
using System.Diagnostics;
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
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel2 = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-95, 0), new Point(0, 5));
			StandardButton val = new StandardButton();
			val.set_Text(Strings.PatchNotes);
			((Control)val).set_BasicTooltipText(Strings.PatchNotes_Tooltip);
			Control patchNotesButton;
			FlowPanel panel3 = panel2.AddFlowControl((Control)val, out patchNotesButton).AddSpace().AddSetting((SettingEntry)(object)Service.Settings.SettingsPanelKeyBind)
				.AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.ApiPollingPeriod)
				.AddSpace();
			StandardButton val2 = new StandardButton();
			val2.set_Text(Strings.Settings_RefreshNow);
			Control refreshButton;
			FlowPanel panel = panel3.AddFlowControl((Control)val2, out refreshButton).AddSpace().AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.GlobalCornerIconEnabled);
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val3).set_Parent((Container)(object)panel);
			((Panel)val3).set_ShowTint(false);
			((Panel)val3).set_ShowBorder(false);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Control)panel).get_Width() - 40);
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val3, Strings.Setting_cornerIconHelpText).AddSetting((SettingEntry)(object)Service.Settings.RaidSettings.Generic.ToolbarIcon).AddSetting((SettingEntry)(object)Service.Settings.DungeonSettings.Generic.ToolbarIcon)
				.AddSetting((SettingEntry)(object)Service.Settings.StrikeSettings.Generic.ToolbarIcon));
			refreshButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.ApiPollingService?.Invoke();
				refreshButton.set_Enabled(false);
			});
			patchNotesButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://pkgs.blishhud.com/Soeed.RaidClears.html",
					UseShellExecute = true
				});
			});
		}

		public MainSettingsView()
			: this()
		{
		}
	}
}
