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
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSetting((SettingEntry)(object)_settings.StrikeVisiblePriority).AddSpace();
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
			FlowPanel panel3 = panel2.AddChildPanel((Panel)(object)FlowPanelExtensions.AddSpace(val).AddString(Strings.Settings_Strike_IBS_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.IbsMissions)
				.AddSpace()).AddSpace().AddSetting((SettingEntry)(object)_settings.StrikeVisibleEod);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val2).set_Parent((Container)(object)panel);
			((Panel)val2).set_ShowTint(false);
			((Panel)val2).set_ShowBorder(false);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val2).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.EoDLogo));
			panel3.AddChildPanel((Panel)(object)FlowPanelExtensions.AddSpace(val2).AddString(Strings.Settings_Strike_EOD_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.EodMissions)
				.AddSpace()
				.AddSpace());
		}
	}
}
