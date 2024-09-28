using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class CustomStatProfitTabView : View
	{
		private readonly Model _model;

		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		public CustomStatProfitTabView(Model model, Services services)
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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			CollapsibleHelp collapsibleHelp = new CollapsibleHelp("ADD CUSTOM PROFIT:\nIn the 'Summary' tab right click on an item or currency icon to set its custom profit to 0. After that you can edit the custom profit in this tab.\n\nREMOVE CUSTOM PROFIT:\nIn this tab click on the 'x' button of a custom profit row.\n\nHOW DOES IT WORK?\n- If a custom profit is set, the custom profit will be used to calculate the total profit instead of the trading post or vendor sell price. Even when the custom profit is lower or 0.\n- Trading post taxes are not deducted from the custom profit.\n- The custom profit applies to a single item/currency. Because of that you can not set the custom profit of 10 karma to 1 copper for example (0.1 copper/Karma).\n\nWHY CUSTOM PROFIT?\n- Currencies like volatile magic cannot be sold to the vendor or on the trading post. But there are other ways to make profit with them. Use the custom profit to take that into account depending on the method you use.\n- Set an item with an unrealistically high/low trading post price to a more realistic custom profit.", buildPanel.get_ContentRegion().Width - 30, (Container)(object)_rootFlowPanel);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				collapsibleHelp.UpdateSize(e.get_CurrentRegion().Width - 30);
			});
			HintLabel hintLabel = new HintLabel((Container?)(object)_rootFlowPanel, "");
			List<CustomStatProfit> customStatProfits = _model.CustomStatProfits.ToListSafe();
			if (customStatProfits.IsEmpty())
			{
				ShowNoCustomStatProfitsExistHintIfNecessary(hintLabel, _model);
				return;
			}
			StatsSnapshot statsSnapshot = _model.Stats.StatsSnapshot;
			List<Stat> currencies = statsSnapshot.CurrencyById.Values.Where((Stat c) => !c.IsCoin).ToList();
			IEnumerable<Stat> items = statsSnapshot.ItemById.Values;
			bool num = currencies.Any((Stat i) => i.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet);
			bool itemApiDataMissing = items.Any((Stat i) => i.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet);
			if (num || itemApiDataMissing)
			{
				ShowLoadingHint(hintLabel);
				return;
			}
			FlowPanel statsFlowPanel = CreateStatsFlowPanel(buildPanel, (Container)(object)_rootFlowPanel);
			foreach (CustomStatProfit customStatProfit in customStatProfits)
			{
				IEnumerable<Stat> source;
				if (customStatProfit.StatType != 0)
				{
					IEnumerable<Stat> enumerable = currencies;
					source = enumerable;
				}
				else
				{
					source = items;
				}
				Stat stat = source.SingleOrDefault((Stat s) => customStatProfit.BelongsToStat(s));
				if (stat == null)
				{
					Module.Logger.Error($"Missing stat in model for customStatprofit id: {customStatProfit.ApiId}");
				}
				else
				{
					new CustomStatProfitRowPanel(customStatProfit, stat, hintLabel, _model, _services, (Container)(object)statsFlowPanel);
				}
			}
		}

		private static FlowPanel CreateStatsFlowPanel(Container buildPanel, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(0f, 5f));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 30);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			return val;
		}

		public static void ShowNoCustomStatProfitsExistHintIfNecessary(HintLabel hintLabel, Model model)
		{
			if (!model.CustomStatProfits.AnySafe())
			{
				((Label)hintLabel).set_Text("  No custom profits are set.\n  You can set custom profits by right clicking an item/currency in the 'Summary' tab.");
			}
		}

		private static void ShowLoadingHint(HintLabel hintLabel)
		{
			((Label)hintLabel).set_Text("  This tab will not refresh automatically.\n  Go to 'Summary' tab and wait until the 'Updating...' hint disappears.\n  Then come back here.");
		}
	}
}
