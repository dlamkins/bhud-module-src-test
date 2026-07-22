using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ControlExtensions
	{
		public static bool SetTexture(this Image image, Texture2D texture2D)
		{
			if (image != null)
			{
				image.Texture = texture2D;
				return true;
			}
			return false;
		}

		public static bool IsParentSetAndVisible(this Control ctrl)
		{
			if (ctrl?.Parent?.Visible ?? false)
			{
				if (ctrl.Parent != GameService.Graphics.SpriteScreen)
				{
					return ctrl.Parent.IsParentSetAndVisible();
				}
				return true;
			}
			return false;
		}

		public static bool IsVisible(this Control ctrl)
		{
			if (ctrl?.Visible ?? false)
			{
				return ctrl.IsParentSetAndVisible();
			}
			return false;
		}

		public static bool IsDrawn(this Control c)
		{
			if (c.Parent != null && c.Parent.Visible && c.Parent.AbsoluteBounds.Contains(c.AbsoluteBounds.Center))
			{
				if (c.Parent != GameService.Graphics.SpriteScreen)
				{
					return c.Parent.IsDrawn();
				}
				return true;
			}
			return false;
		}

		public static bool IsDrawn(this Control c, Rectangle b)
		{
			if (c.Parent != null && c.Parent.Visible && c.Parent.AbsoluteBounds.Contains(b.Center))
			{
				if (c.Parent != GameService.Graphics.SpriteScreen)
				{
					return c.Parent.IsDrawn(b);
				}
				return true;
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
			c.Location = location;
		}

		public static void SetSize(this Control c, int? width = null, int? height = null)
		{
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
			c.Size = size;
		}

		public static void SetBounds(this Control c, Rectangle bounds)
		{
			c.SetLocation(bounds.Location);
			c.SetSize(bounds.Size);
		}
	}
}
