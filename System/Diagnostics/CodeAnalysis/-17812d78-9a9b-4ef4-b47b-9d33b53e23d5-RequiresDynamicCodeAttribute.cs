namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	internal sealed class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCodeAttribute : Attribute
	{
		public string Message { get; }

		public string? Url { get; set; }

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCodeAttribute(string message)
		{
			Message = message;
		}
	}
}
