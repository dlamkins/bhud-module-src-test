using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerMap
	{
		[JsonPropertyName("continent")]
		public int Continent { get; set; }

		[JsonPropertyName("id")]
		public int ID { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("position")]
		public PlayerPosition Position { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerMap playerMap = obj as PlayerMap;
				if (playerMap != null)
				{
					return true & Continent.Equals(playerMap.Continent) & ID.Equals(playerMap.ID) & Name.Equals(playerMap.Name) & Position.Equals(playerMap.Position);
				}
			}
			return false;
		}
	}
}
