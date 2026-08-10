namespace System.Text.Json.Serialization.Converters
{
	internal sealed class VersionConverter : JsonPrimitiveConverter<Version>
	{
		public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}
			if (reader.TokenType != JsonTokenType.String)
			{
				ThrowHelper.ThrowInvalidOperationException_ExpectedString(reader.TokenType);
			}
			return ReadCore(ref reader);
		}

		private static Version ReadCore(ref Utf8JsonReader reader)
		{
			string @string = reader.GetString();
			if (!string.IsNullOrEmpty(@string) && (!char.IsDigit(@string[0]) || !char.IsDigit(@string[@string.Length - 1])))
			{
				ThrowHelper.ThrowFormatException(DataType.Version);
			}
			if (Version.TryParse(@string, out var result))
			{
				return result;
			}
			ThrowHelper.ThrowJsonException();
			return null;
		}

		public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options)
		{
			if ((object)value == null)
			{
				writer.WriteNullValue();
			}
			else
			{
				writer.WriteStringValue(value.ToString());
			}
		}

		internal override Version ReadAsPropertyNameCore(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return ReadCore(ref reader);
		}

		internal override void WriteAsPropertyNameCore(Utf8JsonWriter writer, Version value, JsonSerializerOptions options, bool isWritingExtensionDataProperty)
		{
			if ((object)value == null)
			{
				ThrowHelper.ThrowArgumentNullException("value");
			}
			writer.WritePropertyName(value.ToString());
		}
	}
}
