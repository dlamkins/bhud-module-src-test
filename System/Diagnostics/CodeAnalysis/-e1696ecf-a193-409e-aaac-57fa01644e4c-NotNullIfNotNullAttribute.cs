namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
