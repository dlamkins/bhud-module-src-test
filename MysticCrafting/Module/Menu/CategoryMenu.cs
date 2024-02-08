using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Menu
{
	public class CategoryMenu : Blish_HUD.Controls.Container, ICategoryMenuItem
	{
		private const int DefaultItemHeight = 40;

		private readonly AsyncTexture2D _textureMenuItemFade = AsyncTexture2D.FromAssetId(156044);

		protected int _menuItemHeight = 40;

		protected bool _shouldShift;

		private bool _canSelect;

		private CategoryMenuItem _selectedMenuItem;

		public int MenuItemHeight
		{
			get
			{
				return _menuItemHeight;
			}
			set
			{
				if (!SetProperty(ref _menuItemHeight, value, invalidateLayout: false, "MenuItemHeight"))
				{
					return;
				}
				foreach (ICategoryMenuItem item in _children.Cast<ICategoryMenuItem>())
				{
					item.MenuItemHeight = value;
				}
			}
		}

		public bool ShouldShift
		{
			get
			{
				return _shouldShift;
			}
			set
			{
				SetProperty(ref _shouldShift, value, invalidateLayout: true, "ShouldShift");
			}
		}

		public bool CanSelect
		{
			get
			{
				return _canSelect;
			}
			set
			{
				SetProperty(ref _canSelect, value, invalidateLayout: false, "CanSelect");
			}
		}

		bool ICategoryMenuItem.Selected => false;

		public CategoryMenuItem SelectedMenuItem => _selectedMenuItem;

		public event EventHandler<ControlActivatedEventArgs> ItemSelected;

		protected virtual void OnItemSelected(ControlActivatedEventArgs e)
		{
			this.ItemSelected?.Invoke(this, e);
		}

		void ICategoryMenuItem.Select()
		{
			throw new InvalidOperationException("The root Menu instance can not be selected.");
		}

		public void Select(CategoryMenuItem menuItem, List<CategoryMenuItem> itemPath)
		{
			if (!_canSelect)
			{
				itemPath.ForEach(delegate(CategoryMenuItem i)
				{
					i.Deselect();
				});
				return;
			}
			foreach (ICategoryMenuItem item in GetDescendants().Cast<ICategoryMenuItem>().Except(itemPath))
			{
				item.Collapse();
				item.Deselect();
			}
			if (_selectedMenuItem == null || _selectedMenuItem.Text != menuItem.Text)
			{
				_selectedMenuItem = menuItem;
			}
			else
			{
				CategoryMenuItem parent = _selectedMenuItem.Parent as CategoryMenuItem;
				if (parent != null)
				{
					_selectedMenuItem = parent;
				}
			}
			OnItemSelected(new ControlActivatedEventArgs(menuItem));
		}

		public void Select(CategoryMenuItem menuItem)
		{
			menuItem.Select();
		}

		void ICategoryMenuItem.Deselect()
		{
			Select(null, null);
		}

		public void Collapse()
		{
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			foreach (Control child in _children)
			{
				child.Width = e.CurrentSize.X;
			}
			base.OnResized(e);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			ICategoryMenuItem newChild = e.ChangedChild as ICategoryMenuItem;
			if (newChild == null)
			{
				e.Cancel = true;
				return;
			}
			newChild.MenuItemHeight = MenuItemHeight;
			e.ChangedChild.Width = base.Width;
			Control lastItem = _children.LastOrDefault();
			if (lastItem != null)
			{
				lastItem.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "Bottom")
					{
						e.ChangedChild.Top = lastItem.Bottom;
					}
				};
				e.ChangedChild.Top = lastItem.Bottom;
			}
			ShouldShift = e.ResultingChildren.Any(delegate(Control mi)
			{
				CategoryMenuItem categoryMenuItem = (CategoryMenuItem)mi;
				return categoryMenuItem.CanCheck || categoryMenuItem.Icon != null || categoryMenuItem.Children.Any();
			});
			base.OnChildAdded(e);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			int totalItemHeight = 0;
			foreach (Control child in _children)
			{
				totalItemHeight = Math.Max(child.Bottom, totalItemHeight);
			}
			base.Height = totalItemHeight;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			for (int sec = 0; sec < _size.Y / MenuItemHeight; sec += 2)
			{
				spriteBatch.DrawOnCtrl(this, _textureMenuItemFade.Texture, new Rectangle(0, MenuItemHeight * sec - base.VerticalScrollOffset, _size.X, MenuItemHeight), Color.Black * 0.7f);
			}
		}

		public override void RecalculateLayout()
		{
			int lastBottom = 0;
			foreach (Control item in _children.Where((Control c) => c.Visible))
			{
				item.Location = new Point(0, lastBottom);
				item.Width = base.Width;
				lastBottom = item.Bottom;
			}
		}
	}
}
