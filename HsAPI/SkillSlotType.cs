using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SkillSlotType
	{
		[EnumMember(Value = "Weapon_1")]
		Weapon1,
		[EnumMember(Value = "Weapon_2")]
		Weapon2,
		[EnumMember(Value = "Weapon_3")]
		Weapon3,
		[EnumMember(Value = "Weapon_4")]
		Weapon4,
		[EnumMember(Value = "Weapon_5")]
		Weapon5,
		Heal,
		Utility,
		Elite,
		Pet,
		[EnumMember(Value = "Profession_1")]
		Profession1,
		[EnumMember(Value = "Profession_2")]
		Profession2,
		[EnumMember(Value = "Profession_3")]
		Profession3,
		[EnumMember(Value = "Profession_4")]
		Profession4,
		[EnumMember(Value = "Profession_5")]
		Profession5,
		[EnumMember(Value = "Transformation_1")]
		Transformation1,
		[EnumMember(Value = "Transformation_2")]
		Transformation2,
		[EnumMember(Value = "Transformation_3")]
		Transformation3,
		[EnumMember(Value = "Transformation_4")]
		Transformation4,
		[EnumMember(Value = "Transformation_5")]
		Transformation5,
		[EnumMember(Value = "Transformation_6")]
		Transformation6,
		[EnumMember(Value = "Transformation_7")]
		Transformation7,
		[EnumMember(Value = "Transformation_8")]
		Transformation8,
		[EnumMember(Value = "Transformation_9")]
		Transformation9,
		Gathering,
		MountSummon,
		Reaction
	}
}
