namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003EMemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003EMemberNotNullAttribute(string member)
		{
			Members = new string[1] { member };
		}

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003EMemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
}
