using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamChangedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public SpamDetailDto Spam { get; }

		public AllianceSpamChangedEventArgs(Guid allianceId, SpamDetailDto spam)
		{
			AllianceId = allianceId;
			Spam = spam;
		}
	}
}
