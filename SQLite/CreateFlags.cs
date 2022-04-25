using System;

namespace SQLite
{
	[Flags]
	public enum CreateFlags
	{
		None = 0x0,
		ImplicitPK = 0x1,
		ImplicitIndex = 0x2,
		AllImplicit = 0x3,
		AutoIncPK = 0x4,
		FullTextSearch3 = 0x100,
		FullTextSearch4 = 0x200
	}
}
