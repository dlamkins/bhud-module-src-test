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

		protected override void Bind(ItemsListViewModel viewModel, ListItem<ItemsListViewModel> listItem)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			ItemsListViewModel viewModel2 = viewModel;
			Binder.Bind(viewModel2, (ItemsListViewModel vm) => vm.IsSelected, listItem);
			((Control)listItem).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[5]
			{
				viewModel2.ToggleCommand.ToMenuItem(() => (!viewModel2.IsSelected) ? viewModel2.SelectLabel : viewModel2.DeselectLabel),
				viewModel2.CopyNameCommand.ToMenuItem(() => viewModel2.CopyNameLabel),
				viewModel2.CopyChatLinkCommand.ToMenuItem(() => viewModel2.CopyChatLinkLabel),
				viewModel2.OpenWikiCommand.ToMenuItem(() => viewModel2.OpenWikiLabel),
				viewModel2.OpenApiCommand.ToMenuItem(() => viewModel2.OpenApiLabel)
			}))));
		}
	}
}
