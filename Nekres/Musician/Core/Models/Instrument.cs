using System.Runtime.Serialization;

namespace Nekres.Musician.Core.Models
{
	public enum Instrument
	{
		[EnumMember(Value = "bass")]
		Bass,
		[EnumMember(Value = "bell")]
		Bell,
		[EnumMember(Value = "bell2")]
		Bell2,
		[EnumMember(Value = "flute")]
		Flute,
		[EnumMember(Value = "harp")]
		Harp,
		[EnumMember(Value = "horn")]
		Horn,
		[EnumMember(Value = "lute")]
		Lute
	}
}
