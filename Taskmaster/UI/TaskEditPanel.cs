using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
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

		private sealed class SubtaskEditorRow
		{
			public TodoTask Subtask;

			public TextBox NameBox;

			public TextBox ClipboardBox;

			public Checkbox OptionalCheckbox;

			public IconButton DeleteButton;
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

		private readonly bool _isNew;

		private readonly TaskmasterSizing _sizing;

		private readonly List<TodoTask> _workingSubtasks;

		private readonly List<SubtaskEditorRow> _subtaskRows = new List<SubtaskEditorRow>();

		private readonly Panel _topBorder;

		private readonly Label _detailsHeading;

		private readonly Label _nameLabel;

		private readonly TextBox _nameBox;

		private readonly Label _scheduleLabel;

		private readonly Dropdown _scheduleDropdown;

		private readonly Label _localTimeLabel;

		private readonly TextBox _localTimeBox;

		private readonly Label _durationLabel;

		private readonly TextBox _durationBox;

		private readonly Label _countLabel;

		private readonly TextBox _countBox;

		private readonly Label _clipboardLabel;

		private readonly TextBox _clipboardBox;

		private readonly Label _notesLabel;

		private readonly TextBox _notesBox;

		private readonly Panel _subtasksDivider;

		private readonly Label _subtasksHeading;

		private readonly Label _subtaskNameHeader;

		private readonly Label _subtaskClipboardHeader;

		private readonly Label _subtaskOptionalHeader;

		private readonly Panel _subtaskPanel;

		private readonly Label _emptySubtasksLabel;

		private readonly TextBox _newSubtaskBox;

		private readonly StandardButton _addSubtaskButton;

		private readonly Label _validationLabel;

		private readonly Panel _footerDivider;

		private readonly StandardButton _cancelButton;

		private readonly StandardButton _saveButton;

		private bool _isConstructed;

		public Guid TaskId => _task.Id;

		private int PanelPadding => _sizing.Px(12);

		private int ScrollbarGutter => _sizing.Px(20);

		private int FieldHeight => _sizing.Px(26);

		private int FieldLabelHeight => _sizing.Px(18);

		private int RowGap => _sizing.Px(10);

		private int SectionGap => _sizing.Px(14);

		private int SubtaskRowHeight => _sizing.Px(30);

		private int FooterHeight => _sizing.Px(30);

		private ResetScheduleType SelectedSchedule => ScheduleNames.First((KeyValuePair<ResetScheduleType, string> kv) => kv.Value == _scheduleDropdown.get_SelectedItem()).Key;

		public event Action Saved;

		public event Action Cancelled;

		public event Action ContentHeightChanging;

		public TaskEditPanel(TodoTask task, bool isNew, TaskmasterSizing sizing, Draft draft = null)
			: this()
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Expected O, but got Unknown
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Expected O, but got Unknown
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Expected O, but got Unknown
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Expected O, but got Unknown
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Expected O, but got Unknown
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Expected O, but got Unknown
			_task = task;
			_isNew = isNew;
			_sizing = sizing ?? new TaskmasterSizing(1f, 1f);
			_workingSubtasks = (draft?.Subtasks ?? task.Subtasks).OrderBy((TodoTask subtask) => subtask.Order).Select(CloneForEditing).ToList();
			TaskOrdering.Normalize(_workingSubtasks);
			((Control)this).set_BackgroundColor(TaskmasterTheme.EditorFill);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(1);
			((Control)val).set_BackgroundColor(TaskmasterTheme.EditorBorder);
			_topBorder = val;
			_detailsHeading = CreateLabel(isNew ? "New task" : "Edit task", _sizing.HeadingFont, TaskmasterTheme.CreamWhite);
			_nameLabel = CreateFieldLabel("Task name");
			_nameBox = CreateTextBox(draft?.Name ?? (isNew ? "" : task.Name));
			if (isNew)
			{
				((TextInputBase)_nameBox).set_PlaceholderText(task.Name);
			}
			_scheduleLabel = CreateFieldLabel("Reset schedule");
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(FieldHeight);
			_scheduleDropdown = val2;
			ResetScheduleType selectedSchedule = draft?.Schedule ?? task.Schedule;
			foreach (KeyValuePair<ResetScheduleType, string> schedule in ScheduleNames)
			{
				if (schedule.Key != ResetScheduleType.Psna || selectedSchedule == ResetScheduleType.Psna)
				{
					_scheduleDropdown.get_Items().Add(schedule.Value);
				}
			}
			_scheduleDropdown.set_SelectedItem(ScheduleNames[selectedSchedule]);
			DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
			_localTimeLabel = CreateFieldLabel("Local time");
			_localTimeBox = CreateTextBox(draft?.LocalTime ?? (task.LocalResetTime.HasValue ? $"{task.LocalResetTime.Value.Hours:00}:{task.LocalResetTime.Value.Minutes:00}" : $"{nowLocal.Hour:00}:{nowLocal.Minute:00}"));
			((TextInputBase)_localTimeBox).set_PlaceholderText("19:30");
			_durationLabel = CreateFieldLabel("Cooldown");
			_durationBox = CreateTextBox(draft?.Duration ?? (task.ResetDuration.HasValue ? task.ResetDuration.Value.ToString() : "1:00:00"));
			((TextInputBase)_durationBox).set_PlaceholderText("1:00:00");
			_countLabel = CreateFieldLabel("Target count");
			_countBox = CreateTextBox(draft?.Count ?? task.TargetCount.ToString());
			((TextInputBase)_countBox).set_PlaceholderText("1");
			_clipboardLabel = CreateFieldLabel("Clipboard");
			_clipboardBox = CreateTextBox(draft?.Clipboard ?? task.ClipboardContent ?? "");
			((TextInputBase)_clipboardBox).set_PlaceholderText("Waypoint, chat code, or text to copy");
			_notesLabel = CreateFieldLabel("Notes");
			_notesBox = CreateTextBox(draft?.Notes ?? task.Notes ?? "");
			((TextInputBase)_notesBox).set_PlaceholderText("Shown as a tooltip on the task");
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Height(1);
			((Control)val3).set_BackgroundColor(TaskmasterTheme.SubtleBorder);
			_subtasksDivider = val3;
			_subtasksHeading = CreateLabel("Subtasks", _sizing.HeadingFont, TaskmasterTheme.CreamWhite);
			_subtaskNameHeader = CreateColumnHeader("Name");
			_subtaskClipboardHeader = CreateColumnHeader("Clipboard");
			_subtaskOptionalHeader = CreateColumnHeader("Optional");
			_subtaskOptionalHeader.set_HorizontalAlignment((HorizontalAlignment)1);
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)this);
			_subtaskPanel = val4;
			_emptySubtasksLabel = CreateLabel("No subtasks yet", _sizing.BodyFont, TaskmasterTheme.DimText);
			_newSubtaskBox = CreateTextBox(draft?.PendingSubtask ?? "");
			((TextInputBase)_newSubtaskBox).set_PlaceholderText("Subtask name");
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Add subtask");
			_addSubtaskButton = val5;
			_validationLabel = CreateLabel("", _sizing.SmallFont, TaskmasterTheme.Danger);
			((Control)_validationLabel).set_Visible(false);
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Height(1);
			((Control)val6).set_BackgroundColor(TaskmasterTheme.SubtleBorder);
			_footerDivider = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Cancel");
			_cancelButton = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(isNew ? "Add task" : "Save changes");
			_saveButton = val8;
			_scheduleDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				ClearValidation();
				this.ContentHeightChanging?.Invoke();
				Relayout();
			});
			_newSubtaskBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				AddPendingSubtask();
			});
			((Control)_addSubtaskButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddPendingSubtask();
			});
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.Cancelled?.Invoke();
			});
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Apply();
			});
			TextBox[] array = (TextBox[])(object)new TextBox[7] { _nameBox, _localTimeBox, _durationBox, _countBox, _clipboardBox, _notesBox, _newSubtaskBox };
			for (int i = 0; i < array.Length; i++)
			{
				((TextInputBase)array[i]).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					ClearValidation();
				});
			}
			RebuildSubtaskList();
			_isConstructed = true;
			Relayout();
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

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			if (_isConstructed)
			{
				Relayout();
			}
		}

		private Label CreateLabel(string text, BitmapFont font, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(text);
			val.set_TextColor(color);
			val.set_Font(font);
			return val;
		}

		private Label CreateFieldLabel(string text)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return CreateLabel(text, _sizing.SmallFont, TaskmasterTheme.MutedCream);
		}

		private Label CreateColumnHeader(string text)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return CreateLabel(text, _sizing.SmallFont, TaskmasterTheme.DimText);
		}

		private TextBox CreateTextBox(string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(FieldHeight);
			((TextInputBase)val).set_Text(text);
			((TextInputBase)val).set_Font(_sizing.BodyFont);
			return val;
		}

		private void Relayout()
		{
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			if (_isConstructed)
			{
				int innerLeft = PanelPadding;
				int innerWidth = Math.Max(_sizing.Px(320), ((Control)this).get_Width() - PanelPadding * 2 - ScrollbarGutter);
				int y = PanelPadding;
				SetBounds((Control)(object)_topBorder, 0, 0, ((Control)this).get_Width(), 1);
				SetBounds((Control)(object)_detailsHeading, innerLeft, y, innerWidth, _sizing.Px(22));
				y += _sizing.Px(28);
				LayoutField(_nameLabel, (Control)(object)_nameBox, innerLeft, y, innerWidth);
				y += FieldLabelHeight + FieldHeight + RowGap;
				bool showLocalTime = SelectedSchedule == ResetScheduleType.LocalTime;
				bool showDuration = SelectedSchedule == ResetScheduleType.Duration;
				int fieldGap = _sizing.Px(12);
				int scheduleWidth = Math.Min(_sizing.Px(360), Math.Max(_sizing.Px(240), (int)((float)innerWidth * 0.58f)));
				int detailWidth = Math.Max(_sizing.Px(110), innerWidth - scheduleWidth - fieldGap);
				LayoutField(_scheduleLabel, (Control)(object)_scheduleDropdown, innerLeft, y, scheduleWidth);
				((Control)_localTimeLabel).set_Visible(showLocalTime);
				((Control)_localTimeBox).set_Visible(showLocalTime);
				((Control)_durationLabel).set_Visible(showDuration);
				((Control)_durationBox).set_Visible(showDuration);
				if (showLocalTime)
				{
					LayoutField(_localTimeLabel, (Control)(object)_localTimeBox, innerLeft + scheduleWidth + fieldGap, y, detailWidth);
				}
				if (showDuration)
				{
					LayoutField(_durationLabel, (Control)(object)_durationBox, innerLeft + scheduleWidth + fieldGap, y, detailWidth);
				}
				y += FieldLabelHeight + FieldHeight + RowGap;
				bool showCount = _workingSubtasks.Count == 0;
				((Control)_countLabel).set_Visible(showCount);
				((Control)_countBox).set_Visible(showCount);
				int countWidth = (showCount ? _sizing.Px(92) : 0);
				int clipboardLeft = (showCount ? (innerLeft + countWidth + fieldGap) : innerLeft);
				int clipboardWidth = (showCount ? (innerWidth - countWidth - fieldGap) : innerWidth);
				if (showCount)
				{
					LayoutField(_countLabel, (Control)(object)_countBox, innerLeft, y, countWidth);
				}
				LayoutField(_clipboardLabel, (Control)(object)_clipboardBox, clipboardLeft, y, clipboardWidth);
				y += FieldLabelHeight + FieldHeight + RowGap;
				LayoutField(_notesLabel, (Control)(object)_notesBox, innerLeft, y, innerWidth);
				y += FieldLabelHeight + FieldHeight + SectionGap;
				SetBounds((Control)(object)_subtasksDivider, innerLeft, y, innerWidth, 1);
				y += SectionGap;
				SetBounds((Control)(object)_subtasksHeading, innerLeft, y, innerWidth, _sizing.Px(22));
				y += _sizing.Px(28);
				int columnGap = _sizing.Px(6);
				int optionalWidth = _sizing.Px(72);
				int deleteWidth = _sizing.Px(28);
				int flexibleWidth = Math.Max(_sizing.Px(220), innerWidth - optionalWidth - deleteWidth - columnGap * 3);
				int nameWidth = Math.Max(_sizing.Px(110), (int)((float)flexibleWidth * 0.52f));
				int subtaskClipboardWidth = flexibleWidth - nameWidth;
				int clipboardX = innerLeft + nameWidth + columnGap;
				int optionalX = clipboardX + subtaskClipboardWidth + columnGap;
				bool showColumnHeaders = _subtaskRows.Count > 0;
				((Control)_subtaskNameHeader).set_Visible(showColumnHeaders);
				((Control)_subtaskClipboardHeader).set_Visible(showColumnHeaders);
				((Control)_subtaskOptionalHeader).set_Visible(showColumnHeaders);
				if (showColumnHeaders)
				{
					SetBounds((Control)(object)_subtaskNameHeader, innerLeft, y, nameWidth, FieldLabelHeight);
					SetBounds((Control)(object)_subtaskClipboardHeader, clipboardX, y, subtaskClipboardWidth, FieldLabelHeight);
					SetBounds((Control)(object)_subtaskOptionalHeader, optionalX, y, optionalWidth, FieldLabelHeight);
					y += FieldLabelHeight;
				}
				((Control)_subtaskPanel).set_Location(new Point(innerLeft, y));
				((Control)_subtaskPanel).set_Size(new Point(innerWidth, _subtaskRows.Count * SubtaskRowHeight));
				for (int index = 0; index < _subtaskRows.Count; index++)
				{
					SubtaskEditorRow subtaskEditorRow = _subtaskRows[index];
					int rowY = index * SubtaskRowHeight;
					SetBounds((Control)(object)subtaskEditorRow.NameBox, 0, rowY, nameWidth, FieldHeight);
					SetBounds((Control)(object)subtaskEditorRow.ClipboardBox, nameWidth + columnGap, rowY, subtaskClipboardWidth, FieldHeight);
					SetBounds((Control)(object)subtaskEditorRow.OptionalCheckbox, nameWidth + columnGap + subtaskClipboardWidth + columnGap + (optionalWidth - _sizing.Px(24)) / 2, rowY, _sizing.Px(24), FieldHeight);
					SetBounds((Control)(object)subtaskEditorRow.DeleteButton, nameWidth + columnGap + subtaskClipboardWidth + columnGap + optionalWidth + columnGap, rowY, deleteWidth, FieldHeight);
				}
				y += ((Control)_subtaskPanel).get_Height();
				((Control)_emptySubtasksLabel).set_Visible(_subtaskRows.Count == 0);
				if (((Control)_emptySubtasksLabel).get_Visible())
				{
					SetBounds((Control)(object)_emptySubtasksLabel, innerLeft, y, innerWidth, _sizing.Px(22));
					y += _sizing.Px(26);
				}
				int addButtonWidth = _sizing.Px(104);
				SetBounds((Control)(object)_newSubtaskBox, innerLeft, y, innerWidth - addButtonWidth - columnGap, FieldHeight);
				SetBounds((Control)(object)_addSubtaskButton, innerLeft + innerWidth - addButtonWidth, y, addButtonWidth, FieldHeight);
				y += FieldHeight + RowGap;
				if (((Control)_validationLabel).get_Visible())
				{
					SetBounds((Control)(object)_validationLabel, innerLeft, y, innerWidth, _sizing.Px(22));
					y += _sizing.Px(26);
				}
				SetBounds((Control)(object)_footerDivider, innerLeft, y, innerWidth, 1);
				y += SectionGap;
				int cancelWidth = _sizing.Px(84);
				int saveWidth = _sizing.Px(_isNew ? 92 : 112);
				SetBounds((Control)(object)_cancelButton, innerLeft + innerWidth - saveWidth - columnGap - cancelWidth, y, cancelWidth, FooterHeight);
				SetBounds((Control)(object)_saveButton, innerLeft + innerWidth - saveWidth, y, saveWidth, FooterHeight);
				y += FooterHeight + PanelPadding;
				if (((Control)this).get_Height() != y)
				{
					((Control)this).set_Height(y);
				}
			}
		}

		private void LayoutField(Label label, Control field, int left, int top, int width)
		{
			SetBounds((Control)(object)label, left, top, width, FieldLabelHeight);
			SetBounds(field, left, top + FieldLabelHeight, width, FieldHeight);
		}

		private static void SetBounds(Control control, int left, int top, int width, int height)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			control.set_Location(new Point(left, top));
			control.set_Size(new Point(Math.Max(1, width), Math.Max(1, height)));
		}

		private void AddPendingSubtask()
		{
			if (string.IsNullOrWhiteSpace(((TextInputBase)_newSubtaskBox).get_Text()))
			{
				ShowValidation("Enter a subtask name before adding it.", _newSubtaskBox);
				return;
			}
			this.ContentHeightChanging?.Invoke();
			ClearValidation();
			_workingSubtasks.Add(new TodoTask
			{
				Name = ((TextInputBase)_newSubtaskBox).get_Text().Trim(),
				Order = _workingSubtasks.Count,
				Schedule = _task.Schedule
			});
			((TextInputBase)_newSubtaskBox).set_Text("");
			RebuildSubtaskList();
			((TextInputBase)_newSubtaskBox).set_Focused(true);
		}

		private void RebuildSubtaskList()
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)_subtaskPanel).get_Children().ToList())
			{
				item.Dispose();
			}
			_subtaskRows.Clear();
			foreach (TodoTask subtask in _workingSubtasks.ToList())
			{
				TextBox val = new TextBox();
				((Control)val).set_Parent((Container)(object)_subtaskPanel);
				((Control)val).set_Height(FieldHeight);
				((TextInputBase)val).set_Text(subtask.Name);
				((TextInputBase)val).set_Font(_sizing.BodyFont);
				((TextInputBase)val).set_PlaceholderText("Subtask name");
				TextBox nameBox = val;
				TextBox val2 = new TextBox();
				((Control)val2).set_Parent((Container)(object)_subtaskPanel);
				((Control)val2).set_Height(FieldHeight);
				((TextInputBase)val2).set_Text(subtask.ClipboardContent ?? "");
				((TextInputBase)val2).set_Font(_sizing.BodyFont);
				((TextInputBase)val2).set_PlaceholderText("Waypoint or text");
				TextBox clipboardBox = val2;
				Checkbox val3 = new Checkbox();
				((Control)val3).set_Parent((Container)(object)_subtaskPanel);
				((Control)val3).set_Height(FieldHeight);
				val3.set_Checked(subtask.IsOptional);
				((Control)val3).set_BasicTooltipText("Optional subtasks do not affect parent completion");
				Checkbox optionalCheckbox = val3;
				IconButton iconButton = new IconButton(TaskmasterIcons.Cancel, TaskmasterTheme.IconGlyph);
				((Control)iconButton).set_Parent((Container)(object)_subtaskPanel);
				((Control)iconButton).set_Height(FieldHeight);
				iconButton.GlyphSize = _sizing.Px(17);
				((Control)iconButton).set_BasicTooltipText("Delete subtask");
				IconButton deleteButton = iconButton;
				TodoTask captured = subtask;
				((TextInputBase)nameBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					captured.Name = ((TextInputBase)nameBox).get_Text();
					ClearValidation();
				});
				((TextInputBase)clipboardBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					captured.ClipboardContent = ((TextInputBase)clipboardBox).get_Text();
					ClearValidation();
				});
				optionalCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					captured.IsOptional = e.get_Checked();
				});
				((Control)deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					this.ContentHeightChanging?.Invoke();
					_workingSubtasks.Remove(captured);
					ClearValidation();
					RebuildSubtaskList();
				});
				_subtaskRows.Add(new SubtaskEditorRow
				{
					Subtask = subtask,
					NameBox = nameBox,
					ClipboardBox = clipboardBox,
					OptionalCheckbox = optionalCheckbox,
					DeleteButton = deleteButton
				});
			}
			Relayout();
		}

		private void ShowValidation(string message, TextBox focus)
		{
			bool num = !((Control)_validationLabel).get_Visible();
			_validationLabel.set_Text(message);
			((Control)_validationLabel).set_Visible(true);
			if (num)
			{
				this.ContentHeightChanging?.Invoke();
			}
			Relayout();
			if (focus != null)
			{
				((TextInputBase)focus).set_Focused(true);
			}
		}

		private void ClearValidation()
		{
			if (((Control)_validationLabel).get_Visible())
			{
				this.ContentHeightChanging?.Invoke();
				((Control)_validationLabel).set_Visible(false);
				_validationLabel.set_Text("");
				Relayout();
			}
		}

		public void Apply()
		{
			string validationMessage = TaskEditValidation.Validate(((TextInputBase)_nameBox).get_Text(), SelectedSchedule, ((TextInputBase)_localTimeBox).get_Text(), ((TextInputBase)_durationBox).get_Text(), ((TextInputBase)_countBox).get_Text(), _workingSubtasks.Count > 0, _workingSubtasks.Select((TodoTask subtask) => subtask.Name));
			if (validationMessage != null)
			{
				ShowValidation(validationMessage, FindInvalidField());
				return;
			}
			bool wasDone = _task.IsDone;
			bool hadSubtasks = _task.HasSubtasks;
			bool hadRequiredSubtasks = _task.HasRequiredSubtasks;
			_task.Name = ((TextInputBase)_nameBox).get_Text().Trim();
			_task.Schedule = SelectedSchedule;
			if (_task.Schedule == ResetScheduleType.LocalTime)
			{
				TimeSpan.TryParse(((TextInputBase)_localTimeBox).get_Text(), out var localAt);
				_task.LocalResetTime = localAt;
			}
			if (_task.Schedule == ResetScheduleType.Duration)
			{
				TimeSpan.TryParse(((TextInputBase)_durationBox).get_Text(), out var duration);
				_task.ResetDuration = duration;
			}
			_task.EnsureDurationAnchor(DateTime.UtcNow);
			if (_workingSubtasks.Count == 0)
			{
				int.TryParse(((TextInputBase)_countBox).get_Text(), out var count);
				_task.TargetCount = count;
			}
			_task.ClipboardContent = (string.IsNullOrWhiteSpace(((TextInputBase)_clipboardBox).get_Text()) ? null : ((TextInputBase)_clipboardBox).get_Text().Trim());
			_task.Notes = (string.IsNullOrWhiteSpace(((TextInputBase)_notesBox).get_Text()) ? null : ((TextInputBase)_notesBox).get_Text().Trim());
			_task.Subtasks = _workingSubtasks.ToList();
			TaskOrdering.Normalize(_task.Subtasks);
			foreach (TodoTask subtask2 in _task.Subtasks)
			{
				subtask2.Name = subtask2.Name.Trim();
				subtask2.ClipboardContent = (string.IsNullOrWhiteSpace(subtask2.ClipboardContent) ? null : subtask2.ClipboardContent.Trim());
				subtask2.Schedule = _task.Schedule;
				subtask2.LocalResetTime = _task.LocalResetTime;
				subtask2.ResetDuration = _task.ResetDuration;
			}
			_task.ReconcileSubtaskStructure(wasDone, hadSubtasks, hadRequiredSubtasks, DateTime.UtcNow);
			if (_task.CurrentCount > _task.TargetCount)
			{
				_task.CurrentCount = _task.TargetCount;
			}
			this.Saved?.Invoke();
		}

		private TextBox FindInvalidField()
		{
			if (string.IsNullOrWhiteSpace(((TextInputBase)_nameBox).get_Text()))
			{
				return _nameBox;
			}
			if (SelectedSchedule == ResetScheduleType.LocalTime && (!TimeSpan.TryParse(((TextInputBase)_localTimeBox).get_Text(), out var localAt) || localAt < TimeSpan.Zero || localAt >= TimeSpan.FromDays(1.0)))
			{
				return _localTimeBox;
			}
			if (SelectedSchedule == ResetScheduleType.Duration && (!TimeSpan.TryParse(((TextInputBase)_durationBox).get_Text(), out var duration) || duration <= TimeSpan.Zero))
			{
				return _durationBox;
			}
			if (_workingSubtasks.Count == 0 && (!int.TryParse(((TextInputBase)_countBox).get_Text(), out var count) || count < 1 || count > 999))
			{
				return _countBox;
			}
			return _subtaskRows.FirstOrDefault((SubtaskEditorRow row) => string.IsNullOrWhiteSpace(row.Subtask.Name))?.NameBox;
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
				IsOptional = source.IsOptional,
				Subtasks = (source.Subtasks?.Select(CloneForEditing).ToList() ?? new List<TodoTask>())
			};
		}
	}
}
