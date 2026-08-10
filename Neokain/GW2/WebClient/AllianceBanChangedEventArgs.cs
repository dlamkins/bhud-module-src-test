using System;
using Neokain.GW2.WebClient.Models.Bans;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceBanChangedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public BanDto Ban { get; }

		public AllianceBanChangedEventArgs(Guid allianceId, BanDto ban)
		{
			AllianceId = allianceId;
			Ban = ban;
		}
	}
}
