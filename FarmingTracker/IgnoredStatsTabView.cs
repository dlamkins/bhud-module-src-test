using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class IgnoredStatsTabView : View
	{
		private readonly Model _model;

		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		private const string IGNORED_STATS_PANEL_TITLE = "Ignored items and currencies";

		public IgnoredStatsTabView(Model model, Services services)
			: this()
		{
			_model = model;
			_services = services;
		}

		protected override void Unload()
		{
			FlowPanel? rootFlowPanel = _rootFlowPanel;
			if (rootFlowPanel != null)
			{
				((Control)rootFlowPanel).Dispose();
			}
			_rootFlowPanel = null;
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			CollapsibleHelp collapsibleHelp = new CollapsibleHelp("IGNORE ITEM / CURRENCY:\nIn the 'Summary' tab right click on an item / currency icon to ignore it.\n\nUNIGNORE ITEM / CURRENCY:\nleft click on an item or currency here to unignore it.\n\nWHY IGNORE?\nAn ignored item / currency will appear here. It is hidden in the 'Summary' tab and does not contribute to profit calculations. That can be usefull to prevent that none-legendary equipment that you swap manually is tracked accidently.", buildPanel.get_ContentRegion().Width - 30, (Container)(object)_rootFlowPanel);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				collapsibleHelp.UpdateSize(e.get_CurrentRegion().Width - 30);
			});
			AutoSizeContainer flowPanelWithButtonContainer = new AutoSizeContainer((Container)(object)_rootFlowPanel);
			FlowPanel val2 = new FlowPanel();
			((Panel)val2).set_Title("Ignored items and currencies");
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_Icon(AsyncTexture2D.op_Implicit(_services.TextureService.IgnoredStatsPanelIconTexture));
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width - 30);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)flowPanelWithButtonContainer);
			FlowPanel ignoredStatsWrapperFlowPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Unignore all");
			((Control)val3).set_Enabled(false);
			((Control)val3).set_Width(150);
			((Control)val3).set_Top(5);
			((Control)val3).set_Right(buildPanel.get_ContentRegion().Width - 30);
			((Control)val3).set_Parent((Container)(object)flowPanelWithButtonContainer);
			StandardButton unignoreAllButton = val3;
			HintLabel hintLabel = new HintLabel((Container?)(object)ignoredStatsWrapperFlowPanel, "");
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Control)val4).set_Parent((Container)(object)ignoredStatsWrapperFlowPanel);
			FlowPanel ignoredStatsFlowPanel = val4;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				((Control)ignoredStatsWrapperFlowPanel).set_Width(e.get_CurrentRegion().Width - 30);
				((Control)unignoreAllButton).set_Right(e.get_CurrentRegion().Width - 30);
			});
			IEnumerable<Stat> ignoredStats = from s in _model.Stats.GetStats()
				where s.StatVisibility == StatVisibility.Ignored
				select s;
			if (ignoredStats.IsEmpty())
			{
				ShowNoStatsAreIgnoredHintIfNecessary(hintLabel, _model);
				return;
			}
			if (ignoredStats.Any((Stat i) => i.Details.State == StatApiDetailsState.MissingBecauseApiNotCalledYet))
			{
				ShowLoadingHint(hintLabel);
				return;
			}
			((Label)hintLabel).set_Text("Left click an item or currency to unignore it.");
			foreach (Stat item in ignoredStats)
			{
				ShowIgnoredStat(item, _model, _services, hintLabel, (Container)(object)ignoredStatsFlowPanel);
			}
			((Control)unignoreAllButton).set_Enabled(true);
			((Control)unignoreAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (Control item2 in ((Container)ignoredStatsFlowPanel).get_Children().ToList())
				{
					item2.Dispose();
				}
				foreach (Stat current in ignoredStats)
				{
					if (current.StatVisibility == StatVisibility.Ignored)
					{
						current.StatVisibility = StatVisibility.Regular;
					}
				}
				_services.UpdateLoop.TriggerUpdateUi();
				_services.UpdateLoop.TriggerSaveModel();
				ShowNoStatsAreIgnoredHintIfNecessary(hintLabel, _model);
			});
		}

		private static void ShowIgnoredStat(Stat ignoredStat, Model model, Services services, HintLabel hintLabel, Container parent)
		{
			Stat ignoredStat2 = ignoredStat;
			Model model2 = model;
			Services services2 = services;
			HintLabel hintLabel2 = hintLabel;
			StatContainer statContainer2 = new StatContainer(ignoredStat2, PanelType.IgnoredStats, model2, services2);
			((Control)statContainer2).set_Parent(parent);
			StatContainer statContainer = statContainer2;
			((Control)statContainer).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				UnignoreStat(ignoredStat2, model2, services2);
				((Control)statContainer).Dispose();
				ShowNoStatsAreIgnoredHintIfNecessary(hintLabel2, model2);
			});
		}

		private static void UnignoreStat(Stat stat, Model model, Services services)
		{
			Stat stat2 = stat;
			Stat matchingFavoriteStat = (from s in model.Stats.GetStats()
				where s.StatVisibility == StatVisibility.Ignored
				select s).FirstOrDefault((Stat i) => i.StatType == stat2.StatType && i.ApiId == stat2.ApiId);
			if (matchingFavoriteStat == null)
			{
				Module.Logger.Error("Failed to remove ignored stat because ignored stat did not exist. That should not be possible.");
				return;
			}
			matchingFavoriteStat.StatVisibility = StatVisibility.Regular;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void ShowNoStatsAreIgnoredHintIfNecessary(HintLabel hintLabel, Model model)
		{
			if (!(from s in model.Stats.GetStats()
				where s.StatVisibility == StatVisibility.Ignored
				select s).Any())
			{
				((Label)hintLabel).set_Text("Nothing is ignored.\nYou can ignore an item or currency by right clicking it in the 'Summary' tab.");
			}
		}

		private static void ShowLoadingHint(HintLabel hintLabel)
		{
			((Label)hintLabel).set_Text("This tab will not refresh automatically.\nGo to 'Summary' tab and wait until the 'Updating...' hint disappears.\nThen come back here and your ignored items and currencies will be displayed.");
		}
	}
}
