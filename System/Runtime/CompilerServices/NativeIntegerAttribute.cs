using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	[Microsoft.CodeAnalysis.Embedded]
	[CompilerGenerated]
	internal sealed class NativeIntegerAttribute : Attribute
	{
		public readonly bool[] TransformFlags;

		public NativeIntegerAttribute()
		{
			TransformFlags = new bool[1] { true };
		}

		public NativeIntegerAttribute(bool[] A_0)
		{
			TransformFlags = A_0;
		}
	}
}
