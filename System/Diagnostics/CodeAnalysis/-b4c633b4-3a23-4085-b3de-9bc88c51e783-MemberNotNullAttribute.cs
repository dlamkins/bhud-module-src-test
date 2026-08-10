namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
