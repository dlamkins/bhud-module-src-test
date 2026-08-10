namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string? EntryPoint { get; set; }

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EStringMarshalling StringMarshalling { get; set; }

		public Type? StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
