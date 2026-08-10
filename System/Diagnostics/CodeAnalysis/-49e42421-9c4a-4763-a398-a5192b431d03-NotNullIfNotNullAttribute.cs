namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	internal sealed class _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ENotNullIfNotNullAttribute : Attribute
	{
		public string ParameterName { get; }

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ENotNullIfNotNullAttribute(string parameterName)
		{
			ParameterName = parameterName;
		}
	}
}
