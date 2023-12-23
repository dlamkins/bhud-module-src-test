using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using felix.BlishEmotes.Services;
using felix.BlishEmotes.Strings;
using felix.BlishEmotes.UI.Controls;
using felix.BlishEmotes.UI.Presenters;

namespace felix.BlishEmotes.UI.Views
{
	internal class CategorySettingsView : Blish_HUD.Graphics.UI.View
	{
		private IconSelection IconSelection;

		private Blish_HUD.Controls.Panel CategoryListPanel;

		private ReorderableMenu CategoryListMenu;

		private StandardButton AddCategoryButton;

		private Blish_HUD.Controls.Panel CategoryEditPanel;

		private Dictionary<ReorderableMenuItem, Category> MenuItemsMap;

		private Category _editingCategory;

		private const int _labelWidth = 200;

		private const int _controlWidth = 150;

		private const int _height = 20;

		public List<Category> Categories { get; set; }

		public List<Emote> Emotes { get; set; }

		public event EventHandler<AddCategoryArgs> AddCategory;

		public event EventHandler<Category> UpdateCategory;

		public event EventHandler<Category> DeleteCategory;

		public event EventHandler<List<Category>> ReorderCategories;

		public CategorySettingsView(CategoriesManager categoriesManager, EmotesManager emotesManager)
		{
			WithPresenter(new CategorySettingsPresenter(this, (categoriesManager, emotesManager)));
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
			IconSelection = new IconSelection(buildPanel, null)
			{
				Options = Emotes.Select((Emote emote) => new SelectionOption(emote.Texture, "emotes/" + emote.TextureRef)).ToList(),
				Padding = new Thickness(5f)
			};
			IconSelection.Selected += delegate(object sender, SelectionOption item)
			{
				if (_editingCategory != null)
				{
					_editingCategory.TextureFileName = item.TextureRef;
					_editingCategory.Texture = item.Texture;
					BuildEditPanel();
				}
			};
			int addBtnHeight = 30;
			CategoryListPanel = new Blish_HUD.Controls.Panel
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
			CategoryEditPanel = new Blish_HUD.Controls.Panel
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
				_editingCategory = category.Clone();
				BuildEditPanel();
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
					_editingCategory = category.Clone();
					BuildEditPanel();
				};
				MenuItemsMap.Add(menuItem, category);
			}
		}

		private Blish_HUD.Controls.ContextMenuStrip BuildCategoryRightClickMenu(Category category)
		{
			Blish_HUD.Controls.ContextMenuStrip contextMenuStrip = new Blish_HUD.Controls.ContextMenuStrip();
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

		private void BuildEditPanel()
		{
			CategoryEditPanel.ClearChildren();
			Blish_HUD.Controls.Panel settingsPanel = new Blish_HUD.Controls.Panel
			{
				Parent = CategoryEditPanel,
				CanCollapse = false,
				CanScroll = false,
				Size = CategoryEditPanel.ContentRegion.Size
			};
			Blish_HUD.Controls.Panel header = new Blish_HUD.Controls.Panel
			{
				Parent = settingsPanel,
				Width = settingsPanel.Width,
				Height = 60,
				CanScroll = false,
				ShowBorder = false
			};
			Image icon = new Image
			{
				Parent = header,
				BasicTooltipText = "Click to change icon.",
				Width = 40,
				Height = 40,
				Texture = _editingCategory.Texture,
				Location = new Point(10)
			};
			IconSelection.AttachedToControl = icon;
			icon.LeftMouseButtonPressed += delegate
			{
				Thread thread = new Thread(OpenFileDialogThreadMethod);
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
			};
			if (_editingCategory.IsFavourite)
			{
				new Blish_HUD.Controls.Label
				{
					Parent = header,
					Width = 200,
					Height = 40,
					Text = _editingCategory.Name,
					Location = new Point(header.Size.X / 2 - 100, 10),
					Font = GameService.Content.DefaultFont18
				};
			}
			else
			{
				Blish_HUD.Controls.TextBox textBox = new Blish_HUD.Controls.TextBox();
				textBox.Parent = header;
				textBox.Width = 200;
				textBox.Height = 40;
				textBox.Text = _editingCategory.Name;
				textBox.MaxLength = 20;
				textBox.Location = new Point(header.Size.X / 2 - 100, 10);
				textBox.Font = GameService.Content.DefaultFont18;
				textBox.TextChanged += delegate(object s, EventArgs args)
				{
					ValueChangedEventArgs<string> valueChangedEventArgs = args as ValueChangedEventArgs<string>;
					if (valueChangedEventArgs != null)
					{
						_editingCategory.Name = valueChangedEventArgs.NewValue;
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
				this.UpdateCategory?.Invoke(this, _editingCategory);
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
				new Blish_HUD.Controls.Label
				{
					Parent = emoteInCategoryRow,
					Text = emote.Label,
					Size = new Point(100, 20),
					Location = new Point(0, 0)
				};
				Checkbox checkbox = new Checkbox();
				checkbox.Parent = emoteInCategoryRow;
				checkbox.Checked = (base.Presenter as CategorySettingsPresenter).IsEmoteInCategory(_editingCategory.Id, emote);
				checkbox.Size = new Point(150, 20);
				checkbox.Location = new Point(205, 0);
				checkbox.CheckedChanged += delegate(object s, CheckChangedEvent args)
				{
					if (args.Checked)
					{
						_editingCategory.AddEmote(emote);
					}
					else
					{
						_editingCategory.RemoveEmote(emote);
					}
				};
			}
		}

		private void OpenFileDialogThreadMethod()
		{
			using OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.InitialDirectory = "c:\\";
			string extensionsFilterString = string.Join(";", TexturesManager.TextureExtensionMasks);
			fileDialog.Filter = "Image files|" + extensionsFilterString;
			fileDialog.RestoreDirectory = true;
			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				_editingCategory.TextureFileName = fileDialog.FileName;
				Stream fileStream = fileDialog.OpenFile();
				_editingCategory.Texture = TextureUtil.FromStreamPremultiplied(fileStream);
				BuildEditPanel();
			}
		}

		protected override void Unload()
		{
			IconSelection?.Dispose();
			CategoryListPanel?.Dispose();
			AddCategoryButton?.Dispose();
			CategoryEditPanel?.Dispose();
			CategoryListMenu?.Dispose();
		}
	}
}
