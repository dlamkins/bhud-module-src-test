namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
