using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ideka.CustomCombatText
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EventResult
	{
		[EnumMember(Value = "1DcrUSv8QpKhW0AJ5OBaDg")]
		Strike,
		[EnumMember(Value = "KSinNkd4EEifSJLHR-HFpg")]
		Crit,
		[EnumMember(Value = "Tr6954TcQG-0hq2HRwzqGA")]
		Glance,
		[EnumMember(Value = "KYwIP2__CUqbTq2_TTospQ")]
		Block,
		[EnumMember(Value = "zSIAvAY9lUKySRTexRrAgQ")]
		Evade,
		[EnumMember(Value = "PmLrIl1Zsk-cz5sOw3Jlkw")]
		Invuln,
		[EnumMember(Value = "1uHogwDh_0Chd8rxwVikXA")]
		Miss,
		[EnumMember(Value = "YSfSsoHOmEyP7rePsCsu0A")]
		Bleeding,
		[EnumMember(Value = "MExazeLNnEeA4_SS68fZ_A")]
		Burning,
		[EnumMember(Value = "UaDPUpkrFk6olYhHgEVDwA")]
		Poison,
		[EnumMember(Value = "JHqBj1MRsEaDl69OMFwxYg")]
		Confusion,
		[EnumMember(Value = "y6sJZ6TIvkSnNbB7GfyD5g")]
		Torment,
		[EnumMember(Value = "p5MbQ3xISauRbPuaEzHxNQ")]
		DamageTick,
		[EnumMember(Value = "a00CCNo3RECpwniLnuVo0g")]
		Heal,
		[EnumMember(Value = "9wriGSeNQ12qEJ1ZRo4R0Q")]
		Barrier,
		[EnumMember(Value = "Qq5274QvQ6-4qqoZOwni0w")]
		HealTick,
		[EnumMember(Value = "9UxBDq9TSceVn_Tncf2sDA")]
		Interrupt,
		[EnumMember(Value = "eAqruxsURl-GaMAx3z_ORA")]
		Breakbar
	}
}
