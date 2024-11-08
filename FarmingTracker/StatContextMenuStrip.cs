using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class StatContextMenuStrip : ContextMenuStrip
	{
		private readonly ContextMenuStripItem _wikiMenuItem;

		private readonly ContextMenuStripItem? _ignoreMenuItem;

		private readonly ContextMenuStripItem? _addFavoriteMenuItem;

		private readonly ContextMenuStripItem? _removeFavoriteMenuItem;

		private readonly ContextMenuStripItem? _setCustomProfitMenuItem;

		public StatContextMenuStrip(Stat stat, PanelType panelType, SafeList<int> ignoredItemApiIds, SafeList<int> favoriteItemApiIds, SafeList<CustomStatProfit> customStatProfits, Services services)
		{
			Stat stat2 = stat;
			SafeList<int> ignoredItemApiIds2 = ignoredItemApiIds;
			Services services2 = services;
			SafeList<int> favoriteItemApiIds2 = favoriteItemApiIds;
			SafeList<CustomStatProfit> customStatProfits2 = customStatProfits;
			((ContextMenuStrip)this)._002Ector();
			_wikiMenuItem = ((ContextMenuStrip)this).AddMenuItem("Open wiki");
			((Control)_wikiMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenWiki(stat2);
			});
			((Control)_wikiMenuItem).set_BasicTooltipText("Open its wiki page in your default browser.");
			if (panelType == PanelType.SummaryRegularItems)
			{
				_ignoreMenuItem = ((ContextMenuStrip)this).AddMenuItem("Ignore item");
				((Control)_ignoreMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					IgnoreItem(stat2, ignoredItemApiIds2, services2);
				});
				((Control)_ignoreMenuItem).set_BasicTooltipText("Ignored items are hidden and dont contribute to profit calculations. They can be managed in the 'Ignored Items'-Tab.");
				_addFavoriteMenuItem = ((ContextMenuStrip)this).AddMenuItem("Add to favorites");
				((Control)_addFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddToFavoriteItems(stat2, favoriteItemApiIds2, services2);
				});
				((Control)_addFavoriteMenuItem).set_BasicTooltipText("Move item from 'Items' to 'Favorite Items panel. Favorite items are not affected by filter or sort.");
			}
			if (panelType == PanelType.SummaryFavoriteItems)
			{
				_removeFavoriteMenuItem = ((ContextMenuStrip)this).AddMenuItem("Remove from favorites");
				((Control)_removeFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					RemoveFromFavoriteItems(stat2, favoriteItemApiIds2, services2);
				});
				((Control)_removeFavoriteMenuItem).set_BasicTooltipText("Move item from 'Favorite Items' to 'Items panel.");
			}
			if (!stat2.IsCoinOrCustomCoin)
			{
				_setCustomProfitMenuItem = ((ContextMenuStrip)this).AddMenuItem("Set to a custom profit of 0 copper. Navigate to 'Custom Profit' tab to edit or remove the custom profit.");
				((Control)_setCustomProfitMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SetToZeroProfitAndNavigateToProfitTab(stat2, customStatProfits2, services2);
				});
				((Control)_setCustomProfitMenuItem).set_BasicTooltipText("Read the help text in the 'Custom Profit' tab for more details.");
			}
		}

		protected override void DisposeControl()
		{
			ContextMenuStripItem? removeFavoriteMenuItem = _removeFavoriteMenuItem;
			if (removeFavoriteMenuItem != null)
			{
				((Control)removeFavoriteMenuItem).Dispose();
			}
			ContextMenuStripItem? addFavoriteMenuItem = _addFavoriteMenuItem;
			if (addFavoriteMenuItem != null)
			{
				((Control)addFavoriteMenuItem).Dispose();
			}
			ContextMenuStripItem? ignoreMenuItem = _ignoreMenuItem;
			if (ignoreMenuItem != null)
			{
				((Control)ignoreMenuItem).Dispose();
			}
			ContextMenuStripItem wikiMenuItem = _wikiMenuItem;
			if (wikiMenuItem != null)
			{
				((Control)wikiMenuItem).Dispose();
			}
			ContextMenuStripItem? setCustomProfitMenuItem = _setCustomProfitMenuItem;
			if (setCustomProfitMenuItem != null)
			{
				((Control)setCustomProfitMenuItem).Dispose();
			}
			((Container)this).DisposeControl();
		}

		private static void SetToZeroProfitAndNavigateToProfitTab(Stat stat, SafeList<CustomStatProfit> customStatProfits, Services services)
		{
			Stat stat2 = stat;
			CustomStatProfit matchingCustomStatProfit = customStatProfits.ToListSafe().SingleOrDefault((CustomStatProfit c) => c.BelongsToStat(stat2));
			if (matchingCustomStatProfit != null)
			{
				matchingCustomStatProfit.Unsigned_CustomProfitInCopper = 0L;
			}
			else
			{
				CustomStatProfit customStatProfit = new CustomStatProfit(stat2.ApiId, stat2.StatType);
				customStatProfits.AddSafe(customStatProfit);
			}
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
			services.WindowTabSelector.SelectWindowTab(WindowTab.CustomProfit, WindowVisibility.Show);
		}

		private static void RemoveFromFavoriteItems(Stat stat, SafeList<int> favoriteItemApiIds, Services services)
		{
			Stat stat2 = stat;
			if (!favoriteItemApiIds.AnySafe((int id) => id == stat2.ApiId))
			{
				Module.Logger.Error("Item is not a favorite item. It shouldnt have been displayed in the first place.");
				return;
			}
			favoriteItemApiIds.RemoveSafe(stat2.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void AddToFavoriteItems(Stat stat, SafeList<int> favoriteItemApiIds, Services services)
		{
			Stat stat2 = stat;
			if (favoriteItemApiIds.AnySafe((int id) => id == stat2.ApiId))
			{
				Module.Logger.Error("Item is already a favorite item. It shouldnt have been displayed in the first place.");
				return;
			}
			favoriteItemApiIds.AddSafe(stat2.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void IgnoreItem(Stat stat, SafeList<int> ignoredItemApiIds, Services services)
		{
			Stat stat2 = stat;
			if (ignoredItemApiIds.AnySafe((int id) => id == stat2.ApiId))
			{
				Module.Logger.Error("Item is already ignored. It shouldnt have been displayed in the first place.");
				return;
			}
			ignoredItemApiIds.AddSafe(stat2.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void OpenWiki(Stat stat)
		{
			if (stat.Details.State == ApiStatDetailsState.MissingBecauseUnknownByApi)
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
