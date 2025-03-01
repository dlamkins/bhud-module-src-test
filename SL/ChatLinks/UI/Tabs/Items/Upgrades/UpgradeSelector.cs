using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSelector : FlowPanel
	{
		private readonly Accordion _accordion;

		public UpgradeSelectorViewModel ViewModel { get; }

		public UpgradeSelector(UpgradeSelectorViewModel viewModel)
			: this()
		{
			ThrowHelper.ThrowIfNull(viewModel, "viewModel");
			ViewModel = viewModel;
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			Accordion accordion = new Accordion();
			((Control)accordion).set_Parent((Container)(object)this);
			_accordion = accordion;
			AddOptions();
			viewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModelPropertyChanged);
		}

		private void AddOptions()
		{
			foreach (IGrouping<string, ItemsListViewModel> group in ViewModel.Options)
			{
				ItemsList list = new ItemsList();
				ItemsList itemsList = list;
				ObservableCollection<ItemsListViewModel> observableCollection = new ObservableCollection<ItemsListViewModel>();
				foreach (ItemsListViewModel item in group)
				{
					observableCollection.Add(item);
				}
				itemsList.SetEntries(observableCollection);
				_accordion.AddSection(group.Key, (Control)(object)list);
				list.SelectionChanged += new EventHandler<ListBoxSelectionChangedEventArgs<ItemsListViewModel>>(SelectionChanged);
				((Control)list).add_MouseEntered((EventHandler<MouseEventArgs>)MouseEnteredUpgradeSelectorCommand);
				((Control)list).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftUpgradeSelectorCommand);
			}
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Options")
			{
				while (((Container)_accordion).get_Children().get_Count() > 0)
				{
					((Container)_accordion).get_Children().get_Item(0).Dispose();
				}
				AddOptions();
			}
		}

		private void SelectionChanged(object sender, ListBoxSelectionChangedEventArgs<ItemsListViewModel> args)
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
