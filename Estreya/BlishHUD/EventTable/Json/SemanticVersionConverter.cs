using System;
using Newtonsoft.Json;
using SemanticVersioning;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class SemanticVersionConverter : JsonConverter<SemanticVersioning.Version>
	{
		public override SemanticVersioning.Version ReadJson(JsonReader reader, Type objectType, SemanticVersioning.Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (objectType != typeof(SemanticVersioning.Version))
			{
				return new SemanticVersioning.Version(0, 0, 0);
			}
			return SemanticVersioning.Version.Parse((string)reader.get_Value());
		}

		public override void WriteJson(JsonWriter writer, SemanticVersioning.Version value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}
