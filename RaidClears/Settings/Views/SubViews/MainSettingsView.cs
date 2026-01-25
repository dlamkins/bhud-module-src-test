using System;
using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;
using RaidClears.Localization;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class MainSettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = new FlowPanel();
			FlowPanel panel2 = panel.BeginFlow(buildPanel, new Point(-95, 0), new Point(0, 5));
			StandardButton val = new StandardButton();
			val.set_Text(Strings.PatchNotes);
			((Control)val).set_BasicTooltipText(Strings.PatchNotes_Tooltip);
			((Control)val).set_Width(300);
			Control patchNotesButton;
			FlowPanel panel3 = panel2.AddFlowControl((Control)val, out patchNotesButton).AddSpace().AddSetting((SettingEntry)(object)Service.Settings.SettingsPanelKeyBind)
				.AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.ApiPollingPeriod)
				.AddSpace();
			StandardButton val2 = new StandardButton();
			val2.set_Text(Strings.Settings_RefreshNow);
			((Control)val2).set_Width(300);
			((Container)(object)panel3.AddFlowControl((Control)val2, out var refreshButton).AddSpace().AddSetting((SettingEntry)(object)Service.Settings.OrganicGridBoxBackgrounds)).AddControl((Control)(object)new GridBox((Container)(object)panel, Strings.Settings_Main_Demo, Strings.Settings_Main_ExampleEncounterBox, Service.Settings.RaidSettings.Style.GridOpacity, Service.Settings.RaidSettings.Style.FontSize), out var grid);
			panel.AddSpace().AddSetting((SettingEntry)(object)Service.Settings.ScreenClamp).AddSpace()
				.AddSetting((SettingEntry)(object)Service.Settings.GlobalCornerIconEnabled);
			(grid as GridBox).BackgroundColor = Service.Settings.RaidSettings.Style.Color.Cleared.get_Value().ToString().HexToXnaColor();
			Control obj = grid;
			obj.set_Location(obj.get_Location() + new Point(20, 0));
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val3).set_Parent((Container)(object)panel);
			((Panel)val3).set_ShowTint(false);
			((Panel)val3).set_ShowBorder(false);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Control)panel).get_Width() - 40);
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val3, Strings.Setting_cornerIconHelpText).AddSetting((SettingEntry)(object)Service.Settings.RaidSettings.Generic.ToolbarIcon).AddSetting((SettingEntry)(object)Service.Settings.DungeonSettings.Generic.ToolbarIcon)
				.AddSetting((SettingEntry)(object)Service.Settings.StrikeSettings.Generic.ToolbarIcon)
				.AddSetting((SettingEntry)(object)Service.Settings.FractalSettings.Generic.ToolbarIcon));
			panel.AddSpace().AddString(Strings.CornerIconPriority_Help).AddSetting((SettingEntry)(object)Service.Settings.CornerIconPriority);
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
