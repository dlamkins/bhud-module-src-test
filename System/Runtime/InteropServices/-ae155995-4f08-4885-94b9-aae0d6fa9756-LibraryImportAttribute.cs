namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string? EntryPoint { get; set; }

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EStringMarshalling StringMarshalling { get; set; }

		public Type? StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
