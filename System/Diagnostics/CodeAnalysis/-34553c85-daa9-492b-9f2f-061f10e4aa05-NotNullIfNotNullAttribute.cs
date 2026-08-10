namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
