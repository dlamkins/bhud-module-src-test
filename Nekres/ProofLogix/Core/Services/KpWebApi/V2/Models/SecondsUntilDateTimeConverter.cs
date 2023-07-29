using System;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	internal class SecondsUntilDateTimeConverter : JsonConverter<DateTime>
	{
		public override bool CanRead => true;

		public override bool CanWrite => false;

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (reader.get_ValueType() != typeof(long) || reader.get_Value() == null)
			{
				throw new JsonSerializationException("Failed to deserialize value to DateTime.");
			}
			return DateTime.UtcNow.AddSeconds((long)reader.get_Value());
		}

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
