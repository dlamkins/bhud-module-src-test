namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	internal sealed class _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute : Attribute
	{
		public string MemberSignature { get; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes MemberTypes { get; }

		public Type Type { get; }

		public string TypeName { get; }

		public string AssemblyName { get; }

		public string Condition { get; set; }

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute(string memberSignature)
		{
			MemberSignature = memberSignature;
		}

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute(string memberSignature, Type type)
		{
			MemberSignature = memberSignature;
			Type = type;
		}

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute(string memberSignature, string typeName, string assemblyName)
		{
			MemberSignature = memberSignature;
			TypeName = typeName;
			AssemblyName = assemblyName;
		}

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes memberTypes, Type type)
		{
			MemberTypes = memberTypes;
			Type = type;
		}

		public _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicDependencyAttribute(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes memberTypes, string typeName, string assemblyName)
		{
			MemberTypes = memberTypes;
			TypeName = typeName;
			AssemblyName = assemblyName;
		}
	}
}
