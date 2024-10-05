using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Res
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class textures_common
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
					resourceMan = new ResourceManager("Kenedia.Modules." + Assembly.GetExecutingAssembly().FullName.Split(',')[0].Substring(Assembly.GetExecutingAssembly().FullName.Split(',')[0].LastIndexOf('.') + 1) + ".Res.textures_common", typeof(textures_common).Assembly);
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

		internal static Bitmap ImageButtonBackground => (Bitmap)ResourceManager.GetObject("ImageButtonBackground", resourceCulture);

		internal static Bitmap ImageButtonBackground_Hovered => (Bitmap)ResourceManager.GetObject("ImageButtonBackground_Hovered", resourceCulture);

		internal static Bitmap RollingChoya => (Bitmap)ResourceManager.GetObject("RollingChoya", resourceCulture);

		internal static Bitmap Tag => (Bitmap)ResourceManager.GetObject("Tag", resourceCulture);

		internal static Bitmap Tag_Hovered => (Bitmap)ResourceManager.GetObject("Tag_Hovered", resourceCulture);

		internal textures_common()
		{
		}
	}
}
