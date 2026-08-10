namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
