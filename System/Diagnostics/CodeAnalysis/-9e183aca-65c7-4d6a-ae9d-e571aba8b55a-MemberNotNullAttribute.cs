namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
