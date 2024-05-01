using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ComboFieldType
	{
		Air,
		Dark,
		Fire,
		Ice,
		Light,
		Lightning,
		Poison,
		Smoke,
		Ethereal,
		Water
	}
}
