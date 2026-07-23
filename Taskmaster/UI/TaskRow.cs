using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Taskmaster.Models;
using Taskmaster.Services;
using Taskmaster.UI.Controls;

namespace Taskmaster.UI
{
	public class TaskRow : Control
	{
		private static readonly TimeSpan DueSoonThreshold = TimeSpan.FromHours(1.0);

		private readonly TodoTask _task;

		private readonly bool _isSubtask;

		private readonly TaskmasterSizing _sizing;

		private string _countdownText = "";

		private string _displayName = "";

		private bool _hasClipboard;

		private bool _dueSoon;

		private bool _hover;

		private bool _isSelected;

		private bool _isDropTarget;

		private DateTime _copiedFlashUntilUtc;

		private Rectangle _chipBounds;

		private Rectangle _clipboardBounds;

		private Rectangle _noteBounds;

		private Rectangle _pencilBounds;

		private Rectangle _saveBounds;

		public TodoTask Task => _task;

		public TodoTask ParentTask { get; set; }

		public bool IsExpanded { get; set; }

		public bool Locked { get; set; }

		public bool CanEdit { get; set; } = true;


		public bool CanOpenContextMenu { get; set; } = true;


		public bool DragReorderingEnabled { get; set; }

		public bool DropAfter { get; set; }

		public bool IsDropTarget
		{
			get
			{
				return _isDropTarget;
			}
			set
			{
				if (_isDropTarget != value)
				{
					_isDropTarget = value;
					((Control)this).Invalidate();
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					((Control)this).Invalidate();
				}
			}
		}

		public bool IsEditing { get; set; }

		public TodoTask CountdownAnchor { get; set; }

		private int RowHeight => _sizing.Px(34);

		private int SubtaskIndent => _sizing.Px(22);

		private int CheckboxSize => _sizing.Px(18);

		private int InlineIconHitSize => _sizing.Px(26);

		private int InlineIconSize => _sizing.Px(18);

		private int ChevronHitSize => _sizing.Px(26);

		private int ChevronIconSize => _sizing.Px(19);

		private int EditIconSize => _sizing.Px(24);

		private int EditActionIconSize => _sizing.Px(28);

		private int Pad => _sizing.Px(6);

		private int LeftOffset => Pad + (_isSubtask ? SubtaskIndent : 0);

		private int ScrollbarMargin => _sizing.Px(20);

		private Rectangle ChevronBounds => new Rectangle(((Control)this).get_Width() - ScrollbarMargin - ChevronHitSize, (((Control)this).get_Height() - ChevronHitSize) / 2, ChevronHitSize, ChevronHitSize);

		private Rectangle ChevronGlyphBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle hitBounds = ChevronBounds;
				return new Rectangle(hitBounds.X + (hitBounds.Width - ChevronIconSize) / 2, hitBounds.Y + (hitBounds.Height - ChevronIconSize) / 2, ChevronIconSize, ChevronIconSize);
			}
		}

		private Rectangle CheckboxBounds => new Rectangle(LeftOffset, (((Control)this).get_Height() - CheckboxSize) / 2, CheckboxSize, CheckboxSize);

		private bool HasClipboard => _hasClipboard;

		private bool HasCounter
		{
			get
			{
				if (_task.TargetCount > 1)
				{
					return !_task.HasSubtasks;
				}
				return false;
			}
		}

		public event Action ToggleRequested;

		public event Action EditRequested;

		public event Action SaveRequested;

		public event Action ContextMenuRequested;

		public event Action ExpandToggled;

		public event Action CopyRequested;

		public event Action<bool, bool> SelectionRequested;

		public event Action<bool> DragCandidateRequested;

		public TaskRow(TodoTask task, bool isSubtask, TaskmasterSizing sizing)
			: this()
		{
			_task = task;
			_isSubtask = isSubtask;
			_sizing = sizing ?? new TaskmasterSizing(1f, 1f);
			((Control)this).set_Height(RowHeight);
		}

		private bool InChipZone(Point p)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (HasCounter)
			{
				return ((Rectangle)(ref _chipBounds)).Contains(p);
			}
			return false;
		}

		public void RefreshDisplay(DateTime nowUtc)
		{
			_displayName = TaskPresetService.ResolveName(_task, nowUtc);
			_hasClipboard = !string.IsNullOrEmpty(TaskPresetService.ResolveClipboardContent(_task, nowUtc));
			TodoTask anchor = CountdownAnchor ?? _task;
			DateTime? next = ResetEngine.NextBoundary(anchor, nowUtc);
			_countdownText = (next.HasValue ? FormatCountdown(next.Value - nowUtc) : "");
			_dueSoon = next.HasValue && !_task.IsDone && next.Value - nowUtc < DueSoonThreshold && anchor.Schedule != ResetScheduleType.Never;
			((Control)this).set_BasicTooltipText(string.IsNullOrEmpty(_task.Notes) ? null : _task.Notes);
			((Control)this).Invalidate();
		}

		public static string FormatCountdown(TimeSpan t)
		{
			if (t < TimeSpan.Zero)
			{
				t = TimeSpan.Zero;
			}
			if (t.TotalDays >= 1.0)
			{
				return $"{(int)t.TotalDays}d {t.Hours}h";
			}
			if (t.TotalHours >= 1.0)
			{
				return $"{(int)t.TotalHours}h {t.Minutes:00}m";
			}
			return $"{t.Minutes}m";
		}

		public void FlashCopied()
		{
			_copiedFlashUntilUtc = DateTime.UtcNow.AddSeconds(1.5);
			UpdateTooltip();
			((Control)this).Invalidate();
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			_hover = true;
			UpdateTooltip();
			((Control)this).Invalidate();
		}

		private void UpdateTooltip()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BasicTooltipText((!HasClipboard || !((Rectangle)(ref _clipboardBounds)).Contains(((Control)this).get_RelativeMousePosition())) ? (string.IsNullOrEmpty(_task.Notes) ? null : _task.Notes) : ((DateTime.UtcNow < _copiedFlashUntilUtc) ? "Copied!" : "Click to copy to clipboard"));
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			_hover = false;
			((Control)this).Invalidate();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point p = ((Control)this).get_RelativeMousePosition();
			Rectangle val;
			if (!_isSubtask && _task.HasSubtasks)
			{
				val = ChevronBounds;
				if (((Rectangle)(ref val)).Contains(p))
				{
					this.ExpandToggled?.Invoke();
					return;
				}
			}
			val = CheckboxBounds;
			if (((Rectangle)(ref val)).Contains(p) || InChipZone(p))
			{
				this.ToggleRequested?.Invoke();
				return;
			}
			if (!Locked && CanEdit && _saveBounds != Rectangle.get_Empty() && ((Rectangle)(ref _saveBounds)).Contains(p))
			{
				this.SaveRequested?.Invoke();
				return;
			}
			if (!Locked && CanEdit && _pencilBounds != Rectangle.get_Empty() && ((Rectangle)(ref _pencilBounds)).Contains(p))
			{
				this.EditRequested?.Invoke();
				return;
			}
			if (HasClipboard && ((Rectangle)(ref _clipboardBounds)).Contains(p))
			{
				this.CopyRequested?.Invoke();
				return;
			}
			bool selectSingleOnRelease = false;
			if (!Locked && !_isSubtask)
			{
				ModifierKeys modifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
				bool extendRange = ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)4);
				bool toggle = ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)1);
				selectSingleOnRelease = DragReorderingEnabled && !extendRange && !toggle && IsSelected;
				if (!selectSingleOnRelease)
				{
					this.SelectionRequested?.Invoke(extendRange, toggle);
				}
			}
			if (!Locked && DragReorderingEnabled)
			{
				this.DragCandidateRequested?.Invoke(selectSingleOnRelease);
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			if (!Locked && CanOpenContextMenu)
			{
				this.ContextMenuRequested?.Invoke();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_0728: Unknown result type (might be due to invalid IL or missing references)
			//IL_0733: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_073f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Unknown result type (might be due to invalid IL or missing references)
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			BitmapFont font = _sizing.BodyFont;
			BitmapFont smallFont = _sizing.SmallFont;
			if (IsSelected)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bounds, TaskmasterTheme.RowSelected);
			}
			if (_hover)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bounds, TaskmasterTheme.RowHover);
			}
			if (IsDropTarget)
			{
				int markerHeight = _sizing.Px(2);
				int markerY = (DropAfter ? (bounds.Height - markerHeight) : 0);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(_sizing.Px(4), markerY, Math.Max(0, bounds.Width - _sizing.Px(8)), markerHeight), TaskmasterTheme.Gold);
			}
			bool done = _task.IsDone;
			Color textColor = (done ? TaskmasterTheme.DoneText : (_dueSoon ? TaskmasterTheme.DueSoon : TaskmasterTheme.CreamWhite));
			if (!_isSubtask && _task.HasSubtasks)
			{
				Texture2D chevron = (IsExpanded ? TaskmasterIcons.ChevronUp : TaskmasterIcons.ChevronDown);
				int num;
				if (_hover)
				{
					Rectangle chevronBounds = ChevronBounds;
					num = (((Rectangle)(ref chevronBounds)).Contains(((Control)this).get_RelativeMousePosition()) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool chevronHovered = (byte)num != 0;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, chevron, ChevronGlyphBounds, chevronHovered ? TaskmasterTheme.Gold : TaskmasterTheme.MutedCream);
			}
			Rectangle cb = CheckboxBounds;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, cb, TaskmasterTheme.ChipFill);
			DrawBorder(spriteBatch, cb, done ? TaskmasterTheme.Success : TaskmasterTheme.ChipBorder);
			if (done)
			{
				Rectangle glyph = default(Rectangle);
				((Rectangle)(ref glyph))._002Ector(cb.X + 2, cb.Y + 2, cb.Width - 4, cb.Height - 4);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Check, glyph, TaskmasterTheme.Success);
			}
			int x = ((Rectangle)(ref cb)).get_Right() + 8;
			int itemGap = _sizing.Px(8);
			int compactGap = _sizing.Px(6);
			Size2 nameSize = font.MeasureString(_displayName);
			Rectangle nameRect = default(Rectangle);
			((Rectangle)(ref nameRect))._002Ector(x, 0, (int)nameSize.Width + 2, ((Control)this).get_Height());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _displayName, font, nameRect, textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (done)
			{
				Rectangle strike = default(Rectangle);
				((Rectangle)(ref strike))._002Ector(x, ((Control)this).get_Height() / 2, (int)nameSize.Width, 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, strike, TaskmasterTheme.DoneText);
			}
			x = ((Rectangle)(ref nameRect)).get_Right() + itemGap;
			if (HasCounter)
			{
				string chipText = $"{_task.CurrentCount}/{_task.TargetCount}";
				Size2 chipSize = smallFont.MeasureString(chipText);
				int chipHeight = _sizing.Px(18);
				_chipBounds = new Rectangle(x, (((Control)this).get_Height() - chipHeight) / 2, (int)chipSize.Width + _sizing.Px(12), chipHeight);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, _chipBounds, TaskmasterTheme.ChipFill);
				DrawBorder(spriteBatch, _chipBounds, TaskmasterTheme.ChipBorder);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, chipText, smallFont, _chipBounds, TaskmasterTheme.Gold, false, (HorizontalAlignment)1, (VerticalAlignment)1);
				x = ((Rectangle)(ref _chipBounds)).get_Right() + itemGap;
			}
			else if (!_isSubtask && _task.HasSubtasks)
			{
				int subCount = _task.Subtasks.Count;
				string subText = ((subCount == 1) ? "1 subtask" : $"{subCount} subtasks");
				Size2 subSize = smallFont.MeasureString(subText);
				Rectangle subRect = default(Rectangle);
				((Rectangle)(ref subRect))._002Ector(x, 0, (int)subSize.Width + 2, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, subText, smallFont, subRect, TaskmasterTheme.DimText, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				x = ((Rectangle)(ref subRect)).get_Right() + itemGap;
				_chipBounds = Rectangle.get_Empty();
			}
			else
			{
				_chipBounds = Rectangle.get_Empty();
			}
			if (HasClipboard)
			{
				_clipboardBounds = new Rectangle(x, (((Control)this).get_Height() - InlineIconHitSize) / 2, InlineIconHitSize, InlineIconHitSize);
				Rectangle clipboardGlyphBounds = CenteredGlyph(_clipboardBounds, InlineIconSize);
				bool flashing = DateTime.UtcNow < _copiedFlashUntilUtc;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Clipboard, clipboardGlyphBounds, flashing ? TaskmasterTheme.Success : TaskmasterTheme.DimText);
				x = ((Rectangle)(ref _clipboardBounds)).get_Right() + compactGap;
			}
			else
			{
				_clipboardBounds = Rectangle.get_Empty();
			}
			if (!string.IsNullOrEmpty(_task.Notes))
			{
				_noteBounds = new Rectangle(x, (((Control)this).get_Height() - InlineIconHitSize) / 2, InlineIconHitSize, InlineIconHitSize);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Note, CenteredGlyph(_noteBounds, InlineIconSize), TaskmasterTheme.DimText);
				x = ((Rectangle)(ref _noteBounds)).get_Right() + compactGap;
			}
			else
			{
				_noteBounds = Rectangle.get_Empty();
			}
			int rightX = (_isSubtask ? (((Control)this).get_Width() - ScrollbarMargin) : (((Control)this).get_Width() - ScrollbarMargin - ChevronHitSize - compactGap));
			if (!_isSubtask && !string.IsNullOrEmpty(_countdownText))
			{
				Size2 cdSize = smallFont.MeasureString(_countdownText);
				Rectangle cdRect = default(Rectangle);
				((Rectangle)(ref cdRect))._002Ector(rightX - (int)cdSize.Width - 2, 0, (int)cdSize.Width + 2, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _countdownText, smallFont, cdRect, _dueSoon ? TaskmasterTheme.DueSoon : TaskmasterTheme.DimText, false, (HorizontalAlignment)2, (VerticalAlignment)1);
				rightX = cdRect.X - itemGap;
			}
			if (!_isSubtask && CanEdit && (_hover || IsEditing) && !Locked)
			{
				if (IsEditing)
				{
					int hitSize2 = RowHeight;
					int actionGap = _sizing.Px(2);
					_pencilBounds = new Rectangle(rightX - hitSize2, 0, hitSize2, hitSize2);
					Rectangle closeGlyphBounds = default(Rectangle);
					((Rectangle)(ref closeGlyphBounds))._002Ector(_pencilBounds.X + (hitSize2 - EditActionIconSize) / 2, _pencilBounds.Y + (hitSize2 - EditActionIconSize) / 2, EditActionIconSize, EditActionIconSize);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Cancel, closeGlyphBounds, TaskmasterTheme.CreamWhite);
					_saveBounds = new Rectangle(_pencilBounds.X - actionGap - hitSize2, 0, hitSize2, hitSize2);
					Rectangle saveGlyphBounds = default(Rectangle);
					((Rectangle)(ref saveGlyphBounds))._002Ector(_saveBounds.X + (hitSize2 - EditActionIconSize) / 2, _saveBounds.Y + (hitSize2 - EditActionIconSize) / 2, EditActionIconSize, EditActionIconSize);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Check, saveGlyphBounds, TaskmasterTheme.Success);
				}
				else
				{
					int hitSize = RowHeight;
					_pencilBounds = new Rectangle(rightX - hitSize, (((Control)this).get_Height() - hitSize) / 2, hitSize, hitSize);
					Rectangle pencilGlyphBounds = default(Rectangle);
					((Rectangle)(ref pencilGlyphBounds))._002Ector(_pencilBounds.X + (hitSize - EditIconSize) / 2, _pencilBounds.Y + (hitSize - EditIconSize) / 2, EditIconSize, EditIconSize);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Pencil, pencilGlyphBounds, TaskmasterTheme.CreamWhite);
					_saveBounds = Rectangle.get_Empty();
				}
			}
			else
			{
				_pencilBounds = Rectangle.get_Empty();
				_saveBounds = Rectangle.get_Empty();
			}
		}

		private void DrawBorder(SpriteBatch spriteBatch, Rectangle r, Color color)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, r.Y, r.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, ((Rectangle)(ref r)).get_Bottom() - 1, r.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, r.Y, 1, r.Height), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(((Rectangle)(ref r)).get_Right() - 1, r.Y, 1, r.Height), color);
		}

		private static Rectangle CenteredGlyph(Rectangle bounds, int glyphSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(bounds.X + (bounds.Width - glyphSize) / 2, bounds.Y + (bounds.Height - glyphSize) / 2, glyphSize, glyphSize);
		}
	}
}
