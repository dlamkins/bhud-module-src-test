namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public string[] Members { get; }

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullWhenAttribute(bool returnValue, string member)
		{
			ReturnValue = returnValue;
			Members = new string[1] { member };
		}

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EMemberNotNullWhenAttribute(bool returnValue, params string[] members)
		{
			ReturnValue = returnValue;
			Members = members;
		}
	}
}
