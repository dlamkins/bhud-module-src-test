using System.Runtime.Serialization;

namespace KillProofModule.Models
{
	public enum Mode
	{
		[EnumMember(Value = "none")]
		None,
		[EnumMember(Value = "fractal")]
		Fractal,
		[EnumMember(Value = "raid")]
		Raid
	}
}
