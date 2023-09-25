using System;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Converter
{
	public class SemverVersionConverter : JsonConverter<Version>
	{
		public override Version ReadJson(JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			if (reader.get_Value() != null)
			{
				return new Version((string)reader.get_Value(), false);
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, Version value, JsonSerializer serializer)
		{
			if (value != (Version)null)
			{
				writer.WriteValue(((object)value).ToString());
			}
		}
	}
}
