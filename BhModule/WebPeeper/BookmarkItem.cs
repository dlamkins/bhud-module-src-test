using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CefHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class BookmarkItem : MenuItem
	{
		private readonly Bookmark _bookmark;

		private readonly IconButton _removeBtn;

		private readonly IconButton _sortBtn;

		private readonly TextBox _nameInput;

		private static readonly Texture2D _removeTexture = GameService.Content.GetTexture("common/733270");

		private static readonly Texture2D _sortTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("sort.png");

		private bool _editing;

		private int _changeOrderAbsoluteStartPointY;

		private int _changeOrderStartPointY;

		private int _maxChangeOrderY;

		public event EventHandler<EventArgs> Removed;

		public event EventHandler<BookmarkDraggedEventArgs> Dragged;

		public BookmarkItem(Bookmark bookmark)
			: this(bookmark.Name)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			_bookmark = bookmark;
			IconButton iconButton = new IconButton(_sortTexture, 25, 3, 1.3f);
			((Control)iconButton).set_Visible(false);
			((Control)iconButton).set_BasicTooltipText("Drag to reorder");
			_sortBtn = iconButton;
			((Control)_sortBtn).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)StartChangeOrder);
			IconButton iconButton2 = new IconButton(_removeTexture, 25, 1.5f);
			((Control)iconButton2).set_Visible(false);
			_removeBtn = iconButton2;
			((Control)_removeBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.Removed?.Invoke(this, EventArgs.Empty);
				((Control)this).Dispose();
			});
			TextBox val = new TextBox();
			((Control)val).set_Visible(false);
			((TextInputBase)val).set_Font(Control.get_Content().get_DefaultFont16());
			_nameInput = val;
			((TextInputBase)_nameInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> e)
			{
				if (!e.get_Value())
				{
					_bookmark.Name = (string.IsNullOrWhiteSpace(((TextInputBase)_nameInput).get_Text()) ? "-" : ((TextInputBase)_nameInput).get_Text());
					((TextInputBase)_nameInput).set_SelectionStart(((TextInputBase)_nameInput).get_Text().Length);
					((TextInputBase)_nameInput).set_SelectionEnd(((TextInputBase)_nameInput).get_Text().Length);
				}
				else
				{
					((TextInputBase)_nameInput).set_SelectionStart(0);
					((TextInputBase)_nameInput).set_SelectionEnd(((TextInputBase)_nameInput).get_Text().Length);
				}
			});
		}

		public override void UpdateContainer(GameTime t)
		{
			if (_sortBtn != null && ((Control)_sortBtn).get_Parent() == null)
			{
				((Control)_sortBtn).set_Parent((Container)(object)BookmarkPanel.Instance);
			}
			if (_removeBtn != null && ((Control)_removeBtn).get_Parent() == null)
			{
				((Control)_removeBtn).set_Parent((Container)(object)BookmarkPanel.Instance);
			}
			if (_nameInput != null && ((Control)_nameInput).get_Parent() == null)
			{
				((Control)_nameInput).set_Parent((Container)(object)BookmarkPanel.Instance);
			}
			SetEditControlBounds();
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			SetEditControlBounds();
			((Control)this).OnMoved(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			SetEditControlBounds();
			((Container)this).OnResized(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!_editing)
			{
				Browser.LoadUrlAsync(_bookmark.URL);
				BookmarkPanel instance = BookmarkPanel.Instance;
				if (instance != null)
				{
					((Control)instance).Hide();
				}
			}
			((MenuItem)this).OnClick(e);
		}

		private void StartChangeOrder(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			_changeOrderAbsoluteStartPointY = ((Rectangle)(ref absoluteBounds)).get_Location().Y;
			_changeOrderStartPointY = ((Control)this).get_Location().Y;
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)StopChangeOrder);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)StopChangeOrder);
			GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnChangingOrder);
			(((Control)this).get_Parent() as BookmarkMenu)?.SeparateChildren();
			_maxChangeOrderY = ((IEnumerable<Control>)((Control)this).get_Parent().get_Children()).Last().get_Bottom();
		}

		private void StopChangeOrder(object sender, EventArgs e)
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)StopChangeOrder);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)StopChangeOrder);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnChangingOrder);
			BookmarkMenu bookmarkMenu = ((Control)this).get_Parent() as BookmarkMenu;
			if (bookmarkMenu != null && bookmarkMenu.ChildrenSeparated)
			{
				this.Dragged?.Invoke(this, new BookmarkDraggedEventArgs(_bookmark, ((Control)this).get_Top()));
			}
		}

		private void OnChangingOrder(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			int num = e.get_MousePosition().Y - _changeOrderAbsoluteStartPointY;
			int num2 = _changeOrderStartPointY + num;
			if (num2 > _maxChangeOrderY)
			{
				num2 = _maxChangeOrderY;
			}
			else if (num2 < 0)
			{
				num2 = 0;
			}
			((Control)this).set_Top(num2);
		}

		private void SetEditControlBounds()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			IconButton sortBtn = _sortBtn;
			if (((sortBtn != null) ? ((Control)sortBtn).get_Parent() : null) == null)
			{
				return;
			}
			IconButton removeBtn = _removeBtn;
			if (((removeBtn != null) ? ((Control)removeBtn).get_Parent() : null) != null)
			{
				TextBox nameInput = _nameInput;
				if (((nameInput != null) ? ((Control)nameInput).get_Parent() : null) != null && _editing)
				{
					((Control)_sortBtn).set_Location(new Point(0, ((Control)this).get_Location().Y + ((Control)this).get_Height() / 2 - ((Control)_sortBtn).get_Height() / 2));
					((Control)_nameInput).set_Location(new Point(((Control)_sortBtn).get_Right(), ((Control)this).get_Location().Y + ((Control)this).get_Height() / 2 - ((Control)_nameInput).get_Height() / 2));
					((Control)_nameInput).set_Width(((Control)this).get_Width() - 70 - ((Control)_nameInput).get_Location().X);
					((Control)_removeBtn).set_Location(new Point(((Control)this).get_Width() - 50, ((Control)this).get_Location().Y + ((Control)this).get_Height() / 2 - ((Control)_removeBtn).get_Height() / 2));
				}
			}
		}

		public void SetEditState(bool val)
		{
			_editing = val;
			if (_editing)
			{
				SetEditControlBounds();
				((TextInputBase)_nameInput).set_Text(base._text);
				base._text = "";
			}
			else
			{
				base._text = _bookmark.Name;
			}
			((Control)_sortBtn).set_Visible(_editing);
			((Control)_nameInput).set_Visible(_editing);
			((Control)_removeBtn).set_Visible(_editing);
		}

		protected override void DisposeControl()
		{
			StopChangeOrder(this, new EventArgs());
			((Control)_sortBtn).Dispose();
			((Control)_nameInput).Dispose();
			((Control)_removeBtn).Dispose();
			this.Removed = null;
			this.Dragged = null;
			((Container)this).DisposeControl();
		}
	}
}
