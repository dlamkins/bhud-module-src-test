namespace System.Text.Json.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	internal sealed class JsonRequiredAttribute : JsonAttribute
	{
	}
}
