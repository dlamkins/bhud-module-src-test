using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Taskmaster.Models;
using Taskmaster.Services;

namespace Taskmaster.UI
{
	public class TaskListPanel : FlowPanel
	{
		private TodoTab _tab;

		private readonly HashSet<Guid> _expanded = new HashSet<Guid>();

		private Guid? _editingTaskId;

		private Guid? _newTaskId;

		private bool _hideDone;

		private bool _locked;

		private bool _dragReorderingEnabled;

		private readonly List<TaskRow> _rows = new List<TaskRow>();

		private readonly List<Guid> _selectableTaskIds = new List<Guid>();

		private readonly TaskSelection _selection = new TaskSelection();

		private Guid? _pendingScrollTaskId;

		private int _scrollApplyFrames;

		private float? _pendingScrollDistance;

		private int _scrollRestoreFrames;

		private Scrollbar _scrollbar;

		private TaskmasterSizing _sizing = new TaskmasterSizing(1f, 1f);

		private TaskRow _dragCandidate;

		private TaskRow _dropTarget;

		private Point _dragStart;

		private bool _dragging;

		private bool _dropAfter;

		private bool _selectSingleOnRelease;

		private static readonly FieldInfo PanelScrollbarField = typeof(Panel).GetField("_panelScrollbar", BindingFlags.Instance | BindingFlags.NonPublic);

		private TaskEditPanel _activeEditPanel;

		private TaskEditPanel.Draft _editingDraft;

		public TaskmasterSizing Sizing
		{
			get
			{
				return _sizing;
			}
			set
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				_sizing = value ?? new TaskmasterSizing(1f, 1f);
				((FlowPanel)this).set_ControlPadding(new Vector2(0f, (float)_sizing.Px(2)));
				PreserveScrollPosition();
				Rebuild();
			}
		}

		public bool HideDone
		{
			get
			{
				return _hideDone;
			}
			set
			{
				_hideDone = value;
				Rebuild();
			}
		}

		public bool Locked
		{
			get
			{
				return _locked;
			}
			set
			{
				_locked = value;
				if (_locked)
				{
					_selection.Clear();
				}
				Rebuild();
			}
		}

		public bool DragReorderingEnabled
		{
			get
			{
				return _dragReorderingEnabled;
			}
			set
			{
				_dragReorderingEnabled = value;
				if (!value)
				{
					CancelDrag();
				}
				foreach (TaskRow row in _rows)
				{
					int dragReorderingEnabled;
					if (value)
					{
						TodoTask parentTask = row.ParentTask;
						dragReorderingEnabled = ((parentTask == null || !parentTask.IsManagedPresetParent) ? 1 : 0);
					}
					else
					{
						dragReorderingEnabled = 0;
					}
					row.DragReorderingEnabled = (byte)dragReorderingEnabled != 0;
				}
			}
		}

		public event Action DataChanged;

		public event Action<TodoTask, TodoTask> TaskContextMenuRequested;

		public event Action<TodoTask> CopyToClipboardRequested;

		public TaskListPanel()
			: this()
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, (float)_sizing.Px(2)));
		}

		public void ShowTab(TodoTab tab)
		{
			if (_tab?.Id != tab?.Id)
			{
				_editingTaskId = null;
				_editingDraft = null;
				_selection.Clear();
				_pendingScrollTaskId = null;
				_scrollApplyFrames = 0;
				_pendingScrollDistance = null;
				_scrollRestoreFrames = 0;
			}
			_tab = tab;
			Rebuild();
		}

		public IReadOnlyList<TodoTask> GetSelectedTasks()
		{
			if (_tab == null)
			{
				return Array.Empty<TodoTask>();
			}
			return (from task in _tab.Tasks
				where _selection.IsSelected(task.Id)
				orderby task.Order
				select task).ToList();
		}

		public void ClearSelection()
		{
			_selection.Clear();
			ApplySelectionToRows();
		}

		public void DeleteSubtask(TodoTask parent, TodoTask subtask)
		{
			if (parent?.Subtasks != null && parent.Subtasks.Contains(subtask))
			{
				PreserveScrollDistance();
				parent.Subtasks.Remove(subtask);
				DateTime nowUtc = DateTime.UtcNow;
				if (parent.HasSubtasks)
				{
					parent.SyncGroupAnchor(nowUtc);
				}
				else
				{
					parent.CurrentCount = 0;
					parent.LastCompletedUtc = null;
					parent.LastActivityUtc = null;
					parent.EnsureDurationAnchor(nowUtc);
				}
				AfterMutation();
			}
		}

		public void BeginEdit(TodoTask task)
		{
			if (task != null && !task.IsManagedPreset)
			{
				PreserveScrollDistance();
				_editingTaskId = task.Id;
				_newTaskId = null;
				_editingDraft = null;
				Rebuild();
			}
		}

		public void AddNewTask(TodoTask task)
		{
			_editingTaskId = task.Id;
			_newTaskId = task.Id;
			_editingDraft = null;
			_pendingScrollTaskId = task.Id;
			_scrollApplyFrames = 5;
			Rebuild();
		}

		public void ExpandTask(TodoTask task)
		{
			if (task != null && task.HasSubtasks)
			{
				_expanded.Add(task.Id);
				_pendingScrollTaskId = task.Id;
				_scrollApplyFrames = 5;
				Rebuild();
				_selection.Select(task.Id, _selectableTaskIds, extendRange: false, toggle: false);
				ApplySelectionToRows();
			}
		}

		public void RefreshCountdowns(DateTime nowUtc)
		{
			foreach (TaskRow row in _rows)
			{
				row.RefreshDisplay(nowUtc);
			}
		}

		public void Rebuild()
		{
			CancelDrag();
			if (_activeEditPanel != null && _editingTaskId.HasValue && _activeEditPanel.TaskId == _editingTaskId.Value)
			{
				_editingDraft = _activeEditPanel.CaptureDraft();
			}
			foreach (Control item in ((Container)this).get_Children().ToList())
			{
				item.Dispose();
			}
			_rows.Clear();
			_selectableTaskIds.Clear();
			_activeEditPanel = null;
			if (_tab == null)
			{
				return;
			}
			DateTime nowUtc = DateTime.UtcNow;
			foreach (TodoTask task in _tab.Tasks.OrderBy((TodoTask t) => t.Order).ToList())
			{
				Guid id;
				Guid? editingTaskId;
				if (_hideDone && task.IsDone)
				{
					id = task.Id;
					editingTaskId = _editingTaskId;
					if (id != editingTaskId)
					{
						continue;
					}
				}
				_selectableTaskIds.Add(task.Id);
				AddRow(task, isSubtask: false, nowUtc);
				if (task.HasSubtasks && _expanded.Contains(task.Id))
				{
					foreach (TodoTask sub in task.Subtasks.OrderBy((TodoTask subtask) => subtask.Order))
					{
						if (!_hideDone || !sub.IsDone)
						{
							AddRow(sub, isSubtask: true, nowUtc, task);
						}
					}
				}
				id = task.Id;
				editingTaskId = _editingTaskId;
				if (id == editingTaskId)
				{
					id = task.Id;
					editingTaskId = _newTaskId;
					AddEditPanel(task, id == editingTaskId);
				}
			}
			_selection.Retain(_selectableTaskIds);
			ApplySelectionToRows();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (_dragging)
			{
				AutoScrollDuringDrag();
			}
			if (_pendingScrollDistance.HasValue && _scrollRestoreFrames > 0)
			{
				Scrollbar scrollbar = GetScrollbar();
				if (scrollbar != null)
				{
					scrollbar.set_ScrollDistance(_pendingScrollDistance.Value);
				}
				_scrollRestoreFrames--;
				if (_scrollRestoreFrames == 0)
				{
					_pendingScrollDistance = null;
				}
			}
			else if (_pendingScrollTaskId.HasValue && _scrollApplyFrames > 0)
			{
				_scrollApplyFrames--;
				ScrollPendingTaskIntoView();
				if (_scrollApplyFrames == 0)
				{
					_pendingScrollTaskId = null;
				}
			}
		}

		private void AddRow(TodoTask task, bool isSubtask, DateTime nowUtc, TodoTask parent = null)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			TaskRow taskRow = new TaskRow(task, isSubtask, _sizing);
			((Control)taskRow).set_Parent((Container)(object)this);
			((Control)taskRow).set_Width(((Container)this).get_ContentRegion().Width);
			taskRow.IsExpanded = _expanded.Contains(task.Id);
			taskRow.Locked = _locked;
			taskRow.CanEdit = !task.IsManagedPreset;
			taskRow.CanOpenContextMenu = !isSubtask || !(parent?.IsManagedPresetParent ?? false);
			taskRow.DragReorderingEnabled = _dragReorderingEnabled && (!isSubtask || !(parent?.IsManagedPresetParent ?? false));
			taskRow.ParentTask = parent;
			taskRow.IsSelected = !isSubtask && _selection.IsSelected(task.Id);
			Guid id = task.Id;
			Guid? editingTaskId = _editingTaskId;
			taskRow.IsEditing = id == editingTaskId;
			taskRow.CountdownAnchor = parent;
			TaskRow row = taskRow;
			row.RefreshDisplay(nowUtc);
			_rows.Add(row);
			row.ToggleRequested += delegate
			{
				if (task.IsDone)
				{
					task.UncheckAll();
				}
				else
				{
					task.Increment(DateTime.UtcNow);
				}
				(parent ?? task).SyncGroupAnchor(DateTime.UtcNow);
				AfterMutation();
			};
			row.ExpandToggled += delegate
			{
				PreserveScrollDistance();
				if (!_expanded.Remove(task.Id))
				{
					_expanded.Add(task.Id);
				}
				Rebuild();
			};
			row.SaveRequested += delegate
			{
				PreserveScrollDistance();
				_activeEditPanel?.Apply();
			};
			row.EditRequested += delegate
			{
				if (_editingTaskId == task.Id)
				{
					PreserveScrollDistance();
					_editingTaskId = null;
					_newTaskId = null;
					_editingDraft = null;
					Rebuild();
				}
				else
				{
					BeginEdit(task);
				}
			};
			row.SelectionRequested += delegate(bool extendRange, bool toggle)
			{
				_selection.Select(task.Id, _selectableTaskIds, extendRange, toggle);
				ApplySelectionToRows();
			};
			row.ContextMenuRequested += delegate
			{
				if (isSubtask)
				{
					_selection.Clear();
				}
				else
				{
					_selection.SelectForContext(task.Id);
				}
				ApplySelectionToRows();
				this.TaskContextMenuRequested?.Invoke(task, parent);
			};
			row.CopyRequested += delegate
			{
				this.CopyToClipboardRequested?.Invoke(task);
				row.FlashCopied();
			};
			row.DragCandidateRequested += delegate(bool selectSingleOnRelease)
			{
				BeginDrag(row, selectSingleOnRelease);
			};
		}

		private void AddEditPanel(TodoTask task, bool isNew = false)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (!task.IsManagedPreset)
			{
				TaskEditPanel taskEditPanel = new TaskEditPanel(task, isNew, _sizing, _editingDraft);
				((Control)taskEditPanel).set_Parent((Container)(object)this);
				((Control)taskEditPanel).set_Width(((Container)this).get_ContentRegion().Width);
				TaskEditPanel edit = (_activeEditPanel = taskEditPanel);
				edit.ContentHeightChanging += PreserveScrollDistance;
				edit.Saved += delegate
				{
					_editingTaskId = null;
					_newTaskId = null;
					_editingDraft = null;
					AfterMutation();
				};
			}
		}

		private void AfterMutation()
		{
			this.DataChanged?.Invoke();
			Rebuild();
		}

		private void ApplySelectionToRows()
		{
			foreach (TaskRow row in _rows)
			{
				row.IsSelected = _selection.IsSelected(row.Task.Id);
			}
		}

		public void PreserveScrollPosition()
		{
			Scrollbar scrollbar = GetScrollbar();
			if (scrollbar != null)
			{
				_pendingScrollDistance = scrollbar.get_ScrollDistance();
				_scrollRestoreFrames = 5;
			}
		}

		private void PreserveScrollDistance()
		{
			PreserveScrollPosition();
		}

		private Scrollbar GetScrollbar()
		{
			_scrollbar = (Scrollbar)((_scrollbar != null && ((Control)_scrollbar).get_Parent() != null) ? ((object)_scrollbar) : ((object)/*isinst with value type is only supported in some contexts*/));
			return _scrollbar;
		}

		private void BeginDrag(TaskRow row, bool selectSingleOnRelease)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (_dragReorderingEnabled && !_locked && row != null)
			{
				CancelDrag();
				_dragCandidate = row;
				_selectSingleOnRelease = selectSingleOnRelease;
				_dragStart = GameService.Input.get_Mouse().get_Position();
				GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
				GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			}
		}

		private void OnGlobalMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (_dragCandidate != null)
			{
				Point mouse = GameService.Input.get_Mouse().get_Position();
				if (!_dragging && Math.Abs(mouse.X - _dragStart.X) + Math.Abs(mouse.Y - _dragStart.Y) >= _sizing.Px(6))
				{
					_dragging = true;
				}
				if (_dragging)
				{
					UpdateDropTarget();
				}
			}
		}

		private void UpdateDropTarget()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			if (!_dragging || _dragCandidate == null)
			{
				return;
			}
			Point mouse = GameService.Input.get_Mouse().get_Position();
			List<TaskRow> eligibleRows = _rows.Where((TaskRow row) => row != _dragCandidate && row.ParentTask == _dragCandidate.ParentTask).ToList();
			if (eligibleRows.Count == 0)
			{
				SetDropTarget(null, after: false);
				return;
			}
			TaskRow target = eligibleRows.OrderBy(delegate(TaskRow row)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				int y2 = mouse.Y;
				Rectangle absoluteBounds2 = ((Control)row).get_AbsoluteBounds();
				return Math.Abs(y2 - (((Rectangle)(ref absoluteBounds2)).get_Top() + ((Control)row).get_Height() / 2));
			}).First();
			int y = mouse.Y;
			Rectangle absoluteBounds = ((Control)target).get_AbsoluteBounds();
			bool after = y >= ((Rectangle)(ref absoluteBounds)).get_Top() + ((Control)target).get_Height() / 2;
			SetDropTarget(target, after);
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			TaskRow draggedRow = _dragCandidate;
			TaskRow targetRow = _dropTarget;
			bool dragging = _dragging;
			bool insertAfter = _dropAfter;
			bool selectSingleOnRelease = _selectSingleOnRelease;
			CancelDrag();
			if (!dragging)
			{
				if (selectSingleOnRelease && draggedRow != null)
				{
					_selection.Select(draggedRow.Task.Id, _selectableTaskIds, extendRange: false, toggle: false);
					ApplySelectionToRows();
				}
			}
			else
			{
				if (draggedRow == null || targetRow == null)
				{
					return;
				}
				IList<TodoTask> list;
				if (draggedRow.ParentTask != null)
				{
					IList<TodoTask> subtasks = draggedRow.ParentTask.Subtasks;
					list = subtasks;
				}
				else
				{
					IList<TodoTask> subtasks = _tab?.Tasks;
					list = subtasks;
				}
				IList<TodoTask> siblings = list;
				if (siblings == null)
				{
					return;
				}
				int sourceIndex = TaskOrdering.OrderedIndexOf(siblings, draggedRow.Task);
				int targetIndex = TaskOrdering.OrderedIndexOf(siblings, targetRow.Task);
				if (sourceIndex >= 0 && targetIndex >= 0)
				{
					if (insertAfter)
					{
						targetIndex++;
					}
					if (sourceIndex < targetIndex)
					{
						targetIndex--;
					}
					PreserveScrollPosition();
					if (TaskOrdering.MoveToIndex(siblings, draggedRow.Task, targetIndex))
					{
						AfterMutation();
					}
				}
			}
		}

		private void SetDropTarget(TaskRow target, bool after)
		{
			if (_dropTarget != null)
			{
				_dropTarget.IsDropTarget = false;
			}
			_dropTarget = target;
			_dropAfter = after;
			if (_dropTarget != null)
			{
				_dropTarget.DropAfter = after;
				_dropTarget.IsDropTarget = true;
			}
		}

		private void AutoScrollDuringDrag()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			Scrollbar scrollbar = GetScrollbar();
			if (scrollbar != null)
			{
				int y = GameService.Input.get_Mouse().get_Position().Y;
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int relativeY = y - ((Rectangle)(ref absoluteBounds)).get_Top();
				int edge = _sizing.Px(34);
				if (relativeY < edge)
				{
					scrollbar.set_ScrollDistance(Math.Max(0f, scrollbar.get_ScrollDistance() - 0.018f));
				}
				else if (relativeY > ((Control)this).get_Height() - edge)
				{
					scrollbar.set_ScrollDistance(Math.Min(1f, scrollbar.get_ScrollDistance() + 0.018f));
				}
				UpdateDropTarget();
			}
		}

		private void CancelDrag()
		{
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			SetDropTarget(null, after: false);
			_dragCandidate = null;
			_dragging = false;
			_selectSingleOnRelease = false;
		}

		private void ScrollPendingTaskIntoView()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (!_pendingScrollTaskId.HasValue)
			{
				return;
			}
			TaskRow row = _rows.FirstOrDefault((TaskRow candidate) => candidate.Task.Id == _pendingScrollTaskId.Value);
			if (row == null)
			{
				return;
			}
			Control target = (Control)((_activeEditPanel != null && _editingTaskId == _pendingScrollTaskId) ? ((object)_activeEditPanel) : ((object)row));
			int viewportHeight = ((Container)this).get_ContentRegion().Height;
			int contentHeight = (from child in (IEnumerable<Control>)((Container)this).get_Children()
				where child.get_Visible() && !(child is Scrollbar)
				select child.get_Bottom()).DefaultIfEmpty(viewportHeight).Max();
			int scrollableRange = Math.Max(0, contentHeight - viewportHeight);
			if (scrollableRange != 0)
			{
				Scrollbar scrollbar = GetScrollbar();
				if (scrollbar != null)
				{
					int targetOffset = Math.Max(0, Math.Min(target.get_Bottom() - viewportHeight + 8, scrollableRange));
					scrollbar.set_ScrollDistance((float)targetOffset / (float)scrollableRange);
				}
			}
		}

		protected override void DisposeControl()
		{
			CancelDrag();
			((FlowPanel)this).DisposeControl();
		}
	}
}
