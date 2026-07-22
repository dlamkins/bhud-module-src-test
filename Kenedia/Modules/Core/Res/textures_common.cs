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
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
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

		internal static Bitmap Cancel => (Bitmap)ResourceManager.GetObject("Cancel", resourceCulture);

		internal static Bitmap Cancel_Active => (Bitmap)ResourceManager.GetObject("Cancel_Active", resourceCulture);

		internal static Bitmap Cancel_Hovered => (Bitmap)ResourceManager.GetObject("Cancel_Hovered", resourceCulture);

		internal static Bitmap Copy => (Bitmap)ResourceManager.GetObject("Copy", resourceCulture);

		internal static Bitmap Copy_Active => (Bitmap)ResourceManager.GetObject("Copy_Active", resourceCulture);

		internal static Bitmap Copy_Hovered => (Bitmap)ResourceManager.GetObject("Copy_Hovered", resourceCulture);

		internal static Bitmap Delete => (Bitmap)ResourceManager.GetObject("Delete", resourceCulture);

		internal static Bitmap Delete_Active => (Bitmap)ResourceManager.GetObject("Delete_Active", resourceCulture);

		internal static Bitmap Delete_Hovered => (Bitmap)ResourceManager.GetObject("Delete_Hovered", resourceCulture);

		internal static Bitmap ImageButtonBackground => (Bitmap)ResourceManager.GetObject("ImageButtonBackground", resourceCulture);

		internal static Bitmap ImageButtonBackground_Hovered => (Bitmap)ResourceManager.GetObject("ImageButtonBackground_Hovered", resourceCulture);

		internal static Bitmap RollingChoya => (Bitmap)ResourceManager.GetObject("RollingChoya", resourceCulture);

		internal static Bitmap Save => (Bitmap)ResourceManager.GetObject("Save", resourceCulture);

		internal static Bitmap Save_Active => (Bitmap)ResourceManager.GetObject("Save_Active", resourceCulture);

		internal static Bitmap Save_Hovered => (Bitmap)ResourceManager.GetObject("Save_Hovered", resourceCulture);

		internal static Bitmap Tag => (Bitmap)ResourceManager.GetObject("Tag", resourceCulture);

		internal static Bitmap Tag_Hovered => (Bitmap)ResourceManager.GetObject("Tag_Hovered", resourceCulture);

		internal static Bitmap ToggleAreaLeft => (Bitmap)ResourceManager.GetObject("ToggleAreaLeft", resourceCulture);

		internal static Bitmap ToggleAreaMid => (Bitmap)ResourceManager.GetObject("ToggleAreaMid", resourceCulture);

		internal static Bitmap ToggleAreaRight => (Bitmap)ResourceManager.GetObject("ToggleAreaRight", resourceCulture);

		internal textures_common()
		{
		}
	}
}
