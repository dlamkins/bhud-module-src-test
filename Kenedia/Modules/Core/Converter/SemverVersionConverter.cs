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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if ((int)reader.get_TokenType() == 11)
			{
				return null;
			}
			if ((int)reader.get_TokenType() == 9)
			{
				string s = (string)reader.get_Value();
				if (string.IsNullOrWhiteSpace(s))
				{
					return new Version(0, 0, 0, (string)null, (string)null);
				}
				return new Version(s, false);
			}
			throw new JsonSerializationException($"Unexpected token {reader.get_TokenType()} when parsing Version.");
		}
	}
}
