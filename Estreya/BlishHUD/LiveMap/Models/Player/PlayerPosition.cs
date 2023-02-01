using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerPosition
	{
		[JsonPropertyName("x")]
		public double X { get; set; }

		[JsonPropertyName("y")]
		public double Y { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerPosition playerPosition = obj as PlayerPosition;
				if (playerPosition != null)
				{
					return true & (X == playerPosition.X) & (Y == playerPosition.Y);
				}
			}
			return false;
		}
	}
}
