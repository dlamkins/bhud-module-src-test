using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace entrhopi.Guild_Missions.Strings
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
					resourceMan = new ResourceManager("entrhopi.Guild_Missions.Strings.Common", typeof(Common).Assembly);
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

		internal static string gmButtonClearAll => ResourceManager.GetString("gmButtonClearAll", resourceCulture);

		internal static string gmButtonExport => ResourceManager.GetString("gmButtonExport", resourceCulture);

		internal static string gmButtonImport => ResourceManager.GetString("gmButtonImport", resourceCulture);

		internal static string gmButtonSendToChat => ResourceManager.GetString("gmButtonSendToChat", resourceCulture);

		internal static string gmButtonWiki => ResourceManager.GetString("gmButtonWiki", resourceCulture);

		internal static string gmNotificationClipboardError => ResourceManager.GetString("gmNotificationClipboardError", resourceCulture);

		internal static string gmNotificationClipboardRead => ResourceManager.GetString("gmNotificationClipboardRead", resourceCulture);

		internal static string gmNotificationClipboardSaved => ResourceManager.GetString("gmNotificationClipboardSaved", resourceCulture);

		internal static string gmPanelInfo => ResourceManager.GetString("gmPanelInfo", resourceCulture);

		internal static string gmPanelList => ResourceManager.GetString("gmPanelList", resourceCulture);

		internal static string gmPanelSavedTreks => ResourceManager.GetString("gmPanelSavedTreks", resourceCulture);

		internal static string gmPanelSearchResults => ResourceManager.GetString("gmPanelSearchResults", resourceCulture);

		internal static string gmSearchPlaceholder => ResourceManager.GetString("gmSearchPlaceholder", resourceCulture);

		internal static string gmTabName => ResourceManager.GetString("gmTabName", resourceCulture);

		internal static string gmTypeBounty => ResourceManager.GetString("gmTypeBounty", resourceCulture);

		internal static string gmTypeChallenge => ResourceManager.GetString("gmTypeChallenge", resourceCulture);

		internal static string gmTypePuzzle => ResourceManager.GetString("gmTypePuzzle", resourceCulture);

		internal static string gmTypeRace => ResourceManager.GetString("gmTypeRace", resourceCulture);

		internal static string gmTypeSelect => ResourceManager.GetString("gmTypeSelect", resourceCulture);

		internal static string gmTypeTrek => ResourceManager.GetString("gmTypeTrek", resourceCulture);

		internal Common()
		{
		}
	}
}
