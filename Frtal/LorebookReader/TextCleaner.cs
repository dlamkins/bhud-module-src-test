using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Frtal.LorebookReader
{
	public static class TextCleaner
	{
		private static readonly Regex Vowels = new Regex("[aeiouAEIOU]");

		private static readonly Regex NonLetters = new Regex("[^A-Za-z']");

		private static bool IsValidWord(string w)
		{
			if (NonLetters.Replace(w, "").Length >= 2)
			{
				return Vowels.IsMatch(w);
			}
			return false;
		}

		private static bool IsGoodLine(string line)
		{
			string[] words = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 0)
			{
				return false;
			}
			return (double)words.Count(IsValidWord) / (double)words.Length >= 0.5;
		}

		public static string CleanForTts(string raw)
		{
			List<string> lines = (from l in raw.Split('\n')
				select l.Trim() into l
				where l.Length > 0
				select l).ToList();
			while (lines.Count > 0 && !IsGoodLine(lines[0]))
			{
				lines.RemoveAt(0);
			}
			while (lines.Count > 0 && !IsGoodLine(lines[lines.Count - 1]))
			{
				lines.RemoveAt(lines.Count - 1);
			}
			return TrimTrailingNoise(CleanInline(string.Join(" ", lines)));
		}

		public static string CleanForEncyclopedia(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
			{
				return "";
			}
			string[] array = Regex.Split(raw, "\\n[ \\t]*\\n");
			bool verse = LooksLikeVerse(raw);
			List<string> outParas = new List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				IEnumerable<string> plines = from l in array2[i].Split('\n')
					select l.Trim() into l
					where l.Length > 0
					select l;
				string joined = (verse ? string.Join("\n", from l in plines.Select(CleanInline)
					where l.Length > 0
					select l) : CleanInline(string.Join(" ", plines)));
				if (joined.Length > 0)
				{
					outParas.Add(joined);
				}
			}
			while (outParas.Count > 0 && !IsGoodLine(outParas[outParas.Count - 1]))
			{
				outParas.RemoveAt(outParas.Count - 1);
			}
			return string.Join("\n\n", outParas).Trim();
		}

		private static bool LooksLikeVerse(string raw)
		{
			List<string> lines = (from l in raw.Split('\n')
				select l.Trim() into l
				where l.Length > 0
				select l).ToList();
			if (lines.Count < 3)
			{
				return false;
			}
			if (lines.Count((string l) => Regex.IsMatch(l, "^([•·●○◦▪‣*\\-–—]|\\d{1,3}[.)])\\s")) >= 2)
			{
				return true;
			}
			int maxLen = lines.Max((string l) => l.Length);
			if (maxLen < 12)
			{
				return false;
			}
			return (double)lines.Count((string l) => (double)l.Length < (double)maxLen * 0.75) >= (double)lines.Count * 0.5;
		}

		private static string CleanInline(string text)
		{
			text = Regex.Replace(text, "\\s+", " ");
			text = Regex.Replace(text, "(?<![\\w])\\|(?=\\s+[a-z])", "I");
			text = Regex.Replace(text, "(?<![\\w])\\|(?![\\w])", " ");
			text = Regex.Replace(text, "(?<=[A-Za-z,.!?;:])11(?=\\s|$)", " ");
			text = Regex.Replace(text, "(?<!\\w)11(?=[A-Za-z])", " ");
			text = Regex.Replace(text, " {2,}", " ");
			text = Regex.Replace(text, "(?<!\\w)1(?=\\s+[a-z])", "I");
			text = Regex.Replace(text, "(?<![A-Za-z])J(?=\\s+[a-z])", "I");
			text = FixConfusableChars(text);
			return text.Trim();
		}

		private static string TrimTrailingNoise(string text)
		{
			Match i = Regex.Match(text, "^(.*[.!?][\"')\\]]*)(.*)$", RegexOptions.Singleline);
			if (i.Success)
			{
				string tail = i.Groups[2].Value.Trim();
				if (tail.Length > 0 && !Regex.IsMatch(tail, "[A-Za-z]{2,}"))
				{
					text = i.Groups[1].Value;
				}
			}
			return text.Trim();
		}

		private static string FixConfusableChars(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			StringBuilder result = new StringBuilder(text.Length);
			int i = 0;
			while (i < text.Length)
			{
				if (!char.IsLetterOrDigit(text[i]) && text[i] != '|')
				{
					result.Append(text[i]);
					i++;
					continue;
				}
				int start = i;
				for (; i < text.Length && (char.IsLetterOrDigit(text[i]) || text[i] == '|' || text[i] == '\''); i++)
				{
				}
				string word = text.Substring(start, i - start);
				if (IsNumericToken(word))
				{
					result.Append(word);
				}
				else if (start >= 1 && text[start - 1] == ':' && word.Length <= 2 && IsAllDigits(word))
				{
					result.Append(word);
				}
				else
				{
					result.Append(FixWord(word));
				}
			}
			return result.ToString();
		}

		private static string FixWord(string word)
		{
			StringBuilder sb = new StringBuilder(word.Length);
			for (int i = 0; i < word.Length; i++)
			{
				char c = word[i];
				char prev = ((i > 0) ? word[i - 1] : ' ');
				char next = ((i < word.Length - 1) ? word[i + 1] : ' ');
				switch (c)
				{
				case '0':
					if (HasLetterContext(prev, next))
					{
						if (i == 0)
						{
							sb.Append('O');
						}
						else
						{
							sb.Append(char.IsUpper(NearestLetter(word, i)) ? 'O' : 'o');
						}
					}
					else
					{
						sb.Append(c);
					}
					break;
				case '1':
					if (HasLetterContext(prev, next))
					{
						if (i == 0 || prev == ' ' || prev == '.' || prev == '!' || prev == '?')
						{
							sb.Append('I');
						}
						else
						{
							sb.Append('l');
						}
					}
					else
					{
						sb.Append(c);
					}
					break;
				case '|':
					if (i == 0 && char.IsLower(next))
					{
						sb.Append('I');
					}
					else if (char.IsLetter(prev) || char.IsLetter(next))
					{
						sb.Append('l');
					}
					else
					{
						sb.Append('I');
					}
					break;
				case '5':
					if (HasLetterContext(prev, next) && char.IsLetter(prev) && char.IsLetter(next))
					{
						sb.Append(char.IsUpper(prev) ? 'S' : 's');
					}
					else
					{
						sb.Append(c);
					}
					break;
				case '8':
					if (HasLetterContext(prev, next) && char.IsLetter(prev) && char.IsLetter(next))
					{
						sb.Append(char.IsUpper(prev) ? 'B' : 'b');
					}
					else
					{
						sb.Append(c);
					}
					break;
				default:
					sb.Append(c);
					break;
				}
			}
			return sb.ToString();
		}

		private static bool IsNumericToken(string word)
		{
			if (string.IsNullOrEmpty(word))
			{
				return false;
			}
			if (Regex.IsMatch(word, "^\\d+(st|nd|rd|th)$", RegexOptions.IgnoreCase))
			{
				return true;
			}
			if (Regex.IsMatch(word, "^[\\d,.']+$"))
			{
				return true;
			}
			return false;
		}

		private static bool IsAllDigits(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsDigit(s[i]))
				{
					return false;
				}
			}
			return s.Length > 0;
		}

		private static bool HasLetterContext(char prev, char next)
		{
			if (!char.IsLetter(prev))
			{
				return char.IsLetter(next);
			}
			return true;
		}

		private static char NearestLetter(string word, int pos)
		{
			for (int d = 1; d < word.Length; d++)
			{
				if (pos + d < word.Length && char.IsLetter(word[pos + d]))
				{
					return word[pos + d];
				}
				if (pos - d >= 0 && char.IsLetter(word[pos - d]))
				{
					return word[pos - d];
				}
			}
			return 'a';
		}

		private static bool IsVowel(char c)
		{
			c = char.ToLower(c);
			if (c != 'a' && c != 'e' && c != 'i' && c != 'o')
			{
				return c == 'u';
			}
			return true;
		}

		public static List<string> SplitChunks(string text, int maxLen = 200)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return new List<string>();
			}
			text = Regex.Replace(text, "\\s+", " ").Trim();
			string[] array = Regex.Split(text, "(?<=[.!?\\u2026])\\s+");
			List<string> phrases = new List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i].Trim();
				if (s.Length != 0)
				{
					if (s.Length <= maxLen)
					{
						phrases.Add(s);
					}
					else
					{
						SplitLongSentence(s, maxLen, phrases);
					}
				}
			}
			List<string> result = new List<string>();
			string buf = "";
			foreach (string p in phrases)
			{
				if (buf.Length == 0)
				{
					buf = p;
					continue;
				}
				if (buf.Length + 1 + p.Length <= maxLen)
				{
					buf = buf + " " + p;
					continue;
				}
				result.Add(buf);
				buf = p;
			}
			if (buf.Length > 0)
			{
				result.Add(buf);
			}
			return result;
		}

		private static void SplitLongSentence(string sent, int maxLen, List<string> output)
		{
			string[][] array = new string[5][]
			{
				new string[2] { ";\\s+", ":\\s+" },
				new string[2] { "\\s*[\\u2014\\u2013]\\s*", "\\s+--\\s+" },
				new string[5] { ",\\s+and\\s+", ",\\s+but\\s+", ",\\s+or\\s+", ",\\s+yet\\s+", ",\\s+so\\s+" },
				new string[5] { ",\\s+who\\s+", ",\\s+which\\s+", ",\\s+that\\s+", ",\\s+where\\s+", ",\\s+when\\s+" },
				new string[1] { ",\\s+" }
			};
			foreach (string[] patterns in array)
			{
				if (TrySplitAtPatterns(sent, patterns, maxLen, output))
				{
					return;
				}
			}
			ForceSplitByWords(sent, maxLen, output);
		}

		private static bool TrySplitAtPatterns(string sent, string[] patterns, int maxLen, List<string> output)
		{
			List<int> splits = new List<int>();
			foreach (string pattern in patterns)
			{
				foreach (Match i in Regex.Matches(sent, pattern, RegexOptions.IgnoreCase))
				{
					splits.Add(i.Index + i.Length);
				}
			}
			if (splits.Count == 0)
			{
				return false;
			}
			splits.Sort();
			List<string> fragments = new List<string>();
			int pos = 0;
			foreach (int splitAt in splits)
			{
				if (splitAt > pos && splitAt <= sent.Length)
				{
					string frag = sent.Substring(pos, splitAt - pos).Trim();
					if (frag.Length > 0)
					{
						fragments.Add(frag);
					}
					pos = splitAt;
				}
			}
			if (pos < sent.Length)
			{
				string rest = sent.Substring(pos).Trim();
				if (rest.Length > 0)
				{
					fragments.Add(rest);
				}
			}
			bool anyFits = false;
			foreach (string item in fragments)
			{
				if (item.Length <= maxLen)
				{
					anyFits = true;
					break;
				}
			}
			if (!anyFits)
			{
				return false;
			}
			foreach (string f in fragments)
			{
				if (f.Length <= maxLen)
				{
					output.Add(f);
				}
				else
				{
					SplitLongSentence(f, maxLen, output);
				}
			}
			return true;
		}

		private static void ForceSplitByWords(string sent, int maxLen, List<string> output)
		{
			string[] array = sent.Split(' ');
			string line = "";
			string[] array2 = array;
			foreach (string w in array2)
			{
				if (line.Length == 0)
				{
					line = w;
					continue;
				}
				if (line.Length + 1 + w.Length <= maxLen)
				{
					line = line + " " + w;
					continue;
				}
				output.Add(line);
				line = w;
			}
			if (line.Length > 0)
			{
				output.Add(line);
			}
		}

		public static string SanitizeForDisplay(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			StringBuilder sb = new StringBuilder(s.Length);
			foreach (char c in s)
			{
				switch (c)
				{
				case '–':
				case '—':
					sb.Append('-');
					continue;
				case '‘':
				case '’':
					sb.Append('\'');
					continue;
				case '“':
				case '”':
					sb.Append('"');
					continue;
				case '…':
					sb.Append("...");
					continue;
				}
				if ((c >= ' ' && c < '\u007f') || char.IsLetterOrDigit(c))
				{
					sb.Append(c);
				}
				else
				{
					sb.Append(' ');
				}
			}
			return sb.ToString();
		}
	}
}
