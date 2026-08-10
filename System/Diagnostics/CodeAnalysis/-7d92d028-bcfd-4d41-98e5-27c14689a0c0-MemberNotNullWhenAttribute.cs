namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EMemberNotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public string[] Members { get; }

		public _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EMemberNotNullWhenAttribute(bool returnValue, string member)
		{
			ReturnValue = returnValue;
			Members = new string[1] { member };
		}

		public _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EMemberNotNullWhenAttribute(bool returnValue, params string[] members)
		{
			ReturnValue = returnValue;
			Members = members;
		}
	}
}
