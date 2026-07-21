using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Taskmaster.Models;

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

		private readonly List<TaskRow> _rows = new List<TaskRow>();

		private readonly List<Guid> _selectableTaskIds = new List<Guid>();

		private readonly TaskSelection _selection = new TaskSelection();

		private Guid? _pendingScrollTaskId;

		private int _scrollApplyFrames;

		private float? _pendingScrollDistance;

		private int _scrollRestoreFrames;

		private Scrollbar _scrollbar;

		private static readonly FieldInfo PanelScrollbarField = typeof(Panel).GetField("_panelScrollbar", BindingFlags.Instance | BindingFlags.NonPublic);

		private TaskEditPanel _activeEditPanel;

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

		public event Action DataChanged;

		public event Action<TodoTask, TodoTask> TaskContextMenuRequested;

		public event Action<TodoTask> CopyToClipboardRequested;

		public TaskListPanel()
			: this()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 2f));
		}

		public void ShowTab(TodoTab tab)
		{
			if (_tab?.Id != tab?.Id)
			{
				_editingTaskId = null;
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
			PreserveScrollDistance();
			_editingTaskId = task.Id;
			_newTaskId = null;
			Rebuild();
		}

		public void AddNewTask(TodoTask task)
		{
			_editingTaskId = task.Id;
			_newTaskId = task.Id;
			_pendingScrollTaskId = task.Id;
			_scrollApplyFrames = 5;
			Rebuild();
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
					foreach (TodoTask sub in task.Subtasks)
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
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			TaskRow taskRow = new TaskRow(task, isSubtask);
			((Control)taskRow).set_Parent((Container)(object)this);
			((Control)taskRow).set_Width(((Container)this).get_ContentRegion().Width);
			taskRow.IsExpanded = _expanded.Contains(task.Id);
			taskRow.Locked = _locked;
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
		}

		private void AddEditPanel(TodoTask task, bool isNew = false)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			TaskEditPanel taskEditPanel = new TaskEditPanel(task, isNew);
			((Control)taskEditPanel).set_Parent((Container)(object)this);
			((Control)taskEditPanel).set_Width(((Container)this).get_ContentRegion().Width);
			TaskEditPanel edit = (_activeEditPanel = taskEditPanel);
			edit.ContentHeightChanging += PreserveScrollDistance;
			edit.Saved += delegate
			{
				_editingTaskId = null;
				_newTaskId = null;
				AfterMutation();
			};
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

		private void PreserveScrollDistance()
		{
			Scrollbar scrollbar = GetScrollbar();
			if (scrollbar != null)
			{
				_pendingScrollDistance = scrollbar.get_ScrollDistance();
				_scrollRestoreFrames = 5;
			}
		}

		private Scrollbar GetScrollbar()
		{
			_scrollbar = (Scrollbar)((_scrollbar != null && ((Control)_scrollbar).get_Parent() != null) ? ((object)_scrollbar) : ((object)/*isinst with value type is only supported in some contexts*/));
			return _scrollbar;
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
	}
}
