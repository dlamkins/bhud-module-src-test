namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal sealed class _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EUnconditionalSuppressMessageAttribute : Attribute
	{
		public string Category { get; }

		public string CheckId { get; }

		public string Scope { get; set; }

		public string Target { get; set; }

		public string MessageId { get; set; }

		public string Justification { get; set; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EUnconditionalSuppressMessageAttribute(string category, string checkId)
		{
			Category = category;
			CheckId = checkId;
		}
	}
}
