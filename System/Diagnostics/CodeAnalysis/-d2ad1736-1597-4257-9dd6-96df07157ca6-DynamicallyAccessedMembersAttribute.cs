namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, Inherited = false)]
	internal sealed class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembersAttribute : Attribute
	{
		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes MemberTypes { get; }

		public _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembersAttribute(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes memberTypes)
		{
			MemberTypes = memberTypes;
		}
	}
}
