namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
