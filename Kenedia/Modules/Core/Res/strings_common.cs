using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Res
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class strings_common
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
					resourceMan = new ResourceManager("Kenedia.Modules." + Assembly.GetExecutingAssembly().FullName.Split(',')[0].Substring(Assembly.GetExecutingAssembly().FullName.Split(',')[0].LastIndexOf('.') + 1) + ".Res.strings_common", typeof(strings_common).Assembly);
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

		internal static string BottomLeftCorner => ResourceManager.GetString("BottomLeftCorner", resourceCulture);

		internal static string BottomOffset => ResourceManager.GetString("BottomOffset", resourceCulture);

		internal static string BottomRightCorner => ResourceManager.GetString("BottomRightCorner", resourceCulture);

		internal static string DeleteX => ResourceManager.GetString("DeleteX", resourceCulture);

		internal static string Edit => ResourceManager.GetString("Edit", resourceCulture);

		internal static string EditItem => ResourceManager.GetString("EditItem", resourceCulture);

		internal static string FetchingApiData => ResourceManager.GetString("FetchingApiData", resourceCulture);

		internal static string GeneralSettings => ResourceManager.GetString("GeneralSettings", resourceCulture);

		internal static string GW2API_RequestFailed => ResourceManager.GetString("GW2API_RequestFailed", resourceCulture);

		internal static string GW2API_Unavailable => ResourceManager.GetString("GW2API_Unavailable", resourceCulture);

		internal static string ItemSettings => ResourceManager.GetString("ItemSettings", resourceCulture);

		internal static string LeftOffset => ResourceManager.GetString("LeftOffset", resourceCulture);

		internal static string OpenSettings => ResourceManager.GetString("OpenSettings", resourceCulture);

		internal static string RightOffset => ResourceManager.GetString("RightOffset", resourceCulture);

		internal static Bitmap RollingChoya => (Bitmap)ResourceManager.GetObject("RollingChoya", resourceCulture);

		internal static string Search => ResourceManager.GetString("Search", resourceCulture);

		internal static string Settings => ResourceManager.GetString("Settings", resourceCulture);

		internal static string SharedSettings => ResourceManager.GetString("SharedSettings", resourceCulture);

		internal static string ShowCornerIcon => ResourceManager.GetString("ShowCornerIcon", resourceCulture);

		internal static string ShowCornerIcon_ttp => ResourceManager.GetString("ShowCornerIcon_ttp", resourceCulture);

		internal static string Tag => ResourceManager.GetString("Tag", resourceCulture);

		internal static string ToggleItem => ResourceManager.GetString("ToggleItem", resourceCulture);

		internal static string TopLeftCorner => ResourceManager.GetString("TopLeftCorner", resourceCulture);

		internal static string TopOffset => ResourceManager.GetString("TopOffset", resourceCulture);

		internal static string TopRightCorner => ResourceManager.GetString("TopRightCorner", resourceCulture);

		internal static string WindowBorder_Tooltip => ResourceManager.GetString("WindowBorder_Tooltip", resourceCulture);

		internal static string WindowBorders => ResourceManager.GetString("WindowBorders", resourceCulture);

		internal strings_common()
		{
		}
	}
}
