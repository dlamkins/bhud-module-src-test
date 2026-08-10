using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamCategoryAddedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public SpamCategoryDto Category { get; }

		public AllianceSpamCategoryAddedEventArgs(Guid allianceId, SpamCategoryDto category)
		{
			AllianceId = allianceId;
			Category = category;
		}
	}
}
