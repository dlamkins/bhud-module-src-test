using System.Collections.Generic;
using Kenedia.Modules.Characters.Controls;

namespace Kenedia.Modules.Characters.Extensions
{
	public static class FilterTagListExtension
	{
		public static List<FilterTag> CreateFilterTagList(this List<string> strings)
		{
			List<FilterTag> list = new List<FilterTag>();
			foreach (string s in strings)
			{
				list.Add(new FilterTag
				{
					Tag = s
				});
			}
			return list;
		}
	}
}
