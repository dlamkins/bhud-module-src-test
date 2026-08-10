using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
	internal interface IOptionsChangeTokenSource<out TOptions>
	{
		string? Name { get; }

		IChangeToken GetChangeToken();
	}
}
