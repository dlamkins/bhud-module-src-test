using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Ui
{
	public sealed class AnnouncementToast : Container
	{
		public AnnouncementToast(ModuleSettings settings, string title, string subtitle, string body)
			: this()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont titleFont = settings.TitleFont();
			BitmapFont bodyFont = settings.BodyFont();
			int titleH = titleFont.get_LineHeight();
			int bodyH = bodyFont.get_LineHeight();
			int bodyBlock = bodyH * 3 + 4;
			int height = 12 + titleH + 4 + bodyH + 6 + bodyBlock + 12;
			((Control)this).set_Height(height);
			((Control)this).set_BackgroundColor(new Color(14, 14, 18, 235));
			Color accent = Color.get_Goldenrod();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(accent);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(4);
			((Control)val).set_Height(height);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(title);
			val2.set_Font(titleFont);
			val2.set_TextColor(accent);
			((Control)val2).set_Location(new Point(12, 8));
			((Control)val2).set_Width(320);
			((Control)val2).set_Height(titleH + 4);
			val2.set_AutoSizeWidth(false);
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("X");
			((Control)val3).set_Width(30);
			((Control)val3).set_Height(22);
			((Control)val3).set_Location(new Point(348, 6));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Dispose();
			});
			if (!string.IsNullOrWhiteSpace(subtitle))
			{
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)this);
				val4.set_Text(subtitle);
				val4.set_Font(bodyFont);
				val4.set_TextColor(new Color(230, 220, 180));
				((Control)val4).set_Location(new Point(12, 12 + titleH));
				((Control)val4).set_Width(360);
				((Control)val4).set_Height(bodyH + 4);
				val4.set_AutoSizeWidth(false);
			}
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(string.IsNullOrWhiteSpace(body) ? "(no content)" : body);
			val5.set_Font(bodyFont);
			val5.set_TextColor(new Color(220, 220, 220));
			((Control)val5).set_Location(new Point(12, 12 + titleH + 4 + bodyH + 4));
			((Control)val5).set_Width(360);
			((Control)val5).set_Height(bodyBlock);
			val5.set_AutoSizeWidth(false);
			val5.set_WrapText(true);
		}
	}
}
