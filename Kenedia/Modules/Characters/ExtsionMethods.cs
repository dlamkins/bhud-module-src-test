using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kenedia.Modules.Characters
{
	public static class ExtsionMethods
	{
		public static bool IsValidJson(this string strInput)
		{
			if (string.IsNullOrWhiteSpace(strInput))
			{
				return false;
			}
			strInput = strInput.Trim();
			if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || (strInput.StartsWith("[") && strInput.EndsWith("]")))
			{
				try
				{
					JToken.Parse(strInput);
					return true;
				}
				catch (JsonReaderException ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
				catch (Exception ex2)
				{
					Console.WriteLine(ex2.ToString());
					return false;
				}
			}
			return false;
		}
	}
}
