namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	internal sealed class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EStringSyntaxAttribute : Attribute
	{
		public const string CompositeFormat = "CompositeFormat";

		public const string DateOnlyFormat = "DateOnlyFormat";

		public const string DateTimeFormat = "DateTimeFormat";

		public const string EnumFormat = "EnumFormat";

		public const string GuidFormat = "GuidFormat";

		public const string Json = "Json";

		public const string NumericFormat = "NumericFormat";

		public const string Regex = "Regex";

		public const string TimeOnlyFormat = "TimeOnlyFormat";

		public const string TimeSpanFormat = "TimeSpanFormat";

		public const string Uri = "Uri";

		public const string Xml = "Xml";

		public string Syntax { get; }

		public object[] Arguments { get; }

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EStringSyntaxAttribute(string syntax)
		{
			Syntax = syntax;
			Arguments = Array.Empty<object>();
		}

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EStringSyntaxAttribute(string syntax, params object[] arguments)
		{
			Syntax = syntax;
			Arguments = arguments;
		}
	}
}
