namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string? EntryPoint { get; set; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EStringMarshalling StringMarshalling { get; set; }

		public Type? StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
