namespace System.Diagnostics.CodeAnalysis
{
	[Flags]
	internal enum _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes
	{
		None = 0x0,
		PublicParameterlessConstructor = 0x1,
		PublicConstructors = 0x3,
		NonPublicConstructors = 0x4,
		PublicMethods = 0x8,
		NonPublicMethods = 0x10,
		PublicFields = 0x20,
		NonPublicFields = 0x40,
		PublicNestedTypes = 0x80,
		NonPublicNestedTypes = 0x100,
		PublicProperties = 0x200,
		NonPublicProperties = 0x400,
		PublicEvents = 0x800,
		NonPublicEvents = 0x1000,
		Interfaces = 0x2000,
		All = -1
	}
}
