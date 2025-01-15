using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSelector : FlowPanel
	{
		public UpgradeSelectorViewModel ViewModel { get; }

		public UpgradeSelector(UpgradeSelectorViewModel viewModel)
			: this()
		{
			ViewModel = viewModel;
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			Accordion accordion2 = new Accordion();
			((Control)accordion2).set_Parent((Container)(object)this);
			Accordion accordion = accordion2;
			foreach (IGrouping<string, ItemsListViewModel> group in viewModel.Options)
			{
				ItemsList list = new ItemsList
				{
					Entries = new ObservableCollection<ItemsListViewModel>(group)
				};
				accordion.AddSection(group.Key, (Control)(object)list);
				list.SelectionChanged += new Action<ListBox<ItemsListViewModel>, ListBoxSelectionChangedEventArgs<ItemsListViewModel>>(SelectionChanged);
				((Control)list).add_MouseEntered((EventHandler<MouseEventArgs>)MouseEnteredUpgradeSelectorCommand);
				((Control)list).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftUpgradeSelectorCommand);
			}
		}

		private void SelectionChanged(ListBox<ItemsListViewModel> sender, ListBoxSelectionChangedEventArgs<ItemsListViewModel> args)
		{
			IList<ListItem<ItemsListViewModel>> addedItems = args.AddedItems;
			if (addedItems != null && addedItems.Count == 1)
			{
				ListItem<ItemsListViewModel> item = addedItems[0];
				if (item != null)
				{
					ViewModel.SelectCommand.Execute(item.Data);
					return;
				}
			}
			ViewModel.DeselectCommand.Execute();
		}

		private void MouseEnteredUpgradeSelectorCommand(object sender, MouseEventArgs e)
		{
			ViewModel.MouseEnteredUpgradeSelectorCommand.Execute();
		}

		private void MouseLeftUpgradeSelectorCommand(object sender, MouseEventArgs e)
		{
			ViewModel.MouseLeftUpgradeSelectorCommand.Execute();
		}
	}
}
