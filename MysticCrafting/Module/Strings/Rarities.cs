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
	internal class Rarities
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
					resourceMan = new ResourceManager("MysticCrafting.Module.Strings.Rarities", typeof(Rarities).Assembly);
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

		internal static string Ascended => ResourceManager.GetString("Ascended", resourceCulture);

		internal static string Basic => ResourceManager.GetString("Basic", resourceCulture);

		internal static string Exotic => ResourceManager.GetString("Exotic", resourceCulture);

		internal static string Fine => ResourceManager.GetString("Fine", resourceCulture);

		internal static string Legendary => ResourceManager.GetString("Legendary", resourceCulture);

		internal static string Masterwork => ResourceManager.GetString("Masterwork", resourceCulture);

		internal static string Rare => ResourceManager.GetString("Rare", resourceCulture);

		internal Rarities()
		{
		}
	}
}
