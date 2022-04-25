using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nekres.Musician.Core.Domain.Converters
{
	internal class ChordOffsetConverter : JsonConverter<IEnumerable<ChordOffset>>
	{
		public override void WriteJson(JsonWriter writer, IEnumerable<ChordOffset> value, JsonSerializer serializer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ChordOffset chordOffset in value)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(chordOffset.ToString());
			}
			writer.WriteValue(stringBuilder.ToString());
		}

		public override IEnumerable<ChordOffset> ReadJson(JsonReader reader, Type objectType, IEnumerable<ChordOffset> existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.Value != null)
			{
				return ChordOffset.MelodyFromString((string)reader.Value);
			}
			return null;
		}
	}
}
