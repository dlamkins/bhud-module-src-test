using System;

namespace Neokain.GW2.WebClient
{
	internal sealed class HubException : Exception
	{
		public HubException(string message)
			: base(message)
		{
		}
	}
}
