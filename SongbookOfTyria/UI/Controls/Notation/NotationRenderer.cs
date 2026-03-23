using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SongbookOfTyria.UI.Utilities;

namespace SongbookOfTyria.UI.Controls.Notation
{
	public class NotationRenderer
	{
		private static readonly Logger Logger = Logger.GetLogger<NotationRenderer>();

		private static readonly Regex ColorTagRegex = new Regex("^<c[=:](#?[\\w]+)>(.*?)</c>", RegexOptions.Compiled);

		private static readonly Regex ColorTagNormalizeRegex = new Regex("(<c[=:][^>]+>)(.*?)(</c>)", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex BackgroundColorTagRegex = new Regex("^<bc[=:](#?[\\w]+)>(.*?)</bc>", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex BackgroundColorNormalizeRegex = new Regex("(<bc[=:][^>]+>)(.*?)(</bc>)", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex EmptyColorTagRegex = new Regex("<c[=:][^>]+></c>", RegexOptions.Compiled);

		private static readonly Regex EmptyBackgroundColorTagRegex = new Regex("<bc[=:][^>]+></bc>", RegexOptions.Compiled);

		private static readonly Regex ColorTagOpenRegex = new Regex("^<c[=:](#?[\\w]+)>", RegexOptions.Compiled);

		private static readonly Regex StripInnerColorTagsRegex = new Regex("<c[=:][^>]+>|</c>", RegexOptions.Compiled);

		private static BitmapFont _font16;

		private static BitmapFont _font18;

		private static BitmapFont _font20;

		private static BitmapFont _font22;

		private static BitmapFont _font24;

		private static BitmapFont _font26;

		private static BitmapFont _font28;

		private static BitmapFont _font30;

		private static BitmapFont _font32;

		private const int CharWidth16 = 9;

		private const int CharWidth18 = 10;

		private const int CharWidth20 = 11;

		private const int CharWidth22 = 12;

		private const int CharWidth24 = 14;

		private const int CharWidth26 = 15;

		private const int CharWidth28 = 16;

		private const int CharWidth30 = 17;

		private const int CharWidth32 = 18;

		private const int LineHeight16 = 22;

		private const int LineHeight18 = 25;

		private const int LineHeight20 = 27;

		private const int LineHeight22 = 30;

		private const int LineHeight24 = 33;

		private const int LineHeight26 = 35;

		private const int LineHeight28 = 38;

		private const int LinePadding = 0;

		private const int InitialYOffset = 10;

		private const int InitialXOffset = 15;

		private const int EnclosedAlphanumericExtraSpacing = 4;

		private const int ExtraCharacterSpacing = 1;

		private readonly NotationControl _notationControl;

		private readonly BitmapFont _currentFont;

		private readonly BitmapFont _largerFont;

		private readonly int _currentCharWidth;

		private readonly int _largerCharWidth;

		private readonly int _currentLineHeight;

		private readonly int _explicitWidth;

		public NotationControl Control => _notationControl;

		public static void InitializeFonts(ContentsManager contentsManager)
		{
			try
			{
				_font16 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-16.fnt", "fonts/EversonMono-16_0.png");
				_font18 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-18.fnt", "fonts/EversonMono-18_0.png");
				_font20 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-20.fnt", "fonts/EversonMono-20_0.png");
				_font22 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-22.fnt", "fonts/EversonMono-22_0.png");
				_font24 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-24.fnt", "fonts/EversonMono-24_0.png");
				_font26 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-26.fnt", "fonts/EversonMono-26_0.png");
				_font28 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-28.fnt", "fonts/EversonMono-28_0.png");
				_font30 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-30.fnt", "fonts/EversonMono-30_0.png");
				_font32 = BitmapFontLoader.Load(contentsManager, "fonts/EversonMono-32.fnt", "fonts/EversonMono-32_0.png");
				Logger.Info("Successfully loaded EversonMono fonts (16, 18, 20, 22, 24, 26, 28, 30, 32).");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load EversonMono fonts: {Message}, rendering will be skipped.", new object[1] { ex.Message });
			}
		}

		public NotationRenderer(Panel parentPanel, NotationFontSize fontSize, int width, int height)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			_explicitWidth = width;
			NotationControl notationControl = new NotationControl();
			((Control)notationControl).set_Parent((Container)(object)parentPanel);
			((Control)notationControl).set_Location(Point.get_Zero());
			((Control)notationControl).set_Size(new Point(width, height));
			_notationControl = notationControl;
			switch (fontSize)
			{
			case NotationFontSize.Size16:
				_currentFont = _font16;
				_currentCharWidth = 9;
				_currentLineHeight = 22;
				_largerFont = _font20;
				_largerCharWidth = 11;
				break;
			case NotationFontSize.Size18:
				_currentFont = _font18;
				_currentCharWidth = 10;
				_currentLineHeight = 25;
				_largerFont = _font22;
				_largerCharWidth = 12;
				break;
			case NotationFontSize.Size20:
				_currentFont = _font20;
				_currentCharWidth = 11;
				_currentLineHeight = 27;
				_largerFont = _font24;
				_largerCharWidth = 14;
				break;
			case NotationFontSize.Size22:
				_currentFont = _font22;
				_currentCharWidth = 12;
				_currentLineHeight = 30;
				_largerFont = _font26;
				_largerCharWidth = 15;
				break;
			case NotationFontSize.Size26:
				_currentFont = _font26;
				_currentCharWidth = 15;
				_currentLineHeight = 35;
				_largerFont = _font30;
				_largerCharWidth = 17;
				break;
			case NotationFontSize.Size28:
				_currentFont = _font28;
				_currentCharWidth = 16;
				_currentLineHeight = 38;
				_largerFont = _font32;
				_largerCharWidth = 18;
				break;
			default:
				_currentFont = _font24;
				_currentCharWidth = 14;
				_currentLineHeight = 33;
				_largerFont = _font28;
				_largerCharWidth = 16;
				break;
			}
		}

		public void Render(string notation)
		{
			if (string.IsNullOrEmpty(notation) || _currentFont == null || _largerFont == null)
			{
				return;
			}
			notation = notation.Replace('━', '─');
			notation = ColorTagNormalizeRegex.Replace(notation, delegate(Match match)
			{
				string value4 = match.Groups[1].Value;
				string value5 = match.Groups[2].Value;
				string value6 = match.Groups[3].Value;
				value5 = value5.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
				return value4 + value5 + value6;
			});
			notation = BackgroundColorNormalizeRegex.Replace(notation, delegate(Match match)
			{
				string value = match.Groups[1].Value;
				string value2 = match.Groups[2].Value;
				string value3 = match.Groups[3].Value;
				value2 = value2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
				return value + value2 + value3;
			});
			string previousNotation;
			do
			{
				previousNotation = notation;
				notation = EmptyColorTagRegex.Replace(notation, "");
				notation = EmptyBackgroundColorTagRegex.Replace(notation, "");
			}
			while (notation != previousNotation);
			string[] array = notation.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			int currentY = 10;
			string[] array2 = array;
			foreach (string line in array2)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					currentY += _currentLineHeight;
					continue;
				}
				currentY = RenderLine(line, currentY);
				currentY = currentY;
			}
			((Control)_notationControl).set_Width(_explicitWidth);
			((Control)_notationControl).set_Height(currentY + _currentLineHeight);
			((Control)_notationControl).Invalidate();
		}

		private int RenderLine(string line, int startY)
		{
			int currentX = 15;
			int currentY = startY;
			string remainingText = line;
			while (!string.IsNullOrEmpty(remainingText))
			{
				if (remainingText.Length <= 0 || remainingText[0] != '<' || (!TryRenderBackgroundColorTag(ref remainingText, ref currentX, ref currentY) && !TryRenderColorTag(ref remainingText, ref currentX, ref currentY, null)))
				{
					RenderPlainText(ref remainingText, ref currentX, ref currentY, null);
				}
			}
			return currentY + _currentLineHeight;
		}

		private bool TryRenderBackgroundColorTag(ref string remainingText, ref int currentX, ref int currentY)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Match match = BackgroundColorTagRegex.Match(remainingText);
			if (!match.Success)
			{
				return false;
			}
			string value = match.Groups[1].Value;
			string innerContent = match.Groups[2].Value;
			Color backgroundColor = NotationColorHelper.GetColorFromName(value);
			RenderContentWithBackground(innerContent, backgroundColor, ref currentX, ref currentY);
			remainingText = remainingText.Substring(match.Length);
			return true;
		}

		private void RenderContentWithBackground(string content, Color backgroundColor, ref int currentX, ref int currentY)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			string remainingContent = content;
			while (!string.IsNullOrEmpty(remainingContent))
			{
				if (remainingContent.Length <= 0 || remainingContent[0] != '<' || !TryRenderColorTag(ref remainingContent, ref currentX, ref currentY, backgroundColor))
				{
					RenderPlainText(ref remainingContent, ref currentX, ref currentY, backgroundColor);
				}
			}
		}

		private bool TryRenderColorTag(ref string remainingText, ref int currentX, ref int currentY, Color? backgroundColor)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!TryExtractBalancedColorTag(remainingText, out var colorName, out var content, out var totalLength))
			{
				return false;
			}
			content = StripInnerColorTagsRegex.Replace(content, "");
			Color color = NotationColorHelper.GetColorFromName(colorName);
			RenderTextSegment(content, color, ref currentX, ref currentY, backgroundColor);
			remainingText = remainingText.Substring(totalLength);
			return true;
		}

		private static bool TryExtractBalancedColorTag(string text, out string colorName, out string content, out int totalLength)
		{
			colorName = null;
			content = null;
			totalLength = 0;
			Match openMatch = ColorTagOpenRegex.Match(text);
			if (!openMatch.Success)
			{
				return false;
			}
			colorName = openMatch.Groups[1].Value;
			int searchStart = openMatch.Length;
			int depth = 1;
			int pos = searchStart;
			while (pos < text.Length && depth > 0)
			{
				int nextOpen = text.IndexOf("<c", pos, StringComparison.Ordinal);
				int nextClose = text.IndexOf("</c>", pos, StringComparison.Ordinal);
				if (nextClose == -1)
				{
					return false;
				}
				if (nextOpen != -1 && nextOpen < nextClose)
				{
					string potentialTag = text.Substring(nextOpen);
					if (ColorTagOpenRegex.IsMatch(potentialTag))
					{
						depth++;
					}
					pos = nextOpen + 2;
					continue;
				}
				depth--;
				if (depth == 0)
				{
					content = text.Substring(searchStart, nextClose - searchStart);
					totalLength = nextClose + 4;
					return true;
				}
				pos = nextClose + 4;
			}
			return false;
		}

		private void RenderPlainText(ref string remainingText, ref int currentX, ref int currentY, Color? backgroundColor)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			string plainText = ExtractPlainText(ref remainingText);
			if (!string.IsNullOrEmpty(plainText))
			{
				if (string.IsNullOrWhiteSpace(plainText) && !backgroundColor.HasValue)
				{
					currentX += plainText.Length * _currentCharWidth;
				}
				else
				{
					RenderTextSegment(plainText, Color.get_White(), ref currentX, ref currentY, backgroundColor);
				}
			}
		}

		private void RenderTextSegment(string text, Color color, ref int currentX, ref int currentY, Color? backgroundColor = null)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			int maxLineWidth = GetMaxLineWidth();
			string[] array = SplitIntoTokens(text);
			foreach (string token in array)
			{
				int tokenWidth = CalculateTokenWidth(token);
				if (currentX + tokenWidth > maxLineWidth && currentX > 15)
				{
					if (token == " ")
					{
						continue;
					}
					currentY += _currentLineHeight;
					currentX = 15;
				}
				string text2 = token;
				foreach (char c in text2)
				{
					RenderCharacter(c, color, ref currentX, currentY, backgroundColor);
				}
			}
		}

		private static string[] SplitIntoTokens(string text)
		{
			List<string> tokens = new List<string>();
			StringBuilder currentWord = new StringBuilder();
			foreach (char c in text)
			{
				if (c == ' ')
				{
					if (currentWord.Length > 0)
					{
						tokens.Add(currentWord.ToString());
						currentWord.Clear();
					}
					tokens.Add(" ");
				}
				else
				{
					currentWord.Append(c);
				}
			}
			if (currentWord.Length > 0)
			{
				tokens.Add(currentWord.ToString());
			}
			return tokens.ToArray();
		}

		private int CalculateTokenWidth(string token)
		{
			int width = 0;
			for (int i = 0; i < token.Length; i++)
			{
				width = ((!IsEnclosedAlphanumeric(token[i])) ? (width + (_currentCharWidth + 1)) : (width + (_largerCharWidth + 4)));
			}
			return width;
		}

		private void RenderCharacter(char c, Color color, ref int currentX, int currentY, Color? backgroundColor)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			string charStr = c.ToString();
			if (IsEnclosedAlphanumeric(c))
			{
				int verticalOffset = -2;
				_notationControl.AddSegment(charStr, _largerFont, color, currentX, currentY + verticalOffset, _largerCharWidth + 4, _currentLineHeight, backgroundColor);
				currentX += _largerCharWidth + 4;
			}
			else
			{
				_notationControl.AddSegment(charStr, _currentFont, color, currentX, currentY, _currentCharWidth + 1, _currentLineHeight, backgroundColor);
				currentX += _currentCharWidth + 1;
			}
		}

		private static bool IsEnclosedAlphanumeric(char c)
		{
			if (c >= '①')
			{
				return c <= '⓿';
			}
			return false;
		}

		private string ExtractPlainText(ref string remainingText)
		{
			int nextTagIndex = remainingText.IndexOf('<');
			if (nextTagIndex > 0)
			{
				string result = remainingText.Substring(0, nextTagIndex);
				remainingText = remainingText.Substring(nextTagIndex);
				return result;
			}
			if (nextTagIndex == -1)
			{
				string result2 = remainingText;
				remainingText = string.Empty;
				return result2;
			}
			string result3 = remainingText.Substring(0, 1);
			remainingText = remainingText.Substring(1);
			return result3;
		}

		private int GetMaxLineWidth()
		{
			return _explicitWidth - 15 - 20;
		}
	}
}
