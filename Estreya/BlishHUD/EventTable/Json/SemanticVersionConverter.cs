using System;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class SemanticVersionConverter : JsonConverter<SemVer.Version>
	{
		public override SemVer.Version ReadJson(JsonReader reader, Type objectType, SemVer.Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (objectType != typeof(SemVer.Version))
			{
				return new SemVer.Version(0, 0, 0);
			}
			return new SemVer.Version((string)reader.get_Value());
		}

		public override void WriteJson(JsonWriter writer, SemVer.Version value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}
