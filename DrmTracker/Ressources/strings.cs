using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DrmTracker.Ressources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
					resourceMan = new ResourceManager("DrmTracker.Ressources.strings", typeof(strings).Assembly);
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

		internal static string Legend_Done => ResourceManager.GetString("Legend_Done", resourceCulture);

		internal static string Legend_None => ResourceManager.GetString("Legend_None", resourceCulture);

		internal static string Legend_Title => ResourceManager.GetString("Legend_Title", resourceCulture);

		internal static string Legend_Todo => ResourceManager.GetString("Legend_Todo", resourceCulture);

		internal static string LoadingSpinner_Fetch => ResourceManager.GetString("LoadingSpinner_Fetch", resourceCulture);

		internal static string MainWindow_Button_Refresh_Label => ResourceManager.GetString("MainWindow_Button_Refresh_Label", resourceCulture);

		internal static string NoData => ResourceManager.GetString("NoData", resourceCulture);

		internal strings()
		{
		}
	}
}
