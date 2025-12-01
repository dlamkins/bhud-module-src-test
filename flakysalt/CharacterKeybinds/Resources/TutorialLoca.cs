using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace flakysalt.CharacterKeybinds.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class TutorialLoca
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
					resourceMan = new ResourceManager("flakysalt.CharacterKeybinds.Resources.TutorialLoca", typeof(TutorialLoca).Assembly);
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

		internal static string closeButtonText => ResourceManager.GetString("closeButtonText", resourceCulture);

		internal static string initialSetupHeader => ResourceManager.GetString("initialSetupHeader", resourceCulture);

		internal static string initialSetupPanel1 => ResourceManager.GetString("initialSetupPanel1", resourceCulture);

		internal static string initialSetupPanel2 => ResourceManager.GetString("initialSetupPanel2", resourceCulture);

		internal static string initialSetupPanel3 => ResourceManager.GetString("initialSetupPanel3", resourceCulture);

		internal static string initialSetupPanel4 => ResourceManager.GetString("initialSetupPanel4", resourceCulture);

		internal static string initialSetupPanel5 => ResourceManager.GetString("initialSetupPanel5", resourceCulture);

		internal static string nextButtonText => ResourceManager.GetString("nextButtonText", resourceCulture);

		internal static string previousButtonText => ResourceManager.GetString("previousButtonText", resourceCulture);

		internal TutorialLoca()
		{
		}
	}
}
