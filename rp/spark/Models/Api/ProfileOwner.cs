namespace rp.spark.Models.Api
{
	public class ProfileOwner
	{
		public string AccountName { get; set; } = string.Empty;


		public string OfficialCharacterName { get; set; } = string.Empty;


		public string Race { get; set; } = string.Empty;


		public string Profession { get; set; } = string.Empty;


		public string Specialization { get; set; } = string.Empty;


		public ProfileRegion Region { get; set; }

		public bool IsCharacterApiVerified { get; set; }

		public static ProfileOwner FromProfile(CharacterProfile profile)
		{
			return new ProfileOwner
			{
				AccountName = (profile?.AccountName?.Trim() ?? string.Empty),
				OfficialCharacterName = (profile?.CharacterName?.Trim() ?? string.Empty),
				Race = (profile?.Race?.Trim() ?? string.Empty),
				Profession = (profile?.Profession?.Trim() ?? string.Empty),
				Specialization = (profile?.Specialization?.Trim() ?? string.Empty),
				Region = (profile?.Region ?? ProfileRegion.NA),
				IsCharacterApiVerified = (profile?.IsCharacterVerified ?? false)
			};
		}

		public static ProfileOwner FromPresence(PlayerPresence presence)
		{
			return new ProfileOwner
			{
				AccountName = (presence?.AccountName?.Trim() ?? string.Empty),
				OfficialCharacterName = (presence?.OfficialCharacterName?.Trim() ?? string.Empty),
				Race = (presence?.Race?.Trim() ?? string.Empty),
				Profession = (presence?.Profession?.Trim() ?? string.Empty),
				Region = (presence?.Region ?? ProfileRegion.NA),
				IsCharacterApiVerified = (presence?.IsVerified ?? false)
			};
		}
	}
}
