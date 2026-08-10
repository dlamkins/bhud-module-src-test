namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
