using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Nekres.ChatMacros.Core
{
	internal static class StringExtensions
	{
		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		public static IEnumerable<string> Split(this string input, string delimiter)
		{
			return input.Split(new string[1] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static IEnumerable<string> ReadLines(this string input)
		{
			using StringReader sr = new StringReader(input);
			while (true)
			{
				string line = sr.ReadLine();
				if (line != null)
				{
					yield return line;
					continue;
				}
				break;
			}
		}

		public static string GetTextBetweenTags(this string input, string tagName)
		{
			Match match = Regex.Match(input, "<" + tagName + ">(.*?)</" + tagName + ">");
			if (!match.Success || match.Groups.Count <= 1)
			{
				return string.Empty;
			}
			return match.Groups[1].Value;
		}

		public static bool IsWebLink(this string uri)
		{
			if (Uri.TryCreate(uri, UriKind.Absolute, out var uriResult))
			{
				if (!(uriResult.Scheme == Uri.UriSchemeHttp))
				{
					return uriResult.Scheme == Uri.UriSchemeHttps;
				}
				return true;
			}
			return false;
		}

		public static bool IsPathFullyQualified(this string path)
		{
			string root = Path.GetPathRoot(path);
			if (!root.StartsWith("\\\\"))
			{
				if (root.EndsWith("\\"))
				{
					return root != "\\";
				}
				return false;
			}
			return true;
		}

		public static string TrimStart(this string input, int count)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			while (count > 0 && input.Length > 0 && input.StartsWith(" "))
			{
				input = input.Substring(1);
				count--;
			}
			return input;
		}

		public static string Replace(this string text, string search, string replace, int count, bool ignoreCase = true)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			StringComparison comparison = (ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
			int pos = text.IndexOf(search, comparison);
			while (pos > -1 && count > 0)
			{
				text = text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
				pos = text.IndexOf(search, comparison);
				count--;
			}
			return text;
		}
	}
}
