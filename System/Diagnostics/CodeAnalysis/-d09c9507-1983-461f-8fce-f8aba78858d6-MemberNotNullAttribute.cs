namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
