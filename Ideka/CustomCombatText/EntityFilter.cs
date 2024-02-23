using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ideka.CustomCombatText
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EntityFilter
	{
		[EnumMember(Value = "-aS4Lvy8Rd2bfsygi1Rzzg")]
		Any,
		[EnumMember(Value = "IrGj7gO6RNuxfxmuFcN43w")]
		TargetOnly,
		[EnumMember(Value = "NWtqNOkPSCKjpxUg_0T9Bg")]
		NonSelf
	}
}
