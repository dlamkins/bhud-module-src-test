namespace rp.spark.Models.Api
{
	public class ProfileDownload
	{
		public ProfileOwner Identity { get; set; } = new ProfileOwner();


		public ProfileData Profile { get; set; } = new ProfileData();


		public PlayerPresence Presence { get; set; } = new PlayerPresence();


		public CharacterProfile ToCharacterProfile()
		{
			return Profile?.ToProfile(Identity, Presence) ?? new CharacterProfile();
		}
	}
}
