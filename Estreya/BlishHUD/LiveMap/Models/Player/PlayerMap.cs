using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerMap
	{
		[JsonPropertyName("continent")]
		public int Continent { get; set; }

		[JsonPropertyName("position")]
		public PlayerPosition Position { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerMap playerMap = obj as PlayerMap;
				if (playerMap != null)
				{
					return true & Continent.Equals(playerMap.Continent) & (Position?.Equals(playerMap.Position) ?? (Position == null && playerMap.Position == null));
				}
			}
			return false;
		}
	}
}
