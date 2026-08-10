namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
