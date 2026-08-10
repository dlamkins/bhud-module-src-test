namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
