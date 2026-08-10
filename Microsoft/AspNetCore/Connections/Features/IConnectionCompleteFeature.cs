using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionCompleteFeature
	{
		void OnCompleted(Func<object, Task> callback, object state);
	}
}
