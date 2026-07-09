namespace rp.spark.Services
{
	public class PlayerState
	{
		public bool IsMumbleAvailable { get; set; }

		public bool IsInGame { get; set; }

		public string OfficialCharacterName { get; set; } = string.Empty;


		public string Race { get; set; } = string.Empty;


		public string Profession { get; set; } = string.Empty;


		public string Specialization { get; set; } = string.Empty;


		public int MapId { get; set; }

		public string LocationName { get; set; } = string.Empty;


		public bool IsLocationResolved { get; set; } = true;


		public string AccountName { get; set; } = string.Empty;


		public bool IsCharacterApiVerified { get; set; }

		public bool HasCharactersPermission { get; set; }

		public bool CanEditProfile
		{
			get
			{
				if (IsMumbleAvailable && IsInGame)
				{
					return !string.IsNullOrWhiteSpace(OfficialCharacterName);
				}
				return false;
			}
		}
	}
}
