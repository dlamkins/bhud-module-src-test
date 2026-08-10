namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
