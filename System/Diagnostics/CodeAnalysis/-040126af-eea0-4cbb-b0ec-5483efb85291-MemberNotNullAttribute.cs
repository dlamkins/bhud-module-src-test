namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
