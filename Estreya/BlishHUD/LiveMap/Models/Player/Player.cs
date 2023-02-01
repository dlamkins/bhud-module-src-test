using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class Player
	{
		[JsonPropertyName("identification")]
		public PlayerIdentification Identification { get; set; }

		[JsonPropertyName("position")]
		public PlayerPosition Position { get; set; }

		[JsonPropertyName("facing")]
		public PlayerFacing Facing { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				Player player = obj as Player;
				if (player != null)
				{
					return true & Identification.Equals(player.Identification) & Position.Equals(player.Position) & Facing.Equals(player.Facing);
				}
			}
			return false;
		}
	}
}
