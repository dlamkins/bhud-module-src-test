using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Blish_HUD.Extended.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
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
					resourceMan = new ResourceManager("Blish_HUD.Extended.Properties.Resources", typeof(Resources).Assembly);
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

		internal static string API_is_down_ => ResourceManager.GetString("API is down.", resourceCulture);

		internal static string API_unavailable_ => ResourceManager.GetString("API unavailable.", resourceCulture);

		internal static string Insufficient_API_permissions_ => ResourceManager.GetString("Insufficient API permissions.", resourceCulture);

		internal static string Missing_API_key_ => ResourceManager.GetString("Missing API key.", resourceCulture);

		internal static string Please__add_an_API_key_to__0__ => ResourceManager.GetString("Please, add an API key to {0}.", resourceCulture);

		internal static string Please__login_to_a_character_ => ResourceManager.GetString("Please, login to a character.", resourceCulture);

		internal static string Please__try_again_later_ => ResourceManager.GetString("Please, try again later.", resourceCulture);

		internal static string Required___0_ => ResourceManager.GetString("Required: {0}", resourceCulture);

		internal Resources()
		{
		}
	}
}
