using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Nekres.QuickSurrender.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("Nekres.QuickSurrender.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string Chat_Command => ResourceManager.GetString("Chat Command", resourceCulture);

		internal static string Chat_Display => ResourceManager.GetString("Chat Display", resourceCulture);

		internal static string Chat_Message => ResourceManager.GetString("Chat Message", resourceCulture);

		internal static string Command => ResourceManager.GetString("Command", resourceCulture);

		internal static string Command_recharging_ => ResourceManager.GetString("Command recharging.", resourceCulture);

		internal static string Concede_defeat_ => ResourceManager.GetString("Concede defeat.", resourceCulture);

		internal static string Concede_defeat_by_finishing_yourself_ => ResourceManager.GetString("Concede defeat by finishing yourself.", resourceCulture);

		internal static string Control_Options => ResourceManager.GetString("Control Options", resourceCulture);

		internal static string Defeated => ResourceManager.GetString("Defeated", resourceCulture);

		internal static string Determines_how_the_surrender_skill_is_displayed_in_chat_using__Ctrl__or__Shift_____Left_Mouse__ => ResourceManager.GetString("Determines how the surrender skill is displayed in chat using [Ctrl] or [Shift] + [Left Mouse].", resourceCulture);

		internal static string Displays_a_skill_to_assist_in_conceding_defeat_ => ResourceManager.GetString("Displays a skill to assist in conceding defeat.", resourceCulture);

		internal static string Give_focus_to_the_chat_edit_box_ => ResourceManager.GetString("Give focus to the chat edit box.", resourceCulture);

		internal static string Hotkeys => ResourceManager.GetString("Hotkeys", resourceCulture);

		internal static string Show_Surrender_Skill => ResourceManager.GetString("Show Surrender Skill", resourceCulture);

		internal static string Skill => ResourceManager.GetString("Skill", resourceCulture);

		internal static string Skill_recharging_ => ResourceManager.GetString("Skill recharging.", resourceCulture);

		internal static string Surrender => ResourceManager.GetString("Surrender", resourceCulture);

		internal static string User_Interface => ResourceManager.GetString("User Interface", resourceCulture);

		internal static string You_are_defeated_ => ResourceManager.GetString("You are defeated.", resourceCulture);

		internal Resources()
		{
		}
	}
}
