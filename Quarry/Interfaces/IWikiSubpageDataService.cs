using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quarry.Models;

namespace Quarry.Interfaces
{
	public interface IWikiSubpageDataService
	{
		IReadOnlyDictionary<string, DerivedSubpage> ByLink { get; }

		Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
