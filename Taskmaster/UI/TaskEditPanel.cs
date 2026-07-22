using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Taskmaster.Models;
using Taskmaster.Services;
using Taskmaster.UI.Controls;

namespace Taskmaster.UI
{
	public class TaskEditPanel : Panel
	{
		public sealed class Draft
		{
			public string Name;

			public ResetScheduleType Schedule;

			public string LocalTime;

			public string Duration;

			public string Count;

			public string Clipboard;

			public string Notes;

			public string PendingSubtask;

			public List<TodoTask> Subtasks;
		}

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

		private readonly TodoTask _task;

		private readonly TaskmasterSizing _sizing;

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

		public Guid TaskId => _task.Id;

		private int LabelWidth => _sizing.Px(88);

		private int RowH => _sizing.Px(30);

		private int FieldX => LabelWidth + _sizing.Px(12);

		private int SubtaskRowHeight => _sizing.Px(28);

		private int BottomPadding => _sizing.Px(16);

		private ResetScheduleType SelectedSchedule => ScheduleNames.First((KeyValuePair<ResetScheduleType, string> kv) => kv.Value == _scheduleDropdown.get_SelectedItem()).Key;

		public event Action Saved;

		public event Action ContentHeightChanging;

		public TaskEditPanel(TodoTask task, bool isNew, TaskmasterSizing sizing, Draft draft = null)
			: this()
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Expected O, but got Unknown
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Expected O, but got Unknown
			_task = task;
			_sizing = sizing ?? new TaskmasterSizing(1f, 1f);
			_workingSubtasks = (draft?.Subtasks ?? task.Subtasks).OrderBy((TodoTask subtask) => subtask.Order).Select(CloneForEditing).ToList();
			TaskOrdering.Normalize(_workingSubtasks);
			((Control)this).set_BackgroundColor(new Color(26, 26, 31, 220));
			int y = BottomPadding;
			_nameBox = AddField("Name", ref y, draft?.Name ?? (isNew ? "" : task.Name));
			if (isNew)
			{
				((TextInputBase)_nameBox).set_PlaceholderText(task.Name);
			}
			AddLabel("Resets", y);
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(FieldX);
			((Control)val).set_Top(y);
			((Control)val).set_Width(_sizing.Px(220));
			((Control)val).set_Height(_sizing.Px(27));
			_scheduleDropdown = val;
			foreach (string name in ScheduleNames.Values)
			{
				_scheduleDropdown.get_Items().Add(name);
			}
			_scheduleDropdown.set_SelectedItem(ScheduleNames[draft?.Schedule ?? task.Schedule]);
			y = (_dynamicRowsStartY = y + RowH);
			DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
			_localTimeBox = AddDynamicField("Local time", draft?.LocalTime ?? (task.LocalResetTime.HasValue ? $"{task.LocalResetTime.Value.Hours:00}:{task.LocalResetTime.Value.Minutes:00}" : $"{nowLocal.Hour:00}:{nowLocal.Minute:00}"), () => SelectedSchedule == ResetScheduleType.LocalTime);
			_durationBox = AddDynamicField("Cooldown", draft?.Duration ?? (task.ResetDuration.HasValue ? task.ResetDuration.Value.ToString() : "1:00:00"), () => SelectedSchedule == ResetScheduleType.Duration);
			_countBox = AddDynamicField("Count", draft?.Count ?? task.TargetCount.ToString(), () => _workingSubtasks.Count == 0);
			_clipboardBox = AddDynamicField("Clipboard", draft?.Clipboard ?? task.ClipboardContent ?? "");
			_notesBox = AddDynamicField("Notes", draft?.Notes ?? task.Notes ?? "");
			_subtasksLabel = AddLabel("Subtasks", y);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(FieldX);
			((Control)val2).set_Width(_sizing.Px(300));
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			_subtaskPanel = val2;
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Left(FieldX);
			((Control)val3).set_Width(_sizing.Px(220));
			((Control)val3).set_Height(_sizing.Px(24));
			((TextInputBase)val3).set_Font(_sizing.BodyFont);
			((TextInputBase)val3).set_PlaceholderText("add subtask, press Enter");
			((TextInputBase)val3).set_Text(draft?.PendingSubtask ?? "");
			_newSubtaskBox = val3;
			_newSubtaskBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				if (!string.IsNullOrWhiteSpace(((TextInputBase)_newSubtaskBox).get_Text()))
				{
					this.ContentHeightChanging?.Invoke();
					_workingSubtasks.Add(new TodoTask
					{
						Name = ((TextInputBase)_newSubtaskBox).get_Text().Trim(),
						Order = _workingSubtasks.Count,
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

		public Draft CaptureDraft()
		{
			return new Draft
			{
				Name = ((TextInputBase)_nameBox).get_Text(),
				Schedule = SelectedSchedule,
				LocalTime = ((TextInputBase)_localTimeBox).get_Text(),
				Duration = ((TextInputBase)_durationBox).get_Text(),
				Count = ((TextInputBase)_countBox).get_Text(),
				Clipboard = ((TextInputBase)_clipboardBox).get_Text(),
				Notes = ((TextInputBase)_notesBox).get_Text(),
				PendingSubtask = ((TextInputBase)_newSubtaskBox).get_Text(),
				Subtasks = _workingSubtasks.Select(CloneForEditing).ToList()
			};
		}

		private void LayoutTrailing()
		{
			int subtaskAreaHeight = _workingSubtasks.Count * SubtaskRowHeight;
			int y = ((Control)_subtaskPanel).get_Top() + subtaskAreaHeight + _sizing.Px(4);
			((Control)_newSubtaskBox).set_Top(y);
			int newHeight = ((Control)_newSubtaskBox).get_Bottom() + BottomPadding;
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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(_sizing.Px(8));
			((Control)val).set_Top(y + _sizing.Px(4));
			((Control)val).set_Width(LabelWidth);
			((Control)val).set_Height(_sizing.Px(20));
			val.set_Text(text);
			val.set_TextColor(TaskmasterTheme.MutedCream);
			val.set_Font(_sizing.BodyFont);
			return val;
		}

		private TextBox AddField(string label, ref int y, string value)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			AddLabel(label, y);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(FieldX);
			((Control)val).set_Top(y);
			((Control)val).set_Width(_sizing.Px(300));
			((Control)val).set_Height(_sizing.Px(24));
			((TextInputBase)val).set_Text(value);
			((TextInputBase)val).set_Font(_sizing.BodyFont);
			y += RowH;
			return val;
		}

		private TextBox AddDynamicField(string label, string value, Func<bool> shouldShow = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(_sizing.Px(8));
			((Control)val).set_Width(LabelWidth);
			((Control)val).set_Height(_sizing.Px(20));
			val.set_Text(label);
			val.set_TextColor(TaskmasterTheme.MutedCream);
			val.set_Font(_sizing.BodyFont);
			Label lbl = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(FieldX);
			((Control)val2).set_Width(_sizing.Px(300));
			((Control)val2).set_Height(_sizing.Px(24));
			((TextInputBase)val2).set_Text(value);
			((TextInputBase)val2).set_Font(_sizing.BodyFont);
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
					((Control)row.Label).set_Top(y + _sizing.Px(4));
					row.Field.set_Top(y);
					y += RowH;
				}
			}
			((Control)_subtasksLabel).set_Top(y + 4);
			((Control)_subtaskPanel).set_Top(y);
			LayoutTrailing();
			((Control)this).Invalidate();
		}

		private void RebuildSubtaskList()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)_subtaskPanel).get_Children().ToList())
			{
				item.Dispose();
			}
			int y = 0;
			foreach (TodoTask sub in _workingSubtasks.ToList())
			{
				TextBox val = new TextBox();
				((Control)val).set_Parent((Container)(object)_subtaskPanel);
				((Control)val).set_Left(0);
				((Control)val).set_Top(y);
				((Control)val).set_Width(_sizing.Px(118));
				((Control)val).set_Height(_sizing.Px(26));
				((TextInputBase)val).set_Text(sub.Name);
				((TextInputBase)val).set_Font(_sizing.BodyFont);
				((TextInputBase)val).set_PlaceholderText("Subtask name");
				TextBox nameBox = val;
				TextBox val2 = new TextBox();
				((Control)val2).set_Parent((Container)(object)_subtaskPanel);
				((Control)val2).set_Left(_sizing.Px(122));
				((Control)val2).set_Top(y);
				((Control)val2).set_Width(_sizing.Px(126));
				((Control)val2).set_Height(_sizing.Px(26));
				((TextInputBase)val2).set_Text(sub.ClipboardContent ?? "");
				((TextInputBase)val2).set_Font(_sizing.BodyFont);
				((TextInputBase)val2).set_PlaceholderText("waypoint / chat code");
				TextBox clipboardBox = val2;
				IconButton iconButton = new IconButton(TaskmasterIcons.Cancel, TaskmasterTheme.IconGlyph);
				((Control)iconButton).set_Parent((Container)(object)_subtaskPanel);
				((Control)iconButton).set_Left(_sizing.Px(252));
				((Control)iconButton).set_Top(y);
				((Control)iconButton).set_Width(_sizing.Px(26));
				((Control)iconButton).set_Height(_sizing.Px(26));
				iconButton.GlyphSize = _sizing.Px(18);
				TodoTask captured = sub;
				((TextInputBase)nameBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					captured.Name = ((TextInputBase)nameBox).get_Text();
				});
				((TextInputBase)clipboardBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					captured.ClipboardContent = ((TextInputBase)clipboardBox).get_Text();
				});
				((Control)iconButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					this.ContentHeightChanging?.Invoke();
					_workingSubtasks.Remove(captured);
					RebuildSubtaskList();
				});
				y += SubtaskRowHeight;
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
			_task.Subtasks = _workingSubtasks.Where((TodoTask subtask) => !string.IsNullOrWhiteSpace(subtask.Name)).ToList();
			TaskOrdering.Normalize(_task.Subtasks);
			foreach (TodoTask s in _task.Subtasks)
			{
				s.Name = s.Name.Trim();
				s.ClipboardContent = (string.IsNullOrWhiteSpace(s.ClipboardContent) ? null : s.ClipboardContent.Trim());
				s.Schedule = _task.Schedule;
				s.LocalResetTime = _task.LocalResetTime;
				s.ResetDuration = _task.ResetDuration;
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

		private static TodoTask CloneForEditing(TodoTask source)
		{
			return new TodoTask
			{
				Id = source.Id,
				Name = source.Name,
				Order = source.Order,
				Schedule = source.Schedule,
				LocalResetTime = source.LocalResetTime,
				ResetDuration = source.ResetDuration,
				ClipboardContent = source.ClipboardContent,
				Notes = source.Notes,
				TargetCount = source.TargetCount,
				CurrentCount = source.CurrentCount,
				LastCompletedUtc = source.LastCompletedUtc,
				LastActivityUtc = source.LastActivityUtc,
				Subtasks = (source.Subtasks?.Select(CloneForEditing).ToList() ?? new List<TodoTask>())
			};
		}
	}
}
