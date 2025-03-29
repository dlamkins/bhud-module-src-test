using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using GuildWars2.Hero.Achievements.Categories;
using Microsoft.Xna.Framework.Graphics;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementsTabView : View, IDisposable
	{
		private readonly TextBox _searchBox;

		private readonly Panel _categoriesPanel;

		private readonly Menu _sidebar;

		private readonly ViewContainer _selectedCategoryView;

		public AchievementsTabViewModel ViewModel { get; }

		private event EventHandler<EventArgs>? _menuItemExpanded;

		public AchievementsTabView(AchievementsTabViewModel viewModel)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			ThrowHelper.ThrowIfNull(viewModel, "viewModel");
			ViewModel = viewModel;
			TextBox val = new TextBox();
			((Control)val).set_Left(4);
			((Control)val).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			_searchBox = val;
			Binder.Bind<AchievementsTabViewModel, TextBox, string>(viewModel, (Expression<Func<AchievementsTabViewModel, string>>)((AchievementsTabViewModel vm) => vm.SearchPlaceholder), _searchBox, (Expression<Func<TextBox, string>>)((TextBox searchBox) => ((TextInputBase)searchBox).get_PlaceholderText()), BindingMode.ToView);
			Panel val2 = new Panel();
			((Control)val2).set_Top(((Control)_searchBox).get_Height() + 9);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			_categoriesPanel = val2;
			Binder.Bind<AchievementsTabViewModel, Panel, string>(viewModel, (Expression<Func<AchievementsTabViewModel, string>>)((AchievementsTabViewModel vm) => vm.CategoriesTitle), _categoriesPanel, (Expression<Func<Panel, string>>)((Panel panel) => panel.get_Title()), BindingMode.ToView);
			Menu val3 = new Menu();
			((Control)val3).set_Parent((Container)(object)_categoriesPanel);
			((Control)val3).set_Size(((DesignStandard)(ref Panel.MenuStandard)).get_Size());
			val3.set_CanSelect(true);
			_sidebar = val3;
			ViewContainer val4 = new ViewContainer();
			((Panel)val4).set_CanScroll(true);
			_selectedCategoryView = val4;
			viewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
			{
				switch (args.PropertyName)
				{
				case "HeaderText":
					((Panel)_selectedCategoryView).set_Title(ViewModel.HeaderText);
					break;
				case "HeaderIcon":
					((Panel)_selectedCategoryView).set_Icon(ViewModel.HeaderIcon);
					break;
				case "Achievements":
					_selectedCategoryView.Show((IView)(object)new AchievementsListView(ViewModel.Achievements));
					break;
				case "MenuItems":
					ReloadMenuItems();
					break;
				}
			};
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			return ViewModel.Load();
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_searchBox).set_Parent(buildPanel);
			((Control)_categoriesPanel).set_Parent(buildPanel);
			AddAchievementCategories();
			((Control)_selectedCategoryView).set_Parent(buildPanel);
			((Control)_selectedCategoryView).set_Left(((Control)_categoriesPanel).get_Right() + 9);
			((Control)_selectedCategoryView).set_Width(650);
			((Container)_selectedCategoryView).set_HeightSizingMode((SizingMode)2);
			Binder.Bind(ViewModel, (AchievementsTabViewModel vm) => vm.SearchText, _searchBox);
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchTextChanged);
			_searchBox.add_EnterPressed((EventHandler<EventArgs>)SearchEnterPressed);
		}

		private void ReloadMenuItems()
		{
			this._menuItemExpanded = null;
			while (((Container)_sidebar).get_Children().get_Count() > 0)
			{
				((Container)_sidebar).get_Children().get_Item(0).Dispose();
			}
			AddAchievementCategories();
		}

		private void AddAchievementCategories()
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			foreach (AchievementGroupMenuItem menuItem in ViewModel.MenuItems)
			{
				MenuItem groupMenuItem = _sidebar.AddMenuItem(menuItem.Group.Name, (Texture2D)null);
				((Control)groupMenuItem).set_BasicTooltipText(menuItem.Group.Description);
				foreach (AchievementCategory category in menuItem.Categories)
				{
					AsyncTexture2D icon = ViewModel.GetIcon(category.IconHref);
					MenuItem val = new MenuItem(category.Name, icon);
					((Control)val).set_Parent((Container)(object)groupMenuItem);
					((Control)val).set_BasicTooltipText(category.Description);
					MenuItem categoryItem = val;
					categoryItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
					{
						Task.Run(() => ViewModel.SelectCategory(category));
					});
					if (category.Id == ViewModel.SelectedCategory?.Id)
					{
						categoryItem.Select();
					}
				}
				((Control)groupMenuItem).add_PropertyChanged((PropertyChangedEventHandler)delegate(object sender, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "Expand")
					{
						this._menuItemExpanded?.Invoke(sender, EventArgs.Empty);
					}
				});
				_menuItemExpanded += delegate(object sender, EventArgs args)
				{
					if (sender != groupMenuItem)
					{
						groupMenuItem.Collapse();
					}
				};
			}
		}

		private void SearchTextChanged(object sender, EventArgs e)
		{
			MenuItem selectedMenuItem = _sidebar.get_SelectedMenuItem();
			if (selectedMenuItem != null)
			{
				selectedMenuItem.Deselect();
			}
			ViewModel.SearchCommand.Execute(null);
		}

		private void SearchEnterPressed(object sender, EventArgs e)
		{
			MenuItem selectedMenuItem = _sidebar.get_SelectedMenuItem();
			if (selectedMenuItem != null)
			{
				selectedMenuItem.Deselect();
			}
			ViewModel.SearchCommand.Execute(null);
		}

		public void Dispose()
		{
			((Control)_searchBox).Dispose();
			((Control)_categoriesPanel).Dispose();
			((Control)_sidebar).Dispose();
			((Control)_selectedCategoryView).Dispose();
			ViewModel.Dispose();
		}
	}
}
