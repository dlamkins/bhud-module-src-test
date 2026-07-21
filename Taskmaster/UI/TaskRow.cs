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
		private const int RowHeight = 30;

		private const int SubtaskIndent = 22;

		private const int CheckboxSize = 16;

		private const int IconSize = 14;

		private const int ChevronHitSize = 24;

		private const int ChevronIconSize = 18;

		private const int EditIconSize = 24;

		private const int EditActionIconSize = 28;

		private const int Pad = 6;

		private static readonly TimeSpan DueSoonThreshold = TimeSpan.FromHours(1.0);

		private readonly TodoTask _task;

		private readonly bool _isSubtask;

		private string _countdownText = "";

		private bool _dueSoon;

		private bool _hover;

		private bool _isSelected;

		private DateTime _copiedFlashUntilUtc;

		private Rectangle _chipBounds;

		private Rectangle _clipboardBounds;

		private Rectangle _noteBounds;

		private Rectangle _pencilBounds;

		private Rectangle _saveBounds;

		private const int ScrollbarMargin = 20;

		public TodoTask Task => _task;

		public bool IsExpanded { get; set; }

		public bool Locked { get; set; }

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

		private int LeftOffset => 6 + (_isSubtask ? 22 : 0);

		private Rectangle ChevronBounds => new Rectangle(((Control)this).get_Width() - 20 - 24, (((Control)this).get_Height() - 24) / 2, 24, 24);

		private Rectangle ChevronGlyphBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle hitBounds = ChevronBounds;
				return new Rectangle(hitBounds.X + (hitBounds.Width - 18) / 2, hitBounds.Y + (hitBounds.Height - 18) / 2, 18, 18);
			}
		}

		private Rectangle CheckboxBounds => new Rectangle(LeftOffset, (((Control)this).get_Height() - 16) / 2, 16, 16);

		private bool HasClipboard => !string.IsNullOrEmpty(_task.ClipboardContent);

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

		public TaskRow(TodoTask task, bool isSubtask)
			: this()
		{
			_task = task;
			_isSubtask = isSubtask;
			((Control)this).set_Height(30);
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
			((Control)this).Invalidate();
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			_hover = true;
			((Control)this).set_BasicTooltipText((HasClipboard && ((Rectangle)(ref _clipboardBounds)).Contains(((Control)this).get_RelativeMousePosition())) ? "Click to copy to clipboard" : (string.IsNullOrEmpty(_task.Notes) ? null : _task.Notes));
			((Control)this).Invalidate();
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
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
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
			}
			else if (!Locked && _saveBounds != Rectangle.get_Empty() && ((Rectangle)(ref _saveBounds)).Contains(p))
			{
				this.SaveRequested?.Invoke();
			}
			else if (!Locked && _pencilBounds != Rectangle.get_Empty() && ((Rectangle)(ref _pencilBounds)).Contains(p))
			{
				this.EditRequested?.Invoke();
			}
			else if (HasClipboard && ((Rectangle)(ref _clipboardBounds)).Contains(p))
			{
				this.CopyRequested?.Invoke();
			}
			else if (!Locked && !_isSubtask)
			{
				ModifierKeys modifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
				this.SelectionRequested?.Invoke(((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)4), ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)1));
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			if (!Locked)
			{
				this.ContextMenuRequested?.Invoke();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			BitmapFont font = GameService.Content.get_DefaultFont14();
			BitmapFont smallFont = GameService.Content.get_DefaultFont12();
			if (IsSelected)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bounds, TaskmasterTheme.RowSelected);
			}
			if (_hover)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bounds, TaskmasterTheme.RowHover);
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
			Size2 nameSize = font.MeasureString(_task.Name);
			Rectangle nameRect = default(Rectangle);
			((Rectangle)(ref nameRect))._002Ector(x, 0, (int)nameSize.Width + 2, ((Control)this).get_Height());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _task.Name, font, nameRect, textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (done)
			{
				Rectangle strike = default(Rectangle);
				((Rectangle)(ref strike))._002Ector(x, ((Control)this).get_Height() / 2, (int)nameSize.Width, 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, strike, TaskmasterTheme.DoneText);
			}
			x = ((Rectangle)(ref nameRect)).get_Right() + 8;
			if (HasCounter)
			{
				string chipText = $"{_task.CurrentCount}/{_task.TargetCount}";
				Size2 chipSize = smallFont.MeasureString(chipText);
				_chipBounds = new Rectangle(x, (((Control)this).get_Height() - 18) / 2, (int)chipSize.Width + 12, 18);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, _chipBounds, TaskmasterTheme.ChipFill);
				DrawBorder(spriteBatch, _chipBounds, TaskmasterTheme.ChipBorder);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, chipText, smallFont, _chipBounds, TaskmasterTheme.Gold, false, (HorizontalAlignment)1, (VerticalAlignment)1);
				x = ((Rectangle)(ref _chipBounds)).get_Right() + 8;
			}
			else if (!_isSubtask && _task.HasSubtasks)
			{
				int subCount = _task.Subtasks.Count;
				string subText = ((subCount == 1) ? "1 subtask" : $"{subCount} subtasks");
				Size2 subSize = smallFont.MeasureString(subText);
				Rectangle subRect = default(Rectangle);
				((Rectangle)(ref subRect))._002Ector(x, 0, (int)subSize.Width + 2, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, subText, smallFont, subRect, TaskmasterTheme.DimText, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				x = ((Rectangle)(ref subRect)).get_Right() + 8;
				_chipBounds = Rectangle.get_Empty();
			}
			else
			{
				_chipBounds = Rectangle.get_Empty();
			}
			if (HasClipboard)
			{
				_clipboardBounds = new Rectangle(x, (((Control)this).get_Height() - 14) / 2, 14, 14);
				bool flashing = DateTime.UtcNow < _copiedFlashUntilUtc;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Clipboard, _clipboardBounds, flashing ? TaskmasterTheme.Success : TaskmasterTheme.DimText);
				x = ((Rectangle)(ref _clipboardBounds)).get_Right() + 6;
			}
			else
			{
				_clipboardBounds = Rectangle.get_Empty();
			}
			if (!string.IsNullOrEmpty(_task.Notes))
			{
				_noteBounds = new Rectangle(x, (((Control)this).get_Height() - 14) / 2, 14, 14);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Note, _noteBounds, TaskmasterTheme.DimText);
				x = ((Rectangle)(ref _noteBounds)).get_Right() + 6;
			}
			int rightX = (_isSubtask ? (((Control)this).get_Width() - 20) : (((Control)this).get_Width() - 20 - 24 - 6));
			if (!_isSubtask && !string.IsNullOrEmpty(_countdownText))
			{
				Size2 cdSize = smallFont.MeasureString(_countdownText);
				Rectangle cdRect = default(Rectangle);
				((Rectangle)(ref cdRect))._002Ector(rightX - (int)cdSize.Width - 2, 0, (int)cdSize.Width + 2, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _countdownText, smallFont, cdRect, _dueSoon ? TaskmasterTheme.DueSoon : TaskmasterTheme.DimText, false, (HorizontalAlignment)2, (VerticalAlignment)1);
				rightX = cdRect.X - 8;
			}
			if (!_isSubtask && (_hover || IsEditing) && !Locked)
			{
				if (IsEditing)
				{
					_pencilBounds = new Rectangle(rightX - 30, 0, 30, 30);
					Rectangle closeGlyphBounds = default(Rectangle);
					((Rectangle)(ref closeGlyphBounds))._002Ector(_pencilBounds.X + 1, _pencilBounds.Y + 1, 28, 28);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Cancel, closeGlyphBounds, TaskmasterTheme.CreamWhite);
					_saveBounds = new Rectangle(_pencilBounds.X - 2 - 30, 0, 30, 30);
					Rectangle saveGlyphBounds = default(Rectangle);
					((Rectangle)(ref saveGlyphBounds))._002Ector(_saveBounds.X + 1, _saveBounds.Y + 1, 28, 28);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Check, saveGlyphBounds, TaskmasterTheme.Success);
				}
				else
				{
					_pencilBounds = new Rectangle(rightX - 30, (((Control)this).get_Height() - 30) / 2, 30, 30);
					Rectangle pencilGlyphBounds = default(Rectangle);
					((Rectangle)(ref pencilGlyphBounds))._002Ector(_pencilBounds.X + 3, _pencilBounds.Y + 3, 24, 24);
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
	}
}
