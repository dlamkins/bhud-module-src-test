using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using felix.BlishEmotes.Strings;
using felix.BlishEmotes.UI.Controls;
using felix.BlishEmotes.UI.Presenters;

namespace felix.BlishEmotes.UI.Views
{
	internal class CategorySettingsView : View
	{
		private Helper helper;

		private Panel CategoryListPanel;

		private ReorderableMenu CategoryListMenu;

		private StandardButton AddCategoryButton;

		private Panel CategoryEditPanel;

		private Dictionary<ReorderableMenuItem, Category> MenuItemsMap;

		private const int _labelWidth = 200;

		private const int _controlWidth = 150;

		private const int _height = 20;

		public List<Category> Categories { get; set; }

		public List<Emote> Emotes { get; set; }

		public event EventHandler<AddCategoryArgs> AddCategory;

		public event EventHandler<Category> UpdateCategory;

		public event EventHandler<Category> DeleteCategory;

		public event EventHandler<List<Category>> ReorderCategories;

		public CategorySettingsView(CategoriesManager categoriesManager, EmotesManager emotesManager, Helper helper)
		{
			WithPresenter(new CategorySettingsPresenter(this, (categoriesManager, emotesManager)));
			this.helper = helper;
			MenuItemsMap = new Dictionary<ReorderableMenuItem, Category>();
		}

		private FlowPanel CreateRowPanel(Container parent)
		{
			return new FlowPanel
			{
				CanCollapse = false,
				CanScroll = false,
				Parent = parent,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				OuterControlPadding = new Vector2(5f, 5f)
			};
		}

		protected override void Build(Container buildPanel)
		{
			int addBtnHeight = 30;
			CategoryListPanel = new Panel
			{
				Parent = buildPanel,
				ShowBorder = true,
				CanScroll = true,
				HeightSizingMode = SizingMode.Standard,
				WidthSizingMode = SizingMode.Standard,
				Location = new Point(0, 0),
				Size = new Point(200, buildPanel.ContentRegion.Height - 10 - addBtnHeight)
			};
			CategoryListMenu = new ReorderableMenu
			{
				Parent = CategoryListPanel,
				WidthSizingMode = SizingMode.Fill
			};
			CategoryListMenu.Reordered += delegate(object s, List<ReorderableMenuItem> e)
			{
				List<Category> list = new List<Category>();
				foreach (ReorderableMenuItem current in e)
				{
					if (MenuItemsMap.TryGetValue(current, out var value))
					{
						list.Add(value);
					}
				}
				this.ReorderCategories?.Invoke(this, list);
			};
			BuildCategoryMenuItems();
			AddCategoryButton = new StandardButton
			{
				Parent = buildPanel,
				Text = Common.category_add,
				Size = new Point(200, addBtnHeight),
				Location = new Point(0, CategoryListPanel.Bottom + 10)
			};
			AddCategoryButton.Click += delegate
			{
				this.AddCategory?.Invoke(this, new AddCategoryArgs
				{
					Name = CategoriesManager.NEW_CATEGORY_NAME
				});
			};
			CategoryEditPanel = new Panel
			{
				Parent = buildPanel,
				ShowBorder = true,
				CanScroll = false,
				HeightSizingMode = SizingMode.Fill,
				WidthSizingMode = SizingMode.Fill,
				Location = new Point(CategoryListPanel.Size.X + 20, 0)
			};
		}

		public void Rebuild(Category category = null)
		{
			CategoryEditPanel?.ClearChildren();
			BuildCategoryMenuItems();
			if (category != null)
			{
				BuildEditPanel(CategoryEditPanel, category);
			}
		}

		private void BuildCategoryMenuItems()
		{
			CategoryListMenu?.ClearChildren();
			MenuItemsMap.Clear();
			foreach (Category category in Categories)
			{
				ReorderableMenuItem menuItem = new ReorderableMenuItem(category.Name)
				{
					Parent = CategoryListMenu,
					WidthSizingMode = SizingMode.Fill,
					HeightSizingMode = SizingMode.AutoSize,
					Menu = BuildCategoryRightClickMenu(category),
					CanDrag = !category.IsFavourite
				};
				menuItem.Click += delegate
				{
					BuildEditPanel(CategoryEditPanel, category);
				};
				MenuItemsMap.Add(menuItem, category);
			}
		}

		private ContextMenuStrip BuildCategoryRightClickMenu(Category category)
		{
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			ContextMenuStripItem deleteItem = new ContextMenuStripItem
			{
				Text = Common.settings_ui_delete,
				Enabled = !category.IsFavourite
			};
			deleteItem.Click += delegate
			{
				this.DeleteCategory?.Invoke(this, category);
			};
			contextMenuStrip.AddMenuItem(deleteItem);
			return contextMenuStrip;
		}

		private void BuildEditPanel(Panel parent, Category category)
		{
			parent.ClearChildren();
			Panel settingsPanel = new Panel
			{
				Parent = parent,
				CanCollapse = false,
				CanScroll = false,
				Size = parent.ContentRegion.Size
			};
			Panel header = new Panel
			{
				Parent = settingsPanel,
				Width = settingsPanel.Width,
				Height = 60,
				CanScroll = false,
				ShowBorder = false
			};
			if (category.IsFavourite)
			{
				new Label
				{
					Parent = header,
					Width = 200,
					Height = 40,
					Text = category.Name,
					Location = new Point(header.Size.X / 2 - 100, 10),
					Font = GameService.Content.DefaultFont18
				};
			}
			else
			{
				TextBox textBox = new TextBox();
				textBox.Parent = header;
				textBox.Width = 200;
				textBox.Height = 40;
				textBox.Text = category.Name;
				textBox.MaxLength = 20;
				textBox.Location = new Point(header.Size.X / 2 - 100, 10);
				textBox.Font = GameService.Content.DefaultFont18;
				textBox.TextChanged += delegate(object s, EventArgs args)
				{
					ValueChangedEventArgs<string> valueChangedEventArgs = args as ValueChangedEventArgs<string>;
					if (valueChangedEventArgs != null)
					{
						category.Name = valueChangedEventArgs.NewValue;
					}
				};
			}
			StandardButton standardButton = new StandardButton();
			standardButton.Parent = header;
			standardButton.Text = Common.settings_ui_save;
			standardButton.Size = new Point(80, 40);
			standardButton.Location = new Point(header.Size.X - 100, 10);
			standardButton.Click += delegate
			{
				this.UpdateCategory?.Invoke(this, category);
			};
			FlowPanel emotesPanel = new FlowPanel
			{
				Parent = settingsPanel,
				CanCollapse = false,
				CanScroll = false,
				FlowDirection = ControlFlowDirection.TopToBottom,
				HeightSizingMode = SizingMode.Fill,
				WidthSizingMode = SizingMode.Fill,
				Location = new Point(0, 80)
			};
			foreach (Emote emote in Emotes)
			{
				FlowPanel emoteInCategoryRow = CreateRowPanel(emotesPanel);
				emoteInCategoryRow.OuterControlPadding = new Vector2(20f, 5f);
				new Label
				{
					Parent = emoteInCategoryRow,
					Text = helper.EmotesResourceManager.GetString(emote.Id),
					Size = new Point(100, 20),
					Location = new Point(0, 0)
				};
				Checkbox checkbox = new Checkbox();
				checkbox.Parent = emoteInCategoryRow;
				checkbox.Checked = (base.Presenter as CategorySettingsPresenter).IsEmoteInCategory(category.Id, emote);
				checkbox.Size = new Point(150, 20);
				checkbox.Location = new Point(205, 0);
				checkbox.CheckedChanged += delegate(object s, CheckChangedEvent args)
				{
					if (args.Checked)
					{
						category.AddEmote(emote);
					}
					else
					{
						category.RemoveEmote(emote);
					}
				};
			}
		}

		protected override void Unload()
		{
			CategoryListPanel?.Dispose();
			AddCategoryButton?.Dispose();
			CategoryEditPanel?.Dispose();
			CategoryListMenu?.Dispose();
		}
	}
}
