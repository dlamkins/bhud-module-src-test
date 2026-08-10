namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
