using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ModifierFlag
	{
		FormatFraction,
		FormatPercent,
		SkipNextEntry,
		MulByDuration,
		DivDurationBy3,
		DivDurationBy10,
		NonStacking,
		Subtract
	}
}
