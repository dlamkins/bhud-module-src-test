namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
