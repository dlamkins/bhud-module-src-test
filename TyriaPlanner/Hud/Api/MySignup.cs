using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class MySignup : EventBase
	{
		[JsonProperty("checkinReminderMinutes")]
		public int? CheckinReminderMinutes { get; set; }

		[JsonProperty("checkinStatus")]
		public string CheckinStatus { get; set; }

		[JsonProperty("approvalStatus")]
		public string ApprovalStatus { get; set; }

		[JsonProperty("isBench")]
		public bool IsBench { get; set; }

		[JsonProperty("signupCharacter")]
		public SignupCharacter SignupCharacter { get; set; }
	}
}
