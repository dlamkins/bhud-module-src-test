namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
