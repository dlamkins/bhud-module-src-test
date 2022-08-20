using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TTBlockersStuff.Language
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Translations
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
					resourceMan = new ResourceManager("TTBlockersStuff.Language.Translations", typeof(Translations).Assembly);
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

		internal static string ColorSelectionButtonTextCancel => ResourceManager.GetString("ColorSelectionButtonTextCancel", resourceCulture);

		internal static string ColorSelectionButtonTextOk => ResourceManager.GetString("ColorSelectionButtonTextOk", resourceCulture);

		internal static string ColorSelectionTitle => ResourceManager.GetString("ColorSelectionTitle", resourceCulture);

		internal static string GatheringSpotTitleAmber => ResourceManager.GetString("GatheringSpotTitleAmber", resourceCulture);

		internal static string GatheringSpotTitleCobalt => ResourceManager.GetString("GatheringSpotTitleCobalt", resourceCulture);

		internal static string GatheringSpotTitleCrimson => ResourceManager.GetString("GatheringSpotTitleCrimson", resourceCulture);

		internal static string GatheringspotTitleMain => ResourceManager.GetString("GatheringspotTitleMain", resourceCulture);

		internal static string SettingColorSelectionEggsText => ResourceManager.GetString("SettingColorSelectionEggsText", resourceCulture);

		internal static string SettingColorSelectionEggsTooltipText => ResourceManager.GetString("SettingColorSelectionEggsTooltipText", resourceCulture);

		internal static string SettingColorSelectionHusksText => ResourceManager.GetString("SettingColorSelectionHusksText", resourceCulture);

		internal static string SettingColorSelectionHusksTooltipText => ResourceManager.GetString("SettingColorSelectionHusksTooltipText", resourceCulture);

		internal static string TimerBarTextEggs => ResourceManager.GetString("TimerBarTextEggs", resourceCulture);

		internal static string TimerBarTextHusks => ResourceManager.GetString("TimerBarTextHusks", resourceCulture);

		internal static string TimerBarTextReady => ResourceManager.GetString("TimerBarTextReady", resourceCulture);

		internal Translations()
		{
		}
	}
}
