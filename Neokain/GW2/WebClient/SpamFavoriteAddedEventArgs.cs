using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class SpamFavoriteAddedEventArgs : EventArgs
	{
		public Guid AccountId { get; }

		public SpamFavoriteDto Favorite { get; }

		public SpamFavoriteAddedEventArgs(Guid accountId, SpamFavoriteDto favorite)
		{
			AccountId = accountId;
			Favorite = favorite;
		}
	}
}
