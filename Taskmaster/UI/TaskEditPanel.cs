using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Taskmaster.Models;
using Taskmaster.UI.Controls;

namespace Taskmaster.UI
{
	public class TaskEditPanel : Panel
	{
		private static readonly Dictionary<ResetScheduleType, string> ScheduleNames = new Dictionary<ResetScheduleType, string>
		{
			{
				ResetScheduleType.Never,
				"Never resets"
			},
			{
				ResetScheduleType.DailyServer,
				"Daily server reset"
			},
			{
				ResetScheduleType.WeeklyServer,
				"Weekly server reset"
			},
			{
				ResetScheduleType.MapBonus,
				"Map bonus rewards reset"
			},
			{
				ResetScheduleType.WvwEu,
				"EU WvW reset"
			},
			{
				ResetScheduleType.WvwNa,
				"NA WvW reset"
			},
			{
				ResetScheduleType.Psna,
				"Pact Supply Network Agent"
			},
			{
				ResetScheduleType.LocalTime,
				"Local time"
			},
			{
				ResetScheduleType.Duration,
				"Duration"
			}
		};

		private const int LabelWidth = 88;

		private const int RowH = 30;

		private const int FieldX = 100;

		private readonly TodoTask _task;

		private readonly TextBox _nameBox;

		private readonly Dropdown _scheduleDropdown;

		private readonly TextBox _localTimeBox;

		private readonly TextBox _durationBox;

		private readonly TextBox _countBox;

		private readonly TextBox _clipboardBox;

		private readonly TextBox _notesBox;

		private readonly Label _subtasksLabel;

		private readonly Panel _subtaskPanel;

		private readonly TextBox _newSubtaskBox;

		private readonly List<TodoTask> _workingSubtasks;

		private readonly List<(Label Label, Control Field, Func<bool> ShouldShow)> _dynamicRows = new List<(Label, Control, Func<bool>)>();

		private int _dynamicRowsStartY;

		private const int SubtaskRowHeight = 24;

		private const int BottomPadding = 16;

		private ResetScheduleType SelectedSchedule => ScheduleNames.First((KeyValuePair<ResetScheduleType, string> kv) => kv.Value == _scheduleDropdown.get_SelectedItem()).Key;

		public event Action Saved;

		public TaskEditPanel(TodoTask task, bool isNew = false)
			: this()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Expected O, but got Unknown
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			_task = task;
			_workingSubtasks = task.Subtasks.ToList();
			((Control)this).set_BackgroundColor(new Color(26, 26, 31, 220));
			int y = 16;
			_nameBox = AddField("Name", ref y, isNew ? "" : task.Name);
			if (isNew)
			{
				((TextInputBase)_nameBox).set_PlaceholderText(task.Name);
			}
			AddLabel("Resets", y);
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(100);
			((Control)val).set_Top(y);
			((Control)val).set_Width(220);
			_scheduleDropdown = val;
			foreach (string name in ScheduleNames.Values)
			{
				_scheduleDropdown.get_Items().Add(name);
			}
			_scheduleDropdown.set_SelectedItem(ScheduleNames[task.Schedule]);
			y = (_dynamicRowsStartY = y + 30);
			DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
			_localTimeBox = AddDynamicField("Local time", task.LocalResetTime.HasValue ? $"{task.LocalResetTime.Value.Hours:00}:{task.LocalResetTime.Value.Minutes:00}" : $"{nowLocal.Hour:00}:{nowLocal.Minute:00}", () => SelectedSchedule == ResetScheduleType.LocalTime);
			_durationBox = AddDynamicField("Cooldown", task.ResetDuration.HasValue ? task.ResetDuration.Value.ToString() : "1:00:00", () => SelectedSchedule == ResetScheduleType.Duration);
			_countBox = AddDynamicField("Count", task.TargetCount.ToString(), () => _workingSubtasks.Count == 0);
			_clipboardBox = AddDynamicField("Clipboard", task.ClipboardContent ?? "");
			_notesBox = AddDynamicField("Notes", task.Notes ?? "");
			_subtasksLabel = AddLabel("Subtasks", y);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(100);
			((Control)val2).set_Width(300);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			_subtaskPanel = val2;
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Left(100);
			((Control)val3).set_Width(220);
			((Control)val3).set_Height(24);
			((TextInputBase)val3).set_PlaceholderText("add subtask, press Enter");
			_newSubtaskBox = val3;
			_newSubtaskBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				if (!string.IsNullOrWhiteSpace(((TextInputBase)_newSubtaskBox).get_Text()))
				{
					_workingSubtasks.Add(new TodoTask
					{
						Name = ((TextInputBase)_newSubtaskBox).get_Text().Trim(),
						Schedule = _task.Schedule
					});
					((TextInputBase)_newSubtaskBox).set_Text("");
					RebuildSubtaskList();
				}
			});
			_scheduleDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				UpdateConditionalVisibility();
			});
			RebuildSubtaskList();
			if (isNew)
			{
				((TextInputBase)_nameBox).set_Focused(true);
			}
		}

		private void LayoutTrailing()
		{
			int subtaskAreaHeight = _workingSubtasks.Count * 24;
			int y = ((Control)_subtaskPanel).get_Top() + subtaskAreaHeight + 4;
			((Control)_newSubtaskBox).set_Top(y);
			int newHeight = ((Control)_newSubtaskBox).get_Bottom() + 16;
			if (((Control)this).get_Height() != newHeight)
			{
				((Control)this).set_Height(newHeight);
			}
		}

		private Label AddLabel(string text, int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(8);
			((Control)val).set_Top(y + 4);
			((Control)val).set_Width(88);
			((Control)val).set_Height(20);
			val.set_Text(text);
			val.set_TextColor(TaskmasterTheme.MutedCream);
			return val;
		}

		private TextBox AddField(string label, ref int y, string value)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			AddLabel(label, y);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(100);
			((Control)val).set_Top(y);
			((Control)val).set_Width(300);
			((Control)val).set_Height(24);
			((TextInputBase)val).set_Text(value);
			y += 30;
			return val;
		}

		private TextBox AddDynamicField(string label, string value, Func<bool> shouldShow = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(8);
			((Control)val).set_Width(88);
			((Control)val).set_Height(20);
			val.set_Text(label);
			val.set_TextColor(TaskmasterTheme.MutedCream);
			Label lbl = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(100);
			((Control)val2).set_Width(300);
			((Control)val2).set_Height(24);
			((TextInputBase)val2).set_Text(value);
			TextBox box = val2;
			_dynamicRows.Add((lbl, (Control)(object)box, shouldShow));
			return box;
		}

		private void UpdateConditionalVisibility()
		{
			int y = _dynamicRowsStartY;
			foreach (var row in _dynamicRows)
			{
				bool show = row.ShouldShow?.Invoke() ?? true;
				((Control)row.Label).set_Visible(show);
				row.Field.set_Visible(show);
				if (show)
				{
					((Control)row.Label).set_Top(y + 4);
					row.Field.set_Top(y);
					y += 30;
				}
			}
			((Control)_subtasksLabel).set_Top(y + 4);
			((Control)_subtaskPanel).set_Top(y);
			LayoutTrailing();
			((Control)this).Invalidate();
		}

		private void RebuildSubtaskList()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)_subtaskPanel).get_Children().ToList())
			{
				item.Dispose();
			}
			int y = 0;
			foreach (TodoTask sub in _workingSubtasks.ToList())
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_subtaskPanel);
				((Control)val).set_Left(0);
				((Control)val).set_Top(y);
				((Control)val).set_Width(240);
				((Control)val).set_Height(22);
				val.set_Text(sub.Name);
				val.set_TextColor(TaskmasterTheme.CreamWhite);
				IconButton iconButton = new IconButton(TaskmasterIcons.Cancel, TaskmasterTheme.IconGlyph);
				((Control)iconButton).set_Parent((Container)(object)_subtaskPanel);
				((Control)iconButton).set_Left(246);
				((Control)iconButton).set_Top(y);
				((Control)iconButton).set_Width(22);
				((Control)iconButton).set_Height(22);
				TodoTask captured = sub;
				((Control)iconButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_workingSubtasks.Remove(captured);
					RebuildSubtaskList();
				});
				y += 24;
			}
			UpdateConditionalVisibility();
		}

		public void Apply()
		{
			_task.Name = (string.IsNullOrWhiteSpace(((TextInputBase)_nameBox).get_Text()) ? _task.Name : ((TextInputBase)_nameBox).get_Text().Trim());
			_task.Schedule = SelectedSchedule;
			if (_task.Schedule == ResetScheduleType.LocalTime && TimeSpan.TryParse(((TextInputBase)_localTimeBox).get_Text(), out var localAt) && localAt >= TimeSpan.Zero && localAt < TimeSpan.FromDays(1.0))
			{
				_task.LocalResetTime = localAt;
			}
			if (_task.Schedule == ResetScheduleType.Duration && TimeSpan.TryParse(((TextInputBase)_durationBox).get_Text(), out var dur) && dur > TimeSpan.Zero)
			{
				_task.ResetDuration = dur;
			}
			_task.EnsureDurationAnchor(DateTime.UtcNow);
			if (int.TryParse(((TextInputBase)_countBox).get_Text(), out var count) && count >= 1 && count <= 999)
			{
				_task.TargetCount = count;
			}
			_task.ClipboardContent = (string.IsNullOrWhiteSpace(((TextInputBase)_clipboardBox).get_Text()) ? null : ((TextInputBase)_clipboardBox).get_Text());
			_task.Notes = (string.IsNullOrWhiteSpace(((TextInputBase)_notesBox).get_Text()) ? null : ((TextInputBase)_notesBox).get_Text());
			_task.Subtasks = _workingSubtasks;
			foreach (TodoTask subtask in _task.Subtasks)
			{
				subtask.Schedule = _task.Schedule;
				subtask.LocalResetTime = _task.LocalResetTime;
				subtask.ResetDuration = _task.ResetDuration;
			}
			if (_task.HasSubtasks)
			{
				_task.TargetCount = 1;
				_task.CurrentCount = 0;
			}
			if (_task.CurrentCount > _task.TargetCount)
			{
				_task.CurrentCount = _task.TargetCount;
			}
			_task.SyncGroupAnchor(DateTime.UtcNow);
			this.Saved?.Invoke();
		}
	}
}
