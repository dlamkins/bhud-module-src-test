namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessageAttribute : Attribute
	{
		public string Category { get; }

		public string CheckId { get; }

		public string? Scope { get; set; }

		public string? Target { get; set; }

		public string? MessageId { get; set; }

		public string? Justification { get; set; }

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessageAttribute(string category, string checkId)
		{
			Category = category;
			CheckId = checkId;
		}
	}
}
