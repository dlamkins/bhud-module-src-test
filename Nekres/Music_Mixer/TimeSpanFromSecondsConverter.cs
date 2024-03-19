using System;
using Newtonsoft.Json;

namespace Nekres.Music_Mixer
{
	internal class TimeSpanFromSecondsConverter : JsonConverter<TimeSpan>
	{
		public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.get_Value() == null)
			{
				return TimeSpan.Zero;
			}
			string val = reader.get_Value() as string;
			if (val != null && long.TryParse(val, out var time))
			{
				return TimeSpan.FromSeconds(time);
			}
			return TimeSpan.FromSeconds((long)reader.get_Value());
		}
	}
}
