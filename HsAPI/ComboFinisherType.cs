using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ComboFinisherType
	{
		Blast,
		Leap,
		Projectile,
		Projectile20,
		Whirl
	}
}
