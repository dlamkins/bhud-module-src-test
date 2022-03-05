using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Manlaan.Mounts
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
					resourceMan = new ResourceManager("Manlaan.Mounts.Strings", typeof(Strings).Assembly);
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

		internal static string Setting_DefaultMountBehaviour => ResourceManager.GetString("Setting_DefaultMountBehaviour", resourceCulture);

		internal static string Setting_DefaultMountBinding => ResourceManager.GetString("Setting_DefaultMountBinding", resourceCulture);

		internal static string Setting_DefaultMountChoice => ResourceManager.GetString("Setting_DefaultMountChoice", resourceCulture);

		internal static string Setting_DefaultWaterMountChoice => ResourceManager.GetString("Setting_DefaultWaterMountChoice", resourceCulture);

		internal static string Setting_DisplayMountQueueing => ResourceManager.GetString("Setting_DisplayMountQueueing", resourceCulture);

		internal static string Setting_MountBlockKeybindFromGame => ResourceManager.GetString("Setting_MountBlockKeybindFromGame", resourceCulture);

		internal static string Setting_MountDisplay => ResourceManager.GetString("Setting_MountDisplay", resourceCulture);

		internal static string Setting_MountDisplayCornerIcons => ResourceManager.GetString("Setting_MountDisplayCornerIcons", resourceCulture);

		internal static string Setting_MountDisplayManualIcons => ResourceManager.GetString("Setting_MountDisplayManualIcons", resourceCulture);

		internal static string Setting_MountDrag => ResourceManager.GetString("Setting_MountDrag", resourceCulture);

		internal static string Setting_MountImgWidth => ResourceManager.GetString("Setting_MountImgWidth", resourceCulture);

		internal static string Setting_MountLoc => ResourceManager.GetString("Setting_MountLoc", resourceCulture);

		internal static string Setting_MountOpacity => ResourceManager.GetString("Setting_MountOpacity", resourceCulture);

		internal static string Setting_MountRadialCenterMountBehavior => ResourceManager.GetString("Setting_MountRadialCenterMountBehavior", resourceCulture);

		internal static string Setting_MountRadialIconOpacity => ResourceManager.GetString("Setting_MountRadialIconOpacity", resourceCulture);

		internal static string Setting_MountRadialIconSizeModifier => ResourceManager.GetString("Setting_MountRadialIconSizeModifier", resourceCulture);

		internal static string Setting_MountRadialRadiusModifier => ResourceManager.GetString("Setting_MountRadialRadiusModifier", resourceCulture);

		internal static string Setting_MountRadialRemoveCenterMount => ResourceManager.GetString("Setting_MountRadialRemoveCenterMount", resourceCulture);

		internal static string Setting_MountRadialSpawnAtMouse => ResourceManager.GetString("Setting_MountRadialSpawnAtMouse", resourceCulture);

		internal static string Setting_MountRadialToggleActionCameraKeyBinding => ResourceManager.GetString("Setting_MountRadialToggleActionCameraKeyBinding", resourceCulture);

		internal static string Setting_Orientation => ResourceManager.GetString("Setting_Orientation", resourceCulture);

		internal Strings()
		{
		}
	}
}
