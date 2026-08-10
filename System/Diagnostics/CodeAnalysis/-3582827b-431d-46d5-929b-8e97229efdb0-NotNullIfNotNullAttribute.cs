namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
