using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BuffType
	{
		Boon,
		Buff,
		Condition,
		BuffType3,
		Finisher,
		Food,
		Guild,
		Item,
		Persistent,
		Purchased,
		Species,
		Training,
		Trait,
		Transformation,
		Utility,
		Wvw,
		BuffType16,
		BuffType17,
		Realtime
	}
}
