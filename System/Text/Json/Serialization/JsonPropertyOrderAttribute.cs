namespace System.Text.Json.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	internal sealed class JsonPropertyOrderAttribute : JsonAttribute
	{
		public int Order { get; }

		public JsonPropertyOrderAttribute(int order)
		{
			Order = order;
		}
	}
}
