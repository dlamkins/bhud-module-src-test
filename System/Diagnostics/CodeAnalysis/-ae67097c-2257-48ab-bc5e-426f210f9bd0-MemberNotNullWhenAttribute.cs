namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public string[] Members { get; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullWhenAttribute(bool returnValue, string member)
		{
			ReturnValue = returnValue;
			Members = new string[1] { member };
		}

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EMemberNotNullWhenAttribute(bool returnValue, params string[] members)
		{
			ReturnValue = returnValue;
			Members = members;
		}
	}
}
