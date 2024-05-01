using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProfessionState
	{
		None,
		ElementalistAttunementFire,
		ElementalistAttunementWater,
		ElementalistAttunementAir,
		ElementalistAttunementEarth,
		EngineerPhotonForge,
		NecromancerShroud,
		WarriorAdrenalineStage1,
		WarriorAdrenalineStage2,
		WarriorAdrenalineStage3,
		RangerDruid,
		RangerDruidCelestialAvatar,
		RangerSoulbeast,
		RevenantLegendDragon,
		RevenantLegendAssassin,
		RevenantLegendDwarf,
		RevenantLegendDemon,
		RevenantLegendRenegade,
		RevenantLegendCentaur,
		RevenantLegendAlliance
	}
}
