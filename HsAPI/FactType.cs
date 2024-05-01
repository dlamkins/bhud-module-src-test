using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FactType
	{
		AdjustByAttributeAndLevel,
		AttributeAdjust,
		Buff,
		BuffBrief,
		Distance,
		Number,
		Percent,
		PercentHpSelfDamage,
		PercentHealth,
		PercentLifeForceCost,
		PercentLifeForceGain,
		Damage,
		Time,
		ComboField,
		ComboFinisher,
		AttributeConversion,
		NoData,
		PrefixedBuff,
		PrefixedBuffBrief,
		Range,
		StunBreak
	}
}
