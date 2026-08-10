namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public string[] Members { get; }

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullWhenAttribute(bool returnValue, string member)
		{
			ReturnValue = returnValue;
			Members = new string[1] { member };
		}

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EMemberNotNullWhenAttribute(bool returnValue, params string[] members)
		{
			ReturnValue = returnValue;
			Members = members;
		}
	}
}
