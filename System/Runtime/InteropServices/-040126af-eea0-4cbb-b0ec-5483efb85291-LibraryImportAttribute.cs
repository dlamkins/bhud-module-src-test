namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
