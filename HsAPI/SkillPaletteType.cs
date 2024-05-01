using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SkillPaletteType
	{
		Standard,
		Toolbelt,
		Bundle,
		Equipment,
		Heal,
		Elite,
		Profession,
		Monster,
		Transformation,
		Pet,
		Gathering,
		Reaction,
		MountSummon
	}
}
