using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quarry.Models;

namespace Quarry.Interfaces
{
	public interface IHereService
	{
		event Action CandidatesInvalidated;

		Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<HereResult> GetCandidatesAsync(int max, CancellationToken cancellationToken = default(CancellationToken));

		Task<IReadOnlyList<HereCandidate>> GetNearlyDoneAnywhereAsync(int max, CancellationToken cancellationToken = default(CancellationToken));

		Task<IReadOnlyList<HereCandidate>> GetHiddenAsync(int max, CancellationToken cancellationToken = default(CancellationToken));
	}
}
