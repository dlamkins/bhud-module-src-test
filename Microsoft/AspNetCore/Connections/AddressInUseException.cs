using System;

namespace Microsoft.AspNetCore.Connections
{
	internal class AddressInUseException : InvalidOperationException
	{
		public AddressInUseException(string message)
			: base(message)
		{
		}

		public AddressInUseException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
