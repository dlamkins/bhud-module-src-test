using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Universal_Search_Module.Strings
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Common
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
					resourceMan = new ResourceManager("Universal_Search_Module.Strings.Common", typeof(Common).Assembly);
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

		internal static string Icon_Title => ResourceManager.GetString("Icon_Title", resourceCulture);

		internal static string Landmark_Details_ClosestWaypoint => ResourceManager.GetString("Landmark_Details_ClosestWaypoint", resourceCulture);

		internal static string Landmark_Details_ClosestWaypoint_NoneFound => ResourceManager.GetString("Landmark_Details_ClosestWaypoint_NoneFound", resourceCulture);

		internal static string Landmark_Details_CopyChatCode => ResourceManager.GetString("Landmark_Details_CopyChatCode", resourceCulture);

		internal static string Landmark_Details_CopyClosestWaypoint => ResourceManager.GetString("Landmark_Details_CopyClosestWaypoint", resourceCulture);

		internal static string Landmark_FailedToCopy => ResourceManager.GetString("Landmark_FailedToCopy", resourceCulture);

		internal static string Landmark_WaypointCopied => ResourceManager.GetString("Landmark_WaypointCopied", resourceCulture);

		internal static string SearchHandler_Landmarks => ResourceManager.GetString("SearchHandler_Landmarks", resourceCulture);

		internal static string SearchHandler_Landmarks_FloorLoading => ResourceManager.GetString("SearchHandler_Landmarks_FloorLoading", resourceCulture);

		internal static string SearchHandler_Skills => ResourceManager.GetString("SearchHandler_Skills", resourceCulture);

		internal static string SearchHandler_Skills_SkillLoading => ResourceManager.GetString("SearchHandler_Skills_SkillLoading", resourceCulture);

		internal static string SearchHandler_Traits => ResourceManager.GetString("SearchHandler_Traits", resourceCulture);

		internal static string SearchHandler_Traits_TraitLoading => ResourceManager.GetString("SearchHandler_Traits_TraitLoading", resourceCulture);

		internal static string SearchItem_Copied => ResourceManager.GetString("SearchItem_Copied", resourceCulture);

		internal static string SearchItem_FailedToCopy => ResourceManager.GetString("SearchItem_FailedToCopy", resourceCulture);

		internal static string Settings_EnterSelectionIntoChat_Text => ResourceManager.GetString("Settings_EnterSelectionIntoChat_Text", resourceCulture);

		internal static string Settings_EnterSelectionIntoChat_Title => ResourceManager.GetString("Settings_EnterSelectionIntoChat_Title", resourceCulture);

		internal static string Settings_HideWindowAfterSelection_Text => ResourceManager.GetString("Settings_HideWindowAfterSelection_Text", resourceCulture);

		internal static string Settings_HideWindowAfterSelection_Title => ResourceManager.GetString("Settings_HideWindowAfterSelection_Title", resourceCulture);

		internal static string Settings_NotificationAfterCopy_Text => ResourceManager.GetString("Settings_NotificationAfterCopy_Text", resourceCulture);

		internal static string Settings_NotificationAfterCopy_Title => ResourceManager.GetString("Settings_NotificationAfterCopy_Title", resourceCulture);

		internal static string SkillTooltip_BreaksStun => ResourceManager.GetString("SkillTooltip_BreaksStun", resourceCulture);

		internal static string SkillTooltip_Chance => ResourceManager.GetString("SkillTooltip_Chance", resourceCulture);

		internal static string SkillTooltip_CombatOnly => ResourceManager.GetString("SkillTooltip_CombatOnly", resourceCulture);

		internal static string TraitTooltip_BuffConversion => ResourceManager.GetString("TraitTooltip_BuffConversion", resourceCulture);

		internal static string TraitTooltip_Chance => ResourceManager.GetString("TraitTooltip_Chance", resourceCulture);

		internal static string TraitTooltip_CombatOnly => ResourceManager.GetString("TraitTooltip_CombatOnly", resourceCulture);

		internal Common()
		{
		}
	}
}
