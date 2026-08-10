namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
