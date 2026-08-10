using System;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IStreamClosedFeature
	{
		void OnClosed(Action<object?> callback, object? state);
	}
}
