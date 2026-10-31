using System;
using System.Collections.Generic;

namespace SeaStove.Gw2Tldr.Data
{
	public static class WrappedTextLayout
	{
		public sealed class Line
		{
			public int Start;

			public int End;
		}

		public static List<Line> Build(string text, float width, Func<string, float> measure)
		{
			text = text ?? "";
			List<Line> lines = new List<Line>();
			int start = 0;
			while (start < text.Length)
			{
				int end = start;
				int lastSpace = -1;
				while (end < text.Length && text[end] != '\n')
				{
					int next = end + ((!char.IsHighSurrogate(text[end]) || end + 1 >= text.Length || !char.IsLowSurrogate(text[end + 1])) ? 1 : 2);
					if (end > start && measure(text.Substring(start, next - start)) > width)
					{
						break;
					}
					end = next;
					if (char.IsWhiteSpace(text[end - 1]))
					{
						lastSpace = end;
					}
				}
				if (end < text.Length && text[end] != '\n' && lastSpace > start)
				{
					end = lastSpace;
				}
				lines.Add(new Line
				{
					Start = start,
					End = end
				});
				start = ((end < text.Length && text[end] == '\n') ? (end + 1) : end);
			}
			if (lines.Count == 0 || text.EndsWith("\n"))
			{
				lines.Add(new Line
				{
					Start = text.Length,
					End = text.Length
				});
			}
			return lines;
		}
	}
}
