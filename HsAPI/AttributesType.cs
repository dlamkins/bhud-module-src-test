using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AttributesType
	{
		Level,
		Static
	}
}
