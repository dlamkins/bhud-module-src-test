using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabView : View, IDisposable
	{
		[CompilerGenerated]
		private ItemsTabViewModel _003CviewModel_003EP;

		private Panel? _sidePanel;

		private Menu? _sidebar;

		private Container? _layout;

		private Panel? _contentPanel;

		private ItemsList? _searchResults;

		private Container? _selection;

		public ItemsTabView(ItemsTabViewModel viewModel)
		{
			_003CviewModel_003EP = viewModel;
			((View)this)._002Ector();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await _003CviewModel_003EP.Load().ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Expected O, but got Unknown
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Expected O, but got Unknown
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Expected O, but got Unknown
			_layout = buildPanel;
			Panel val = new Panel();
			((Control)val).set_Parent(_layout);
			((Control)val).set_Left(4);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel searchBoxPanel = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)searchBoxPanel);
			((Control)val2).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((TextInputBase)val2).set_PlaceholderText(_003CviewModel_003EP.SearchPlaceholder);
			TextBox searchBox = val2;
			Binder.Bind<ItemsTabViewModel, TextBox, string>(_003CviewModel_003EP, (Expression<Func<ItemsTabViewModel, string>>)((ItemsTabViewModel vm) => vm.SearchPlaceholder), searchBox, (Expression<Func<TextBox, string>>)((TextBox ctl) => ((TextInputBase)ctl).get_PlaceholderText()), BindingMode.ToView);
			Binder.Bind(_003CviewModel_003EP, (ItemsTabViewModel vm) => vm.SearchText, searchBox);
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_003CviewModel_003EP.SearchCommand.Execute();
			});
			searchBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				_003CviewModel_003EP.SearchCommand.Execute();
			});
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
			LoadingSpinner val3 = new LoadingSpinner();
			((Control)val3).set_Parent((Container)(object)searchBoxPanel);
			((Control)val3).set_Size(new Point(((Control)searchBox).get_Height()));
			((Control)val3).set_Right(((Control)searchBox).get_Right());
			LoadingSpinner loadingSpinner = val3;
			Binder.Bind(_003CviewModel_003EP, (ItemsTabViewModel vm) => vm.Searching, loadingSpinner);
			Panel val4 = new Panel();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Top(((Control)searchBox).get_Height() + 9);
			((Control)val4).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			val4.set_CanScroll(true);
			_sidePanel = val4;
			Menu val5 = new Menu();
			((Control)val5).set_Parent((Container)(object)_sidePanel);
			((Control)val5).set_Size(((DesignStandard)(ref Panel.MenuStandard)).get_Size());
			val5.set_CanSelect(true);
			_sidebar = val5;
			WireUp((Container)(object)_sidebar, _003CviewModel_003EP.MenuItems);
			Panel val6 = new Panel();
			((Control)val6).set_Parent(_layout);
			((Control)val6).set_Left(((Control)_sidePanel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((Container)val6).set_WidthSizingMode((SizingMode)2);
			((Container)val6).set_HeightSizingMode((SizingMode)2);
			_contentPanel = val6;
			Binder.Bind<ItemsTabViewModel, Panel, string>(_003CviewModel_003EP, (Expression<Func<ItemsTabViewModel, string>>)((ItemsTabViewModel vm) => vm.ContentTitle), _contentPanel, (Expression<Func<Panel, string>>)((Panel ctl) => ctl.get_Title()), BindingMode.ToView);
			Binder.Bind<ItemsTabViewModel, Panel, AsyncTexture2D>(_003CviewModel_003EP, (Expression<Func<ItemsTabViewModel, AsyncTexture2D>>)((ItemsTabViewModel vm) => vm.ContentIcon), _contentPanel, (Expression<Func<Panel, AsyncTexture2D>>)((Panel ctl) => ctl.get_Icon()), BindingMode.ToView);
			((Control)_contentPanel).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				if (args.get_MousePosition().Y - ((Control)_contentPanel).get_AbsoluteBounds().Y <= 40)
				{
					_003CviewModel_003EP.BackCommand.Execute();
				}
			});
			ItemsList itemsList = new ItemsList();
			((Control)itemsList).set_Parent((Container)(object)_contentPanel);
			((Container)itemsList).set_WidthSizingMode((SizingMode)2);
			((Container)itemsList).set_HeightSizingMode((SizingMode)2);
			_searchResults = itemsList;
			_searchResults!.SetEntries(_003CviewModel_003EP.SearchResults);
			_searchResults!.SelectionChanged += delegate(object sender, ListBoxSelectionChangedEventArgs<ItemsListViewModel> args)
			{
				if (args.AddedItems.Count == 1)
				{
					ItemsListViewModel data = args.AddedItems[0].Data;
					_003CviewModel_003EP.SelectItemCommand.Execute(data.Item);
				}
			};
			Binder.Bind<ItemsTabViewModel, Scrollbar, string>(_003CviewModel_003EP, (Expression<Func<ItemsTabViewModel, string>>)((ItemsTabViewModel vm) => vm.ResultText), ((IEnumerable)((Container)_contentPanel).get_Children()).OfType<Scrollbar>().Single(), (Expression<Func<Scrollbar, string>>)((Scrollbar ctl) => ((Control)ctl).get_BasicTooltipText()), BindingMode.ToView);
			_003CviewModel_003EP.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				switch (args.PropertyName)
				{
				case "MenuItems":
					ReloadMenuItems();
					break;
				case "SearchResults":
					ShowSearchResults();
					break;
				case "SelectedItem":
					ShowItem(_003CviewModel_003EP.SelectedItem);
					break;
				case "ContentIcon":
					((Control)_contentPanel).Invalidate();
					break;
				}
			};
		}

		private void ReloadMenuItems()
		{
			if (_sidebar != null)
			{
				while (((Container)_sidebar).get_Children().get_Count() > 0)
				{
					((Container)_sidebar).get_Children().get_Item(0).Dispose();
				}
				WireUp((Container)(object)_sidebar, _003CviewModel_003EP.MenuItems);
			}
		}

		private void WireUp(Container parent, IList<ItemCategoryMenuItem> categories)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			foreach (ItemCategoryMenuItem category in categories)
			{
				MenuItem val = new MenuItem();
				((Control)val).set_Parent(parent);
				val.set_Text(category.Label);
				MenuItem menuItem = val;
				if (category.CanSelect)
				{
					menuItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
					{
						if (category.Id == "recently_added")
						{
							_003CviewModel_003EP.ShowRecentCommand.Execute();
						}
						else
						{
							_003CviewModel_003EP.ShowCategoryCommand.Execute(new ItemsFilter
							{
								Category = category.Id,
								Label = category.Label
							});
						}
					});
				}
				WireUp((Container)(object)menuItem, category.Subcategories);
				if (category.Id == _003CviewModel_003EP.SelectedCategory)
				{
					menuItem.Select();
				}
				_003CviewModel_003EP.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "SelectedCategory" && _003CviewModel_003EP.SelectedCategory == category.Id)
					{
						menuItem.Select();
					}
				};
			}
		}

		private void ShowSearchResults()
		{
			Container? selection = _selection;
			if (selection != null)
			{
				((Control)selection).Dispose();
			}
			_selection = null;
			_searchResults!.SetEntries(_003CviewModel_003EP.SearchResults);
			((Control)_searchResults).set_Parent((Container)(object)_contentPanel);
		}

		private void ShowItem(Item? item)
		{
			if ((object)item == null)
			{
				ShowSearchResults();
				return;
			}
			((Control)_searchResults).set_Parent((Container)null);
			ChatLinkEditor chatLinkEditor = new ChatLinkEditor(_003CviewModel_003EP.CreateChatLinkEditorViewModel(item));
			((Control)chatLinkEditor).set_Parent((Container)(object)_contentPanel);
			((Container)chatLinkEditor).set_WidthSizingMode((SizingMode)2);
			((Container)chatLinkEditor).set_HeightSizingMode((SizingMode)2);
			ChatLinkEditor editor = chatLinkEditor;
			Container? selection = _selection;
			if (selection != null)
			{
				((Control)selection).Dispose();
			}
			_selection = (Container?)(object)editor;
		}

		protected override void Unload()
		{
			Dispose();
		}

		public void Dispose()
		{
			Panel? sidePanel = _sidePanel;
			if (sidePanel != null)
			{
				((Control)sidePanel).Dispose();
			}
			Menu? sidebar = _sidebar;
			if (sidebar != null)
			{
				((Control)sidebar).Dispose();
			}
			Panel? contentPanel = _contentPanel;
			if (contentPanel != null)
			{
				((Control)contentPanel).Dispose();
			}
			ItemsList? searchResults = _searchResults;
			if (searchResults != null)
			{
				((Control)searchResults).Dispose();
			}
			Container? selection = _selection;
			if (selection != null)
			{
				((Control)selection).Dispose();
			}
			_003CviewModel_003EP.Dispose();
		}
	}
}
