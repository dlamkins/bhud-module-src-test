using System;
using System.Collections;
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
	public class CategoryMenu : Container, ICategoryMenuItem
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
				if (!((Control)this).SetProperty<int>(ref _menuItemHeight, value, false, "MenuItemHeight"))
				{
					return;
				}
				foreach (ICategoryMenuItem item in ((IEnumerable)base._children).Cast<ICategoryMenuItem>())
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
				((Control)this).SetProperty<bool>(ref _shouldShift, value, true, "ShouldShift");
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
				((Control)this).SetProperty<bool>(ref _canSelect, value, false, "CanSelect");
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
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			if (!_canSelect)
			{
				itemPath.ForEach(delegate(CategoryMenuItem i)
				{
					i.Deselect();
				});
				return;
			}
			foreach (ICategoryMenuItem item in ((Container)this).GetDescendants().Cast<ICategoryMenuItem>().Except(itemPath))
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
				CategoryMenuItem parent = ((Control)_selectedMenuItem).get_Parent() as CategoryMenuItem;
				if (parent != null)
				{
					_selectedMenuItem = parent;
				}
			}
			OnItemSelected(new ControlActivatedEventArgs((Control)(object)menuItem));
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
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control child in base._children)
			{
				child.set_Width(e.get_CurrentSize().X);
			}
			((Container)this).OnResized(e);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			ICategoryMenuItem newChild = e.get_ChangedChild() as ICategoryMenuItem;
			if (newChild == null)
			{
				((CancelEventArgs)(object)e).Cancel = true;
				return;
			}
			newChild.MenuItemHeight = MenuItemHeight;
			e.get_ChangedChild().set_Width(((Control)this).get_Width());
			Control lastItem = ((IEnumerable<Control>)base._children).LastOrDefault();
			if (lastItem != null)
			{
				lastItem.add_PropertyChanged((PropertyChangedEventHandler)delegate(object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "Bottom")
					{
						e.get_ChangedChild().set_Top(lastItem.get_Bottom());
					}
				});
				e.get_ChangedChild().set_Top(lastItem.get_Bottom());
			}
			ShouldShift = e.get_ResultingChildren().Any(delegate(Control mi)
			{
				CategoryMenuItem categoryMenuItem = (CategoryMenuItem)(object)mi;
				return categoryMenuItem.CanCheck || categoryMenuItem.Icon != null || ((IEnumerable<Control>)((Container)categoryMenuItem).get_Children()).Any();
			});
			((Container)this).OnChildAdded(e);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			int totalItemHeight = 0;
			foreach (Control child in base._children)
			{
				totalItemHeight = Math.Max(child.get_Bottom(), totalItemHeight);
			}
			((Control)this).set_Height(totalItemHeight);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			for (int sec = 0; sec < ((Control)this)._size.Y / MenuItemHeight; sec += 2)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureMenuItemFade.get_Texture(), new Rectangle(0, MenuItemHeight * sec - ((Container)this).get_VerticalScrollOffset(), ((Control)this)._size.X, MenuItemHeight), Color.get_Black() * 0.7f);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			int lastBottom = 0;
			foreach (Control item in ((IEnumerable<Control>)base._children).Where((Control c) => c.get_Visible()))
			{
				item.set_Location(new Point(0, lastBottom));
				item.set_Width(((Control)this).get_Width());
				lastBottom = item.get_Bottom();
			}
		}

		public CategoryMenu()
			: this()
		{
		}
	}
}
