using System;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace GW2app
{
	internal static class ActionButton
	{
		public const int BaseWidth = 180;

		public const int BaseHeight = 26;

		public static readonly Color LabelBg = new Color(66, 68, 89);

		public static readonly Color LabelBgHover = new Color(189, 148, 250);

		public static readonly Color LabelText = new Color(250, 250, 250);

		public static readonly Color LabelTextHover = new Color(13, 9, 22);

		private static readonly FieldInfo _labelBaseFontField = typeof(LabelBase).GetField("_font", BindingFlags.Instance | BindingFlags.NonPublic);

		public static int WidthFor(float uiScale)
		{
			return (int)Math.Round(180f * uiScale);
		}

		public static int HeightFor(float uiScale)
		{
			return (int)Math.Round(26f * uiScale);
		}

		public static Control Create(Container parent, string text, Point location, int width, int height, float uiScale, GW2appWindow.WindowTheme theme, Action onClick)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			if (theme == GW2appWindow.WindowTheme.Game)
			{
				StandardButton val = new StandardButton();
				val.set_Text(text);
				((Control)val).set_Width(width);
				((Control)val).set_Height(height);
				((Control)val).set_Location(location);
				((Control)val).set_Parent(parent);
				StandardButton btn = val;
				if (uiScale < 1f && _labelBaseFontField != null)
				{
					try
					{
						_labelBaseFontField.SetValue(btn, GameService.Content.get_DefaultFont12());
					}
					catch
					{
					}
					((Control)btn).Invalidate();
				}
				((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					onClick();
				});
				return (Control)(object)btn;
			}
			Label val2 = new Label();
			val2.set_Text(text);
			val2.set_Font((uiScale < 1f) ? GameService.Content.get_DefaultFont12() : GameService.Content.get_DefaultFont14());
			val2.set_TextColor(LabelText);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Width(width);
			((Control)val2).set_Height(height);
			((Control)val2).set_Location(location);
			((Control)val2).set_BackgroundColor(LabelBg);
			((Control)val2).set_Parent(parent);
			Label lbl = val2;
			((Control)lbl).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				((Control)lbl).set_BackgroundColor(LabelBgHover);
				lbl.set_TextColor(LabelTextHover);
			});
			((Control)lbl).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				((Control)lbl).set_BackgroundColor(LabelBg);
				lbl.set_TextColor(LabelText);
			});
			((Control)lbl).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onClick();
			});
			return (Control)(object)lbl;
		}
	}
}
