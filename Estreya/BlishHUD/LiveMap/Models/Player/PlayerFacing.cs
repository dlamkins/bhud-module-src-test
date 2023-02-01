using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerFacing
	{
		[JsonPropertyName("angle")]
		public double Angle { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerFacing playerFacing = obj as PlayerFacing;
				if (playerFacing != null)
				{
					return true & (Angle == playerFacing.Angle);
				}
			}
			return false;
		}
	}
}
