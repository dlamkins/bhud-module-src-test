using System;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceBanRemovedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public Guid BanId { get; }

		public AllianceBanRemovedEventArgs(Guid allianceId, Guid banId)
		{
			AllianceId = allianceId;
			BanId = banId;
		}
	}
}
