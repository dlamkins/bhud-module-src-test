using Blish_HUD;
using Blish_HUD.Controls;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class ContextMenuPresenter
	{
		private readonly IWikiLinkRepository _wikiLinkRepository;

		public ContextMenuPresenter(IWikiLinkRepository wikiLinkRepository)
		{
			_wikiLinkRepository = wikiLinkRepository;
		}

		public ContextMenuStrip BuildMenuStrip(MysticItem item)
		{
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			ContextMenuStripItem wikiItem = BuildWikiItem(item.Id);
			if (wikiItem != null)
			{
				menuStrip.AddMenuItem(wikiItem);
			}
			menuStrip.AddMenuItem(BuildGw2Bltc(item.Id));
			menuStrip.AddMenuItem(BuildCopyChatLink(item.ChatLink));
			return menuStrip;
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
