using System;
using System.Collections.Generic;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Controls
{
	internal class SparkMultiline : MultilineTextBox
	{
		private const int TextTopPadding = 7;

		private const int TextLeftPadding = 10;

		private const int ScrollbarWidth = 4;

		private const int ScrollbarInset = 3;

		private const int MinThumbHeight = 18;

		private const int WheelStepPixels = 42;

		private const int WrapPadding = 6;

		private const char NewLine = '\n';

		private string _displayText = string.Empty;

		private string[] _displayLines = new string[1] { string.Empty };

		private int[] _displayNewLineIndices = Array.Empty<int>();

		private Rectangle _textRegion = Rectangle.get_Empty();

		private Rectangle[] _highlightRegions = Array.Empty<Rectangle>();

		private Rectangle _cursorRegion = Rectangle.get_Empty();

		private int _verticalScrollOffset;

		private int _maxVerticalScrollOffset;

		private bool _isManualScroll;

		private int _manualScrollCaretIndex;

		private Container _wheelSource;

		public void AttachWheelSource(Container wheelSource)
		{
			if (_wheelSource != wheelSource)
			{
				if (_wheelSource != null)
				{
					((Control)_wheelSource).remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)HandleWheelScrolled);
				}
				_wheelSource = wheelSource;
				if (_wheelSource != null)
				{
					((Control)_wheelSource).add_MouseWheelScrolled((EventHandler<MouseEventArgs>)HandleWheelScrolled);
				}
			}
		}

		private void HandleWheelScrolled(object sender, MouseEventArgs e)
		{
			TryScrollWheel();
		}

		public SparkMultiline()
			: this()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				((Control)this).RecalculateLayout();
			});
		}

		protected override void DisposeControl()
		{
			AttachWheelSource(null);
			((TextInputBase)this).DisposeControl();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		protected override void MoveLine(int delta)
		{
			int newDisplayIndex = 0;
			string[] lines = _displayLines ?? new string[1] { string.Empty };
			(int Line, int Character) splitIndex = GetSplitIndex(((TextInputBase)this)._cursorIndex);
			int line = splitIndex.Line;
			int character = splitIndex.Character;
			int targetLine = line + delta;
			if (targetLine >= lines.Length)
			{
				newDisplayIndex = _displayText.Length;
			}
			else if (targetLine >= 0)
			{
				string sourceLine = lines[MathHelper.Clamp(line, 0, lines.Length - 1)];
				string targetLineText = lines[targetLine];
				int cursorCharacter = MathHelper.Clamp(character, 0, sourceLine.Length);
				float cursorLeft = ((TextInputBase)this).MeasureStringWidth(sourceLine.Substring(0, cursorCharacter));
				newDisplayIndex = GetLineStartIndex(targetLine) + GetCharacterFromX(targetLineText, cursorLeft);
			}
			((TextInputBase)this).UserSetCursorIndex(GetCursorIndexFromDisplayIndex(newDisplayIndex));
			((TextInputBase)this).UpdateSelectionIfShiftDown();
		}

		public override int GetCursorIndexFromPosition(int x, int y)
		{
			x -= 10;
			y -= 7;
			y += _verticalScrollOffset;
			string[] lines = _displayLines ?? new string[1] { string.Empty };
			int clickedLine = Math.Max(0, y / Math.Max(1, ((TextInputBase)this)._font.get_LineHeight()));
			if (clickedLine > lines.Length - 1)
			{
				return ((TextInputBase)this)._text.Length;
			}
			int displayIndex = GetLineStartIndex(clickedLine) + GetCharacterFromX(lines[clickedLine], x);
			return GetCursorIndexFromDisplayIndex(displayIndex);
		}

		public override void RecalculateLayout()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			_textRegion = CalculateTextRegion(reserveScrollbar: true);
			RebuildDisplayLayout();
			SetVerticalScrollOffset(_verticalScrollOffset, invalidate: false);
			((TextInputBase)this).UpdateScrolling();
			_highlightRegions = CalculateHighlightRegions();
			_cursorRegion = CalculateCursorRegion();
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			TryScrollWheel();
			((Control)this).OnMouseWheelScrolled(e);
		}

		private bool TryScrollWheel()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (_maxVerticalScrollOffset > 0 && ((Control)this).get_Visible())
			{
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(GameService.Input.get_Mouse().get_Position()))
				{
					MouseState state = GameService.Input.get_Mouse().get_State();
					int scrollValue = ((MouseState)(ref state)).get_ScrollWheelValue();
					if (scrollValue == 0)
					{
						return false;
					}
					return ScrollBy((scrollValue > 0) ? (-42) : 42);
				}
			}
			return false;
		}

		protected override void UpdateScrolling()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			if (!(_textRegion == Rectangle.get_Empty()) && _displayLines.Length != 0 && (!_isManualScroll || ((TextInputBase)this)._cursorIndex != _manualScrollCaretIndex))
			{
				_isManualScroll = false;
				int cursorTop = GetSplitIndex(((TextInputBase)this)._cursorIndex).Line * ((TextInputBase)this)._font.get_LineHeight();
				int cursorBottom = cursorTop + ((TextInputBase)this)._font.get_LineHeight();
				if (cursorTop < _verticalScrollOffset)
				{
					SetVerticalScrollOffset(cursorTop, invalidate: false);
				}
				else if (cursorBottom > _verticalScrollOffset + _textRegion.Height)
				{
					SetVerticalScrollOffset(cursorBottom - _textRegion.Height, invalidate: false);
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			PaintBackground(spriteBatch, bounds);
			PaintDisplayText(spriteBatch);
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
			PaintScrollbar(spriteBatch);
		}

		private void RebuildDisplayLayout()
		{
			_displayText = ProcessDisplayText(((TextInputBase)this)._text);
			_displayLines = _displayText.Split('\n');
			int contentHeight = Math.Max(((TextInputBase)this)._font.get_LineHeight(), _displayLines.Length * ((TextInputBase)this)._font.get_LineHeight());
			_maxVerticalScrollOffset = Math.Max(0, contentHeight - _textRegion.Height);
		}

		private string ProcessDisplayText(string value)
		{
			if (_textRegion.Width <= 0)
			{
				_displayNewLineIndices = Array.Empty<int>();
				return value ?? string.Empty;
			}
			int wrapWidth = Math.Max(1, _textRegion.Width - 6);
			return WrapText(((TextInputBase)this)._font, value ?? string.Empty, wrapWidth, out _displayNewLineIndices);
		}

		private int GetCursorIndexFromDisplayIndex(int displayIndex)
		{
			int cursorIndex = MathHelper.Clamp(displayIndex, 0, _displayText.Length);
			int[] displayNewLineIndices = _displayNewLineIndices;
			for (int i = 0; i < displayNewLineIndices.Length && displayNewLineIndices[i] <= displayIndex; i++)
			{
				cursorIndex--;
			}
			return MathHelper.Clamp(cursorIndex, 0, ((TextInputBase)this)._text.Length);
		}

		private int GetDisplayIndexFromCursorIndex(int cursorIndex)
		{
			int displayIndex = MathHelper.Clamp(cursorIndex, 0, ((TextInputBase)this)._text.Length);
			int[] displayNewLineIndices = _displayNewLineIndices;
			for (int i = 0; i < displayNewLineIndices.Length && displayNewLineIndices[i] <= displayIndex; i++)
			{
				displayIndex++;
			}
			return MathHelper.Clamp(displayIndex, 0, _displayText.Length);
		}

		private (int Line, int Character) GetSplitIndex(int index)
		{
			int displayIndex = GetDisplayIndexFromCursorIndex(index);
			int lineIndex = 0;
			int charIndex = 0;
			for (int i = 0; i < displayIndex && i < _displayText.Length; i++)
			{
				if (_displayText[i] == '\n')
				{
					lineIndex++;
					charIndex = 0;
					continue;
				}
				charIndex++;
				if (i < _displayText.Length - 1 && char.IsSurrogatePair(_displayText, i))
				{
					i++;
					charIndex++;
				}
			}
			return (MathHelper.Clamp(lineIndex, 0, Math.Max(0, _displayLines.Length - 1)), charIndex);
		}

		private Rectangle[] CalculateHighlightRegions()
		{
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			int selectionStart = Math.Min(((TextInputBase)this)._selectionStart, ((TextInputBase)this)._selectionEnd);
			int selectionLength = Math.Abs(((TextInputBase)this)._selectionStart - ((TextInputBase)this)._selectionEnd);
			if (selectionLength <= 0 || selectionStart + selectionLength > ((TextInputBase)this)._text.Length)
			{
				return Array.Empty<Rectangle>();
			}
			string[] lines = _displayLines ?? new string[1] { string.Empty };
			(int Line, int Character) splitIndex = GetSplitIndex(selectionStart);
			int startLine = splitIndex.Line;
			int startChar = splitIndex.Character;
			(int Line, int Character) splitIndex2 = GetSplitIndex(selectionStart + selectionLength);
			int endLine = splitIndex2.Line;
			int endChar = splitIndex2.Character;
			int lineSpanCount = endLine - startLine;
			Rectangle[] regions = (Rectangle[])(object)new Rectangle[lineSpanCount + 1];
			if (lineSpanCount == 0)
			{
				string line = lines[startLine];
				int startCharacter = MathHelper.Clamp(startChar, 0, line.Length);
				int endCharacter = MathHelper.Clamp(endChar, startCharacter, line.Length);
				float highlightLeft = ((TextInputBase)this).MeasureStringWidth(line.Substring(0, startCharacter));
				float highlightWidth = ((TextInputBase)this).MeasureStringWidth(line.Substring(startCharacter, endCharacter - startCharacter)) + 1f;
				regions[0] = new Rectangle(((Rectangle)(ref _textRegion)).get_Left() + (int)highlightLeft, GetLineTop(startLine), (int)highlightWidth, ((TextInputBase)this)._font.get_LineHeight() - 1);
			}
			else
			{
				string firstLine = lines[startLine];
				int firstCharacter = MathHelper.Clamp(startChar, 0, firstLine.Length);
				float firstLeft = ((TextInputBase)this).MeasureStringWidth(firstLine.Substring(0, firstCharacter));
				float firstWidth = ((TextInputBase)this).MeasureStringWidth(firstLine.Substring(firstCharacter)) + 1f;
				regions[0] = new Rectangle(((Rectangle)(ref _textRegion)).get_Left() + (int)firstLeft, GetLineTop(startLine), (int)firstWidth, ((TextInputBase)this)._font.get_LineHeight() - 1);
				for (int i = startLine + 1; i < endLine; i++)
				{
					float fullWidth = ((TextInputBase)this).MeasureStringWidth(lines[i]) + 1f;
					regions[i - startLine] = new Rectangle(((Rectangle)(ref _textRegion)).get_Left(), GetLineTop(i), (int)fullWidth, ((TextInputBase)this)._font.get_LineHeight() - 1);
				}
				string lastLine = lines[endLine];
				int lastCharacter = MathHelper.Clamp(endChar, 0, lastLine.Length);
				float lastWidth = ((TextInputBase)this).MeasureStringWidth(lastLine.Substring(0, lastCharacter)) + 1f;
				regions[lineSpanCount] = new Rectangle(((Rectangle)(ref _textRegion)).get_Left(), GetLineTop(endLine), (int)lastWidth, ((TextInputBase)this)._font.get_LineHeight() - 1);
			}
			return regions;
		}

		private Rectangle CalculateCursorRegion()
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			(int Line, int Character) splitIndex = GetSplitIndex(((TextInputBase)this)._cursorIndex);
			int cursorLine = splitIndex.Line;
			int cursorCharacter = splitIndex.Character;
			string[] lines = _displayLines ?? new string[1] { string.Empty };
			string line = lines[MathHelper.Clamp(cursorLine, 0, lines.Length - 1)];
			int character = MathHelper.Clamp(cursorCharacter, 0, line.Length);
			float cursorLeft = ((TextInputBase)this).MeasureStringWidth(line.Substring(0, character));
			return new Rectangle(_textRegion.X + (int)cursorLeft, GetLineTop(cursorLine) + 2, 2, ((TextInputBase)this)._font.get_LineHeight() - 4);
		}

		private Rectangle CalculateTextRegion(bool reserveScrollbar)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			int rightPadding = 20 + (reserveScrollbar ? 7 : 0);
			return new Rectangle(10, 7, ((Control)this)._size.X - rightPadding, ((Control)this)._size.Y - 14);
		}

		private int GetCharacterFromX(string line, float x)
		{
			line = line ?? string.Empty;
			if (x <= 0f || line.Length == 0)
			{
				return 0;
			}
			int characterIndex = 0;
			float previousWidth = 0f;
			while (characterIndex < line.Length)
			{
				int charCount = ((characterIndex >= line.Length - 1 || !char.IsSurrogatePair(line, characterIndex)) ? 1 : 2);
				int nextIndex = Math.Min(line.Length, characterIndex + charCount);
				float nextWidth = ((TextInputBase)this).MeasureStringWidth(line.Substring(0, nextIndex));
				float midpoint = previousWidth + (nextWidth - previousWidth) / 2f;
				if (x < midpoint)
				{
					break;
				}
				characterIndex = nextIndex;
				previousWidth = nextWidth;
			}
			return characterIndex;
		}

		private int GetLineStartIndex(int line)
		{
			int startIndex = 0;
			int targetLine = MathHelper.Clamp(line, 0, Math.Max(0, _displayLines.Length - 1));
			for (int i = 0; i < targetLine; i++)
			{
				startIndex += _displayLines[i].Length + 1;
			}
			return startIndex;
		}

		private int GetLineTop(int line)
		{
			return ((Rectangle)(ref _textRegion)).get_Top() + line * ((TextInputBase)this)._font.get_LineHeight() - _verticalScrollOffset;
		}

		private bool ScrollBy(int pixels)
		{
			bool num = SetVerticalScrollOffset(_verticalScrollOffset + pixels, invalidate: true);
			if (num)
			{
				_isManualScroll = true;
				_manualScrollCaretIndex = ((TextInputBase)this)._cursorIndex;
			}
			return num;
		}

		private bool SetVerticalScrollOffset(int value, bool invalidate)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			int clampedValue = MathHelper.Clamp(value, 0, _maxVerticalScrollOffset);
			if (_verticalScrollOffset == clampedValue)
			{
				return false;
			}
			_verticalScrollOffset = clampedValue;
			_highlightRegions = CalculateHighlightRegions();
			_cursorRegion = CalculateCursorRegion();
			if (invalidate)
			{
				((Control)this).Invalidate();
			}
			return true;
		}

		private void PaintBackground(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			if (!((MultilineTextBox)this).get_HideBackground())
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
		}

		private void PaintDisplayText(SpriteBatch spriteBatch)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			if (!((TextInputBase)this)._focused && string.IsNullOrEmpty(((TextInputBase)this)._text))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((TextInputBase)this)._placeholderText, ((TextInputBase)this)._font, _textRegion, Color.get_LightGray(), false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)0);
				return;
			}
			string[] displayLines = _displayLines ?? Array.Empty<string>();
			if (displayLines.Length == 0)
			{
				return;
			}
			int lineHeight = Math.Max(1, ((TextInputBase)this)._font.get_LineHeight());
			int firstVisibleLine = Math.Max(0, _verticalScrollOffset / lineHeight);
			int lastVisibleLine = Math.Min(displayLines.Length - 1, (_verticalScrollOffset + _textRegion.Height) / lineHeight + 1);
			if (firstVisibleLine > lastVisibleLine)
			{
				return;
			}
			for (int i = firstVisibleLine; i <= lastVisibleLine; i++)
			{
				int lineTop = GetLineTop(i);
				if (lineTop + lineHeight >= ((Rectangle)(ref _textRegion)).get_Top() && lineTop <= ((Rectangle)(ref _textRegion)).get_Bottom())
				{
					string lineText = displayLines[i] ?? string.Empty;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, lineText, ((TextInputBase)this)._font, new Rectangle(_textRegion.X, lineTop, _textRegion.Width, lineHeight), ((TextInputBase)this)._foreColor, false, false, 0, (HorizontalAlignment)0, (VerticalAlignment)0);
				}
			}
		}

		private void PaintScrollbar(SpriteBatch spriteBatch)
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			int contentHeight = Math.Max(((TextInputBase)this)._font.get_LineHeight(), _displayLines.Length * ((TextInputBase)this)._font.get_LineHeight());
			if (_maxVerticalScrollOffset > 0 && contentHeight > 0)
			{
				int trackHeight = _textRegion.Height;
				Rectangle track = default(Rectangle);
				((Rectangle)(ref track))._002Ector(((Control)this)._size.X - 3 - 4, ((Rectangle)(ref _textRegion)).get_Top(), 4, trackHeight);
				float visibleRatio = MathHelper.Clamp((float)_textRegion.Height / (float)contentHeight, 0f, 1f);
				int thumbHeight = MathHelper.Clamp((int)((float)trackHeight * visibleRatio), 18, trackHeight);
				int thumbTravel = Math.Max(0, trackHeight - thumbHeight);
				float scrollRatio = ((_maxVerticalScrollOffset == 0) ? 0f : ((float)_verticalScrollOffset / (float)_maxVerticalScrollOffset));
				Rectangle thumb = default(Rectangle);
				((Rectangle)(ref thumb))._002Ector(track.X, track.Y + (int)((float)thumbTravel * scrollRatio), 4, thumbHeight);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), track, Color.get_Black() * 0.35f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), thumb, new Color(210, 210, 210, 150));
			}
		}

		private static string WrapText(BitmapFont spriteFont, string text, float maxLineWidth, out int[] newLineIndices)
		{
			newLineIndices = Array.Empty<int>();
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			StringBuilder wrappedText = new StringBuilder();
			List<int> wrapIndices = new List<int>();
			int sourceOffset = 0;
			string[] lines = text.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				wrappedText.Append(WrapTextSegment(spriteFont, lines[i], maxLineWidth, out var segmentWrapIndices));
				int indexOffset = sourceOffset + wrapIndices.Count;
				int[] array = segmentWrapIndices;
				foreach (int segmentIndex in array)
				{
					wrapIndices.Add(segmentIndex + indexOffset);
				}
				sourceOffset += lines[i].Length;
				if (i < lines.Length - 1)
				{
					wrappedText.Append('\n');
					sourceOffset++;
				}
			}
			newLineIndices = wrapIndices.ToArray();
			return wrappedText.ToString();
		}

		private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth, out int[] newLineIndices)
		{
			newLineIndices = Array.Empty<int>();
			if (string.IsNullOrEmpty(text) || maxLineWidth <= 0f)
			{
				return text ?? string.Empty;
			}
			string[] words = text.Split(' ');
			StringBuilder sb = new StringBuilder();
			List<int> indices = new List<int>();
			float lineWidth = 0f;
			float spaceWidth = MeasureStringWidth(spriteFont, " ");
			int sourceOffset = 0;
			for (int i = 0; i < words.Length; i++)
			{
				string word = words[i];
				float wordWidth = MeasureStringWidth(spriteFont, word);
				if (lineWidth > 0f && lineWidth + wordWidth > maxLineWidth)
				{
					if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
					{
						sb[sb.Length - 1] = '\n';
					}
					else
					{
						sb.Append('\n');
						indices.Add(sourceOffset + indices.Count);
					}
					lineWidth = 0f;
				}
				sb.Append(word);
				lineWidth += wordWidth;
				sourceOffset += word.Length;
				if (i < words.Length - 1)
				{
					sb.Append(' ');
					lineWidth += spaceWidth;
					sourceOffset++;
				}
			}
			newLineIndices = indices.ToArray();
			return sb.ToString();
		}

		private static float MeasureStringWidth(BitmapFont spriteFont, string text)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return spriteFont.MeasureString(text ?? string.Empty).Width;
		}
	}
}
