using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace felix.BlishEmotes.Strings
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Common
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("felix.BlishEmotes.Strings.Common", typeof(Common).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
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

		public static string category_add => ResourceManager.GetString("category_add", resourceCulture);

		public static string cornerIcon_tooltip => ResourceManager.GetString("cornerIcon_tooltip", resourceCulture);

		public static string emote_categoryFavourite => ResourceManager.GetString("emote_categoryFavourite", resourceCulture);

		public static string emote_synchronizeActive => ResourceManager.GetString("emote_synchronizeActive", resourceCulture);

		public static string emote_targetingActive => ResourceManager.GetString("emote_targetingActive", resourceCulture);

		public static string settings_button => ResourceManager.GetString("settings_button", resourceCulture);

		public static string settings_emotesKeybindSubCollection => ResourceManager.GetString("settings_emotesKeybindSubCollection", resourceCulture);

		public static string settings_global_hideCornerIcon => ResourceManager.GetString("settings_global_hideCornerIcon", resourceCulture);

		public static string settings_global_keybindToggleEmoteList => ResourceManager.GetString("settings_global_keybindToggleEmoteList", resourceCulture);

		public static string settings_global_keybindToggleSynchronize => ResourceManager.GetString("settings_global_keybindToggleSynchronize", resourceCulture);

		public static string settings_global_keybindToggleTargeting => ResourceManager.GetString("settings_global_keybindToggleTargeting", resourceCulture);

		public static string settings_global_useCategories => ResourceManager.GetString("settings_global_useCategories", resourceCulture);

		public static string settings_global_useRadialMenu => ResourceManager.GetString("settings_global_useRadialMenu", resourceCulture);

		public static string settings_radial_actionCamKeybind => ResourceManager.GetString("settings_radial_actionCamKeybind", resourceCulture);

		public static string settings_radial_emotesEnabled => ResourceManager.GetString("settings_radial_emotesEnabled", resourceCulture);

		public static string settings_radial_iconOpacity => ResourceManager.GetString("settings_radial_iconOpacity", resourceCulture);

		public static string settings_radial_iconSizeModifier => ResourceManager.GetString("settings_radial_iconSizeModifier", resourceCulture);

		public static string settings_radial_innerRadiusPercentage => ResourceManager.GetString("settings_radial_innerRadiusPercentage", resourceCulture);

		public static string settings_radial_innerRadiusPercentage_description => ResourceManager.GetString("settings_radial_innerRadiusPercentage_description", resourceCulture);

		public static string settings_radial_radiusModifier => ResourceManager.GetString("settings_radial_radiusModifier", resourceCulture);

		public static string settings_radial_spawnAtCursor => ResourceManager.GetString("settings_radial_spawnAtCursor", resourceCulture);

		public static string settings_ui_categories_tab => ResourceManager.GetString("settings_ui_categories_tab", resourceCulture);

		public static string settings_ui_delete => ResourceManager.GetString("settings_ui_delete", resourceCulture);

		public static string settings_ui_emoteHotkeys_tab => ResourceManager.GetString("settings_ui_emoteHotkeys_tab", resourceCulture);

		public static string settings_ui_global_tab => ResourceManager.GetString("settings_ui_global_tab", resourceCulture);

		public static string settings_ui_save => ResourceManager.GetString("settings_ui_save", resourceCulture);

		public static string settings_ui_title => ResourceManager.GetString("settings_ui_title", resourceCulture);

		internal Common()
		{
		}
	}
}