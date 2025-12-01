using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace flakysalt.CharacterKeybinds.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class SettingsLoca
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
					resourceMan = new ResourceManager("flakysalt.CharacterKeybinds.Resources.SettingsLoca", typeof(SettingsLoca).Assembly);
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

		internal static string anetMacroPolicyButton => ResourceManager.GetString("anetMacroPolicyButton", resourceCulture);

		internal static string autoClickSpeedMultiplierHint => ResourceManager.GetString("autoClickSpeedMultiplierHint", resourceCulture);

		internal static string autoClickSpeedMultiplierSetting => ResourceManager.GetString("autoClickSpeedMultiplierSetting", resourceCulture);

		internal static string changeKeybindsOnSpecSwitchHint => ResourceManager.GetString("changeKeybindsOnSpecSwitchHint", resourceCulture);

		internal static string changeKeybindsOnSpecSwitchSetting => ResourceManager.GetString("changeKeybindsOnSpecSwitchSetting", resourceCulture);

		internal static string helpFaqButton => ResourceManager.GetString("helpFaqButton", resourceCulture);

		internal static string keybindsDirectoryHint => ResourceManager.GetString("keybindsDirectoryHint", resourceCulture);

		internal static string keybindsDirectoryInvalid => ResourceManager.GetString("keybindsDirectoryInvalid", resourceCulture);

		internal static string keybindsDirectorySetting => ResourceManager.GetString("keybindsDirectorySetting", resourceCulture);

		internal static string keybindsDirectoryValid => ResourceManager.GetString("keybindsDirectoryValid", resourceCulture);

		internal static string keybindSettingsButton => ResourceManager.GetString("keybindSettingsButton", resourceCulture);

		internal static string optionsMenuKeybindsSetting => ResourceManager.GetString("optionsMenuKeybindsSetting", resourceCulture);

		internal static string reportBugButton => ResourceManager.GetString("reportBugButton", resourceCulture);

		internal static string showCornerIconHint => ResourceManager.GetString("showCornerIconHint", resourceCulture);

		internal static string showCornerIconSetting => ResourceManager.GetString("showCornerIconSetting", resourceCulture);

		internal static string troubleshootButton => ResourceManager.GetString("troubleshootButton", resourceCulture);

		internal static string tutorialButton => ResourceManager.GetString("tutorialButton", resourceCulture);

		internal static string useDefaultKeybindsHint => ResourceManager.GetString("useDefaultKeybindsHint", resourceCulture);

		internal static string useDefaultKeybindsSetting => ResourceManager.GetString("useDefaultKeybindsSetting", resourceCulture);

		internal SettingsLoca()
		{
		}
	}
}
