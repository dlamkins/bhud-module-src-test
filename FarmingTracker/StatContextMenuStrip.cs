using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class StatContextMenuStrip : CustomContextMenuStrip
	{
		private readonly CustomContextMenuStripItem _generalHeaderMenuItem;

		private readonly CustomContextMenuStripItem _copyHeaderMenuItem;

		private readonly CustomContextMenuStripItem _websitesHeaderMenuItem;

		private readonly CustomContextMenuStripItem _wikiMenuItem;

		private readonly CustomContextMenuStripItem? _ignoreMenuItem;

		private readonly CustomContextMenuStripItem? _addFavoriteMenuItem;

		private readonly CustomContextMenuStripItem? _removeFavoriteMenuItem;

		private readonly CustomContextMenuStripItem? _setCustomProfitMenuItem;

		private readonly CustomContextMenuStripItem _gw2BltcMenuItem;

		private readonly CustomContextMenuStripItem _gw2TreasuresMenuItem;

		private readonly CustomContextMenuStripItem _gw2TpMenuItem;

		private readonly CustomContextMenuStripItem _gw2ProfitsMenuItem;

		private readonly CustomContextMenuStripItem _copyNameMenuItem;

		private readonly CustomContextMenuStripItem _gw2EfficiencyTradingPostMenuItem;

		private readonly CustomContextMenuStripItem _gw2EfficiencyAccountMenuItem;

		private readonly CustomContextMenuStripItem _copyChatLinkMenuItem;

		private const string NOT_AVAILABLE_FOR_CURRENCY_TOOLTIP = "Not available for currencies";

		public StatContextMenuStrip(Stat stat, PanelType panelType, Model model, Services services)
		{
			Stat stat2 = stat;
			Services services2 = services;
			Model model2 = model;
			base._002Ector();
			_generalHeaderMenuItem = new CustomContextMenuStripItem("General", (Container)(object)this, isHeader: true);
			_ignoreMenuItem = new CustomContextMenuStripItem("Ignore", (Container)(object)this);
			((Control)_ignoreMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				IgnoreStat(stat2, services2);
			});
			((Control)_ignoreMenuItem).set_Enabled(!stat2.IsCoinOrCustomCoin);
			((Control)_ignoreMenuItem).set_BasicTooltipText(stat2.IsCoinOrCustomCoin ? "Coins cannot be ignored." : "Ignored items/currencies are hidden and do not contribute to profit calculations. In the 'Ignored Items and Currencies'-Tab you can unignore items/currencies.");
			if (panelType == PanelType.SummaryFavorites)
			{
				_removeFavoriteMenuItem = new CustomContextMenuStripItem("Remove from favorites", (Container)(object)this);
				((Control)_removeFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					FavoriteStatService.RemoveFromFavoriteStats(stat2, model2, services2);
				});
				((Control)_removeFavoriteMenuItem).set_BasicTooltipText("Move item/currency from 'Favorites' to 'Items/'Currencies' panel.");
			}
			else
			{
				_addFavoriteMenuItem = new CustomContextMenuStripItem("Add to favorites", (Container)(object)this);
				((Control)_addFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					FavoriteStatService.AddToFavoriteStats(stat2, model2, services2);
				});
				((Control)_addFavoriteMenuItem).set_BasicTooltipText("Move item/currency from 'Items'/'Currencies' to 'Favorites' panel. Favorite items are not affected by filter or sort.");
			}
			_setCustomProfitMenuItem = new CustomContextMenuStripItem("Set to a custom profit of 0 copper. Navigate to 'Custom Profit' tab to edit or remove the custom profit.", (Container)(object)this);
			((Control)_setCustomProfitMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetToZeroProfitAndNavigateToProfitTab(stat2, services2);
			});
			((Control)_setCustomProfitMenuItem).set_Enabled(!stat2.IsCoinOrCustomCoin);
			((Control)_setCustomProfitMenuItem).set_BasicTooltipText(stat2.IsCoinOrCustomCoin ? "Coins cannot have a custom profit because that makes no sense." : "Read the help text in the 'Custom Profit' tab for more details.");
			_copyHeaderMenuItem = new CustomContextMenuStripItem("Copy", (Container)(object)this, isHeader: true);
			_copyNameMenuItem = new CustomContextMenuStripItem("Name", (Container)(object)this);
			((Control)_copyNameMenuItem).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(stat2.Details.Name);
			});
			((Control)_copyNameMenuItem).set_BasicTooltipText("Copy item/currency name to clipboard (like CTRL + C). You can paste it somewhere else with CTRL + V");
			bool hasName = !string.IsNullOrEmpty(stat2.Details.Name);
			((Control)_copyNameMenuItem).set_Enabled(hasName);
			((Control)_copyNameMenuItem).set_BasicTooltipText(hasName ? "Copy item/currency name to clipboard (like CTRL + C). You can paste it somewhere else with CTRL + V" : "Name is missing");
			_copyChatLinkMenuItem = new CustomContextMenuStripItem("Chat link", (Container)(object)this);
			((Control)_copyChatLinkMenuItem).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(stat2.Details.ChatLink);
			});
			bool hasChatLink = !string.IsNullOrEmpty(stat2.Details.ChatLink);
			((Control)_copyChatLinkMenuItem).set_Enabled(stat2.IsItem && hasChatLink);
			((Control)_copyChatLinkMenuItem).set_BasicTooltipText((stat2.IsItem && hasChatLink) ? "Copy item/currency chat link to clipboard (like CTRL + C). You can paste it somewhere else with CTRL + V. Chat links for items you dont own anymore, may not work." : (stat2.IsCurrency ? "Not available for currencies" : "No chat link available"));
			_websitesHeaderMenuItem = new CustomContextMenuStripItem("Open website", (Container)(object)this, isHeader: true);
			_wikiMenuItem = new CustomContextMenuStripItem("Wiki", (Container)(object)this);
			((Control)_wikiMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenWiki(stat2);
			});
			((Control)_wikiMenuItem).set_BasicTooltipText("Open its wiki page in your default browser.");
			string languageString = BrowserService.GetGw2EfficiencyLanguageString();
			_gw2EfficiencyTradingPostMenuItem = new CustomContextMenuStripItem("GW2 Efficiency (trading post search)", (Container)(object)this);
			((Control)_gw2EfficiencyTradingPostMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser("https://gw2efficiency.com/tradingpost?filter.search.term=" + Uri.EscapeDataString(stat2.Details.Name) + "&lang=" + languageString);
			});
			((Control)_gw2EfficiencyTradingPostMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2EfficiencyTradingPostMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 efficiency' trading post search page in your default browser" : "Not available for currencies");
			_gw2EfficiencyAccountMenuItem = new CustomContextMenuStripItem("GW2 Efficiency (account search)", (Container)(object)this);
			((Control)_gw2EfficiencyAccountMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser("https://gw2efficiency.com/account/overview?filter.name=" + Uri.EscapeDataString(stat2.Details.Name) + "&lang=" + languageString);
			});
			((Control)_gw2EfficiencyAccountMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2EfficiencyAccountMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 Efficiency' account search page in your default browser" : "Not available for currencies");
			_gw2BltcMenuItem = new CustomContextMenuStripItem("GW2 BLTC", (Container)(object)this);
			((Control)_gw2BltcMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser($"https://www.gw2bltc.com/en/item/{stat2.ApiId}");
			});
			((Control)_gw2BltcMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2BltcMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 BLTC' page in your default browser" : "Not available for currencies");
			_gw2TreasuresMenuItem = new CustomContextMenuStripItem("GW2 Treasures", (Container)(object)this);
			((Control)_gw2TreasuresMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser($"https://en.gw2treasures.com/item/{stat2.ApiId}");
			});
			((Control)_gw2TreasuresMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2TreasuresMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 Treasures' page in your default browser" : "Not available for currencies");
			_gw2TpMenuItem = new CustomContextMenuStripItem("GW2 TP", (Container)(object)this);
			((Control)_gw2TpMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser($"https://www.gw2tp.com/item/{stat2.ApiId}");
			});
			((Control)_gw2TpMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2TpMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 TP' page in your default browser" : "Not available for currencies");
			_gw2ProfitsMenuItem = new CustomContextMenuStripItem("GW2 Profits", (Container)(object)this);
			((Control)_gw2ProfitsMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BrowserService.OpenUrlInDefaultBrowser($"https://gw2profits.com/items.php?iid={stat2.ApiId}");
			});
			((Control)_gw2ProfitsMenuItem).set_Enabled(stat2.IsItem);
			((Control)_gw2ProfitsMenuItem).set_BasicTooltipText(stat2.IsItem ? "Open its 'GW2 Profits' page in your default browser" : "Not available for currencies");
		}

		protected override void DisposeControl()
		{
			CustomContextMenuStripItem generalHeaderMenuItem = _generalHeaderMenuItem;
			if (generalHeaderMenuItem != null)
			{
				((Control)generalHeaderMenuItem).Dispose();
			}
			CustomContextMenuStripItem? removeFavoriteMenuItem = _removeFavoriteMenuItem;
			if (removeFavoriteMenuItem != null)
			{
				((Control)removeFavoriteMenuItem).Dispose();
			}
			CustomContextMenuStripItem? addFavoriteMenuItem = _addFavoriteMenuItem;
			if (addFavoriteMenuItem != null)
			{
				((Control)addFavoriteMenuItem).Dispose();
			}
			CustomContextMenuStripItem? ignoreMenuItem = _ignoreMenuItem;
			if (ignoreMenuItem != null)
			{
				((Control)ignoreMenuItem).Dispose();
			}
			CustomContextMenuStripItem? setCustomProfitMenuItem = _setCustomProfitMenuItem;
			if (setCustomProfitMenuItem != null)
			{
				((Control)setCustomProfitMenuItem).Dispose();
			}
			CustomContextMenuStripItem copyHeaderMenuItem = _copyHeaderMenuItem;
			if (copyHeaderMenuItem != null)
			{
				((Control)copyHeaderMenuItem).Dispose();
			}
			CustomContextMenuStripItem copyNameMenuItem = _copyNameMenuItem;
			if (copyNameMenuItem != null)
			{
				((Control)copyNameMenuItem).Dispose();
			}
			CustomContextMenuStripItem copyChatLinkMenuItem = _copyChatLinkMenuItem;
			if (copyChatLinkMenuItem != null)
			{
				((Control)copyChatLinkMenuItem).Dispose();
			}
			CustomContextMenuStripItem websitesHeaderMenuItem = _websitesHeaderMenuItem;
			if (websitesHeaderMenuItem != null)
			{
				((Control)websitesHeaderMenuItem).Dispose();
			}
			CustomContextMenuStripItem wikiMenuItem = _wikiMenuItem;
			if (wikiMenuItem != null)
			{
				((Control)wikiMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2EfficiencyTradingPostMenuItem = _gw2EfficiencyTradingPostMenuItem;
			if (gw2EfficiencyTradingPostMenuItem != null)
			{
				((Control)gw2EfficiencyTradingPostMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2EfficiencyAccountMenuItem = _gw2EfficiencyAccountMenuItem;
			if (gw2EfficiencyAccountMenuItem != null)
			{
				((Control)gw2EfficiencyAccountMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2BltcMenuItem = _gw2BltcMenuItem;
			if (gw2BltcMenuItem != null)
			{
				((Control)gw2BltcMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2TreasuresMenuItem = _gw2TreasuresMenuItem;
			if (gw2TreasuresMenuItem != null)
			{
				((Control)gw2TreasuresMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2TpMenuItem = _gw2TpMenuItem;
			if (gw2TpMenuItem != null)
			{
				((Control)gw2TpMenuItem).Dispose();
			}
			CustomContextMenuStripItem gw2ProfitsMenuItem = _gw2ProfitsMenuItem;
			if (gw2ProfitsMenuItem != null)
			{
				((Control)gw2ProfitsMenuItem).Dispose();
			}
			((Container)this).DisposeControl();
		}

		private static void SetToZeroProfitAndNavigateToProfitTab(Stat stat, Services services)
		{
			stat.Profit.Unsigned_Custom_ProfitInCopper = 0L;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
			services.WindowTabSelector.SelectWindowTab(WindowTab.CustomProfit, WindowVisibility.Show);
		}

		private static void IgnoreStat(Stat stat, Services services)
		{
			if (!stat.IsCoinOrCustomCoin)
			{
				stat.StatVisibility = StatVisibility.Ignored;
				services.UpdateLoop.TriggerUpdateUi();
				services.UpdateLoop.TriggerSaveModel();
			}
		}

		private static void OpenWiki(Stat stat)
		{
			if (stat.Details.State == StatApiDetailsState.MissingBecauseUnknownByApi)
			{
				WikiService.OpenWikiIdQueryInDefaultBrowser(stat.ApiId);
			}
			if (stat.Details.HasWikiSearchTerm)
			{
				WikiService.OpenWikiSearchInDefaultBrowser(stat.Details.WikiSearchTerm);
			}
		}
	}
}
