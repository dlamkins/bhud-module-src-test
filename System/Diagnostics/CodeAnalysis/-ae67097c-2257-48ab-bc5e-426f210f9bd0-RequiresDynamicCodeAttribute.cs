namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	internal sealed class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ERequiresDynamicCodeAttribute : Attribute
	{
		public string Message { get; }

		public string? Url { get; set; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ERequiresDynamicCodeAttribute(string message)
		{
			Message = message;
		}
	}
}
