using System;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.SignalR
{
	[Serializable]
	internal class HubException : Exception
	{
		public HubException()
		{
		}

		public HubException(string? message)
			: base(message)
		{
		}

		public HubException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
		public HubException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
