using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Flag
	{
		AimIkEnabled = 1,
		AllowPlayerControl,
		AnimateUpperBody,
		AutoFollow,
		BasicAttack,
		BreakStun,
		ClientCancelable,
		ControlsTurret,
		DecalAnchored,
		DoNotReplaceEffect,
		GroundFastCast,
		GroundIgnoreLos,
		GroundTargeted,
		HideLeftHandWeapon,
		HideRightHandWeapon,
		IgnoreEvasionTypeExtreme,
		IgnoreEvasionTypeFly,
		IgnoreEvasionTypeHop,
		IgnoreEvasionTypeSidestep,
		Instant,
		MultiHit,
		NonCombat,
		NotClientCancelable,
		NotCancelledByMovement,
		RequiresTarget,
		UsableAir,
		UsableLand,
		UsableOutOfCombat,
		UsableUnderWater,
		UsableWaterSurface,
		NoTarget,
		SkillFlag32
	}
}
