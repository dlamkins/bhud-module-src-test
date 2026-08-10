using System;
using Neokain.GW2.WebClient.Models.Bans;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceBanAddedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public BanDto Ban { get; }

		public AllianceBanAddedEventArgs(Guid allianceId, BanDto ban)
		{
			AllianceId = allianceId;
			Ban = ban;
		}
	}
}
