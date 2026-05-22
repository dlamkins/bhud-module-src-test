using System;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class PendingApproval
	{
		[JsonProperty("signupId")]
		public string SignupId { get; set; }

		[JsonProperty("eventId")]
		public string EventId { get; set; }

		[JsonProperty("eventTitle")]
		public string EventTitle { get; set; }

		[JsonProperty("eventType")]
		public string EventType { get; set; }

		[JsonProperty("scheduledAt")]
		public DateTime ScheduledAt { get; set; }

		[JsonProperty("guildName")]
		public string GuildName { get; set; }

		[JsonProperty("guildTag")]
		public string GuildTag { get; set; }

		[JsonProperty("applicantUsername")]
		public string ApplicantUsername { get; set; }

		[JsonProperty("applicantDisplayName")]
		public string ApplicantDisplayName { get; set; }

		[JsonProperty("applicantAccountName")]
		public string ApplicantAccountName { get; set; }

		[JsonProperty("characterName")]
		public string CharacterName { get; set; }

		[JsonProperty("characterProfession")]
		public string CharacterProfession { get; set; }

		[JsonProperty("characterEliteSpec")]
		public string CharacterEliteSpec { get; set; }

		[JsonProperty("defaultRoleKey")]
		public string DefaultRoleKey { get; set; }

		[JsonProperty("note")]
		public string Note { get; set; }
	}
}
