using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	public sealed class GuideComparison : IDisposable
	{
		private readonly StandardWindow _window;

		public GuideComparison(GuideEntry custom, GuideEntry bundled)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			StandardWindow val = new StandardWindow(AsyncTexture2D.FromAssetId(155997), new Rectangle(25, 26, 560, 640), new Rectangle(40, 50, 540, 590));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Compare guide versions");
			((WindowBase2)val).set_Subtitle(custom.Name);
			((Control)val).set_Location(new Point(300, 200));
			((WindowBase2)val).set_CanClose(true);
			((WindowBase2)val).set_CanCloseWithEscape(true);
			_window = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_window);
			((Control)val2).set_Location(new Point(8, 5));
			((Control)val2).set_Size(new Point(235, 24));
			val2.set_Text("Your saved notes");
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_window);
			((Control)val3).set_Location(new Point(260, 5));
			((Control)val3).set_Size(new Point(235, 24));
			val3.set_Text("Latest bundled guide");
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)_window);
			((Control)val4).set_Location(new Point(8, 32));
			((Control)val4).set_Size(new Point(240, 505));
			val4.set_CanScroll(true);
			Panel left = val4;
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)_window);
			((Control)val5).set_Location(new Point(260, 32));
			((Control)val5).set_Size(new Point(240, 505));
			val5.set_CanScroll(true);
			Populate(left, custom);
			Populate(val5, bundled);
			((Control)_window).Show();
		}

		private static void Populate(Panel panel, GuideEntry guide)
		{
			int top = 8;
			bool[] array = new bool[2] { false, true };
			foreach (bool mode in array)
			{
				IReadOnlyList<Mechanic> notes = guide.GetMechanics(mode);
				if (mode && notes.Count == 0)
				{
					continue;
				}
				Add(panel, mode ? "Challenge mode" : "Normal mode", ref top);
				foreach (Mechanic note in notes)
				{
					Add(panel, "• " + note.Text, ref top);
				}
			}
		}

		private static void Add(Panel panel, string text, ref int top)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Location(new Point(5, top));
			((Control)val).set_Width(210);
			val.set_Text(text);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			val.set_TextColor(Color.get_White());
			Label label = val;
			top += ((Control)label).get_Height() + 10;
		}

		public void Dispose()
		{
			((Control)_window).Dispose();
		}
	}
}
