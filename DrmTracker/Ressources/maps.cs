using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DrmTracker.Ressources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class maps
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
					resourceMan = new ResourceManager("DrmTracker.Ressources.maps", typeof(maps).Assembly);
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

		internal static string BloodtideLabel => ResourceManager.GetString("BloodtideLabel", resourceCulture);

		internal static string BloodtideTooltip => ResourceManager.GetString("BloodtideTooltip", resourceCulture);

		internal static string BrisbanLabel => ResourceManager.GetString("BrisbanLabel", resourceCulture);

		internal static string BrisbanTooltip => ResourceManager.GetString("BrisbanTooltip", resourceCulture);

		internal static string CaledonLabel => ResourceManager.GetString("CaledonLabel", resourceCulture);

		internal static string CaledonTooltip => ResourceManager.GetString("CaledonTooltip", resourceCulture);

		internal static string DoricLabel => ResourceManager.GetString("DoricLabel", resourceCulture);

		internal static string DoricTooltip => ResourceManager.GetString("DoricTooltip", resourceCulture);

		internal static string FireheartLabel => ResourceManager.GetString("FireheartLabel", resourceCulture);

		internal static string FireheartTooltip => ResourceManager.GetString("FireheartTooltip", resourceCulture);

		internal static string GendarranLabel => ResourceManager.GetString("GendarranLabel", resourceCulture);

		internal static string GendarranTooltip => ResourceManager.GetString("GendarranTooltip", resourceCulture);

		internal static string MetricaLabel => ResourceManager.GetString("MetricaLabel", resourceCulture);

		internal static string MetricaTooltip => ResourceManager.GetString("MetricaTooltip", resourceCulture);

		internal static string RuinsLabel => ResourceManager.GetString("RuinsLabel", resourceCulture);

		internal static string RuinsTooltip => ResourceManager.GetString("RuinsTooltip", resourceCulture);

		internal static string SnowdenLabel => ResourceManager.GetString("SnowdenLabel", resourceCulture);

		internal static string SnowdenTooltip => ResourceManager.GetString("SnowdenTooltip", resourceCulture);

		internal static string ThunderheadLabel => ResourceManager.GetString("ThunderheadLabel", resourceCulture);

		internal static string ThunderheadTooltip => ResourceManager.GetString("ThunderheadTooltip", resourceCulture);

		internal maps()
		{
		}
	}
}
