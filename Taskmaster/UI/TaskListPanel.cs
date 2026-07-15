using System;
using System.Collections.Generic;
using System.Linq;
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
				Rebuild();
			}
		}

		public event Action DataChanged;

		public event Action<TodoTask> TaskContextMenuRequested;

		public event Action<TodoTask> CopyToClipboardRequested;

		public TaskListPanel()
			: this()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 2f));
		}

		public void ShowTab(TodoTab tab)
		{
			if (_tab?.Id != tab?.Id)
			{
				_editingTaskId = null;
			}
			_tab = tab;
			Rebuild();
		}

		public void BeginEdit(TodoTask task)
		{
			_editingTaskId = task.Id;
			_newTaskId = null;
			Rebuild();
		}

		public void AddNewTask(TodoTask task)
		{
			_editingTaskId = task.Id;
			_newTaskId = task.Id;
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
		}

		private void AddRow(TodoTask task, bool isSubtask, DateTime nowUtc, TodoTask parent = null)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			TaskRow taskRow = new TaskRow(task, isSubtask);
			((Control)taskRow).set_Parent((Container)(object)this);
			((Control)taskRow).set_Width(((Container)this).get_ContentRegion().Width);
			taskRow.IsExpanded = _expanded.Contains(task.Id);
			taskRow.Locked = _locked;
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
				if (!_expanded.Remove(task.Id))
				{
					_expanded.Add(task.Id);
				}
				Rebuild();
			};
			row.SaveRequested += delegate
			{
				_activeEditPanel?.Apply();
			};
			row.EditRequested += delegate
			{
				if (_editingTaskId == task.Id)
				{
					_editingTaskId = null;
					_newTaskId = null;
					Rebuild();
				}
				else
				{
					BeginEdit(task);
				}
			};
			row.ContextMenuRequested += delegate
			{
				this.TaskContextMenuRequested?.Invoke(task);
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
			(_activeEditPanel = taskEditPanel).Saved += delegate
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
	}
}
