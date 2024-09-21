using System;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Atzie.MysticCrafting.Models.Items;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class ContextMenuPresenter
	{
		public ContextMenuStrip BuildMenuStrip(Currency currency, IngredientNode node)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			Locale locale = GameService.Overlay.get_UserLocale().get_Value();
			string itemName = currency.Name;
			if ((int)locale == 3 || (int)locale == 2)
			{
				itemName = currency.LocalizedName();
				ContextMenuStripItem enWikiItem = BuildSearchWikiItem(currency.Name, (Locale)0);
				if (enWikiItem != null)
				{
					menuStrip.AddMenuItem(enWikiItem);
				}
			}
			ContextMenuStripItem wikiItem = BuildSearchWikiItem(itemName, locale);
			if (wikiItem != null)
			{
				menuStrip.AddMenuItem(wikiItem);
			}
			menuStrip.AddMenuItem(BuildCopyText(currency.LocalizedName(), Recipe.CopyName));
			return menuStrip;
		}

		public ContextMenuStrip BuildMenuStrip(Item item, IngredientNode node)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			Locale locale = GameService.Overlay.get_UserLocale().get_Value();
			string itemName = item.Name;
			if ((int)locale == 3 || (int)locale == 2)
			{
				itemName = item.LocalizedName();
				ContextMenuStripItem enWikiItem = BuildSearchWikiItem(item.Name, (Locale)0);
				if (enWikiItem != null)
				{
					menuStrip.AddMenuItem(enWikiItem);
				}
			}
			ContextMenuStripItem wikiItem = BuildSearchWikiItem(itemName, locale);
			if (wikiItem != null)
			{
				menuStrip.AddMenuItem(wikiItem);
			}
			menuStrip.AddMenuItem(BuildGw2Bltc(item.Id));
			menuStrip.AddMenuItem(BuildCopyText(item.LocalizedName(), Recipe.CopyName));
			menuStrip.AddMenuItem(BuildCopyText(item.ChatLink, Recipe.CopyChatLink));
			menuStrip.AddMenuItem(BuildTradingPostItem(node));
			return menuStrip;
		}

		public ContextMenuStrip BuildMenuStrip(VendorSellsItemGroup vendorGroup)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			foreach (Vendor vendor in vendorGroup.Vendors.Take(8))
			{
				ContextMenuStripItem wikiItem = BuildWikiItem(vendor.WikiAlias);
				if (wikiItem != null)
				{
					wikiItem.set_Text(Recipe.OpenWikiTo + " " + vendor.Name);
					menuStrip.AddMenuItem(wikiItem);
				}
			}
			return menuStrip;
		}

		public ContextMenuStripItem BuildClearChoices(IngredientNode node)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem confirmation = new ContextMenuStripItem("Yes I'm sure");
			((Control)confirmation).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ServiceContainer.ChoiceRepository.DeleteAllChoices(node.FullPath);
			});
			Submenu.AddMenuItem(confirmation);
			ContextMenuStripItem val = new ContextMenuStripItem("Clear my choices");
			val.set_Submenu(Submenu);
			return val;
		}

		public ContextMenuStripItem BuildTradingPostItem(IngredientNode node)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem buyAll = new ContextMenuStripItem(Recipe.ContextMenuBuyAll);
			((Control)buyAll).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.UpdateTradingPostOptions(TradingPostOptions.Buy);
			});
			Submenu.AddMenuItem(buyAll);
			ContextMenuStripItem sellAll = new ContextMenuStripItem(Recipe.ContextMenuSellAll);
			((Control)sellAll).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.UpdateTradingPostOptions(TradingPostOptions.Sell);
			});
			Submenu.AddMenuItem(sellAll);
			ContextMenuStripItem val = new ContextMenuStripItem(Recipe.ContextMenuUpdateTradingPost);
			val.set_Submenu(Submenu);
			return val;
		}

		public ContextMenuStripItem BuildSearchWikiItem(string name, Locale locale = 0)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			string wikiLink = "http://wiki-en.guildwars2.com/index.php?search=" + name;
			string text = Recipe.OpenWiki;
			if ((int)locale == 3)
			{
				wikiLink = "http://wiki-fr.guildwars2.com/index.php?search=" + name;
				text = Recipe.OpenFrenchWiki;
			}
			if ((int)locale == 2)
			{
				wikiLink = "http://wiki-de.guildwars2.com/index.php?search=" + name;
				text = "Open German Wiki";
			}
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(text);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LinkHelper.OpenUrl(wikiLink);
			});
			return val;
		}

		public ContextMenuStripItem BuildWikiItem(string pageAlias)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			if (string.IsNullOrEmpty(pageAlias))
			{
				return null;
			}
			string wikiLink = "https://wiki.guildwars2.com/wiki/" + pageAlias;
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(Recipe.OpenWiki);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LinkHelper.OpenUrl(wikiLink);
			});
			return val;
		}

		public ContextMenuStripItem BuildGw2Bltc(int itemId)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(Recipe.OpenGW2BLTC);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LinkHelper.OpenGw2Bltc(itemId);
			});
			return val;
		}

		public ContextMenuStripItem BuildCopyText(string copyText, string labelText)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(labelText);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(copyText);
			});
			return val;
		}

		public ContextMenuStripItem BuildExpandOption(TradeableItemNode node)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem all = new ContextMenuStripItem("All");
			((Control)all).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.ExpandAllActiveNodes();
			});
			Submenu.AddMenuItem(all);
			ContextMenuStripItem sellAll = new ContextMenuStripItem("Incomplete only");
			((Control)sellAll).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.ExpandAllActiveNodes(includeIncomplete: false);
			});
			Submenu.AddMenuItem(sellAll);
			ContextMenuStripItem val = new ContextMenuStripItem("Expand");
			val.set_Submenu(Submenu);
			return val;
		}

		public ContextMenuStripItem BuildCollapseOption(TradeableItemNode node)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem all = new ContextMenuStripItem("All");
			((Control)all).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.CollapseAllActiveNodes(isComplete: false);
			});
			Submenu.AddMenuItem(all);
			ContextMenuStripItem sellAll = new ContextMenuStripItem("Completed only");
			((Control)sellAll).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				node.CollapseAllActiveNodes();
			});
			Submenu.AddMenuItem(sellAll);
			ContextMenuStripItem val = new ContextMenuStripItem("Collapse");
			val.set_Submenu(Submenu);
			return val;
		}
	}
}
