namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
