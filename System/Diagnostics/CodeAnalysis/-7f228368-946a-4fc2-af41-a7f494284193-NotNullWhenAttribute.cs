namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003ENotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003ENotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
