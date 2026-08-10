namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
