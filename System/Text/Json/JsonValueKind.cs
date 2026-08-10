namespace System.Text.Json
{
	internal enum JsonValueKind : byte
	{
		Undefined,
		Object,
		Array,
		String,
		Number,
		True,
		False,
		Null
	}
}
