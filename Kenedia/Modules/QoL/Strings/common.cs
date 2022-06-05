using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.QoL.Strings
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class common
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
					resourceMan = new ResourceManager("Kenedia.Modules.QoL.Strings.common", typeof(common).Assembly);
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

		internal static string Activated => ResourceManager.GetString("Activated", resourceCulture);

		internal static string AllowManualZoom_Name => ResourceManager.GetString("AllowManualZoom_Name", resourceCulture);

		internal static string AllowManualZoom_Tooltip => ResourceManager.GetString("AllowManualZoom_Tooltip", resourceCulture);

		internal static string ClickItem => ResourceManager.GetString("ClickItem", resourceCulture);

		internal static string Deactivated => ResourceManager.GetString("Deactivated", resourceCulture);

		internal static string ManualMaxZoomOut_Name => ResourceManager.GetString("ManualMaxZoomOut_Name", resourceCulture);

		internal static string ManualMaxZoomOut_Tooltip => ResourceManager.GetString("ManualMaxZoomOut_Tooltip", resourceCulture);

		internal static string RunStateChange => ResourceManager.GetString("RunStateChange", resourceCulture);

		internal static string ShowCorner_Name => ResourceManager.GetString("ShowCorner_Name", resourceCulture);

		internal static string ShowCorner_Tooltip => ResourceManager.GetString("ShowCorner_Tooltip", resourceCulture);

		internal static string ThrowItem => ResourceManager.GetString("ThrowItem", resourceCulture);

		internal static string Toggle => ResourceManager.GetString("Toggle", resourceCulture);

		internal static string ZoomOnCameraChange_Name => ResourceManager.GetString("ZoomOnCameraChange_Name", resourceCulture);

		internal static string ZoomOnCameraChange_Tooltip => ResourceManager.GetString("ZoomOnCameraChange_Tooltip", resourceCulture);

		internal common()
		{
		}
	}
}
