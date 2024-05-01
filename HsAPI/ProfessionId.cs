using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProfessionId
	{
		Guardian = 1,
		Warrior,
		Engineer,
		Ranger,
		Thief,
		Elementalist,
		Mesmer,
		Necromancer,
		Revenant
	}
}
