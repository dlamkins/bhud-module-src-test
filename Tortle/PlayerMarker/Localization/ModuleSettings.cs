using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Tortle.PlayerMarker.Localization
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class ModuleSettings
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
					resourceMan = new ResourceManager("Tortle.PlayerMarker.Localization.ModuleSettings", typeof(ModuleSettings).Assembly);
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

		internal static string CleanupDuplicates_Name => ResourceManager.GetString("CleanupDuplicates_Name", resourceCulture);

		internal static string CleanupDuplicates_Tooltip => ResourceManager.GetString("CleanupDuplicates_Tooltip", resourceCulture);

		internal static string PlayerMarkerColor_Name => ResourceManager.GetString("PlayerMarkerColor_Name", resourceCulture);

		internal static string PlayerMarkerColor_Tooltip => ResourceManager.GetString("PlayerMarkerColor_Tooltip", resourceCulture);

		internal static string PlayerMarkerEnable_Name => ResourceManager.GetString("PlayerMarkerEnable_Name", resourceCulture);

		internal static string PlayerMarkerEnable_Tooltip => ResourceManager.GetString("PlayerMarkerEnable_Tooltip", resourceCulture);

		internal static string PlayerMarkerImage_Description => ResourceManager.GetString("PlayerMarkerImage_Description", resourceCulture);

		internal static string PlayerMarkerImage_Name => ResourceManager.GetString("PlayerMarkerImage_Name", resourceCulture);

		internal static string PlayerMarkerImage_Tooltip => ResourceManager.GetString("PlayerMarkerImage_Tooltip", resourceCulture);

		internal static string PlayerMarkerOpacity_Name => ResourceManager.GetString("PlayerMarkerOpacity_Name", resourceCulture);

		internal static string PlayerMarkerOpacity_Tooltip => ResourceManager.GetString("PlayerMarkerOpacity_Tooltip", resourceCulture);

		internal static string PlayerMarkerSize_Name => ResourceManager.GetString("PlayerMarkerSize_Name", resourceCulture);

		internal static string PlayerMarkerSize_Tooltip => ResourceManager.GetString("PlayerMarkerSize_Tooltip", resourceCulture);

		internal static string PlayerMarkerVerticalOffset_Name => ResourceManager.GetString("PlayerMarkerVerticalOffset_Name", resourceCulture);

		internal static string PlayerMarkerVerticalOffset_Tooltip => ResourceManager.GetString("PlayerMarkerVerticalOffset_Tooltip", resourceCulture);

		internal ModuleSettings()
		{
		}
	}
}
