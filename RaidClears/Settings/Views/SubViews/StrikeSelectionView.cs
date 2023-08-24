using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class StrikeSelectionView : View
	{
		private readonly StrikeSettings _settings;

		public StrikeSelectionView(StrikeSettings settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSetting((SettingEntry)(object)_settings.StrikeVisiblePriority).AddSpace();
			((Panel)panel).set_CanScroll(true);
			FlowPanel panel2 = panel.AddSetting((SettingEntry)(object)_settings.StrikeVisibleIbs);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent((Container)(object)panel);
			((Panel)val).set_ShowTint(false);
			((Panel)val).set_ShowBorder(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.IBSLogo));
			FlowPanel panel3 = panel2.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val, Strings.Settings_Strike_IBS_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.IbsMissions).AddSpace()).AddSetting((SettingEntry)(object)_settings.StrikeVisibleEod);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val2).set_Parent((Container)(object)panel);
			((Panel)val2).set_ShowTint(false);
			((Panel)val2).set_ShowBorder(false);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val2).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.EoDLogo));
			FlowPanel panel4 = panel3.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val2, Strings.Settings_Strike_EOD_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.EodMissions).AddSpace()).AddSetting((SettingEntry)(object)_settings.StrikeVisibleSotO);
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val3).set_Parent((Container)(object)panel);
			((Panel)val3).set_ShowTint(false);
			((Panel)val3).set_ShowBorder(false);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val3).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.SotOLogo));
			panel4.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val3, "Enable individual Secrets of the Obscure strikes").AddSetting((IEnumerable<SettingEntry>?)_settings.SotOMissions).AddSpace()
				.AddSpace()
				.AddSpace()
				.AddSpace());
		}
	}
}
