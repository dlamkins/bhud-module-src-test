using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RaidWeekPlanner.Ressources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class areas
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
					resourceMan = new ResourceManager("RaidWeekPlanner.Ressources.areas", typeof(areas).Assembly);
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

		internal static string EODLabel => ResourceManager.GetString("EODLabel", resourceCulture);

		internal static string EODTooltip => ResourceManager.GetString("EODTooltip", resourceCulture);

		internal static string IBSLabel => ResourceManager.GetString("IBSLabel", resourceCulture);

		internal static string IBSTooltip => ResourceManager.GetString("IBSTooltip", resourceCulture);

		internal static string SOTOLabel => ResourceManager.GetString("SOTOLabel", resourceCulture);

		internal static string SOTOTooltip => ResourceManager.GetString("SOTOTooltip", resourceCulture);

		internal static string VOELabel => ResourceManager.GetString("VOELabel", resourceCulture);

		internal static string VOETooltip => ResourceManager.GetString("VOETooltip", resourceCulture);

		internal static string W1Label => ResourceManager.GetString("W1Label", resourceCulture);

		internal static string W1Tooltip => ResourceManager.GetString("W1Tooltip", resourceCulture);

		internal static string W2Label => ResourceManager.GetString("W2Label", resourceCulture);

		internal static string W2Tooltip => ResourceManager.GetString("W2Tooltip", resourceCulture);

		internal static string W3Label => ResourceManager.GetString("W3Label", resourceCulture);

		internal static string W3Tooltip => ResourceManager.GetString("W3Tooltip", resourceCulture);

		internal static string W4Label => ResourceManager.GetString("W4Label", resourceCulture);

		internal static string W4Tooltip => ResourceManager.GetString("W4Tooltip", resourceCulture);

		internal static string W5Label => ResourceManager.GetString("W5Label", resourceCulture);

		internal static string W5Tooltip => ResourceManager.GetString("W5Tooltip", resourceCulture);

		internal static string W6Label => ResourceManager.GetString("W6Label", resourceCulture);

		internal static string W6Tooltip => ResourceManager.GetString("W6Tooltip", resourceCulture);

		internal static string W7Label => ResourceManager.GetString("W7Label", resourceCulture);

		internal static string W7Tooltip => ResourceManager.GetString("W7Tooltip", resourceCulture);

		internal static string W8Label => ResourceManager.GetString("W8Label", resourceCulture);

		internal static string W8Tooltip => ResourceManager.GetString("W8Tooltip", resourceCulture);

		internal areas()
		{
		}
	}
}
