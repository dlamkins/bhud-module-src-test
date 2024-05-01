using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GameMode
	{
		Pve,
		Pvp,
		NotPvp,
		Wvw,
		Fractals
	}
}
