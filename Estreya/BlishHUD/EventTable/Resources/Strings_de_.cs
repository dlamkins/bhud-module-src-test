using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Estreya.BlishHUD.EventTable.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings_de_
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
					resourceMan = new ResourceManager("Estreya.BlishHUD.EventTable.Resources.Strings.de.", typeof(Strings_de_).Assembly);
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

		internal static string event_dayNightCycle_dawn => ResourceManager.GetString("event-dayNightCycle-dawn", resourceCulture);

		internal static string eventCategory_dayNightCycle => ResourceManager.GetString("eventCategory-dayNightCycle", resourceCulture);

		internal Strings_de_()
		{
		}
	}
}
