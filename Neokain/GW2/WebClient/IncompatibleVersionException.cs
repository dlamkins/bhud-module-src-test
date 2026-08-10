using System;

namespace Neokain.GW2.WebClient
{
	internal sealed class IncompatibleVersionException : Exception
	{
		public IncompatibleVersionException(string message)
			: base(message)
		{
		}
	}
}
