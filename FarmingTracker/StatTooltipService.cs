using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class StatTooltipService
	{
		private const int ROW_HEIGHT = 22;

		public static void AddText(string text, BitmapFont font, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(font);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Parent(parent);
		}

		public static void AddProfitTable(Stat stat, long? unsigned_customStatProfitInCopper, BitmapFont font, Services services, Container parent)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			if (!stat.Profits.CanNotBeSold || unsigned_customStatProfitInCopper.HasValue)
			{
				if (!stat.IsSingleItem)
				{
					AddEmptyLine(parent);
				}
				FlowPanel val = new FlowPanel();
				val.set_FlowDirection((ControlFlowDirection)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Container)val).set_WidthSizingMode((SizingMode)1);
				((Control)val).set_Parent(parent);
				FlowPanel profitColumnsFlowPanel = val;
				AddTitleColumn(stat, unsigned_customStatProfitInCopper.HasValue, font, services, (Container)(object)profitColumnsFlowPanel);
				if (stat.IsSingleItem)
				{
					AddProfitColumn("", stat.Profits.Each, stat.CountSign * unsigned_customStatProfitInCopper, stat, font, services, (Container)(object)profitColumnsFlowPanel);
				}
				else
				{
					AddProfitColumn("all", stat.Profits.All, stat.Signed_Count * unsigned_customStatProfitInCopper, stat, font, services, (Container)(object)profitColumnsFlowPanel);
					AddProfitColumn("each", stat.Profits.Each, stat.CountSign * unsigned_customStatProfitInCopper, stat, font, services, (Container)(object)profitColumnsFlowPanel);
				}
				if (stat.Profits.CanBeSoldOnTp)
				{
					AddText("\n(15% trading post fee is already deducted from TP sell/buy)", font, parent);
				}
			}
		}

		private static void AddTitleColumn(Stat stat, bool hasCustomProfit, BitmapFont font, Services services, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			FlowPanel titleColumnFlowPanel = val;
			Label val2 = new Label();
			val2.set_Text(" ");
			val2.set_Font(font);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Height(22);
			((Control)val2).set_Parent((Container)(object)titleColumnFlowPanel);
			if (stat.Profits.CanBeSoldOnTp)
			{
				new IconLabel("TP Sell", services.TextureService.TradingPostTexture, 22, font, (Container)(object)titleColumnFlowPanel);
				new IconLabel("TP Buy", services.TextureService.TradingPostTexture, 22, font, (Container)(object)titleColumnFlowPanel);
			}
			if (stat.Profits.CanBeSoldToVendor)
			{
				new IconLabel("Vendor", services.TextureService.MerchantTexture, 22, font, (Container)(object)titleColumnFlowPanel);
			}
			if (hasCustomProfit)
			{
				new IconLabel("Custom", AsyncTexture2D.op_Implicit(services.TextureService.CustomStatProfitTabIconTexture), 22, font, (Container)(object)titleColumnFlowPanel);
			}
		}

		private static void AddProfitColumn(string columnHeaderText, Profit profit, long? signed_customStatProfitInCopper, Stat stat, BitmapFont font, Services services, Container parent)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			FlowPanel columnFlowPanel = val;
			Label val2 = new Label();
			val2.set_Text(columnHeaderText);
			val2.set_Font(font);
			((Control)val2).set_Height(22);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Parent((Container)(object)columnFlowPanel);
			Label headerLabel = val2;
			List<CoinsPanel> profitPanels = new List<CoinsPanel>();
			List<FixedWidthContainer> containers = new List<FixedWidthContainer>();
			if (stat.Profits.CanBeSoldOnTp)
			{
				FixedWidthContainer tpSellProfitContainer = new FixedWidthContainer((Container)(object)columnFlowPanel);
				CoinsPanel tpSellProfitPanel = new CoinsPanel(null, font, services.TextureService, (Container)(object)tpSellProfitContainer, 22);
				tpSellProfitPanel.SetCoins(stat.CountSign * profit.Unsigned_TpSellProfitInCopper);
				profitPanels.Add(tpSellProfitPanel);
				containers.Add(tpSellProfitContainer);
				FixedWidthContainer tpBuyProfitContainer = new FixedWidthContainer((Container)(object)columnFlowPanel);
				CoinsPanel tpBuyProfitPanel = new CoinsPanel(null, font, services.TextureService, (Container)(object)tpBuyProfitContainer, 22);
				tpBuyProfitPanel.SetCoins(stat.CountSign * profit.Unsigned_TpBuyProfitInCopper);
				profitPanels.Add(tpBuyProfitPanel);
				containers.Add(tpBuyProfitContainer);
			}
			if (stat.Profits.CanBeSoldToVendor)
			{
				FixedWidthContainer vendorProfitContainer2 = new FixedWidthContainer((Container)(object)columnFlowPanel);
				CoinsPanel vendorProfitPanel2 = new CoinsPanel(null, font, services.TextureService, (Container)(object)vendorProfitContainer2, 22);
				vendorProfitPanel2.SetCoins(stat.CountSign * profit.Unsigned_VendorProfitInCopper);
				profitPanels.Add(vendorProfitPanel2);
				containers.Add(vendorProfitContainer2);
			}
			if (signed_customStatProfitInCopper.HasValue)
			{
				FixedWidthContainer vendorProfitContainer = new FixedWidthContainer((Container)(object)columnFlowPanel);
				CoinsPanel vendorProfitPanel = new CoinsPanel(null, font, services.TextureService, (Container)(object)vendorProfitContainer, 22);
				vendorProfitPanel.SetCoins(signed_customStatProfitInCopper.Value);
				profitPanels.Add(vendorProfitPanel);
				containers.Add(vendorProfitContainer);
			}
			foreach (CoinsPanel item in profitPanels)
			{
				((Container)item).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate
				{
					int num = profitPanels.Max((CoinsPanel p) => ((Control)p).get_Width()) + 30;
					((Control)headerLabel).set_Width(num);
					for (int i = 0; i < containers.Count; i++)
					{
						((Control)containers[i]).set_Width(num);
						((Control)profitPanels[i]).set_Right(num);
					}
				});
			}
		}

		private static void AddEmptyLine(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(" ");
			((Control)val).set_Parent(parent);
		}
	}
}
