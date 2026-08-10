using System;

namespace Microsoft.AspNetCore.Connections
{
	[Flags]
	internal enum TransferFormat
	{
		Binary = 0x1,
		Text = 0x2
	}
}
