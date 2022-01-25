using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nekres.Regions_Of_Tyria
{
	public static class StringExtensions
	{
		public static IReadOnlyList<(Color, string)> FetchMarkupColoredText(this string input, Color? regularTextColor = null)
		{
			if (!regularTextColor.HasValue)
			{
				regularTextColor = Color.White;
			}
			if (string.IsNullOrEmpty(input))
			{
				return new List<(Color, string)> { (regularTextColor.Value, input) };
			}
			Regex regex = new Regex("<(c|color)=(#?((?'rgb'[a-fA-F0-9]{6})|(?'argb'[a-fA-F0-9]{8})))?>(?'text'.*?)<\\s*\\/\\s*\\1\\s*>", RegexOptions.Multiline);
			List<(Color, string)> lines = new List<(Color, string)>();
			int startIndex = 0;
			foreach (Match i in regex.Matches(input))
			{
				if (startIndex != i.Index)
				{
					lines.Add((regularTextColor.Value, input.Substring(startIndex, i.Index - startIndex)));
				}
				startIndex = i.Index + i.Length;
				Color color = Color.FromArgb(int.Parse(i.Groups["rgb"].Success ? ("FF" + i.Groups["rgb"].Value) : i.Groups["argb"].Value, NumberStyles.HexNumber));
				lines.Add((color, i.Groups["text"].Value));
			}
			if (startIndex != input.Length)
			{
				lines.Add((regularTextColor.Value, input.Substring(startIndex, input.Length - startIndex)));
			}
			return lines;
		}

		public static string StripMarkupLazy(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			return Regex.Replace(input, "<.*?>", string.Empty);
		}

		public static string StripMarkup(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			Regex matchingTags = new Regex("<\\s*([^ >]+)[^>]*>(?'text'.*?)<\\s*\\/\\s*\\1\\s*>", RegexOptions.Multiline);
			while (matchingTags.IsMatch(input))
			{
				Match match = matchingTags.Match(input);
				string text = match.Groups["text"].Value;
				input = input.Replace(match.Value, text);
			}
			return input;
		}
	}
}
