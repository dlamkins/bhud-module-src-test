namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
