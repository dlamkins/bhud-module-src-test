namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
