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
	internal class Loca
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
					resourceMan = new ResourceManager("flakysalt.CharacterKeybinds.Resources.Loca", typeof(Loca).Assembly);
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

		internal static string addNewBindingButtonText => ResourceManager.GetString("addNewBindingButtonText", resourceCulture);

		internal static string apiLoadingHint => ResourceManager.GetString("apiLoadingHint", resourceCulture);

		internal static string apply => ResourceManager.GetString("apply", resourceCulture);

		internal static string characterSpecificKeybinds => ResourceManager.GetString("characterSpecificKeybinds", resourceCulture);

		internal static string characterSpecificKeybindsHint => ResourceManager.GetString("characterSpecificKeybindsHint", resourceCulture);

		internal static string coreSpecializationName => ResourceManager.GetString("coreSpecializationName", resourceCulture);

		internal static string defaultCharacterEntry => ResourceManager.GetString("defaultCharacterEntry", resourceCulture);

		internal static string defaultKeybind => ResourceManager.GetString("defaultKeybind", resourceCulture);

		internal static string defaultKeybindHint => ResourceManager.GetString("defaultKeybindHint", resourceCulture);

		internal static string defaultKeybindsEntry => ResourceManager.GetString("defaultKeybindsEntry", resourceCulture);

		internal static string defaultSpecializationEntry => ResourceManager.GetString("defaultSpecializationEntry", resourceCulture);

		internal static string delete => ResourceManager.GetString("delete", resourceCulture);

		internal static string errorMessageInvalidFolder => ResourceManager.GetString("errorMessageInvalidFolder", resourceCulture);

		internal static string errorMessageIssueCounter => ResourceManager.GetString("errorMessageIssueCounter", resourceCulture);

		internal static string errorMessageMissingApiDown => ResourceManager.GetString("errorMessageMissingApiDown", resourceCulture);

		internal static string errorMessageMissingApiPermissions => ResourceManager.GetString("errorMessageMissingApiPermissions", resourceCulture);

		internal static string errorMessageMissingSubtoken => ResourceManager.GetString("errorMessageMissingSubtoken", resourceCulture);

		internal static string errorMessageNeedsMigration => ResourceManager.GetString("errorMessageNeedsMigration", resourceCulture);

		internal static string invalidSpecializationName => ResourceManager.GetString("invalidSpecializationName", resourceCulture);

		internal static string migration => ResourceManager.GetString("migration", resourceCulture);

		internal static string moduleName => ResourceManager.GetString("moduleName", resourceCulture);

		internal static string switchKeybindsNotificationText => ResourceManager.GetString("switchKeybindsNotificationText", resourceCulture);

		internal static string troubleshootMarkers => ResourceManager.GetString("troubleshootMarkers", resourceCulture);

		internal static string troubleshootResetMarkers => ResourceManager.GetString("troubleshootResetMarkers", resourceCulture);

		internal static string troubleshootTestButtonHint => ResourceManager.GetString("troubleshootTestButtonHint", resourceCulture);

		internal static string troubleshootTestButtonText => ResourceManager.GetString("troubleshootTestButtonText", resourceCulture);

		internal static string troubleshootText => ResourceManager.GetString("troubleshootText", resourceCulture);

		internal static string troubleshootWindowName => ResourceManager.GetString("troubleshootWindowName", resourceCulture);

		internal static string wildcardSpecializationName => ResourceManager.GetString("wildcardSpecializationName", resourceCulture);

		internal Loca()
		{
		}
	}
}
