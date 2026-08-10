using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SpamsListControl : Panel
	{
		private const string UNCATEGORIZED_LABEL = "Uncategorized";

		private const int ADD_BUTTON_HEIGHT = 24;

		private readonly SpamClient _spamClient;

		private readonly SpamSource _source;

		private readonly Dictionary<MenuItem, SpamDto> _menuItemToSpams = new Dictionary<MenuItem, SpamDto>();

		private readonly Dictionary<Guid, MenuItem> _categoryMenuItems = new Dictionary<Guid, MenuItem>();

		private readonly Dictionary<Guid, SpamCategoryDto> _categories = new Dictionary<Guid, SpamCategoryDto>();

		private readonly Menu _menu;

		private StandardButton _addCategoryButton;

		private ContextMenuStrip _categoryContextMenu;

		private SpamCategoryDto _contextMenuCategory;

		private MenuItem _uncategorizedMenuItem;

		private MenuItem _selectedMenuItem;

		public event EventHandler<SelectedSpamChangedEventArgs> SelectedSpamChanged;

		public event EventHandler<CategoriesLoadedEventArgs> CategoriesLoaded;

		public event EventHandler CreateCategoryRequested;

		public event EventHandler<CategoryEventArgs> EditCategoryRequested;

		public event EventHandler<CategoryEventArgs> DeleteCategoryRequested;

		public SpamsListControl(SpamClient spamClient, SpamSource source)
			: this()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_source = source;
			((Control)this).set_Width(300);
			((Control)this).set_Height(400);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("+ Category");
			((Control)val).set_Width(((Control)this).get_Width() - 10);
			((Control)val).set_Height(24);
			((Control)val).set_Left(5);
			((Control)val).set_Top(2);
			_addCategoryButton = val;
			((Control)_addCategoryButton).add_Click((EventHandler<MouseEventArgs>)AddCategoryButton_Click);
			Menu val2 = new Menu();
			((Control)val2).set_Width(((Control)this).get_Width());
			((Control)val2).set_Height(((Control)this).get_Height() - 24 - 4);
			((Control)val2).set_Top(28);
			((Control)val2).set_Parent((Container)(object)this);
			_menu = val2;
			_menu.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)Menu_ItemSelected);
			CreateCategoryContextMenu();
			LoadDataAsync();
		}

		private void CreateCategoryContextMenu()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			_categoryContextMenu = new ContextMenuStrip();
			((Control)_categoryContextMenu.AddMenuItem("Edit Category")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_contextMenuCategory != null)
				{
					this.EditCategoryRequested?.Invoke(this, new CategoryEventArgs
					{
						Category = _contextMenuCategory
					});
				}
			});
			((Control)_categoryContextMenu.AddMenuItem("Delete Category")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_contextMenuCategory != null)
				{
					this.DeleteCategoryRequested?.Invoke(this, new CategoryEventArgs
					{
						Category = _contextMenuCategory
					});
				}
			});
		}

		private void AddCategoryButton_Click(object sender, MouseEventArgs e)
		{
			this.CreateCategoryRequested?.Invoke(this, EventArgs.Empty);
		}

		private async Task LoadDataAsync()
		{
			Module instance = Module.Instance;
			if (instance == null || !instance.IsConnected)
			{
				return;
			}
			try
			{
				List<SpamCategoryDto> categories = await _spamClient.GetCategories(_source);
				if (categories != null)
				{
					foreach (SpamCategoryDto category in categories.OrderBy((SpamCategoryDto c) => c.DisplayOrder))
					{
						_categories[category.Id] = category;
						CreateCategoryMenuItem(category);
					}
				}
				this.CategoriesLoaded?.Invoke(this, new CategoriesLoadedEventArgs
				{
					Categories = GetCategories()
				});
				SpamsListControl spamsListControl = this;
				MenuItem val = new MenuItem("Uncategorized");
				((Control)val).set_Parent((Container)(object)_menu);
				val.set_CanCheck(false);
				spamsListControl._uncategorizedMenuItem = val;
				await LoadSpamsAsync();
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load data: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		public async Task LoadSpamsAsync()
		{
			try
			{
				List<SpamDto> spams = await _spamClient.GetSpams(_source);
				if (spams == null)
				{
					return;
				}
				foreach (SpamDto spam in spams.OrderBy((SpamDto s) => s.Name))
				{
					CreateOrUpdateMenuItem(spam);
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load spams: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void CreateCategoryMenuItem(SpamCategoryDto category)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			if (!_categoryMenuItems.ContainsKey(category.Id))
			{
				MenuItem val = new MenuItem(category.Name);
				((Control)val).set_Parent((Container)(object)_menu);
				((Control)val).set_BasicTooltipText(category.Description ?? "Right-click for options");
				val.set_CanCheck(false);
				MenuItem menuItem = val;
				((Control)menuItem).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					_contextMenuCategory = category;
					_categoryContextMenu.Show(GameService.Input.get_Mouse().get_Position());
				});
				_categoryMenuItems[category.Id] = menuItem;
			}
		}

		public void AddOrUpdateSpam(SpamDto spam)
		{
			if (spam != null)
			{
				CreateOrUpdateMenuItem(spam);
			}
		}

		public void RemoveSpam(Guid spamId)
		{
			SpamDto spam = _menuItemToSpams.Values.FirstOrDefault((SpamDto s) => s.Id == spamId);
			if (spam == null)
			{
				return;
			}
			DeleteMenuItem(spam);
			if (_selectedMenuItem != null)
			{
				SpamDto spam2 = GetSpam(_selectedMenuItem);
				if (spam2 != null && spam2.Id == spamId)
				{
					_selectedMenuItem = null;
					OnSelectedSpamChanged(new SelectedSpamChangedEventArgs
					{
						SelectedSpam = null
					});
				}
			}
		}

		public void AddCategory(SpamCategoryDto category)
		{
			if (category != null)
			{
				_categories[category.Id] = category;
				CreateCategoryMenuItem(category);
				this.CategoriesLoaded?.Invoke(this, new CategoriesLoadedEventArgs
				{
					Categories = GetCategories()
				});
			}
		}

		public void UpdateCategory(SpamCategoryDto category)
		{
			if (category != null)
			{
				_categories[category.Id] = category;
				if (_categoryMenuItems.TryGetValue(category.Id, out var menuItem))
				{
					menuItem.set_Text(category.Name);
					((Control)menuItem).set_BasicTooltipText(category.Description);
				}
				this.CategoriesLoaded?.Invoke(this, new CategoriesLoadedEventArgs
				{
					Categories = GetCategories()
				});
			}
		}

		public void RemoveCategory(Guid categoryId)
		{
			_categories.Remove(categoryId);
			if (_categoryMenuItems.TryGetValue(categoryId, out var menuItem))
			{
				foreach (KeyValuePair<MenuItem, SpamDto> item in _menuItemToSpams.Where((KeyValuePair<MenuItem, SpamDto> kvp) => ((Control)kvp.Key).get_Parent() == menuItem).ToList())
				{
					((Control)item.Key).set_Parent((Container)(object)_uncategorizedMenuItem);
				}
				((Control)menuItem).Dispose();
				((Container)_menu).RemoveChild((Control)(object)menuItem);
				_categoryMenuItems.Remove(categoryId);
			}
			this.CategoriesLoaded?.Invoke(this, new CategoriesLoadedEventArgs
			{
				Categories = GetCategories()
			});
		}

		public List<SpamCategoryDto> GetCategories()
		{
			return _categories.Values.OrderBy((SpamCategoryDto c) => c.DisplayOrder).ToList();
		}

		private MenuItem GetMenuItem(SpamDto spam)
		{
			if (spam == null)
			{
				return null;
			}
			return _menuItemToSpams.FirstOrDefault((KeyValuePair<MenuItem, SpamDto> kvp) => kvp.Value != null && kvp.Value.Id == spam.Id).Key;
		}

		private SpamDto GetSpam(MenuItem menuItem)
		{
			if (!_menuItemToSpams.TryGetValue(menuItem, out var spam))
			{
				return null;
			}
			return spam;
		}

		private void DeleteMenuItem(SpamDto spam)
		{
			if (spam == null)
			{
				return;
			}
			MenuItem menuItem = GetMenuItem(spam);
			if (menuItem != null)
			{
				Container parent = ((Control)menuItem).get_Parent();
				MenuItem parentItem = (MenuItem)(object)((parent is MenuItem) ? parent : null);
				if (parentItem != null)
				{
					((Container)parentItem).get_Children().Remove((Control)(object)menuItem);
				}
				((Control)menuItem).Dispose();
				_menuItemToSpams.Remove(menuItem);
			}
		}

		private void CreateOrUpdateMenuItem(SpamDto spam)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			string tooltip = $"{spam.CreatedAt:g}: {spam.Description}";
			MenuItem menuItem = GetMenuItem(spam);
			MenuItem parentMenuItem = GetParentMenuItemForSpam(spam);
			if (menuItem == null)
			{
				MenuItem val = new MenuItem(spam.Name);
				((Control)val).set_BasicTooltipText(tooltip);
				((Control)val).set_Parent((Container)(object)parentMenuItem);
				menuItem = val;
				menuItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)MenuItem_ItemSelected);
				_menuItemToSpams.Add(menuItem, spam);
				return;
			}
			Container parent = ((Control)menuItem).get_Parent();
			if (((parent is MenuItem) ? parent : null) != parentMenuItem)
			{
				((Control)menuItem).set_Parent((Container)(object)parentMenuItem);
			}
			menuItem.set_Text(spam.Name);
			((Control)menuItem).set_BasicTooltipText(tooltip);
			_menuItemToSpams[menuItem] = spam;
		}

		private MenuItem GetParentMenuItemForSpam(SpamDto spam)
		{
			if (spam.CategoryId.HasValue && _categoryMenuItems.TryGetValue(spam.CategoryId.Value, out var categoryMenuItem))
			{
				return categoryMenuItem;
			}
			return _uncategorizedMenuItem;
		}

		private void MenuItem_ItemSelected(object sender, ControlActivatedEventArgs e)
		{
			Control activatedControl = e.get_ActivatedControl();
			MenuItem menuItem = (MenuItem)(object)((activatedControl is MenuItem) ? activatedControl : null);
			if (menuItem == null)
			{
				ScreenNotification.ShowNotification("Selected menu item not found.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			SpamDto spam = GetSpam(menuItem);
			if (spam == null)
			{
				ScreenNotification.ShowNotification("Selected spam not found.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			_selectedMenuItem = menuItem;
			OnSelectedSpamChanged(new SelectedSpamChangedEventArgs
			{
				SelectedSpam = spam
			});
		}

		private void Menu_ItemSelected(object sender, ControlActivatedEventArgs e)
		{
			MenuItem_ItemSelected(sender, e);
		}

		protected virtual void OnSelectedSpamChanged(SelectedSpamChangedEventArgs e)
		{
			this.SelectedSpamChanged?.Invoke(this, e);
		}

		protected override void DisposeControl()
		{
			if (_addCategoryButton != null)
			{
				((Control)_addCategoryButton).remove_Click((EventHandler<MouseEventArgs>)AddCategoryButton_Click);
				((Control)_addCategoryButton).Dispose();
			}
			ContextMenuStrip categoryContextMenu = _categoryContextMenu;
			if (categoryContextMenu != null)
			{
				((Control)categoryContextMenu).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
