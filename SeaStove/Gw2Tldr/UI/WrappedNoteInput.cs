using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class WrappedNoteInput : TextInputBase
	{
		private readonly List<WrappedTextLayout.Line> _lines = new List<WrappedTextLayout.Line>();

		private int _firstLine;

		private string _laidOutText;

		private int _laidOutWidth = -1;

		private const int Pad = 5;

		private int VisibleLines => Math.Max(1, (((Control)this).get_Height() - 10) / Math.Max(1, base._font.get_LineHeight()));

		public WrappedNoteInput()
			: this()
		{
			base._multiline = true;
		}

		private void EnsureLines()
		{
			int width = Math.Max(1, ((Control)this).get_Width() - 10);
			if (!(_laidOutText == base._text) || _laidOutWidth != width)
			{
				_laidOutText = base._text;
				_laidOutWidth = width;
				_lines.Clear();
				_lines.AddRange(WrappedTextLayout.Build(base._text, (float)width, (Func<string, float>)base.MeasureStringWidth));
				_firstLine = Math.Max(0, Math.Min(_firstLine, _lines.Count - 1));
			}
		}

		private int LineFor(int index)
		{
			EnsureLines();
			for (int i = 0; i < _lines.Count - 1; i++)
			{
				if (index < _lines[i].End || (index == _lines[i].End && _lines[i].End < base._text.Length && base._text[_lines[i].End] == '\n'))
				{
					return i;
				}
			}
			return _lines.Count - 1;
		}

		protected override void UpdateScrolling()
		{
			int line = LineFor(base._cursorIndex);
			if (line < _firstLine)
			{
				_firstLine = line;
			}
			if (line >= _firstLine + VisibleLines)
			{
				_firstLine = line - VisibleLines + 1;
			}
		}

		public override void RecalculateLayout()
		{
			EnsureLines();
			((TextInputBase)this).UpdateScrolling();
		}

		public override int GetCursorIndexFromPosition(int x, int y)
		{
			EnsureLines();
			int lineIndex = Math.Max(0, Math.Min(_lines.Count - 1, _firstLine + Math.Max(0, y - 5) / Math.Max(1, base._font.get_LineHeight())));
			return IndexAt(lineIndex, Math.Max(0, x - 5));
		}

		private int IndexAt(int lineIndex, float target)
		{
			WrappedTextLayout.Line line = _lines[lineIndex];
			float previousWidth = 0f;
			int i = line.Start;
			while (i < line.End)
			{
				int next = i + ((!char.IsHighSurrogate(base._text[i]) || i + 1 >= line.End || !char.IsLowSurrogate(base._text[i + 1])) ? 1 : 2);
				float width = ((TextInputBase)this).MeasureStringWidth(base._text.Substring(line.Start, next - line.Start));
				if (target < (previousWidth + width) / 2f)
				{
					return i;
				}
				previousWidth = width;
				i = next;
			}
			return line.End;
		}

		protected override void MoveLine(int delta)
		{
			EnsureLines();
			int current = LineFor(base._cursorIndex);
			int target = Math.Max(0, Math.Min(_lines.Count - 1, current + delta));
			WrappedTextLayout.Line line = _lines[current];
			float column = ((TextInputBase)this).MeasureStringWidth(base._text.Substring(line.Start, Math.Max(0, Math.Min(base._cursorIndex, line.End) - line.Start)));
			((TextInputBase)this).UserSetCursorIndex(IndexAt(target, column));
			((TextInputBase)this).UpdateSelectionIfShiftDown();
		}

		protected override void Paint(SpriteBatch batch, Rectangle bounds)
		{
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			EnsureLines();
			int selectionStart = Math.Min(base._selectionStart, base._selectionEnd);
			int selectionEnd = Math.Max(base._selectionStart, base._selectionEnd);
			for (int i = _firstLine; i < _lines.Count && i < _firstLine + VisibleLines; i++)
			{
				WrappedTextLayout.Line line2 = _lines[i];
				int y = 5 + (i - _firstLine) * base._font.get_LineHeight();
				int from = Math.Max(line2.Start, selectionStart);
				int to = Math.Min(line2.End, selectionEnd);
				if (to > from)
				{
					int x2 = 5 + (int)((TextInputBase)this).MeasureStringWidth(base._text.Substring(line2.Start, from - line2.Start));
					int width = (int)Math.Ceiling(((TextInputBase)this).MeasureStringWidth(base._text.Substring(from, to - from)));
					((TextInputBase)this).PaintHighlight(batch, new Rectangle(x2, y, Math.Max(1, width), base._font.get_LineHeight()));
				}
				SpriteBatchExtensions.DrawStringOnCtrl(batch, (Control)(object)this, base._text.Substring(line2.Start, line2.End - line2.Start), base._font, new Rectangle(5, y, Math.Max(1, bounds.Width - 10), base._font.get_LineHeight()), base._foreColor, false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
			int cursorLine = LineFor(base._cursorIndex);
			if (cursorLine >= _firstLine && cursorLine < _firstLine + VisibleLines)
			{
				WrappedTextLayout.Line line = _lines[cursorLine];
				int x = 5 + (int)((TextInputBase)this).MeasureStringWidth(base._text.Substring(line.Start, Math.Max(0, Math.Min(base._cursorIndex, line.End) - line.Start)));
				((TextInputBase)this).PaintCursor(batch, new Rectangle(x, 5 + (cursorLine - _firstLine) * base._font.get_LineHeight(), 2, base._font.get_LineHeight()));
			}
		}
	}
}
