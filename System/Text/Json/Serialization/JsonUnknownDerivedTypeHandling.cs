namespace System.Text.Json.Serialization
{
	internal enum JsonUnknownDerivedTypeHandling
	{
		FailSerialization,
		FallBackToBaseType,
		FallBackToNearestAncestor
	}
}
