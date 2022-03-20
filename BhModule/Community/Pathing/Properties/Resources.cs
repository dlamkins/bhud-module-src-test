using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BhModule.Community.Pathing.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
					resourceMan = new ResourceManager("BhModule.Community.Pathing.Properties.Resources", typeof(Resources).Assembly);
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

		internal static Bitmap add => (Bitmap)ResourceManager.GetObject("add", resourceCulture);

		internal static Bitmap arrow_merge => (Bitmap)ResourceManager.GetObject("arrow_merge", resourceCulture);

		internal static Bitmap arrow_up_left_right_64px => (Bitmap)ResourceManager.GetObject("arrow_up_left_right_64px", resourceCulture);

		internal static Bitmap bin_closed => (Bitmap)ResourceManager.GetObject("bin_closed", resourceCulture);

		internal static Bitmap box => (Bitmap)ResourceManager.GetObject("box", resourceCulture);

		internal static Bitmap delete_64px => (Bitmap)ResourceManager.GetObject("delete_64px", resourceCulture);

		internal static Bitmap here_64px => (Bitmap)ResourceManager.GetObject("here_64px", resourceCulture);

		internal static Bitmap look_64px => (Bitmap)ResourceManager.GetObject("look_64px", resourceCulture);

		internal static Bitmap move_64px => (Bitmap)ResourceManager.GetObject("move_64px", resourceCulture);

		internal static Bitmap shape_square => (Bitmap)ResourceManager.GetObject("shape_square", resourceCulture);

		internal static Bitmap this_way_up_64px => (Bitmap)ResourceManager.GetObject("this_way_up_64px", resourceCulture);

		internal Resources()
		{
		}
	}
}
