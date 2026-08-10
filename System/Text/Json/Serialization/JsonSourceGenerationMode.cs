namespace System.Text.Json.Serialization
{
	[Flags]
	internal enum JsonSourceGenerationMode
	{
		Default = 0x0,
		Metadata = 0x1,
		Serialization = 0x2
	}
}
