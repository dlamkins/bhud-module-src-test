using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.BuildsManager.Strings
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class common
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
					resourceMan = new ResourceManager("Kenedia.Modules.BuildsManager.Strings.common", typeof(common).Assembly);
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

		internal static string Build => ResourceManager.GetString("Build", resourceCulture);

		internal static string Concentration => ResourceManager.GetString("Concentration", resourceCulture);

		internal static string ConditionDamage => ResourceManager.GetString("ConditionDamage", resourceCulture);

		internal static string Copy => ResourceManager.GetString("Copy", resourceCulture);

		internal static string Create => ResourceManager.GetString("Create", resourceCulture);

		internal static string Delete => ResourceManager.GetString("Delete", resourceCulture);

		internal static string Expertise => ResourceManager.GetString("Expertise", resourceCulture);

		internal static string Ferocity => ResourceManager.GetString("Ferocity", resourceCulture);

		internal static string Gear => ResourceManager.GetString("Gear", resourceCulture);

		internal static string GearTab_Tips => ResourceManager.GetString("GearTab_Tips", resourceCulture);

		internal static string HealingPower => ResourceManager.GetString("HealingPower", resourceCulture);

		internal static string Power => ResourceManager.GetString("Power", resourceCulture);

		internal static string Precision => ResourceManager.GetString("Precision", resourceCulture);

		internal static string Search => ResourceManager.GetString("Search", resourceCulture);

		internal static string Template => ResourceManager.GetString("Template", resourceCulture);

		internal static string TemplateCode => ResourceManager.GetString("TemplateCode", resourceCulture);

		internal static string Toughness => ResourceManager.GetString("Toughness", resourceCulture);

		internal static string Vitality => ResourceManager.GetString("Vitality", resourceCulture);

		internal common()
		{
		}
	}
}
