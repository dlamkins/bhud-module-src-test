using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Ideka.CustomCombatText
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
					resourceMan = new ResourceManager("Ideka.CustomCombatText.Strings", typeof(Strings).Assembly);
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

		internal static string SettingAutocropIcons => ResourceManager.GetString("SettingAutocropIcons", resourceCulture);

		internal static string SettingAutocropIconsText => ResourceManager.GetString("SettingAutocropIconsText", resourceCulture);

		internal static string SettingAutocropTolerance => ResourceManager.GetString("SettingAutocropTolerance", resourceCulture);

		internal static string SettingAutocropToleranceText => ResourceManager.GetString("SettingAutocropToleranceText", resourceCulture);

		internal static string SettingDebug => ResourceManager.GetString("SettingDebug", resourceCulture);

		internal static string SettingDebugText => ResourceManager.GetString("SettingDebugText", resourceCulture);

		internal static string SettingFontName => ResourceManager.GetString("SettingFontName", resourceCulture);

		internal static string SettingFontNameText => ResourceManager.GetString("SettingFontNameText", resourceCulture);

		internal static string SettingFontSize => ResourceManager.GetString("SettingFontSize", resourceCulture);

		internal static string SettingFontSizeText => ResourceManager.GetString("SettingFontSizeText", resourceCulture);

		internal static string SettingMasterToPetIsSelf => ResourceManager.GetString("SettingMasterToPetIsSelf", resourceCulture);

		internal static string SettingMasterToPetIsSelfText => ResourceManager.GetString("SettingMasterToPetIsSelfText", resourceCulture);

		internal static string SettingMergeAttackChains => ResourceManager.GetString("SettingMergeAttackChains", resourceCulture);

		internal static string SettingMergeAttackChainsText => ResourceManager.GetString("SettingMergeAttackChainsText", resourceCulture);

		internal static string SettingMergeMaxMsBuffs => ResourceManager.GetString("SettingMergeMaxMsBuffs", resourceCulture);

		internal static string SettingMergeMaxMsBuffsText => ResourceManager.GetString("SettingMergeMaxMsBuffsText", resourceCulture);

		internal static string SettingMergeMaxMsStrikes => ResourceManager.GetString("SettingMergeMaxMsStrikes", resourceCulture);

		internal static string SettingMergeMaxMsStrikesText => ResourceManager.GetString("SettingMergeMaxMsStrikesText", resourceCulture);

		internal static string SettingMinIconMargin => ResourceManager.GetString("SettingMinIconMargin", resourceCulture);

		internal static string SettingMinIconMarginText => ResourceManager.GetString("SettingMinIconMarginText", resourceCulture);

		internal static string SettingMultiIconMessages => ResourceManager.GetString("SettingMultiIconMessages", resourceCulture);

		internal static string SettingMultiIconMessagesText => ResourceManager.GetString("SettingMultiIconMessagesText", resourceCulture);

		internal static string SettingPetToMasterIsSelf => ResourceManager.GetString("SettingPetToMasterIsSelf", resourceCulture);

		internal static string SettingPetToMasterIsSelfText => ResourceManager.GetString("SettingPetToMasterIsSelfText", resourceCulture);

		internal Strings()
		{
		}
	}
}
