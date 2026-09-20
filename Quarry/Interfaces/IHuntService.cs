using System;
using System.Collections.Generic;

namespace Quarry.Interfaces
{
	public interface IHuntService : IDisposable
	{
		IReadOnlyDictionary<int, List<string>> HuntEnabledNamespaces { get; }

		bool CanPeek { get; }

		void Peek(int achievementId);

		void RevertForCompletion(int achievementId);

		void RevertAllForUnload();
	}
}
