using Blish_HUD;
using Blish_HUD.Controls;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class ContextMenuPresenter
	{
		private readonly IWikiLinkRepository _wikiLinkRepository;

		public ContextMenuPresenter(IWikiLinkRepository wikiLinkRepository)
		{
			_wikiLinkRepository = wikiLinkRepository;
		}

		public ContextMenuStrip BuildMenuStrip(MysticItem item, IngredientNode node)
		{
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			ContextMenuStripItem wikiItem = BuildWikiItem(item.GameId);
			if (wikiItem != null)
			{
				menuStrip.AddMenuItem(wikiItem);
			}
			menuStrip.AddMenuItem(BuildGw2Bltc(item.GameId));
			menuStrip.AddMenuItem(BuildCopyChatLink(item.ChatLink));
			menuStrip.AddMenuItem(BuildTradingPostItem(node));
			return menuStrip;
		}

		public ContextMenuStripItem BuildClearChoices(IngredientNode node)
		{
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem confirmation = new ContextMenuStripItem("Yes I'm sure");
			confirmation.Click += delegate
			{
				ServiceContainer.ChoiceRepository.DeleteAllChoices(node.FullPath);
			};
			Submenu.AddMenuItem(confirmation);
			return new ContextMenuStripItem("Clear my choices")
			{
				Submenu = Submenu
			};
		}

		public ContextMenuStripItem BuildTradingPostItem(IngredientNode node)
		{
			ContextMenuStrip Submenu = new ContextMenuStrip();
			ContextMenuStripItem buyAll = new ContextMenuStripItem(MysticCrafting.Module.Strings.Recipe.ContextMenuBuyAll);
			buyAll.Click += delegate
			{
				node.UpdateTradingPostOptions(TradingPostOptions.Buy);
			};
			Submenu.AddMenuItem(buyAll);
			ContextMenuStripItem sellAll = new ContextMenuStripItem(MysticCrafting.Module.Strings.Recipe.ContextMenuSellAll);
			sellAll.Click += delegate
			{
				node.UpdateTradingPostOptions(TradingPostOptions.Sell);
			};
			Submenu.AddMenuItem(sellAll);
			return new ContextMenuStripItem(MysticCrafting.Module.Strings.Recipe.ContextMenuUpdateTradingPost)
			{
				Submenu = Submenu
			};
		}

		public ContextMenuStripItem BuildWikiItem(int itemId)
		{
			MysticWikiLink wikiLink = _wikiLinkRepository.GetLink(itemId);
			if (wikiLink != null)
			{
				ContextMenuStripItem contextMenuStripItem = new ContextMenuStripItem();
				contextMenuStripItem.Text = "Open Wiki";
				contextMenuStripItem.Click += delegate
				{
					LinkHelper.OpenWiki(wikiLink);
				};
				return contextMenuStripItem;
			}
			return null;
		}

		public ContextMenuStripItem BuildGw2Bltc(int itemId)
		{
			ContextMenuStripItem contextMenuStripItem = new ContextMenuStripItem();
			contextMenuStripItem.Text = "Open GW2BLTC";
			contextMenuStripItem.Click += delegate
			{
				LinkHelper.OpenGw2Bltc(itemId);
			};
			return contextMenuStripItem;
		}

		public ContextMenuStripItem BuildCopyChatLink(string chatLink)
		{
			ContextMenuStripItem contextMenuStripItem = new ContextMenuStripItem();
			contextMenuStripItem.Text = "Copy Chat Link";
			contextMenuStripItem.Click += delegate
			{
				ClipboardUtil.WindowsClipboardService.SetTextAsync(chatLink);
			};
			return contextMenuStripItem;
		}

		public ContextMenuStripItem BuildBaseItemsOption(IngredientNode item, IIngredientNodePresenter itemPresenter)
		{
			return new ContextMenuStripItem
			{
				Text = "Show base materials",
				CanCheck = true
			};
		}
	}
}
