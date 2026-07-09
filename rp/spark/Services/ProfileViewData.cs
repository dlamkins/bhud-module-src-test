using rp.spark.Models;

namespace rp.spark.Services
{
	public sealed class ProfileViewData
	{
		public CharacterProfile Profile { get; }

		public PlayerPresence Presence { get; }

		public ProfileViewData(CharacterProfile profile, PlayerPresence presence)
		{
			Profile = profile ?? new CharacterProfile();
			Presence = presence ?? new PlayerPresence();
		}
	}
}
