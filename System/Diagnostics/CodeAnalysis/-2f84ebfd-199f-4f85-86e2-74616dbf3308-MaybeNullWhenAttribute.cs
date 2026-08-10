namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003EMaybeNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003EMaybeNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
