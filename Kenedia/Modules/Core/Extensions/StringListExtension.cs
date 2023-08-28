using System.Collections.Generic;
using System.Text;

namespace Kenedia.Modules.Core.Extensions
{
	public static class StringListExtension
	{
		public static string Enumerate(this IList<string> list, string separator = ", ", string? enumerationFormat = null)
		{
			string enumFormat = enumerationFormat ?? "{0}";
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				sb.Append(string.Format(enumFormat, i + 1) + list[i]);
				if (i < list.Count - 1)
				{
					sb.Append(separator);
				}
			}
			return sb.ToString();
		}
	}
}
