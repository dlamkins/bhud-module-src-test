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
				if (ctrl.Visible && ctrl.Parent != null)
				{
					if (ctrl.Parent != GameService.Graphics.SpriteScreen || !ctrl.Parent.Visible)
					{
						if (ctrl.Parent.Visible)
						{
							return IsParentSetAndVisible(ctrl.Parent);
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public static bool IsDrawn(this Control c)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (c.Parent != null && c.Parent.Visible)
			{
				Rectangle absoluteBounds = c.Parent.AbsoluteBounds;
				Rectangle absoluteBounds2 = c.AbsoluteBounds;
				if (((Rectangle)(ref absoluteBounds)).Contains(((Rectangle)(ref absoluteBounds2)).get_Center()))
				{
					if (c.Parent != GameService.Graphics.SpriteScreen)
					{
						return c.Parent.IsDrawn();
					}
					return true;
				}
			}
			return false;
		}

		public static bool IsDrawn(this Control c, Rectangle b)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (c.Parent != null && c.Parent.Visible)
			{
				Rectangle absoluteBounds = c.Parent.AbsoluteBounds;
				if (((Rectangle)(ref absoluteBounds)).Contains(((Rectangle)(ref b)).get_Center()))
				{
					if (c.Parent != GameService.Graphics.SpriteScreen)
					{
						return c.Parent.IsDrawn(b);
					}
					return true;
				}
			}
			return false;
		}

		public static bool ToggleVisibility(this Control c, bool? visible = null)
		{
			c.Visible = visible ?? (!c.Visible);
			return c.Visible;
		}

		public static void SetLocation(this Control c, int? x = null, int? y = null)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			int valueOrDefault = x.GetValueOrDefault();
			if (!x.HasValue)
			{
				valueOrDefault = c.Location.X;
				x = valueOrDefault;
			}
			valueOrDefault = y.GetValueOrDefault();
			if (!y.HasValue)
			{
				valueOrDefault = c.Location.Y;
				y = valueOrDefault;
			}
			c.Location = new Point(x.Value, y.Value);
		}

		public static void SetLocation(this Control c, Point location)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			c.Location = location;
		}

		public static void SetSize(this Control c, int? width = null, int? height = null)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			int valueOrDefault = width.GetValueOrDefault();
			if (!width.HasValue)
			{
				valueOrDefault = c.Width;
				width = valueOrDefault;
			}
			valueOrDefault = height.GetValueOrDefault();
			if (!height.HasValue)
			{
				valueOrDefault = c.Height;
				height = valueOrDefault;
			}
			c.Size = new Point(width.Value, height.Value);
		}

		public static void SetSize(this Control c, Point size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			c.Size = size;
		}

		public static void SetBounds(this Control c, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			c.SetLocation(((Rectangle)(ref bounds)).get_Location());
			c.SetSize(((Rectangle)(ref bounds)).get_Size());
		}
	}
}
