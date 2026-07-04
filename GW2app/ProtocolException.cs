using System;

namespace GW2app
{
	internal class ProtocolException : Exception
	{
		public ProtocolException(string message)
			: base(message)
		{
		}
	}
}
