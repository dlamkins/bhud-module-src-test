namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
