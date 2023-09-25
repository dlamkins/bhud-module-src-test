using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace felix.BlishEmotes.UI.Controls
{
	internal class ReorderableMenu : Menu
	{
		private ReorderableMenuItem DraggedChild => base.Children.FirstOrDefault((Control child) => (child as ReorderableMenuItem)?.Dragging ?? false) as ReorderableMenuItem;

		private List<ReorderableMenuItem> ReorderableChildren => base.Children.Where((Control child) => child is ReorderableMenuItem).Cast<ReorderableMenuItem>().ToList();

		public event EventHandler<List<ReorderableMenuItem>> Reordered;

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			if (e.ChangedChild is ReorderableMenuItem)
			{
				e.ChangedChild.LeftMouseButtonPressed += ChangedChild_LeftMouseButtonPressed;
				e.ChangedChild.LeftMouseButtonReleased += ChangedChild_LeftMouseButtonReleased;
				base.OnChildAdded(e);
			}
			else
			{
				e.Cancel = true;
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			e.ChangedChild.LeftMouseButtonPressed -= ChangedChild_LeftMouseButtonPressed;
			e.ChangedChild.LeftMouseButtonReleased -= ChangedChild_LeftMouseButtonReleased;
			base.OnChildRemoved(e);
		}

		private void ChangedChild_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			ReorderableMenuItem draggedEntry = DraggedChild;
			if (draggedEntry == null)
			{
				return;
			}
			ReorderableMenuItem entry = sender as ReorderableMenuItem;
			if (entry == null)
			{
				return;
			}
			if (sender != draggedEntry)
			{
				int draggedOnIndex = base.Children.IndexOf(entry);
				if (draggedOnIndex >= 0)
				{
					DragItem(draggedEntry, draggedOnIndex);
				}
			}
			draggedEntry.Dragging = false;
			this.Reordered?.Invoke(this, ReorderableChildren);
		}

		private void ChangedChild_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (DraggedChild == null)
			{
				ReorderableMenuItem entry = sender as ReorderableMenuItem;
				if (entry != null && entry.CanDrag)
				{
					entry.Dragging = true;
					GameService.Input.Mouse.LeftMouseButtonReleased += Game_OnLeftMouseButtonReleased;
				}
			}
		}

		private int GetNewIndex(ReorderableMenuItem draggedOnEntry)
		{
			int draggedOnIndex = base.Children.ToList().IndexOf(draggedOnEntry);
			if (draggedOnIndex == -1)
			{
				return base.Children.Count - 1;
			}
			return draggedOnIndex;
		}

		private int GetCurrentDragOverIndex()
		{
			List<Control> currentHoveredEntries = base.Children.Where(delegate(Control child)
			{
				if (child is ReorderableMenuItem)
				{
					int num = base.RelativeMousePosition.X + base.HorizontalScrollOffset;
					int num2 = base.RelativeMousePosition.Y + base.VerticalScrollOffset;
					if (num >= child.Left && num < child.Right && num2 >= child.Top)
					{
						return num2 < child.Bottom;
					}
					return false;
				}
				return false;
			}).ToList();
			if (currentHoveredEntries.Count == 0)
			{
				return -1;
			}
			return GetNewIndex(currentHoveredEntries.First() as ReorderableMenuItem);
		}

		private void DragItem(ReorderableMenuItem draggingEntry, int newIndex)
		{
			int currentIndex = base.Children.IndexOf(draggingEntry);
			if (newIndex != currentIndex)
			{
				if (base.Children.Remove(draggingEntry) && newIndex > currentIndex)
				{
					newIndex--;
				}
				base.Children.Insert(newIndex, draggingEntry);
				Invalidate();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			ReorderableMenuItem draggingEntry = DraggedChild;
			if (draggingEntry != null)
			{
				int draggedOnIndex = GetCurrentDragOverIndex();
				if (draggedOnIndex != -1)
				{
					DragItem(draggingEntry, draggedOnIndex);
				}
			}
		}

		private void Game_OnLeftMouseButtonReleased(object sender, MouseEventArgs args)
		{
			MouseHandler mh = sender as MouseHandler;
			if (mh == null)
			{
				return;
			}
			ReorderableMenuItem draggedEntry = DraggedChild;
			if (draggedEntry != null)
			{
				if (mh.ActiveControl is ReorderableMenuItem)
				{
					GameService.Input.Mouse.LeftMouseButtonReleased -= Game_OnLeftMouseButtonReleased;
					return;
				}
				draggedEntry.Dragging = false;
				this.Reordered?.Invoke(this, ReorderableChildren);
				GameService.Input.Mouse.LeftMouseButtonReleased -= Game_OnLeftMouseButtonReleased;
			}
		}

		protected override void DisposeControl()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= Game_OnLeftMouseButtonReleased;
			base.DisposeControl();
		}
	}
}
