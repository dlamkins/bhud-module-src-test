namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C2f84ebfd_002D199f_002D4f85_002D86e2_002D74616dbf3308_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
