using System.Runtime.Serialization;

namespace felix.BlishEmotes
{
	public enum Category
	{
		[EnumMember(Value = "greeting")]
		Greeting,
		[EnumMember(Value = "reaction")]
		Reaction,
		[EnumMember(Value = "fun")]
		Fun,
		[EnumMember(Value = "pose")]
		Pose,
		[EnumMember(Value = "dance")]
		Dance,
		[EnumMember(Value = "miscellaneous")]
		Miscellaneous
	}
}
