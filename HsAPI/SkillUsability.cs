using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SkillUsability
	{
		UsableAir,
		UsableLand,
		UsableUnderWater,
		UsableWaterSurface
	}
}
