namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string? EntryPoint { get; set; }

		public _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EStringMarshalling StringMarshalling { get; set; }

		public Type? StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
