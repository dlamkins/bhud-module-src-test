using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Strikes.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class DynamicStrikeSelectionView : View
	{
		private readonly StrikeSettingsPersistance _setting = Service.StrikeSettings;

		private readonly StrikeData _data = Service.StrikeData;

		public DynamicStrikeSelectionView()
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
			BuildPrioriyPanel(panel, _data.Priority);
			foreach (ExpansionStrikes expac in _data.Expansions)
			{
				BuildExpansionPanel(panel, expac);
			}
		}

		private void BuildPrioriyPanel(FlowPanel panel, ExpansionStrikes expac)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent((Container)(object)panel);
			((Panel)val).set_ShowTint(false);
			((Panel)val).set_ShowBorder(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val).set_BackgroundTexture(Service.Textures!.GetDynamicTexture(expac.asset));
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddString(val, "Display " + expac.Name + " strike missions").AddSetting((SettingEntry)(object)_data.GetPriorityVisible()).AddSpace());
		}

		private void BuildExpansionPanel(FlowPanel panel, ExpansionStrikes expac)
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
			//IL_00df: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			List<SettingEntry<bool>> expansionMissions = new List<SettingEntry<bool>>();
			foreach (StrikeMission mission in expac.Missions)
			{
				expansionMissions.Add(_data.GetMissionVisible(mission));
			}
			FlowPanel panel2 = panel.AddSetting((SettingEntry)(object)_data.GetExpansionVisible(expac));
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent((Container)(object)panel);
			((Panel)val).set_ShowTint(false);
			((Panel)val).set_ShowBorder(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val).set_BackgroundTexture(Service.Textures!.GetDynamicTexture(expac.asset));
			panel2.AddFlowControl((Control)(object)FlowPanelExtensions.AddString(val, "Display individual " + expac.Name + " strike missions").AddSetting((IEnumerable<SettingEntry>?)expansionMissions).AddSpace(), out var childPanel);
			for (int i = expansionMissions.Count; i <= 5; i++)
			{
				FlowPanelExtensions.AddSpace((FlowPanel)childPanel);
			}
		}
	}
}
