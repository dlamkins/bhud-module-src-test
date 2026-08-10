using System.Collections.Generic;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionMetricsTagsFeature
	{
		ICollection<KeyValuePair<string, object?>> Tags { get; }
	}
}
