using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Ideka.HitboxView
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
					resourceMan = new ResourceManager("Ideka.HitboxView.Strings", typeof(Strings).Assembly);
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

		internal static string SettingGamePing => ResourceManager.GetString("SettingGamePing", resourceCulture);

		internal static string SettingGamePingText => ResourceManager.GetString("SettingGamePingText", resourceCulture);

		internal static string SettingGamePingValidation => ResourceManager.GetString("SettingGamePingValidation", resourceCulture);

		internal static string SettingHitboxColor => ResourceManager.GetString("SettingHitboxColor", resourceCulture);

		internal static string SettingHitboxColorText => ResourceManager.GetString("SettingHitboxColorText", resourceCulture);

		internal static string SettingHitboxOutlineColor => ResourceManager.GetString("SettingHitboxOutlineColor", resourceCulture);

		internal static string SettingHitboxOutlineColorText => ResourceManager.GetString("SettingHitboxOutlineColorText", resourceCulture);

		internal static string SettingHitboxSmoothing => ResourceManager.GetString("SettingHitboxSmoothing", resourceCulture);

		internal static string SettingHitboxSmoothingText => ResourceManager.GetString("SettingHitboxSmoothingText", resourceCulture);

		internal static string SettingHitboxVisible => ResourceManager.GetString("SettingHitboxVisible", resourceCulture);

		internal static string SettingHitboxVisibleText => ResourceManager.GetString("SettingHitboxVisibleText", resourceCulture);

		internal static string SettingToggleHitboxKey => ResourceManager.GetString("SettingToggleHitboxKey", resourceCulture);

		internal static string SettingToggleHitboxKeyText => ResourceManager.GetString("SettingToggleHitboxKeyText", resourceCulture);

		internal Strings()
		{
		}
	}
}
