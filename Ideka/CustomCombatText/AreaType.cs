using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ideka.CustomCombatText
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AreaType
	{
		[EnumMember(Value = "Mx09eVdDRr2lAjaaEM-NhA")]
		Container,
		[EnumMember(Value = "PI0Kpa6NQxacdz7zKKkgQw")]
		Scroll,
		[EnumMember(Value = "sCweSx62RveRsLuuhmNqOA")]
		Top
	}
}
