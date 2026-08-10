using System;

namespace Neokain.GW2.WebClient
{
	public sealed class SpamFavoritesReorderedEventArgs : EventArgs
	{
		public Guid AccountId { get; }

		public SpamFavoritesReorderedEventArgs(Guid accountId)
		{
			AccountId = accountId;
		}
	}
}
