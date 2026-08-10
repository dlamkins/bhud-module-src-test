namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	internal sealed class _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresDynamicCodeAttribute : Attribute
	{
		public string Message { get; }

		public string Url { get; set; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresDynamicCodeAttribute(string message)
		{
			Message = message;
		}
	}
}
