using System.ComponentModel;

namespace TargetYourFeet.Settings.Enums
{
	public enum KeybindBehaviour
	{
		[Description("Press and hold")]
		Hold,
		[Description("Toggle")]
		Toggle,
		[Description("Single press")]
		MoveOnly
	}
}
