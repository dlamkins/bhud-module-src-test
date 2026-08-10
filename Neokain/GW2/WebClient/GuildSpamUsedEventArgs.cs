using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamUsedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public SpamUsedNotificationDto Notification { get; }

		public GuildSpamUsedEventArgs(Guid guildId, SpamUsedNotificationDto notification)
		{
			GuildId = guildId;
			Notification = notification;
		}
	}
}
