using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemsTabView : View
	{
		private readonly FlowPanel _layout;

		private readonly ViewContainer _editor;

		public ItemsTabViewModel ViewModel { get; }

		public ItemsTabView(ItemsTabViewModel viewModel)
			: this()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			ItemsTabView itemsTabView = this;
			ViewModel = viewModel;
			ViewModel.Initialize();
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			_layout = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_layout);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Control)val2).set_Width(400);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			FlowPanel searchLayout = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)searchLayout);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			Panel searchBoxPanel = val3;
			TextBox val4 = new TextBox();
			((Control)val4).set_Parent((Container)(object)searchBoxPanel);
			((Control)val4).set_Width(400);
			((TextInputBase)val4).set_PlaceholderText(viewModel.SearchPlaceholderText);
			TextBox searchBox = val4;
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)SearchTextChanged);
			searchBox.add_EnterPressed((EventHandler<EventArgs>)SearchEnterPressed);
			((TextInputBase)searchBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> args)
			{
				if (args.get_Value())
				{
					((TextInputBase)searchBox).set_SelectionStart(0);
					((TextInputBase)searchBox).set_SelectionEnd(((TextInputBase)searchBox).get_Length());
				}
				else
				{
					((TextInputBase)searchBox).set_SelectionStart(((TextInputBase)searchBox).get_SelectionEnd());
				}
			});
			LoadingSpinner val5 = new LoadingSpinner();
			((Control)val5).set_Parent((Container)(object)searchBoxPanel);
			((Control)val5).set_Size(new Point(((Control)searchBox).get_Height()));
			((Control)val5).set_Right(((Control)searchBox).get_Right());
			LoadingSpinner loadingSpinner = val5;
			ItemsList itemsList = new ItemsList();
			((Control)itemsList).set_Parent((Container)(object)searchLayout);
			((Container)itemsList).set_WidthSizingMode((SizingMode)0);
			((Control)itemsList).set_Width(400);
			((Container)itemsList).set_HeightSizingMode((SizingMode)2);
			itemsList.Entries = ViewModel.SearchResults;
			ItemsList searchResults = itemsList;
			searchResults.SelectionChanged += new Action<ListBox<ItemsListViewModel>, ListBoxSelectionChangedEventArgs<ItemsListViewModel>>(SelectionChanged);
			ViewContainer val6 = new ViewContainer();
			((Control)val6).set_Parent((Container)(object)_layout);
			((Control)val6).set_Width(450);
			((Container)val6).set_HeightSizingMode((SizingMode)2);
			val6.set_FadeView(true);
			_editor = val6;
			Binder.Bind(ViewModel, (ItemsTabViewModel vm) => vm.SearchText, searchBox);
			Binder.Bind(ViewModel, (ItemsTabViewModel vm) => vm.Searching, loadingSpinner);
			Binder.Bind<ItemsTabViewModel, Scrollbar, string>(ViewModel, (Expression<Func<ItemsTabViewModel, string>>)((ItemsTabViewModel vm) => vm.ResultText), ((IEnumerable)((Container)searchLayout).get_Children()).OfType<Scrollbar>().Single(), (Expression<Func<Scrollbar, string>>)((Scrollbar ctl) => ((Control)ctl).get_BasicTooltipText()), BindingMode.ToView);
			viewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				string propertyName = args.PropertyName;
				if (!(propertyName == "SearchPlaceholderText"))
				{
					if (propertyName == "SearchResults")
					{
						searchResults.Entries = itemsTabView.ViewModel.SearchResults;
					}
				}
				else
				{
					((TextInputBase)searchBox).set_PlaceholderText(itemsTabView.ViewModel.SearchPlaceholderText);
				}
			};
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
						ViewModel.SelectedItem = listItem.Item;
						_editor.Show((IView)(object)new ChatLinkEditorView(ViewModel.CreateChatLinkEditorViewModel(listItem.Item)));
						return;
					}
				}
			}
			ViewModel.SelectedItem = null;
			_editor.Clear();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await ViewModel.LoadAsync();
			return true;
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_layout).set_Parent(buildPanel);
		}

		protected override void Unload()
		{
			ViewModel.Unload();
		}

		private void SearchTextChanged(object sender, EventArgs e)
		{
			ViewModel.SearchCommand.Execute(null);
		}

		private void SearchEnterPressed(object sender, EventArgs e)
		{
			ViewModel.SearchCommand.Execute(null);
		}
	}
}
