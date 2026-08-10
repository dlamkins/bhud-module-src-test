namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ENotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ENotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
}
