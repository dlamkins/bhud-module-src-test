using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamUsedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public SpamUsedNotificationDto Notification { get; }

		public AllianceSpamUsedEventArgs(Guid allianceId, SpamUsedNotificationDto notification)
		{
			AllianceId = allianceId;
			Notification = notification;
		}
	}
}
