using System;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamRemovedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public Guid SpamId { get; }

		public AllianceSpamRemovedEventArgs(Guid allianceId, Guid spamId)
		{
			AllianceId = allianceId;
			SpamId = spamId;
		}
	}
}
