using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections.Abstractions
{
	internal interface IStatefulReconnectFeature
	{
		void OnReconnected(Func<PipeWriter, Task> notifyOnReconnect);

		void DisableReconnect();
	}
}
