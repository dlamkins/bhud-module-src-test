using System.Collections.Generic;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionItemsFeature
	{
		IDictionary<object, object?> Items { get; set; }
	}
}
