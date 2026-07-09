namespace rp.spark.Models.Api
{
	public class ProfileReportRequest
	{
		public string AccountName { get; set; } = string.Empty;


		public string OfficialCharacterName { get; set; } = string.Empty;


		public string ProfileId { get; set; } = string.Empty;


		public string Reason { get; set; } = string.Empty;


		public static ProfileReportRequest FromProfile(CharacterProfile profile, PlayerPresence presence, string reason)
		{
			ProfileReportRequest profileReportRequest = new ProfileReportRequest();
			profileReportRequest.AccountName = TextUtil.FirstNonEmpty(presence?.AccountName, profile?.AccountName);
			profileReportRequest.OfficialCharacterName = TextUtil.FirstNonEmpty(presence?.OfficialCharacterName, profile?.CharacterName);
			profileReportRequest.ProfileId = TextUtil.FirstNonEmpty(presence?.ActiveProfileId, profile?.ProfileId);
			profileReportRequest.Reason = reason?.Trim() ?? string.Empty;
			return profileReportRequest;
		}
	}
}
