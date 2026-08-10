using System;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionHeartbeatFeature
	{
		void OnHeartbeat(Action<object> action, object state);
	}
}
