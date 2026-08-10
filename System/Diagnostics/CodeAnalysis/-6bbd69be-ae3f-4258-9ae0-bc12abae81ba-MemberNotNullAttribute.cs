namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
