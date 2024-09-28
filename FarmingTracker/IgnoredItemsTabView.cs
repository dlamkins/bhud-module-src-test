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
	public class IgnoredItemsTabView : View
	{
		private readonly Model _model;

		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		private const string IGNORED_ITEMS_PANEL_TITLE = "Ignored Items";

		public IgnoredItemsTabView(Model model, Services services)
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
			CollapsibleHelp collapsibleHelp = new CollapsibleHelp("IGNORE ITEM:\nIn the 'Summary' tab right click on an item icon in the 'Items' panel to ignore it.\n\nUNIGNORE ITEM:\nleft click on an item here to unignore it.\n\nWHY IGNORE?\nAn ignored item will appear here. It is hidden in the 'Summary' tab and does not contribute to profit calculations. That can be usefull to prevent that none-legendary equipment that you swap manually is tracked accidently.", buildPanel.get_ContentRegion().Width - 30, (Container)(object)_rootFlowPanel);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				collapsibleHelp.UpdateSize(e.get_CurrentRegion().Width - 30);
			});
			AutoSizeContainer flowPanelWithButtonContainer = new AutoSizeContainer((Container)(object)_rootFlowPanel);
			FlowPanel val2 = new FlowPanel();
			((Panel)val2).set_Title("Ignored Items");
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_Icon(AsyncTexture2D.op_Implicit(_services.TextureService.IgnoredItemsPanelIconTexture));
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width - 30);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)flowPanelWithButtonContainer);
			FlowPanel ignoredItemsWrapperFlowPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Unignore all items");
			((Control)val3).set_Enabled(false);
			((Control)val3).set_Width(150);
			((Control)val3).set_Top(5);
			((Control)val3).set_Right(buildPanel.get_ContentRegion().Width - 30);
			((Control)val3).set_Parent((Container)(object)flowPanelWithButtonContainer);
			StandardButton unignoreAllButton = val3;
			HintLabel hintLabel = new HintLabel((Container?)(object)ignoredItemsWrapperFlowPanel, "");
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Control)val4).set_Parent((Container)(object)ignoredItemsWrapperFlowPanel);
			FlowPanel ignoredItemsFlowPanel = val4;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				((Control)ignoredItemsWrapperFlowPanel).set_Width(e.get_CurrentRegion().Width - 30);
				((Control)unignoreAllButton).set_Right(e.get_CurrentRegion().Width - 30);
			});
			List<Stat> ignoredItems = (from i in _model.IgnoredItemApiIds.ToListSafe()
				select _model.Stats.StatsSnapshot.ItemById[i]).ToList();
			if (ignoredItems.IsEmpty())
			{
				ShowNoItemsAreIgnoredHintIfNecessary(hintLabel, _model);
				return;
			}
			if (ignoredItems.Any((Stat i) => i.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet))
			{
				ShowLoadingHint(hintLabel);
				return;
			}
			((Label)hintLabel).set_Text("  Left click an item to unignore it.");
			foreach (Stat item in ignoredItems)
			{
				ShowIgnoredItem(item, _model, _services, hintLabel, (Container)(object)ignoredItemsFlowPanel);
			}
			((Control)unignoreAllButton).set_Enabled(true);
			((Control)unignoreAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (Control item2 in ((Container)ignoredItemsFlowPanel).get_Children().ToList())
				{
					item2.Dispose();
				}
				_model.IgnoredItemApiIds.ClearSafe();
				_services.UpdateLoop.TriggerUpdateUi();
				_services.UpdateLoop.TriggerSaveModel();
				ShowNoItemsAreIgnoredHintIfNecessary(hintLabel, _model);
			});
		}

		private static void ShowIgnoredItem(Stat ignoredItem, Model model, Services services, HintLabel hintLabel, Container parent)
		{
			Stat ignoredItem2 = ignoredItem;
			Model model2 = model;
			Services services2 = services;
			HintLabel hintLabel2 = hintLabel;
			StatContainer statContainer2 = new StatContainer(ignoredItem2, PanelType.IgnoredItems, model2.IgnoredItemApiIds, model2.FavoriteItemApiIds, model2.CustomStatProfits, services2);
			((Control)statContainer2).set_Parent(parent);
			StatContainer statContainer = statContainer2;
			((Control)statContainer).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				UnignoreItem(ignoredItem2, model2, services2);
				((Control)statContainer).Dispose();
				ShowNoItemsAreIgnoredHintIfNecessary(hintLabel2, model2);
			});
		}

		private static void UnignoreItem(Stat item, Model model, Services services)
		{
			model.IgnoredItemApiIds.RemoveSafe(item.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void ShowNoItemsAreIgnoredHintIfNecessary(HintLabel hintLabel, Model model)
		{
			if (!model.IgnoredItemApiIds.AnySafe())
			{
				((Label)hintLabel).set_Text("  No items are ignored.\n  You can ignore an item by right clicking it in the 'Summary' tab.");
			}
		}

		private static void ShowLoadingHint(HintLabel hintLabel)
		{
			((Label)hintLabel).set_Text("  This tab will not refresh automatically.\n  Go to 'Summary' tab and wait until the 'Updating...' hint disappears.\n  Then come back here and your ignored items will be displayed.");
		}
	}
}
