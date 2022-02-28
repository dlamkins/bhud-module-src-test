using System.Runtime.Serialization;

namespace KillProofModule.Models
{
	public enum RaidWingEventType
	{
		[EnumMember(Value = "Unknown")]
		Unknown,
		[EnumMember(Value = "Boss")]
		Boss,
		[EnumMember(Value = "Checkpoint")]
		Checkpoint
	}
}
