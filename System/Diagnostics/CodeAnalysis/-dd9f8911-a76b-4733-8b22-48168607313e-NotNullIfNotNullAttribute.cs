namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
