namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMaybeNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMaybeNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
