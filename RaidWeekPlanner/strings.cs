using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RaidWeekPlanner
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class strings
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
					resourceMan = new ResourceManager("RaidWeekPlanner.Ressources.strings", typeof(strings).Assembly);
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

		internal static string CornerIcon_Tooltip => ResourceManager.GetString("CornerIcon_Tooltip", resourceCulture);

		internal static string CornerIcon_Tooltip_Warning => ResourceManager.GetString("CornerIcon_Tooltip_Warning", resourceCulture);

		internal static string day0 => ResourceManager.GetString("day0", resourceCulture);

		internal static string day1 => ResourceManager.GetString("day1", resourceCulture);

		internal static string day2 => ResourceManager.GetString("day2", resourceCulture);

		internal static string day3 => ResourceManager.GetString("day3", resourceCulture);

		internal static string day4 => ResourceManager.GetString("day4", resourceCulture);

		internal static string day5 => ResourceManager.GetString("day5", resourceCulture);

		internal static string day6 => ResourceManager.GetString("day6", resourceCulture);

		internal static string Legend_Done_Label => ResourceManager.GetString("Legend_Done_Label", resourceCulture);

		internal static string Legend_Done_Tooltip => ResourceManager.GetString("Legend_Done_Tooltip", resourceCulture);

		internal static string Legend_None_Label => ResourceManager.GetString("Legend_None_Label", resourceCulture);

		internal static string Legend_None_Tooltip => ResourceManager.GetString("Legend_None_Tooltip", resourceCulture);

		internal static string Legend_Planned_Label => ResourceManager.GetString("Legend_Planned_Label", resourceCulture);

		internal static string Legend_Planned_Tooltip => ResourceManager.GetString("Legend_Planned_Tooltip", resourceCulture);

		internal static string Legend_Title => ResourceManager.GetString("Legend_Title", resourceCulture);

		internal static string Legend_Todo_Label => ResourceManager.GetString("Legend_Todo_Label", resourceCulture);

		internal static string Legend_Todo_Tooltip => ResourceManager.GetString("Legend_Todo_Tooltip", resourceCulture);

		internal static string LoadingSpinner_Fetch => ResourceManager.GetString("LoadingSpinner_Fetch", resourceCulture);

		internal static string MainWindow_Button_Refresh_Label => ResourceManager.GetString("MainWindow_Button_Refresh_Label", resourceCulture);

		internal static string MainWindow_Button_Refresh_Tooltip => ResourceManager.GetString("MainWindow_Button_Refresh_Tooltip", resourceCulture);

		internal static string MainWindow_Button_ToggleFuture_Future => ResourceManager.GetString("MainWindow_Button_ToggleFuture_Future", resourceCulture);

		internal static string MainWindow_Button_ToggleFuture_Present => ResourceManager.GetString("MainWindow_Button_ToggleFuture_Present", resourceCulture);

		internal static string MainWindow_Button_ToggleFuture_Tooltip => ResourceManager.GetString("MainWindow_Button_ToggleFuture_Tooltip", resourceCulture);

		internal static string MainWindow_Button_ToggleTableDrawMode_Areas => ResourceManager.GetString("MainWindow_Button_ToggleTableDrawMode_Areas", resourceCulture);

		internal static string MainWindow_Button_ToggleTableDrawMode_Tooltip => ResourceManager.GetString("MainWindow_Button_ToggleTableDrawMode_Tooltip", resourceCulture);

		internal static string MainWindow_Button_ToggleTableDrawMode_Week => ResourceManager.GetString("MainWindow_Button_ToggleTableDrawMode_Week", resourceCulture);

		internal static string MainWindow_Label_ClearTrack_Notice => ResourceManager.GetString("MainWindow_Label_ClearTrack_Notice", resourceCulture);

		internal static string MainWindow_Label_ClearTrack_Notice2 => ResourceManager.GetString("MainWindow_Label_ClearTrack_Notice2", resourceCulture);

		internal static string NoData => ResourceManager.GetString("NoData", resourceCulture);

		internal strings()
		{
		}
	}
}
