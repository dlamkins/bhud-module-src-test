namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
