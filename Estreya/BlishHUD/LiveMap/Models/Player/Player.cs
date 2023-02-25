using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class Player
	{
		[JsonPropertyName("identification")]
		public PlayerIdentification Identification { get; set; }

		[JsonPropertyName("map")]
		public PlayerMap Map { get; set; }

		[JsonPropertyName("facing")]
		public PlayerFacing Facing { get; set; }

		[JsonPropertyName("wvw")]
		public PlayerWvW WvW { get; set; }

		[JsonPropertyName("group")]
		public PlayerGroup Group { get; set; }

		[JsonPropertyName("commander")]
		public bool Commander { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				Player player = obj as Player;
				if (player != null)
				{
					return true & Identification.Equals(player.Identification) & Map.Equals(player.Map) & Facing.Equals(player.Facing) & Commander.Equals(player.Commander) & Group.Equals(player.Group) & WvW.Equals(player.WvW);
				}
			}
			return false;
		}
	}
}
