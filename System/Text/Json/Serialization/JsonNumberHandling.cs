namespace System.Text.Json.Serialization
{
	[Flags]
	internal enum JsonNumberHandling
	{
		Strict = 0x0,
		AllowReadingFromString = 0x1,
		WriteAsString = 0x2,
		AllowNamedFloatingPointLiterals = 0x4
	}
}
