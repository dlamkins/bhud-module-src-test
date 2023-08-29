using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TargetYourFeet.Localization
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
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
					resourceMan = new ResourceManager("TargetYourFeet.Localization.Strings", typeof(Strings).Assembly);
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

		internal static string ActionCamInUse_Label => ResourceManager.GetString("ActionCamInUse_Label", resourceCulture);

		internal static string ActionCamInUse_Tooltip => ResourceManager.GetString("ActionCamInUse_Tooltip", resourceCulture);

		internal static string ActionCamKeybind_Label => ResourceManager.GetString("ActionCamKeybind_Label", resourceCulture);

		internal static string ActionCamKeybind_Tooltip => ResourceManager.GetString("ActionCamKeybind_Tooltip", resourceCulture);

		internal static string DisableDisclaimer => ResourceManager.GetString("DisableDisclaimer", resourceCulture);

		internal static string Keybind_Label => ResourceManager.GetString("Keybind_Label", resourceCulture);

		internal static string Keybind_Tooltip => ResourceManager.GetString("Keybind_Tooltip", resourceCulture);

		internal static string Module_Title => ResourceManager.GetString("Module_Title", resourceCulture);

		internal static string ModuleSettings_OpenSettings => ResourceManager.GetString("ModuleSettings_OpenSettings", resourceCulture);

		internal static string PatchNotes => ResourceManager.GetString("PatchNotes", resourceCulture);

		internal static string PatchNotes_Tooltip => ResourceManager.GetString("PatchNotes_Tooltip", resourceCulture);

		internal Strings()
		{
		}
	}
}
