using System;
using System.Net;

namespace Microsoft.AspNetCore.Connections
{
	internal class FileHandleEndPoint : EndPoint
	{
		public ulong FileHandle { get; }

		public FileHandleType FileHandleType { get; }

		public FileHandleEndPoint(ulong fileHandle, FileHandleType fileHandleType)
		{
			FileHandle = fileHandle;
			FileHandleType = fileHandleType;
			if ((uint)fileHandleType > 2u)
			{
				throw new NotSupportedException();
			}
		}
	}
}
