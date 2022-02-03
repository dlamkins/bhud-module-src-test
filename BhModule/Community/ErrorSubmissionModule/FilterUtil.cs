using System;
using System.Collections.Generic;
using System.IO;

namespace BhModule.Community.ErrorSubmissionModule
{
	public static class FilterUtil
	{
		private const string FILTER_PATTERN = "#{0}#";

		private static readonly List<Func<string, string>> _filters = new List<Func<string, string>> { FilterUser };

		public static string FilterAll(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return data;
			}
			for (int i = 0; i < _filters.Count; i++)
			{
				data = _filters[i](data);
			}
			return data;
		}

		private static string FilterUser(string data)
		{
			string actualUsername = Environment.UserName;
			data = data.ReplaceUsingStringComparison(actualUsername, string.Format("#{0}#", "USERNAME"), StringComparison.InvariantCultureIgnoreCase);
			string profileUsername = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
			data = data.ReplaceUsingStringComparison(profileUsername, string.Format("#{0}#", "PROFILENAME"), StringComparison.InvariantCultureIgnoreCase);
			return data;
		}
	}
}
