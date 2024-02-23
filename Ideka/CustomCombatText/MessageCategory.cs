using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ideka.CustomCombatText
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MessageCategory
	{
		[EnumMember(Value = "VIEa0rBA-kO5lpsiQNTRXA")]
		PlayerOut,
		[EnumMember(Value = "WEVcwa9dg0iluzuWIfPfag")]
		PlayerIn,
		[EnumMember(Value = "2r3hGh4P3UGpxAEY28VuAw")]
		PetOut,
		[EnumMember(Value = "DoXIlFuRS0On0WMwgGh5bQ")]
		PetIn
	}
}
