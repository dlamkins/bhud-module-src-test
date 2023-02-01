using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerIdentification
	{
		[JsonPropertyName("account")]
		public string Account { get; set; }

		[JsonPropertyName("character")]
		public string Character { get; set; }

		[JsonPropertyName("guild")]
		public string GuildId { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerIdentification playerIdentification = obj as PlayerIdentification;
				if (playerIdentification != null)
				{
					return true & (Account == playerIdentification.Account) & (Character == playerIdentification.Character) & (GuildId == playerIdentification.GuildId);
				}
			}
			return false;
		}
	}
}
