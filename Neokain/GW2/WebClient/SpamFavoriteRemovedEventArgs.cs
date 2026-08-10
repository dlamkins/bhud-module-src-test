using System;

namespace Neokain.GW2.WebClient
{
	public sealed class SpamFavoriteRemovedEventArgs : EventArgs
	{
		public Guid AccountId { get; }

		public Guid FavoriteId { get; }

		public SpamFavoriteRemovedEventArgs(Guid accountId, Guid favoriteId)
		{
			AccountId = accountId;
			FavoriteId = favoriteId;
		}
	}
}
