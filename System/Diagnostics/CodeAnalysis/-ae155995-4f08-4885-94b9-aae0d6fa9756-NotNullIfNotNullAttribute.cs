namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
