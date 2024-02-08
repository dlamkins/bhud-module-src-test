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

		internal static string FavoritesEmpty => ResourceManager.GetString("FavoritesEmpty", resourceCulture);

		internal static string LastLoaded => ResourceManager.GetString("LastLoaded", resourceCulture);

		internal static string LoadingErrorMessage => ResourceManager.GetString("LoadingErrorMessage", resourceCulture);

		internal static string LoadingPlayerItems => ResourceManager.GetString("LoadingPlayerItems", resourceCulture);

		internal static string LoadingPlayerUnlocks => ResourceManager.GetString("LoadingPlayerUnlocks", resourceCulture);

		internal static string LoadingTradingPost => ResourceManager.GetString("LoadingTradingPost", resourceCulture);

		internal static string LoadingWallet => ResourceManager.GetString("LoadingWallet", resourceCulture);

		internal static string MaximumResultsReached => ResourceManager.GetString("MaximumResultsReached", resourceCulture);

		internal static string MenuTitle => ResourceManager.GetString("MenuTitle", resourceCulture);

		internal static string SearchBarPlaceholder => ResourceManager.GetString("SearchBarPlaceholder", resourceCulture);

		internal static string ToggleWindow => ResourceManager.GetString("ToggleWindow", resourceCulture);

		internal Common()
		{
		}
	}
}
