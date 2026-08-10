namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EMaybeNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EMaybeNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
