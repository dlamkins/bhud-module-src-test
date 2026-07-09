namespace rp.spark.Models.Api
{
	public class ProfileUploadRequest
	{
		public ProfileOwner Identity { get; set; } = new ProfileOwner();


		public ProfileData Profile { get; set; } = new ProfileData();


		public PlayerPresence Presence { get; set; } = new PlayerPresence();


		public static ProfileUploadRequest FromProfile(CharacterProfile profile, PlayerPresence presence)
		{
			return new ProfileUploadRequest
			{
				Identity = ((presence != null) ? ProfileOwner.FromPresence(presence) : ProfileOwner.FromProfile(profile)),
				Profile = ProfileData.FromProfile(profile),
				Presence = (presence ?? new PlayerPresence())
			};
		}
	}
}
