namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMaybeNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMaybeNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
