using System;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class SemanticRangeConverter : JsonConverter<Range>
	{
		public override Range ReadJson(JsonReader reader, Type objectType, Range existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if (objectType != typeof(Range))
			{
				return null;
			}
			return new Range((string)reader.get_Value(), false);
		}

		public override void WriteJson(JsonWriter writer, Range value, JsonSerializer serializer)
		{
			writer.WriteValue(((object)value).ToString());
		}
	}
}
