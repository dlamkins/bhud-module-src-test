using System.Linq;
using System.Text.Json.Serialization;

namespace Estreya.BlishHUD.LiveMap.Models.Player
{
	public class PlayerGroup
	{
		[JsonPropertyName("squad")]
		public string[] Squad { get; set; }

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				PlayerGroup playerGroup = obj as PlayerGroup;
				if (playerGroup != null)
				{
					return true & ((Squad != null && playerGroup.Squad != null) ? Squad.SequenceEqual(playerGroup.Squad) : (Squad == null && playerGroup.Squad == null));
				}
			}
			return false;
		}
	}
}
