using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemsTabView : View
	{
		private readonly TextBox _searchBox;

		private readonly LoadingSpinner _loadingSpinner;

		private readonly ItemsList _searchResults;

		private readonly ViewContainer _editor;

		public ItemsTabViewModel ViewModel { get; }

		public ItemsTabView(ILogger<ItemsTabView> logger, ItemsTabViewModel viewModel)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			ViewModel = viewModel;
			TextBox val = new TextBox();
			((Control)val).set_Width(400);
			((TextInputBase)val).set_PlaceholderText("Enter item name or chat link...");
			_searchBox = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Size(new Point(((Control)_searchBox).get_Height()));
			((Control)val2).set_Right(((Control)_searchBox).get_Right());
			_loadingSpinner = val2;
			ItemsList itemsList = new ItemsList();
			((Container)itemsList).set_WidthSizingMode((SizingMode)0);
			((Control)itemsList).set_Width(400);
			((Container)itemsList).set_HeightSizingMode((SizingMode)2);
			((Control)itemsList).set_Top(((Control)_searchBox).get_Bottom());
			itemsList.Entries = ViewModel.SearchResults;
			_searchResults = itemsList;
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Left(((Control)_searchResults).get_Right() + 20);
			((Control)val3).set_Width(450);
			((Container)val3).set_HeightSizingMode((SizingMode)2);
			val3.set_FadeView(true);
			_editor = val3;
			_searchResults.SelectionChanged += new Action<ListBox<ItemsListViewModel>, ListBoxSelectionChangedEventArgs<ItemsListViewModel>>(SelectionChanged);
		}

		private void SelectionChanged(ListBox<ItemsListViewModel> sender, ListBoxSelectionChangedEventArgs<ItemsListViewModel> args)
		{
			IList<ListItem<ItemsListViewModel>> addedItems = args.AddedItems;
			if (addedItems != null && addedItems.Count == 1)
			{
				ListItem<ItemsListViewModel> listItem2 = addedItems[0];
				if (listItem2 != null)
				{
					ItemsListViewModel listItem = listItem2.Data;
					if (listItem != null)
					{
						_editor.Show((IView)(object)new ChatLinkEditorView(ViewModel.CreateChatLinkEditorViewModel(listItem.Item)));
						return;
					}
				}
			}
			_editor.Clear();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await ViewModel.LoadAsync();
			return true;
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_searchBox).set_Parent(buildPanel);
			((Control)_loadingSpinner).set_Parent(buildPanel);
			((Control)_searchResults).set_Parent(buildPanel);
			((Control)_editor).set_Parent(buildPanel);
			Binder.Bind(ViewModel, (ItemsTabViewModel vm) => vm.SearchText, _searchBox);
			Binder.Bind(ViewModel, (ItemsTabViewModel vm) => vm.Searching, _loadingSpinner);
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchTextChanged);
			_searchBox.add_EnterPressed((EventHandler<EventArgs>)SearchEnterPressed);
			((TextInputBase)_searchBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)SearchInputFocusChanged);
		}

		protected override void Unload()
		{
			((TextInputBase)_searchBox).remove_TextChanged((EventHandler<EventArgs>)SearchEnterPressed);
			_searchBox.remove_EnterPressed((EventHandler<EventArgs>)SearchEnterPressed);
			((TextInputBase)_searchBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)SearchInputFocusChanged);
		}

		private void SearchTextChanged(object sender, EventArgs e)
		{
			ViewModel.SearchCommand.Execute(null);
		}

		private void SearchEnterPressed(object sender, EventArgs e)
		{
			ViewModel.SearchCommand.Execute(null);
		}

		private void SearchInputFocusChanged(object sender, ValueEventArgs<bool> args)
		{
			if (args.get_Value())
			{
				((TextInputBase)_searchBox).set_SelectionStart(0);
				((TextInputBase)_searchBox).set_SelectionEnd(((TextInputBase)_searchBox).get_Length());
			}
			else
			{
				((TextInputBase)_searchBox).set_SelectionStart(((TextInputBase)_searchBox).get_SelectionEnd());
			}
		}
	}
}
