using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class StatContextMenuStrip : ContextMenuStrip
	{
		private readonly ContextMenuStripItem _wikiMenuItem;

		private readonly ContextMenuStripItem _ignoreMenuItem;

		private readonly ContextMenuStripItem _addFavoriteMenuItem;

		private readonly ContextMenuStripItem _removeFavoriteMenuItem;

		public StatContextMenuStrip(Stat stat, PanelType panelType, SafeList<int> ignoredItemApiIds, SafeList<int> favoriteItemApiIds, Services services)
			: this()
		{
			_wikiMenuItem = ((ContextMenuStrip)this).AddMenuItem("Open wiki");
			((Control)_wikiMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenWiki(stat);
			});
			((Control)_wikiMenuItem).set_BasicTooltipText("Open its wiki page in your default browser.");
			if (panelType == PanelType.SummaryRegularItems)
			{
				_ignoreMenuItem = ((ContextMenuStrip)this).AddMenuItem("Ignore item");
				((Control)_ignoreMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					IgnoreItem(stat, ignoredItemApiIds, services);
				});
				((Control)_ignoreMenuItem).set_BasicTooltipText("Ignored items are hidden and dont contribute to profit calculations. They can be managed in the 'Ignored Items'-Tab.");
				_addFavoriteMenuItem = ((ContextMenuStrip)this).AddMenuItem("Add to favorites");
				((Control)_addFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddToFavoriteItems(stat, favoriteItemApiIds, services);
				});
				((Control)_addFavoriteMenuItem).set_BasicTooltipText("Move item from 'Items' to 'Favorite Items panel. Favorite items are not affected by filter or sort.");
			}
			if (panelType == PanelType.SummaryFavoriteItems)
			{
				_removeFavoriteMenuItem = ((ContextMenuStrip)this).AddMenuItem("Remove from favorites");
				((Control)_removeFavoriteMenuItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					RemoveFromFavoriteItems(stat, favoriteItemApiIds, services);
				});
				((Control)_removeFavoriteMenuItem).set_BasicTooltipText("Move item from 'Favorite Items' to 'Items panel.");
			}
		}

		protected override void DisposeControl()
		{
			ContextMenuStripItem removeFavoriteMenuItem = _removeFavoriteMenuItem;
			if (removeFavoriteMenuItem != null)
			{
				((Control)removeFavoriteMenuItem).Dispose();
			}
			ContextMenuStripItem addFavoriteMenuItem = _addFavoriteMenuItem;
			if (addFavoriteMenuItem != null)
			{
				((Control)addFavoriteMenuItem).Dispose();
			}
			ContextMenuStripItem ignoreMenuItem = _ignoreMenuItem;
			if (ignoreMenuItem != null)
			{
				((Control)ignoreMenuItem).Dispose();
			}
			ContextMenuStripItem wikiMenuItem = _wikiMenuItem;
			if (wikiMenuItem != null)
			{
				((Control)wikiMenuItem).Dispose();
			}
			((Container)this).DisposeControl();
		}

		private static void RemoveFromFavoriteItems(Stat stat, SafeList<int> favoriteItemApiIds, Services services)
		{
			if (!favoriteItemApiIds.AnySafe((int id) => id == stat.ApiId))
			{
				Module.Logger.Error("Item is not a favorite item. It shouldnt have been displayed in the first place.");
				return;
			}
			favoriteItemApiIds.RemoveSafe(stat.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void AddToFavoriteItems(Stat stat, SafeList<int> favoriteItemApiIds, Services services)
		{
			if (favoriteItemApiIds.AnySafe((int id) => id == stat.ApiId))
			{
				Module.Logger.Error("Item is already a favorite item. It shouldnt have been displayed in the first place.");
				return;
			}
			favoriteItemApiIds.AddSafe(stat.ApiId);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void IgnoreItem(Stat stat, SafeList<int> ignoredItemApiIds, Services services)
		{
			if (ignoredItemApiIds.AnySafe((int id) => id == stat.ApiId))
			{
				Module.Logger.Error("Item is already ignored. It shouldnt have been displayed in the first place.");
				return;
			}
			ignoredItemApiIds.AddSafe(stat.ApiId);
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
