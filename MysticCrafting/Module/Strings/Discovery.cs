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
	internal class Discovery
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
					resourceMan = new ResourceManager("MysticCrafting.Module.Strings.Discovery", typeof(Discovery).Assembly);
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

		internal static string DiscoveryWindowSubTitle => ResourceManager.GetString("DiscoveryWindowSubTitle", resourceCulture);

		internal static string NoResults => ResourceManager.GetString("NoResults", resourceCulture);

		internal static string NoResultsCheckFilters => ResourceManager.GetString("NoResultsCheckFilters", resourceCulture);

		internal static string RaritiesPanelAllOptions => ResourceManager.GetString("RaritiesPanelAllOptions", resourceCulture);

		internal static string RaritiesPanelHeading => ResourceManager.GetString("RaritiesPanelHeading", resourceCulture);

		internal static string SkinsPanelAllOptions => ResourceManager.GetString("SkinsPanelAllOptions", resourceCulture);

		internal static string SkinsPanelHeading => ResourceManager.GetString("SkinsPanelHeading", resourceCulture);

		internal static string SkinsPanelNoSkins => ResourceManager.GetString("SkinsPanelNoSkins", resourceCulture);

		internal static string SkinsPanelSkinsLocked => ResourceManager.GetString("SkinsPanelSkinsLocked", resourceCulture);

		internal static string SkinsPanelSkinsUnlocked => ResourceManager.GetString("SkinsPanelSkinsUnlocked", resourceCulture);

		internal static string SourcesPanelAllOptions => ResourceManager.GetString("SourcesPanelAllOptions", resourceCulture);

		internal static string SourcesPanelHeading => ResourceManager.GetString("SourcesPanelHeading", resourceCulture);

		internal static string TableHeaderCount => ResourceManager.GetString("TableHeaderCount", resourceCulture);

		internal static string TableHeaderFavorite => ResourceManager.GetString("TableHeaderFavorite", resourceCulture);

		internal static string TableHeaderIcon => ResourceManager.GetString("TableHeaderIcon", resourceCulture);

		internal static string TableHeaderName => ResourceManager.GetString("TableHeaderName", resourceCulture);

		internal static string TableHeaderSkin => ResourceManager.GetString("TableHeaderSkin", resourceCulture);

		internal static string TableHeaderType => ResourceManager.GetString("TableHeaderType", resourceCulture);

		internal static string TableHeaderWeight => ResourceManager.GetString("TableHeaderWeight", resourceCulture);

		internal Discovery()
		{
		}
	}
}
