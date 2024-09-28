using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SummaryTabViewControls
	{
		public FlowPanel RootFlowPanel { get; }

		public Label DrfErrorLabel { get; }

		public OpenSettingsButton OpenSettingsButton { get; }

		public FlowPanel FarmingRootFlowPanel { get; }

		public ProfitPanels ProfitPanels { get; }

		public SearchPanel SearchPanel { get; }

		public StatsPanels StatsPanels { get; }

		public ElapsedFarmingTimeLabel ElapsedFarmingTimeLabel { get; }

		public Label HintLabel { get; }

		public CollapsibleHelp CollapsibleHelp { get; }

		public StandardButton ResetButton { get; }

		public SummaryTabViewControls(Model model, Services services)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Expected O, but got Unknown
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Expected O, but got Unknown
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Expected O, but got Unknown
			Services services2 = services;
			Model model2 = model;
			base._002Ector();
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			RootFlowPanel = val;
			Label val2 = new Label();
			val2.set_Text("");
			val2.set_Font(services2.FontService.Fonts[(FontSize)18]);
			val2.set_TextColor(Color.get_Yellow());
			val2.set_StrokeText(true);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)RootFlowPanel);
			DrfErrorLabel = val2;
			OpenSettingsButton = new OpenSettingsButton("Open settings tab to setup DRF", services2.WindowTabSelector, (Container)(object)RootFlowPanel);
			((Control)OpenSettingsButton).Hide();
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 5f));
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent((Container)(object)RootFlowPanel);
			FarmingRootFlowPanel = val3;
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)2);
			val4.set_ControlPadding(new Vector2(5f, 0f));
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Parent((Container)(object)FarmingRootFlowPanel);
			FlowPanel buttonFlowPanel = val4;
			CollapsibleHelp = new CollapsibleHelp("SETUP AND TROUBLESHOOTING:\nDRF setup instructions and DRF and module troubleshooting can be found in the 'Settings' tab.\n\nCONTEXT MENU:\n- right click an item/currency to open a context menu with additional features.\n- the context menu entries differs for items, favorites, currencies, coins.\n\nPROFIT:\n- 15% trading post fee is already deducted.\n- Profit also includes changes in 'raw gold'. In other words coins spent or gained. 'raw gold' changes are also visible in the 'Currencies' panel.\n- Lost items reduce the profit accordingly.\n- Currencies are not included in the profit calculation (except 'raw gold').\n- rough profit = raw gold + item count * tp sell price * 0.85 + ...for all items.\n- When tp sell price does not exist, tp buy price will be used. Vendor price will be used when it is higher than tp sell/buy price * 0.85.\n- Module and DRF live tracking website profit calculation may differ because different profit formulas are used.\n" + $"- Profit per hour is updated every {5} seconds.\n" + "- The profit is only a rough estimate because the trading post buy/sell prices can change over time and only the highest tp buy price and tp sell price for an item are considered. The tp buy/sell prices are a snapshot from when the item was tracked for the first time during a blish sesssion.\n\nRESIZE:\n- You can resize the window by dragging the bottom right window corner. - Some UI elements might be cut off when the window becomes too small.", 270, (Container)(object)buttonFlowPanel);
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)0);
			val5.set_ControlPadding(new Vector2(5f, 5f));
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Parent((Container)(object)buttonFlowPanel);
			FlowPanel subButtonFlowPanel = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Reset");
			((Control)val6).set_BasicTooltipText("Start new farming session by resetting tracked items and currencies.");
			((Control)val6).set_Width(90);
			((Control)val6).set_Parent((Container)(object)subButtonFlowPanel);
			ResetButton = val6;
			((Control)new OpenUrlInBrowserButton("https://drf.rs/dashboard/livetracker/summary", "DRF", "Open DRF live tracking website in your default web browser.\nThe module and the DRF live tracking web page are both DRF clients. But they are independent of each other. They do not synchronize the data they display. So one client may show less or more data dependend on when the client session started.", services2.TextureService.OpenLinkTexture, (Container)(object)subButtonFlowPanel)).set_Width(60);
			StandardButton val7 = new StandardButton();
			val7.set_Text("Export CSV");
			((Control)val7).set_BasicTooltipText("Export tracked items and currencies to '" + services2.CsvFileExporter.ModuleFolderPath + "\\<date-time>.csv'.\nThis feature can be used to import the tracked items/currencies in Microsoft Excel for example.");
			((Control)val7).set_Width(90);
			((Control)val7).set_Parent((Container)(object)subButtonFlowPanel);
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await services2.CsvFileExporter.ExportSummaryAsCsvFile(model2);
			});
			FlowPanel val8 = new FlowPanel();
			val8.set_FlowDirection((ControlFlowDirection)0);
			val8.set_ControlPadding(new Vector2(20f, 0f));
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			((Control)val8).set_Parent((Container)(object)FarmingRootFlowPanel);
			FlowPanel timeAndHintFlowPanel = val8;
			ElapsedFarmingTimeLabel = new ElapsedFarmingTimeLabel(services2, (Container)(object)timeAndHintFlowPanel);
			Label val9 = new Label();
			val9.set_Text(" ");
			val9.set_Font(services2.FontService.Fonts[(FontSize)14]);
			((Control)val9).set_Width(250);
			val9.set_AutoSizeHeight(true);
			((Control)val9).set_Parent((Container)(object)timeAndHintFlowPanel);
			HintLabel = val9;
			ProfitPanels = new ProfitPanels(services2, isProfitWindow: false, (Container)(object)FarmingRootFlowPanel);
			SearchPanel = new SearchPanel(services2, (Container)(object)FarmingRootFlowPanel);
			StatsPanels = CreateStatsPanels(services2, (Container)(object)FarmingRootFlowPanel);
		}

		private static StatsPanels CreateStatsPanels(Services services, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel currenciesFilterIconPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Panel)val2).set_Title("Currencies");
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val2).set_Icon(services.TextureService.MerchantTexture);
			((Panel)val2).set_CanCollapse(true);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)currenciesFilterIconPanel);
			FlowPanel val3 = new FlowPanel();
			((Panel)val3).set_Title("Favorite Items");
			val3.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val3).set_Icon(services.TextureService.FavoriteTexture);
			((Panel)val3).set_CanCollapse(true);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent(parent);
			FlowPanel favoriteItemsFlowPanel = val3;
			Panel val4 = new Panel();
			((Container)val4).set_WidthSizingMode((SizingMode)1);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Parent(parent);
			Panel itemsFilterIconPanel = val4;
			FlowPanel val5 = new FlowPanel();
			((Panel)val5).set_Title("Items");
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val5).set_Icon(services.TextureService.ItemsTexture);
			((Panel)val5).set_CanCollapse(true);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Parent((Container)(object)itemsFilterIconPanel);
			FlowPanel itemsFlowPanel = val5;
			ClickThroughImage currencyFilterIcon = new ClickThroughImage(services.TextureService.FilterTabIconTexture, new Point(380, 3), (Container)(object)currenciesFilterIconPanel);
			ClickThroughImage itemsFilterIcon = new ClickThroughImage(services.TextureService.FilterTabIconTexture, new Point(380, 3), (Container)(object)itemsFilterIconPanel);
			StatsPanels statsPanels = new StatsPanels(val2, favoriteItemsFlowPanel, itemsFlowPanel, currencyFilterIcon, itemsFilterIcon);
			new HintLabel((Container?)(object)statsPanels.CurrenciesFlowPanel, "  Loading...");
			new HintLabel((Container?)(object)statsPanels.FavoriteItemsFlowPanel, "  Loading...");
			new HintLabel((Container?)(object)statsPanels.ItemsFlowPanel, "  Loading...");
			return statsPanels;
		}
	}
}
