namespace System.Text.Json.Serialization.Converters
{
	internal sealed class UnsupportedTypeConverter<T> : JsonConverter<T>
	{
		private readonly string _errorMessage;

		public string ErrorMessage => _errorMessage ?? _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializeTypeInstanceNotSupported, typeof(T).FullName);

		public UnsupportedTypeConverter(string errorMessage = null)
		{
			_errorMessage = errorMessage;
		}

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotSupportedException(ErrorMessage);
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			throw new NotSupportedException(ErrorMessage);
		}
	}
}
