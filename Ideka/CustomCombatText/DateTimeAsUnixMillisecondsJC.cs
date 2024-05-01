using System;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public class DateTimeAsUnixMillisecondsJC : JsonConverter<DateTime>
	{
		public static long To(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeMilliseconds();
		}

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return DateTimeOffset.FromUnixTimeMilliseconds(serializer.Deserialize<long>(reader)).UtcDateTime;
		}

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, To(value));
		}
	}
}
