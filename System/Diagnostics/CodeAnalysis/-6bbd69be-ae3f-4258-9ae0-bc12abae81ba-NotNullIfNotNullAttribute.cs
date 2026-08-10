namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
