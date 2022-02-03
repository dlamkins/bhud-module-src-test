using System;
using System.Text;

namespace BhModule.Community.ErrorSubmissionModule
{
	public static class StringUtil
	{
		public static string ReplaceUsingStringComparison(this string str, string oldValue, string newValue, StringComparison comparisonType)
		{
			StringBuilder resultStringBuilder = new StringBuilder(str.Length);
			bool isReplacementNullOrEmpty = string.IsNullOrEmpty(newValue);
			int startSearchFromIndex = 0;
			int foundAt;
			while ((foundAt = str.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != -1)
			{
				int charsUntilReplacment = foundAt - startSearchFromIndex;
				if (charsUntilReplacment != 0)
				{
					resultStringBuilder.Append(str, startSearchFromIndex, charsUntilReplacment);
				}
				if (!isReplacementNullOrEmpty)
				{
					resultStringBuilder.Append(newValue);
				}
				startSearchFromIndex = foundAt + oldValue.Length;
				if (startSearchFromIndex == str.Length)
				{
					return resultStringBuilder.ToString();
				}
			}
			int charsUntilStringEnd = str.Length - startSearchFromIndex;
			resultStringBuilder.Append(str, startSearchFromIndex, charsUntilStringEnd);
			return resultStringBuilder.ToString();
		}
	}
}
