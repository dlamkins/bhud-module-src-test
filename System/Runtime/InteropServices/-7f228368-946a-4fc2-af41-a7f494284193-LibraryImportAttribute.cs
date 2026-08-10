namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string? EntryPoint { get; set; }

		public _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EStringMarshalling StringMarshalling { get; set; }

		public Type? StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
