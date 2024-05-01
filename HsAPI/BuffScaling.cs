using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BuffScaling
	{
		BuffLevelLinear,
		ConditionDamage,
		ConditionDamageSquared,
		Ferocity,
		FerocitySquared,
		BuffFormulaType5,
		NoScaling,
		HealingPower,
		HealingPowerSquared,
		SpawnScaleLinear,
		TargetLevelLinear,
		BuffFormulaType11,
		InfiniteDungeonScale,
		Power,
		PowerSquared,
		BuffFormulaType15
	}
}
