using System;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	internal static class ControlExtensions
	{
		private static void Arrange(this Control from, int spacing, Func<Control, int> pGet, Action<Control, int> nSet, params Control[] others)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Control prev = from;
			foreach (Control control in others)
			{
				control.set_Location(prev.get_Location());
				nSet(control, pGet(prev) + spacing);
				prev = control;
			}
		}

		public static void ArrangeTopDown(this Control from, int spacing, params Control[] others)
		{
			from.Arrange(spacing, (Control c) => c.get_Bottom(), delegate(Control c, int x)
			{
				c.set_Top(x);
			}, others);
		}

		public static void ArrangeBottomUp(this Control from, int spacing, params Control[] others)
		{
			from.Arrange(-spacing, (Control c) => c.get_Top(), delegate(Control c, int x)
			{
				c.set_Bottom(x);
			}, others);
		}

		public static void ArrangeLeftRight(this Control from, int spacing, params Control[] others)
		{
			from.Arrange(spacing, (Control c) => c.get_Right(), delegate(Control c, int x)
			{
				c.set_Left(x);
			}, others);
		}

		public static void ArrangeRightLeft(this Control from, int spacing, params Control[] others)
		{
			from.Arrange(-spacing, (Control c) => c.get_Left(), delegate(Control c, int x)
			{
				c.set_Right(x);
			}, others);
		}

		public static void WidthFillRight(this Control control, int spacing = 0)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			control.set_Width(control.get_Parent().get_ContentRegion().Width - control.get_Left() - spacing);
		}

		public static void WidthFillLeft(this Control control, int spacing = 0)
		{
			control.set_Width(control.get_Right() - spacing);
			control.set_Left(0);
		}

		public static void HeightFillDown(this Control control, int spacing = 0)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			control.set_Height(control.get_Parent().get_ContentRegion().Height - control.get_Top() - spacing);
		}

		public static void HeightFillUp(this Control control, int spacing = 0)
		{
			control.set_Height(control.get_Bottom() - spacing);
			control.set_Top(0);
		}

		public static void CenterWith(this Control control, Control other)
		{
			control.set_Left(other.get_Left() + other.get_Width() / 2 - control.get_Width() / 2);
		}

		public static void MiddleWith(this Control control, Control other)
		{
			control.set_Top(other.get_Top() + other.get_Height() / 2 - control.get_Height() / 2);
		}

		public static void AlignCenter(this Control control)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			control.set_Left(control.get_Parent().get_ContentRegion().Width / 2 - control.get_Width() / 2);
		}

		public static void AlignMiddle(this Control control)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			control.set_Top(control.get_Parent().get_ContentRegion().Height / 2 - control.get_Height() / 2);
		}

		public static void SetContentRegionWidth(this Container container, int width)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			int diff = ((Control)container).get_Width() - container.get_ContentRegion().Width;
			((Control)container).set_Width(width + diff);
		}

		public static void SetContentRegionHeight(this Container container, int height)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			int diff = ((Control)container).get_Height() - container.get_ContentRegion().Height;
			((Control)container).set_Height(height + diff);
		}

		public static bool IsVisible(this Control control)
		{
			while (control != GameService.Graphics.get_SpriteScreen())
			{
				if (control == null || !control.get_Visible())
				{
					return false;
				}
				control = (Control)(object)control.get_Parent();
			}
			return true;
		}

		public static float NearestScrollTarget(this Panel panel, Control control)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			int viewportTop = ((Container)panel).get_VerticalScrollOffset();
			int viewportHeight = ((Container)panel).get_ContentRegion().Height;
			if (control.get_Top() < viewportTop)
			{
				return control.get_Top();
			}
			if (control.get_Bottom() > viewportTop + viewportHeight)
			{
				return control.get_Bottom() - viewportHeight;
			}
			return -1f;
		}
	}
}
