namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ELibraryImportAttribute : Attribute
	{
		public string LibraryName { get; }

		public string EntryPoint { get; set; }

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003EStringMarshalling StringMarshalling { get; set; }

		public Type StringMarshallingCustomType { get; set; }

		public bool SetLastError { get; set; }

		public _003C49e42421_002D9c4a_002D4763_002Da398_002Da5192b431d03_003ELibraryImportAttribute(string libraryName)
		{
			LibraryName = libraryName;
		}
	}
}
