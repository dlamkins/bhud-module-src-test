namespace System.Text.Json.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	internal sealed class JsonIgnoreAttribute : JsonAttribute
	{
		public JsonIgnoreCondition Condition { get; set; } = JsonIgnoreCondition.Always;

	}
}
