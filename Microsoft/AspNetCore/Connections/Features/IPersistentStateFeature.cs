using System.Collections.Generic;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IPersistentStateFeature
	{
		IDictionary<object, object?> State { get; }
	}
}
