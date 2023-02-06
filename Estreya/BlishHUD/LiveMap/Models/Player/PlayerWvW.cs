using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerWvW
	{
		[JsonPropertyName("match")]
		public string Match { get; set; }

		[JsonPropertyName("teamColor")]
		public string TeamColor { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerWvW playerWvW = obj as PlayerWvW;
				if (playerWvW != null)
				{
					return true & (Match == playerWvW.Match) & (TeamColor == playerWvW.TeamColor);
				}
			}
			return false;
		}
	}
}
