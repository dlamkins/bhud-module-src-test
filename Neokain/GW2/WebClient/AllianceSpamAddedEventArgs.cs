using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamAddedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public SpamDetailDto Spam { get; }

		public AllianceSpamAddedEventArgs(Guid allianceId, SpamDetailDto spam)
		{
			AllianceId = allianceId;
			Spam = spam;
		}
	}
}
