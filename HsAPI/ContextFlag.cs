using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ContextFlag
	{
		Pve,
		Pvp,
		Wvw,
		Any
	}
}
