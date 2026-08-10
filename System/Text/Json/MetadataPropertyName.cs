namespace System.Text.Json
{
	[Flags]
	internal enum MetadataPropertyName : byte
	{
		None = 0x0,
		Values = 0x1,
		Id = 0x2,
		Ref = 0x4,
		Type = 0x8
	}
}
