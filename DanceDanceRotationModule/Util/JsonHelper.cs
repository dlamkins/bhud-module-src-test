using System;
using System.Linq;

namespace DanceDanceRotationModule.Util
{
	public static class JsonHelper
	{
		public static string FormatJson(string json, string indent = "  ")
		{
			int indentation = 0;
			int quoteCount = 0;
			int escapeCount = 0;
			return string.Concat((from ch in json ?? string.Empty
				let escaped = ((ch == '\\') ? escapeCount++ : ((escapeCount > 0) ? escapeCount-- : escapeCount)) > 0
				let quotes = (ch == '"' && !escaped) ? quoteCount++ : quoteCount
				let unquoted = quotes % 2 == 0
				let colon = (ch == ':' && unquoted) ? ": " : null
				let nospace = (char.IsWhiteSpace(ch) && unquoted) ? string.Empty : null
				let lineBreak = (ch == ',' && unquoted) ? string.Concat(ch.ToString(), Environment.NewLine, string.Concat(Enumerable.Repeat(indent, indentation))) : null
				let openChar = ((ch == '{' || ch == '[') && unquoted) ? string.Concat(ch.ToString(), Environment.NewLine, string.Concat(Enumerable.Repeat(indent, ++indentation))) : ch.ToString()
				select new
				{
					_003C_003Eh__TransparentIdentifier6 = _003C_003Eh__TransparentIdentifier6,
					closeChar = (((ch == '}' || ch == ']') && unquoted) ? string.Concat(Environment.NewLine, string.Concat(Enumerable.Repeat(indent, --indentation)), ch.ToString()) : ch.ToString())
				}).Select(_003C_003Eh__TransparentIdentifier7 =>
			{
				string text = _003C_003Eh__TransparentIdentifier7._003C_003Eh__TransparentIdentifier6._003C_003Eh__TransparentIdentifier5._003C_003Eh__TransparentIdentifier4._003C_003Eh__TransparentIdentifier3.colon;
				if (text == null)
				{
					text = _003C_003Eh__TransparentIdentifier7._003C_003Eh__TransparentIdentifier6._003C_003Eh__TransparentIdentifier5._003C_003Eh__TransparentIdentifier4.nospace;
					if (text == null)
					{
						text = _003C_003Eh__TransparentIdentifier7._003C_003Eh__TransparentIdentifier6._003C_003Eh__TransparentIdentifier5.lineBreak;
						if (text == null)
						{
							if (_003C_003Eh__TransparentIdentifier7._003C_003Eh__TransparentIdentifier6.openChar.Length <= 1)
							{
								return _003C_003Eh__TransparentIdentifier7.closeChar;
							}
							text = _003C_003Eh__TransparentIdentifier7._003C_003Eh__TransparentIdentifier6.openChar;
						}
					}
				}
				return text;
			}));
		}
	}
}
