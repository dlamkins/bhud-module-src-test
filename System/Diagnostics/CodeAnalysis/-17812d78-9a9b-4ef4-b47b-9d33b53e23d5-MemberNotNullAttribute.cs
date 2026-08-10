namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
