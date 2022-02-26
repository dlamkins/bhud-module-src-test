using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BhModule.Community.Pathing
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
					resourceMan = new ResourceManager("BhModule.Community.Pathing.Strings", typeof(Strings).Assembly);
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

		internal static string General_UiName => ResourceManager.GetString("General_UiName", resourceCulture);

		internal static string Info_HiddenCategories => ResourceManager.GetString("Info_HiddenCategories", resourceCulture);

		internal static string Repo_Categories => ResourceManager.GetString("Repo_Categories", resourceCulture);

		internal static string Repo_Download => ResourceManager.GetString("Repo_Download", resourceCulture);

		internal static string Repo_Info => ResourceManager.GetString("Repo_Info", resourceCulture);

		internal static string Setting_MapShowAboveBelowIndicators => ResourceManager.GetString("Setting_MapShowAboveBelowIndicators", resourceCulture);

		internal static string Setting_MapShowMarkersOnCompass => ResourceManager.GetString("Setting_MapShowMarkersOnCompass", resourceCulture);

		internal static string Setting_MapShowMarkersOnFullscreen => ResourceManager.GetString("Setting_MapShowMarkersOnFullscreen", resourceCulture);

		internal static string Setting_MapShowTrailsOnCompass => ResourceManager.GetString("Setting_MapShowTrailsOnCompass", resourceCulture);

		internal static string Setting_MapShowTrailsOnFullscreen => ResourceManager.GetString("Setting_MapShowTrailsOnFullscreen", resourceCulture);

		internal static string Setting_PackAllowMarkersToAnimate => ResourceManager.GetString("Setting_PackAllowMarkersToAnimate", resourceCulture);

		internal static string Setting_PackAllowMarkersToAutomaticallyHide => ResourceManager.GetString("Setting_PackAllowMarkersToAutomaticallyHide", resourceCulture);

		internal static string Setting_PackFadeMarkersBetweenCharacterAndCamera => ResourceManager.GetString("Setting_PackFadeMarkersBetweenCharacterAndCamera", resourceCulture);

		internal static string Setting_PackFadePathablesDuringCombat => ResourceManager.GetString("Setting_PackFadePathablesDuringCombat", resourceCulture);

		internal static string Setting_PackFadeTrailsAroundCharacter => ResourceManager.GetString("Setting_PackFadeTrailsAroundCharacter", resourceCulture);

		internal static string Setting_PackMarkerConsentToClipboard => ResourceManager.GetString("Setting_PackMarkerConsentToClipboard", resourceCulture);

		internal static string Setting_PackMaxOpacityOverride => ResourceManager.GetString("Setting_PackMaxOpacityOverride", resourceCulture);

		internal static string Setting_PackMaxTrailAnimationSpeed => ResourceManager.GetString("Setting_PackMaxTrailAnimationSpeed", resourceCulture);

		internal static string Setting_PackMaxViewDistance => ResourceManager.GetString("Setting_PackMaxViewDistance", resourceCulture);

		internal static string Setting_PackShowCategoriesFromAllMaps => ResourceManager.GetString("Setting_PackShowCategoriesFromAllMaps", resourceCulture);

		internal static string Window_DownloadMarkerPacks => ResourceManager.GetString("Window_DownloadMarkerPacks", resourceCulture);

		internal static string Window_KeyBindSettingsTab => ResourceManager.GetString("Window_KeyBindSettingsTab", resourceCulture);

		internal static string Window_MainSettingsTab => ResourceManager.GetString("Window_MainSettingsTab", resourceCulture);

		internal static string Window_MapSettingsTab => ResourceManager.GetString("Window_MapSettingsTab", resourceCulture);

		internal Strings()
		{
		}
	}
}
