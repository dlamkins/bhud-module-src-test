using System.Runtime.Serialization;

namespace Blish_HUD.Extended
{
	public enum TyrianTime
	{
		[EnumMember(Value = "none")]
		None,
		[EnumMember(Value = "dawn")]
		Dawn,
		[EnumMember(Value = "day")]
		Day,
		[EnumMember(Value = "dusk")]
		Dusk,
		[EnumMember(Value = "night")]
		Night
	}
}
