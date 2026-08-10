namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
