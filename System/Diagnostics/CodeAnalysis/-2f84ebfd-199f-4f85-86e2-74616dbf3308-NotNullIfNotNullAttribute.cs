namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
