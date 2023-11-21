using System;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class SemanticVersionConverter : JsonConverter<Version>
	{
		public override Version ReadJson(JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			if (!(objectType != typeof(Version)))
			{
				return new Version((string)reader.Value, false);
			}
			return new Version(0, 0, 0, (string)null, (string)null);
		}

		public override void WriteJson(JsonWriter writer, Version value, JsonSerializer serializer)
		{
			writer.WriteValue(((object)value).ToString());
		}
	}
}
