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
	internal class Professions
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
					resourceMan = new ResourceManager("MysticCrafting.Module.Strings.Professions", typeof(Professions).Assembly);
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

		internal static string Armorsmith => ResourceManager.GetString("Armorsmith", resourceCulture);

		internal static string Artificer => ResourceManager.GetString("Artificer", resourceCulture);

		internal static string Chef => ResourceManager.GetString("Chef", resourceCulture);

		internal static string Huntsman => ResourceManager.GetString("Huntsman", resourceCulture);

		internal static string Jeweler => ResourceManager.GetString("Jeweler", resourceCulture);

		internal static string Leatherworker => ResourceManager.GetString("Leatherworker", resourceCulture);

		internal static string Scribe => ResourceManager.GetString("Scribe", resourceCulture);

		internal static string Tailor => ResourceManager.GetString("Tailor", resourceCulture);

		internal static string Weaponsmith => ResourceManager.GetString("Weaponsmith", resourceCulture);

		internal Professions()
		{
		}
	}
}
