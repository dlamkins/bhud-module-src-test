namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ENotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ENotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
