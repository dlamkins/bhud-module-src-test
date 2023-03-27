using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ControlExtensions
	{
		public static bool IsVisible(this Control control)
		{
			return IsParentSetAndVisible(control);
			static bool IsParentSetAndVisible(Control ctrl)
			{
				if (ctrl.get_Visible() && ctrl.get_Parent() != null)
				{
					if (ctrl.get_Parent() != GameService.Graphics.get_SpriteScreen() || !((Control)ctrl.get_Parent()).get_Visible())
					{
						if (((Control)ctrl.get_Parent()).get_Visible())
						{
							return IsParentSetAndVisible((Control)(object)ctrl.get_Parent());
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public static bool ToggleVisibility(this Control c, bool? visible = null)
		{
			c.set_Visible(visible ?? (!c.get_Visible()));
			return c.get_Visible();
		}

		public static void SetLocation(this Control c, int? x = null, int? y = null)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			int valueOrDefault = x.GetValueOrDefault();
			if (!x.HasValue)
			{
				valueOrDefault = c.get_Location().X;
				x = valueOrDefault;
			}
			valueOrDefault = y.GetValueOrDefault();
			if (!y.HasValue)
			{
				valueOrDefault = c.get_Location().Y;
				y = valueOrDefault;
			}
			c.set_Location(new Point(x.Value, y.Value));
		}

		public static void SetLocation(this Control c, Point location)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			c.set_Location(location);
		}

		public static void SetSize(this Control c, int? width = null, int? height = null)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			int valueOrDefault = width.GetValueOrDefault();
			if (!width.HasValue)
			{
				valueOrDefault = c.get_Width();
				width = valueOrDefault;
			}
			valueOrDefault = height.GetValueOrDefault();
			if (!height.HasValue)
			{
				valueOrDefault = c.get_Height();
				height = valueOrDefault;
			}
			c.set_Size(new Point(width.Value, height.Value));
		}

		public static void SetSize(this Control c, Point size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			c.set_Size(size);
		}
	}
}
