namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
