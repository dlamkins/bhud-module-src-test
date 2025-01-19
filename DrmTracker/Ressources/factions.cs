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
	internal class factions
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
					resourceMan = new ResourceManager("DrmTracker.Ressources.factions", typeof(factions).Assembly);
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

		internal static string CrystalLabel => ResourceManager.GetString("CrystalLabel", resourceCulture);

		internal static string CrystalTooltip => ResourceManager.GetString("CrystalTooltip", resourceCulture);

		internal static string DeldrimorLabel => ResourceManager.GetString("DeldrimorLabel", resourceCulture);

		internal static string DeldrimorTooltip => ResourceManager.GetString("DeldrimorTooltip", resourceCulture);

		internal static string EbonLabel => ResourceManager.GetString("EbonLabel", resourceCulture);

		internal static string EbonTooltip => ResourceManager.GetString("EbonTooltip", resourceCulture);

		internal static string ExaltedLabel => ResourceManager.GetString("ExaltedLabel", resourceCulture);

		internal static string ExaltedTooltip => ResourceManager.GetString("ExaltedTooltip", resourceCulture);

		internal static string KodanLabel => ResourceManager.GetString("KodanLabel", resourceCulture);

		internal static string KodanTooltip => ResourceManager.GetString("KodanTooltip", resourceCulture);

		internal static string LegionLabel => ResourceManager.GetString("LegionLabel", resourceCulture);

		internal static string LegionTooltip => ResourceManager.GetString("LegionTooltip", resourceCulture);

		internal static string OlmakhanLabel => ResourceManager.GetString("OlmakhanLabel", resourceCulture);

		internal static string OlmakhanTooltip => ResourceManager.GetString("OlmakhanTooltip", resourceCulture);

		internal static string SkrittLabel => ResourceManager.GetString("SkrittLabel", resourceCulture);

		internal static string SkrittTooltip => ResourceManager.GetString("SkrittTooltip", resourceCulture);

		internal static string TenguLabel => ResourceManager.GetString("TenguLabel", resourceCulture);

		internal static string TenguTooltip => ResourceManager.GetString("TenguTooltip", resourceCulture);

		internal factions()
		{
		}
	}
}
