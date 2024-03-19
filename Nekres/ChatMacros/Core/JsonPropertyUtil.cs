using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nekres.ChatMacros.Core
{
	internal static class JsonPropertyUtil
	{
		public static string GetPropertyFromJson(string json, string propertyPath)
		{
			try
			{
				JToken token = JToken.Parse(json);
				string[] array2 = propertyPath.Split('.');
				foreach (string segment in array2)
				{
					if (int.TryParse(segment, out var index))
					{
						JArray array = (JArray)(object)((token is JArray) ? token : null);
						if (array != null)
						{
							if (index >= 0 && index < ((JContainer)array).get_Count())
							{
								token = array.get_Item(index);
								continue;
							}
							return string.Empty;
						}
					}
					JObject obj = (JObject)(object)((token is JObject) ? token : null);
					if (obj == null || !obj.TryGetValue(segment, ref token))
					{
						return string.Empty;
					}
				}
				JValue jValue = (JValue)(object)((token is JValue) ? token : null);
				if (jValue != null)
				{
					return jValue.ToString((IFormatProvider)CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
			catch (JsonReaderException)
			{
				return string.Empty;
			}
		}
	}
}
