namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
