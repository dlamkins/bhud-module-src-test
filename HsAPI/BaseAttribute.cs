using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BaseAttribute
	{
		Power,
		Toughness,
		Vitality,
		Precision,
		Ferocity,
		ConditionDamage,
		Expertise,
		Concentration,
		HealingPower,
		AgonyResistance,
		FishingPower
	}
}
