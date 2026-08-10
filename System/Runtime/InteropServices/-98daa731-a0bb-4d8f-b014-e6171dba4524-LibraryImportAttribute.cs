namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
