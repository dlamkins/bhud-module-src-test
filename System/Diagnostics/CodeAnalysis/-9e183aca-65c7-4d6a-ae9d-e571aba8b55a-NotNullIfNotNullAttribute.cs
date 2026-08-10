namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
