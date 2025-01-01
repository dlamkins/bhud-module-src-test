using System;
using System.Diagnostics;
using System.Net;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GuildWars2.Items;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public class ItemContextMenu : ContextMenuStrip
	{
		public ItemContextMenu(Item item)
		{
			Item item2 = item;
			((ContextMenuStrip)this)._002Ector();
			ContextMenuStripItem copyName = ((ContextMenuStrip)this).AddMenuItem("Copy Name");
			ContextMenuStripItem wiki = ((ContextMenuStrip)this).AddMenuItem("Wiki");
			ContextMenuStripItem obj = ((ContextMenuStrip)this).AddMenuItem("API");
			((Control)copyName).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item2.Name);
			});
			((Control)wiki).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://wiki.guildwars2.com/wiki/?search=" + WebUtility.UrlEncode(item2.ChatLink));
			});
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start($"https://api.guildwars2.com/v2/items/{item2.Id}?v=latest");
			});
		}
	}
}
