namespace System.Text.Json.Serialization.Metadata
{
	internal interface IJsonTypeInfoResolver
	{
		JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options);
	}
}
