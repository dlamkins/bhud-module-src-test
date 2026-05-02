using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Blish_HUD.Extended.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
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

		internal static string Action_Accept => ResourceManager.GetString("Action_Accept", resourceCulture);

		internal static string Action_Apply => ResourceManager.GetString("Action_Apply", resourceCulture);

		internal static string Action_Cancel => ResourceManager.GetString("Action_Cancel", resourceCulture);

		internal static string Action_Close => ResourceManager.GetString("Action_Close", resourceCulture);

		internal static string Action_Confirm => ResourceManager.GetString("Action_Confirm", resourceCulture);

		internal static string Action_Decline => ResourceManager.GetString("Action_Decline", resourceCulture);

		internal static string Action_Ignore => ResourceManager.GetString("Action_Ignore", resourceCulture);

		internal static string Action_No => ResourceManager.GetString("Action_No", resourceCulture);

		internal static string Action_OK => ResourceManager.GetString("Action_OK", resourceCulture);

		internal static string Action_Yes => ResourceManager.GetString("Action_Yes", resourceCulture);

		internal static string API_is_down_ => ResourceManager.GetString("API is down.", resourceCulture);

		internal static string API_unavailable_ => ResourceManager.GetString("API unavailable.", resourceCulture);

		internal static string Apply => ResourceManager.GetString("Apply", resourceCulture);

		internal static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

		internal static string Close => ResourceManager.GetString("Close", resourceCulture);

		internal static string Confirm => ResourceManager.GetString("Confirm", resourceCulture);

		internal static string Ignore => ResourceManager.GetString("Ignore", resourceCulture);

		internal static string Insufficient_API_permissions_ => ResourceManager.GetString("Insufficient API permissions.", resourceCulture);

		internal static string Missing_API_key_ => ResourceManager.GetString("Missing API key.", resourceCulture);

		internal static string No => ResourceManager.GetString("No", resourceCulture);

		internal static string OK => ResourceManager.GetString("OK", resourceCulture);

		internal static string Please__add_an_API_key_to__0__ => ResourceManager.GetString("Please, add an API key to {0}.", resourceCulture);

		internal static string Please__login_to_a_character_ => ResourceManager.GetString("Please, login to a character.", resourceCulture);

		internal static string Please__try_again_later_ => ResourceManager.GetString("Please, try again later.", resourceCulture);

		internal static string Required___0_ => ResourceManager.GetString("Required: {0}", resourceCulture);

		internal static string Yes => ResourceManager.GetString("Yes", resourceCulture);

		internal Resources()
		{
		}
	}
}
