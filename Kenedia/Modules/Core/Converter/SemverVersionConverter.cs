using System;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Converter
{
	public class SemverVersionConverter : JsonConverter<Version>
	{
		public override void WriteJson(JsonWriter writer, Version value, JsonSerializer serializer)
		{
			writer.WriteValue(((object)value)?.ToString());
		}

		public override Version ReadJson(JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			if (reader.TokenType == JsonToken.String)
			{
				string s = (string)reader.Value;
				if (string.IsNullOrWhiteSpace(s))
				{
					return new Version(0, 0, 0, (string)null, (string)null);
				}
				return new Version(s, false);
			}
			throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing Version.");
		}
	}
}
