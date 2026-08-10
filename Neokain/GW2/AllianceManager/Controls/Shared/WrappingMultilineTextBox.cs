using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class WrappingMultilineTextBox : TextInputBase
	{
		private class WrappedLine
		{
			public string Text { get; set; }

			public int StartIndex { get; set; }

			public bool IsHardBreak { get; set; }
		}

		private const int TEXT_TOPPADDING = 7;

		private const int TEXT_LEFTPADDING = 10;

		private bool _hideBackground;

		private List<WrappedLine> _wrappedLines = new List<WrappedLine>();

		private int _lastWrapWidth;

		private string _lastWrappedText = string.Empty;

		private Rectangle _textRegion = Rectangle.get_Empty();

		private Rectangle[] _highlightRegions = Array.Empty<Rectangle>();

		private Rectangle _cursorRegion = Rectangle.get_Empty();

		public bool HideBackground
		{
			get
			{
				return _hideBackground;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _hideBackground, value, false, "HideBackground");
			}
		}

		public WrappingMultilineTextBox()
			: this()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			base._multiline = true;
			base._maxLength = 524288;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Control)this).OnResized(e);
			InvalidateWrapping();
		}

		private void InvalidateWrapping()
		{
			int wrapWidth = ((Control)this)._size.X - 20;
			if (wrapWidth != _lastWrapWidth || base._text != _lastWrappedText)
			{
				_lastWrapWidth = wrapWidth;
				_lastWrappedText = base._text;
				RecalculateWrappedLines();
				((Control)this).Invalidate();
			}
		}

		private void RecalculateWrappedLines()
		{
			_wrappedLines.Clear();
			if (string.IsNullOrEmpty(base._text))
			{
				_wrappedLines.Add(new WrappedLine
				{
					Text = string.Empty,
					StartIndex = 0,
					IsHardBreak = false
				});
				return;
			}
			int wrapWidth = Math.Max(((Control)this)._size.X - 20, 50);
			int currentIndex = 0;
			string[] hardLines = base._text.Split('\n');
			for (int hardLineIdx = 0; hardLineIdx < hardLines.Length; hardLineIdx++)
			{
				string hardLine = hardLines[hardLineIdx];
				int hardLineStart = currentIndex;
				if (string.IsNullOrEmpty(hardLine))
				{
					_wrappedLines.Add(new WrappedLine
					{
						Text = string.Empty,
						StartIndex = hardLineStart,
						IsHardBreak = (hardLineIdx < hardLines.Length - 1)
					});
				}
				else
				{
					WrapLine(hardLine, hardLineStart, wrapWidth, hardLineIdx < hardLines.Length - 1);
				}
				currentIndex += hardLine.Length;
				if (hardLineIdx < hardLines.Length - 1)
				{
					currentIndex++;
				}
			}
			if (_wrappedLines.Count == 0)
			{
				_wrappedLines.Add(new WrappedLine
				{
					Text = string.Empty,
					StartIndex = 0,
					IsHardBreak = false
				});
			}
		}

		private void WrapLine(string line, int startIndex, int wrapWidth, bool endsWithHardBreak)
		{
			if (((TextInputBase)this).MeasureStringWidth(line) <= (float)wrapWidth)
			{
				_wrappedLines.Add(new WrappedLine
				{
					Text = line,
					StartIndex = startIndex,
					IsHardBreak = endsWithHardBreak
				});
				return;
			}
			string[] words = line.Split(' ');
			string currentLine = string.Empty;
			int currentLineStart = startIndex;
			foreach (string word in words)
			{
				string testLine = (string.IsNullOrEmpty(currentLine) ? word : (currentLine + " " + word));
				if (((TextInputBase)this).MeasureStringWidth(testLine) <= (float)wrapWidth)
				{
					currentLine = testLine;
				}
				else if (!string.IsNullOrEmpty(currentLine))
				{
					_wrappedLines.Add(new WrappedLine
					{
						Text = currentLine,
						StartIndex = currentLineStart,
						IsHardBreak = false
					});
					currentLineStart += currentLine.Length + 1;
					currentLine = ((!(((TextInputBase)this).MeasureStringWidth(word) > (float)wrapWidth)) ? word : BreakLongWord(word, wrapWidth, ref currentLineStart, startIndex, line));
				}
				else
				{
					currentLine = BreakLongWord(word, wrapWidth, ref currentLineStart, startIndex, line);
				}
			}
			if (!string.IsNullOrEmpty(currentLine))
			{
				_wrappedLines.Add(new WrappedLine
				{
					Text = currentLine,
					StartIndex = currentLineStart,
					IsHardBreak = endsWithHardBreak
				});
			}
		}

		private string BreakLongWord(string word, int wrapWidth, ref int currentLineStart, int lineStartIndex, string fullLine)
		{
			string remaining = word;
			while (((TextInputBase)this).MeasureStringWidth(remaining) > (float)wrapWidth && remaining.Length > 1)
			{
				int breakPoint = remaining.Length - 1;
				while (breakPoint > 0 && ((TextInputBase)this).MeasureStringWidth(remaining.Substring(0, breakPoint)) > (float)wrapWidth)
				{
					breakPoint--;
				}
				if (breakPoint == 0)
				{
					breakPoint = 1;
				}
				_wrappedLines.Add(new WrappedLine
				{
					Text = remaining.Substring(0, breakPoint),
					StartIndex = currentLineStart,
					IsHardBreak = false
				});
				currentLineStart += breakPoint;
				remaining = remaining.Substring(breakPoint);
			}
			return remaining;
		}

		private (int WrappedLine, int Character) GetWrappedPosition(int textIndex)
		{
			if (_wrappedLines.Count == 0)
			{
				return (0, 0);
			}
			for (int i = 0; i < _wrappedLines.Count; i++)
			{
				WrappedLine line = _wrappedLines[i];
				int lineEndIndex = line.StartIndex + line.Text.Length;
				if (line.IsHardBreak)
				{
					lineEndIndex++;
				}
				if (textIndex <= lineEndIndex || i == _wrappedLines.Count - 1)
				{
					int charOffset = textIndex - line.StartIndex;
					return (i, Math.Max(0, Math.Min(charOffset, line.Text.Length)));
				}
			}
			WrappedLine lastLine = _wrappedLines[_wrappedLines.Count - 1];
			return (_wrappedLines.Count - 1, lastLine.Text.Length);
		}

		private int GetTextIndex(int wrappedLineIndex, int charOffset)
		{
			if (_wrappedLines.Count == 0 || wrappedLineIndex < 0)
			{
				return 0;
			}
			if (wrappedLineIndex >= _wrappedLines.Count)
			{
				wrappedLineIndex = _wrappedLines.Count - 1;
			}
			WrappedLine line = _wrappedLines[wrappedLineIndex];
			charOffset = Math.Max(0, Math.Min(charOffset, line.Text.Length));
			return line.StartIndex + charOffset;
		}

		protected override void MoveLine(int delta)
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			InvalidateWrapping();
			(int, int) cursor = GetWrappedPosition(base._cursorIndex);
			int targetLine = cursor.Item1 + delta;
			int newIndex;
			if (targetLine < 0)
			{
				newIndex = 0;
			}
			else if (targetLine >= _wrappedLines.Count)
			{
				newIndex = base._text.Length;
			}
			else
			{
				float cursorLeft = 0f;
				if (cursor.Item1 < _wrappedLines.Count)
				{
					string currentLineText = _wrappedLines[cursor.Item1].Text;
					if (cursor.Item2 <= currentLineText.Length)
					{
						cursorLeft = ((TextInputBase)this).MeasureStringWidth(currentLineText.Substring(0, cursor.Item2));
					}
				}
				string targetLineText = _wrappedLines[targetLine].Text;
				List<BitmapFontGlyph> glyphs = ((IEnumerable<BitmapFontGlyph>)(object)base._font.GetGlyphs(targetLineText, (Point2?)null)).ToList();
				int charIndex = 0;
				float minOffset = float.MaxValue;
				for (int i = 0; i < glyphs.Count; i++)
				{
					float localOffset = Math.Abs(glyphs[i].Position.X - cursorLeft);
					if (localOffset < minOffset)
					{
						minOffset = localOffset;
						charIndex = i;
					}
					else if (localOffset > minOffset)
					{
						break;
					}
				}
				if (Math.Abs(((TextInputBase)this).MeasureStringWidth(targetLineText) - cursorLeft) < minOffset)
				{
					charIndex = targetLineText.Length;
				}
				newIndex = GetTextIndex(targetLine, charIndex);
			}
			((TextInputBase)this).UserSetCursorIndex(newIndex);
			((TextInputBase)this).UpdateSelectionIfShiftDown();
		}

		public override int GetCursorIndexFromPosition(int x, int y)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			InvalidateWrapping();
			x -= 10;
			y -= 7;
			int predictedLine = y / base._font.get_LineHeight();
			if (predictedLine < 0)
			{
				predictedLine = 0;
			}
			if (predictedLine >= _wrappedLines.Count)
			{
				predictedLine = _wrappedLines.Count - 1;
			}
			if (_wrappedLines.Count == 0)
			{
				return 0;
			}
			WrappedLine line = _wrappedLines[predictedLine];
			StringGlyphEnumerable glyphs = base._font.GetGlyphs(line.Text, (Point2?)null);
			int charIndex = 0;
			StringGlyphEnumerator enumerator = ((StringGlyphEnumerable)(ref glyphs)).GetEnumerator();
			try
			{
				while (((StringGlyphEnumerator)(ref enumerator)).MoveNext())
				{
					BitmapFontGlyph glyph = ((StringGlyphEnumerator)(ref enumerator)).get_Current();
					if (glyph.Position.X + (float)glyph.FontRegion.get_Width() / 2f > (float)x)
					{
						break;
					}
					charIndex++;
				}
			}
			finally
			{
				((IDisposable)(StringGlyphEnumerator)(ref enumerator)).Dispose();
			}
			return GetTextIndex(predictedLine, charIndex);
		}

		private Rectangle[] CalculateHighlightRegions()
		{
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			int selectionStart = Math.Min(base._selectionStart, base._selectionEnd);
			int selectionLength = Math.Abs(base._selectionStart - base._selectionEnd);
			if (selectionLength <= 0 || selectionStart + selectionLength > base._text.Length)
			{
				return Array.Empty<Rectangle>();
			}
			InvalidateWrapping();
			(int, int) startPos = GetWrappedPosition(selectionStart);
			(int, int) endPos = GetWrappedPosition(selectionStart + selectionLength);
			int lineSpans = endPos.Item1 - startPos.Item1;
			Rectangle[] regions = (Rectangle[])(object)new Rectangle[lineSpans + 1];
			for (int i = 0; i <= lineSpans; i++)
			{
				int lineIndex = startPos.Item1 + i;
				if (lineIndex >= _wrappedLines.Count)
				{
					break;
				}
				WrappedLine line = _wrappedLines[lineIndex];
				int lineStartChar = ((i == 0) ? startPos.Item2 : 0);
				int lineEndChar = ((i == lineSpans) ? endPos.Item2 : line.Text.Length);
				float highlightLeftOffset = ((TextInputBase)this).MeasureStringWidth(line.Text.Substring(0, lineStartChar));
				float highlightWidth = ((TextInputBase)this).MeasureStringWidth(line.Text.Substring(lineStartChar, lineEndChar - lineStartChar));
				regions[i] = new Rectangle(((Rectangle)(ref _textRegion)).get_Left() + (int)highlightLeftOffset - 1, ((Rectangle)(ref _textRegion)).get_Top() + lineIndex * base._font.get_LineHeight(), (int)highlightWidth, base._font.get_LineHeight() - 1);
			}
			return regions;
		}

		private Rectangle CalculateTextRegion()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(10, 7, ((Control)this)._size.X - 20, ((Control)this)._size.Y - 14);
		}

		private Rectangle CalculateCursorRegion()
		{
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			InvalidateWrapping();
			(int, int) cursor = GetWrappedPosition(base._cursorIndex);
			float cursorLeft = 0f;
			if (cursor.Item1 < _wrappedLines.Count)
			{
				string lineText = _wrappedLines[cursor.Item1].Text;
				if (cursor.Item2 <= lineText.Length)
				{
					cursorLeft = ((TextInputBase)this).MeasureStringWidth(lineText.Substring(0, cursor.Item2));
				}
			}
			return new Rectangle(_textRegion.X + (int)cursorLeft - 2, _textRegion.Y + cursor.Item1 * base._font.get_LineHeight() + 2, 2, base._font.get_LineHeight() - 4);
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			InvalidateWrapping();
			_textRegion = CalculateTextRegion();
			_highlightRegions = CalculateHighlightRegions();
			_cursorRegion = CalculateCursorRegion();
		}

		protected override void UpdateScrolling()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			if (!HideBackground)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(1, 1, bounds.Width - 2, bounds.Height - 2), Color.get_Black() * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(1, 0, bounds.Width - 2, 2), Color.get_Black() * 0.3f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(1, 0, bounds.Width - 2, 1), Color.get_Black() * 0.2f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 1, 2, bounds.Height - 2), Color.get_Black() * 0.3f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 1, 1, bounds.Height - 2), Color.get_Black() * 0.2f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(1, bounds.Height - 2, bounds.Width - 2, 2), Color.get_Black() * 0.3f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(1, bounds.Height - 2, bounds.Width - 2, 1), Color.get_Black() * 0.2f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.Width - 2, 1, 2, bounds.Height - 2), Color.get_Black() * 0.3f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.Width - 2, 1, 1, bounds.Height - 2), Color.get_Black() * 0.2f);
			}
			PaintWrappedText(spriteBatch, _textRegion);
			if (_highlightRegions.Length != 0)
			{
				Rectangle[] highlightRegions = _highlightRegions;
				foreach (Rectangle highlightRegion in highlightRegions)
				{
					((TextInputBase)this).PaintHighlight(spriteBatch, highlightRegion);
				}
			}
			else
			{
				((TextInputBase)this).PaintCursor(spriteBatch, _cursorRegion);
			}
		}

		private void PaintWrappedText(SpriteBatch spriteBatch, Rectangle textRegion)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (!base._focused && base._text.Length == 0)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, base._placeholderText, base._font, textRegion, Color.get_LightGray(), false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)0);
				return;
			}
			InvalidateWrapping();
			Rectangle lineRect = default(Rectangle);
			for (int i = 0; i < _wrappedLines.Count; i++)
			{
				WrappedLine line = _wrappedLines[i];
				((Rectangle)(ref lineRect))._002Ector(((Rectangle)(ref textRegion)).get_Left(), ((Rectangle)(ref textRegion)).get_Top() + i * base._font.get_LineHeight(), textRegion.Width, base._font.get_LineHeight());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, line.Text, base._font, lineRect, base._foreColor, false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
		}

		public int GetContentHeight()
		{
			InvalidateWrapping();
			return 14 + _wrappedLines.Count * base._font.get_LineHeight();
		}
	}
}
