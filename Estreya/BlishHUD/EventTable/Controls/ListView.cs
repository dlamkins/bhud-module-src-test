using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class ListView<T> : FlowPanel
	{
		public ListView()
			: this()
		{
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Container)this).set_HeightSizingMode((SizingMode)2);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Panel)this).set_CanScroll(true);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonReleased);
		}

		private void Mouse_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			if (!(e.get_ChangedChild() is ListEntry<T>))
			{
				((CancelEventArgs)(object)e).Cancel = true;
				return;
			}
			e.get_ChangedChild().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)ChangedChild_LeftMouseButtonPressed);
			e.get_ChangedChild().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ChangedChild_LeftMouseButtonReleased);
			((FlowPanel)this).OnChildAdded(e);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			e.get_ChangedChild().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)ChangedChild_LeftMouseButtonPressed);
			e.get_ChangedChild().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ChangedChild_LeftMouseButtonReleased);
			((FlowPanel)this).OnChildRemoved(e);
		}

		private void ChangedChild_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			List<Control> draggingEntries = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control child) => (child as ListEntry<T>)?.Dragging ?? false).ToList();
			ListEntry<T> draggedOnEntry = sender as ListEntry<T>;
			if (draggedOnEntry != null)
			{
				int newIndex = GetDragIndex(draggedOnEntry);
				if (newIndex > ((Container)this).get_Children().get_Count())
				{
					newIndex = ((Container)this).get_Children().get_Count() - 1;
				}
				draggingEntries.ForEach(delegate(Control draggingEntry)
				{
					int num = ((Container)this).get_Children().ToList().IndexOf(draggingEntry);
					if (((Container)this).get_Children().Remove(draggingEntry) && newIndex > num)
					{
						newIndex--;
					}
					((Container)this).get_Children().Insert(newIndex, draggingEntry);
				});
				((Control)this).Invalidate();
			}
			draggingEntries.ForEach(delegate(Control child)
			{
				ListEntry<T> listEntry = child as ListEntry<T>;
				if (listEntry != null)
				{
					listEntry.Dragging = false;
				}
			});
		}

		private void ChangedChild_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			ListEntry<T> entry = sender as ListEntry<T>;
			if (entry != null && entry.DragDrop)
			{
				entry.Dragging = true;
			}
		}

		private int GetDragIndex(ListEntry<T> entry)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			bool upperHalf = ((Control)this).get_RelativeMousePosition().Y + ((Container)this).get_VerticalScrollOffset() < ((Control)entry).get_Location().Y + ((Control)entry).get_Size().Y / 2;
			int draggedOnIndex = ((Container)this).get_Children().ToList().IndexOf((Control)(object)entry);
			if (draggedOnIndex == -1)
			{
				return ((Container)this).get_Children().get_Count() - 1;
			}
			if (draggedOnIndex == 0 || upperHalf)
			{
				return draggedOnIndex;
			}
			return draggedOnIndex + 1;
		}

		private int GetCurrentDragOverIndex()
		{
			List<Control> currentHoveredEntries = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control child) => true & (((Control)this).get_RelativeMousePosition().X + ((Container)this).get_HorizontalScrollOffset() >= child.get_Left()) & (((Control)this).get_RelativeMousePosition().X + ((Container)this).get_HorizontalScrollOffset() < child.get_Right()) & (((Control)this).get_RelativeMousePosition().Y + ((Container)this).get_VerticalScrollOffset() >= child.get_Top()) & (((Control)this).get_RelativeMousePosition().Y + ((Container)this).get_VerticalScrollOffset() < child.get_Bottom())).ToList();
			if (currentHoveredEntries.Count == 0)
			{
				return -1;
			}
			ListEntry<T> entry = currentHoveredEntries.First() as ListEntry<T>;
			return GetDragIndex(entry);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			List<Control> draggingEntries = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control child) => (child as ListEntry<T>)?.Dragging ?? false).ToList();
			if (draggingEntries.Count > 0)
			{
				ListEntry<T> entry = draggingEntries.First() as ListEntry<T>;
				RectangleF nameRectangle = default(RectangleF);
				((RectangleF)(ref nameRectangle))._002Ector((float)((Control)this).get_RelativeMousePosition().X, (float)(((Control)this).get_RelativeMousePosition().Y - 20), (float)((Control)entry).get_Width(), (float)((Control)entry).get_Height());
				spriteBatch.DrawStringOnCtrl((Control)(object)this, entry.Text, entry.Font, nameRectangle, entry.TextColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
				int draggedOnIndex = GetCurrentDragOverIndex();
				if (draggedOnIndex != -1)
				{
					bool addedLast = draggedOnIndex == ((Container)this).get_Children().get_Count();
					ListEntry<T> draggedOnEntry = ((Container)this).get_Children().get_Item(addedLast ? (draggedOnIndex - 1) : draggedOnIndex) as ListEntry<T>;
					RectangleF lineRectangle = default(RectangleF);
					((RectangleF)(ref lineRectangle))._002Ector((float)((Control)draggedOnEntry).get_Left(), (float)((addedLast ? ((Control)draggedOnEntry).get_Bottom() : ((Control)draggedOnEntry).get_Top()) - ((Container)this).get_VerticalScrollOffset()), (float)((Control)draggedOnEntry).get_Width(), 2f);
					spriteBatch.DrawLine((Control)(object)this, Textures.get_Pixel(), lineRectangle, Color.get_White());
				}
			}
		}
	}
}
