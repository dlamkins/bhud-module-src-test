using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public class ItemsList : ListBox<ItemsListViewModel>
	{
		protected override Control Template(ItemsListViewModel data)
		{
			return (Control)(object)new ItemsListEntry(data);
		}

		protected override void Bind(ItemsListViewModel data, ListItem<ItemsListViewModel> listItem)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			ItemsListViewModel data2 = data;
			Binder.Bind(data2, (ItemsListViewModel vm) => vm.IsSelected, listItem);
			((Control)listItem).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[5]
			{
				data2.ToggleCommand.ToMenuItem(() => (!data2.IsSelected) ? "Select" : "Deselect"),
				data2.CopyNameCommand.ToMenuItem(() => "Copy Name"),
				data2.CopyChatLinkCommand.ToMenuItem(() => "Copy Chat Link"),
				data2.OpenWikiCommand.ToMenuItem(() => "Open Wiki"),
				data2.OpenApiCommand.ToMenuItem(() => "Open API")
			}))));
		}
	}
}
