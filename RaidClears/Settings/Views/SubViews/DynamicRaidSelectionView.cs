using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class DynamicRaidSelectionView : View
	{
		private readonly RaidSettingsPersistance _setting = Service.RaidSettings;

		private readonly RaidData _data = Service.RaidData;

		public DynamicRaidSelectionView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			((Panel)panel).set_CanScroll(true);
			foreach (ExpansionRaid expac in _data.Expansions)
			{
				BuildExpansionPanel(panel, expac);
			}
		}

		private void BuildExpansionPanel(FlowPanel panel, ExpansionRaid expac)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			List<SettingEntry<bool>> wings = new List<SettingEntry<bool>>();
			foreach (RaidWing wing2 in expac.Wings)
			{
				wings.Add(_setting.GetWingVisible(wing2));
			}
			FlowPanel panel2 = panel.AddSetting((SettingEntry)(object)_setting.GetExpansionVisible(expac));
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent((Container)(object)panel);
			((Panel)val).set_ShowTint(false);
			((Panel)val).set_ShowBorder(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val).set_BackgroundTexture(Service.Textures!.GetDynamicTexture(expac.asset));
			panel2.AddFlowControl((Control)val, out var childPanel);
			foreach (RaidWing wing in expac.Wings)
			{
				BuildWingPanel((FlowPanel)childPanel, wing);
			}
			for (int i = wings.Count; i <= 4; i++)
			{
				FlowPanelExtensions.AddSpace((FlowPanel)childPanel);
			}
		}

		private void BuildWingPanel(FlowPanel panel, RaidWing raidWing)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			List<SettingEntry<bool>> encounters = new List<SettingEntry<bool>>();
			foreach (RaidEncounter enc in raidWing.Encounters)
			{
				encounters.Add(_setting.GetEncounterVisible(enc));
			}
			FlowPanel panel2 = panel.AddSetting((SettingEntry)(object)_setting.GetWingVisible(raidWing));
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			panel2.AddFlowControl((Control)(object)FlowPanelExtensions.AddHorizontalSpace(val, 20).AddSetting((IEnumerable<SettingEntry>?)encounters, ((Control)panel).get_Width() / 8));
		}
	}
}
