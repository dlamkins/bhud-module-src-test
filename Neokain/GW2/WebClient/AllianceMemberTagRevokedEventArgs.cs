using System;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceMemberTagRevokedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public Guid TagId { get; }

		public AllianceMemberTagRevokedEventArgs(Guid allianceId, Guid tagId)
		{
			AllianceId = allianceId;
			TagId = tagId;
		}
	}
}
