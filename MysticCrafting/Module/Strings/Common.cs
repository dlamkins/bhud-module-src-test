using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MysticCrafting.Module.Strings
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
					resourceMan = new ResourceManager("MysticCrafting.Module.Strings.Common", typeof(Common).Assembly);
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

		internal static string Dismiss => ResourceManager.GetString("Dismiss", resourceCulture);

		internal static string FavoritesEmpty => ResourceManager.GetString("FavoritesEmpty", resourceCulture);

		internal static string Filters => ResourceManager.GetString("Filters", resourceCulture);

		internal static string HideLockedSkins => ResourceManager.GetString("HideLockedSkins", resourceCulture);

		internal static string HideMaxCollectedItems => ResourceManager.GetString("HideMaxCollectedItems", resourceCulture);

		internal static string HideUnlockedSkins => ResourceManager.GetString("HideUnlockedSkins", resourceCulture);

		internal static string LastLoaded => ResourceManager.GetString("LastLoaded", resourceCulture);

		internal static string LoadingErrorMessage => ResourceManager.GetString("LoadingErrorMessage", resourceCulture);

		internal static string LoadingIntroMessage => ResourceManager.GetString("LoadingIntroMessage", resourceCulture);

		internal static string LoadingMinuteInterval => ResourceManager.GetString("LoadingMinuteInterval", resourceCulture);

		internal static string LoadingPlayerAchievements => ResourceManager.GetString("LoadingPlayerAchievements", resourceCulture);

		internal static string LoadingPlayerItems => ResourceManager.GetString("LoadingPlayerItems", resourceCulture);

		internal static string LoadingPlayerUnlocks => ResourceManager.GetString("LoadingPlayerUnlocks", resourceCulture);

		internal static string LoadingReloadAllServices => ResourceManager.GetString("LoadingReloadAllServices", resourceCulture);

		internal static string LoadingReloadService => ResourceManager.GetString("LoadingReloadService", resourceCulture);

		internal static string LoadingRightClickLabel => ResourceManager.GetString("LoadingRightClickLabel", resourceCulture);

		internal static string LoadingTradingPost => ResourceManager.GetString("LoadingTradingPost", resourceCulture);

		internal static string LoadingWallet => ResourceManager.GetString("LoadingWallet", resourceCulture);

		internal static string MaximumResultsReached => ResourceManager.GetString("MaximumResultsReached", resourceCulture);

		internal static string MenuTitle => ResourceManager.GetString("MenuTitle", resourceCulture);

		internal static string MoreFilters => ResourceManager.GetString("MoreFilters", resourceCulture);

		internal static string SearchBarPlaceholder => ResourceManager.GetString("SearchBarPlaceholder", resourceCulture);

		internal static string SkinLocked => ResourceManager.GetString("SkinLocked", resourceCulture);

		internal static string SkinUnlocked => ResourceManager.GetString("SkinUnlocked", resourceCulture);

		internal static string ToggleWindow => ResourceManager.GetString("ToggleWindow", resourceCulture);

		internal static string TradingPostDefault => ResourceManager.GetString("TradingPostDefault", resourceCulture);

		internal static string VersionClick => ResourceManager.GetString("VersionClick", resourceCulture);

		internal Common()
		{
		}
	}
}
