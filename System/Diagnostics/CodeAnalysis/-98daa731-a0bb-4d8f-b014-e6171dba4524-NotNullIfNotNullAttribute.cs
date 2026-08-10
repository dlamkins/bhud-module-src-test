namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
