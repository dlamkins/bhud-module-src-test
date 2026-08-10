namespace System.Text.Json
{
	internal enum JsonTokenType : byte
	{
		None,
		StartObject,
		EndObject,
		StartArray,
		EndArray,
		PropertyName,
		Comment,
		String,
		Number,
		True,
		False,
		Null
	}
}
