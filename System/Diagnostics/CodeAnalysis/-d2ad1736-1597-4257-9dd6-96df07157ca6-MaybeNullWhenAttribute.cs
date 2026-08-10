namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EMaybeNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EMaybeNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
