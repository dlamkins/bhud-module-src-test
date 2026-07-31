using System;
using System.Text;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace TurtleMyWaypoint
{
	internal static class MapRowBuilder
	{
		private const int ChunkSize = 12;

		private const int LabelMarginLeft = 10;

		private const int LabelWidth = 320;

		private const int RightMargin = 16;

		private const int ButtonWidth = 90;

		private const int ButtonHeight = 24;

		private const int ButtonGap = 8;

		private const int RowMinHeight = 28;

		private const int RowSpacing = 6;

		private static readonly Logger Logger = Logger.GetLogger(typeof(MapRowBuilder));

		private static int ButtonsPerRow(int width)
		{
			int buttonsAreaWidth = width - 16 - 10 - 320;
			return Math.Max(1, (buttonsAreaWidth + 8) / 98);
		}

		private static int EntryHeight(MapEntry map, int buttonsPerRow)
		{
			int buttonRows = ((map.Codes.Length + 12 - 1) / 12 + buttonsPerRow - 1) / buttonsPerRow;
			return Math.Max(28, buttonRows * 24 + (buttonRows - 1) * 4);
		}

		public static int MeasureHeight(MapEntry[] maps, int width)
		{
			int buttonsPerRow = ButtonsPerRow(width);
			int height = 0;
			foreach (MapEntry map in maps)
			{
				height += EntryHeight(map, buttonsPerRow) + 6;
			}
			return height;
		}

		public static int BuildRows(Container parent, MapEntry[] maps, int width, int startY)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			int buttonsPerRow = ButtonsPerRow(width);
			int y = startY;
			for (int i = 0; i < maps.Length; i++)
			{
				MapEntry map = maps[i];
				int buttonCount = (map.Codes.Length + 12 - 1) / 12;
				int buttonRows = (buttonCount + buttonsPerRow - 1) / buttonsPerRow;
				int entryHeight = EntryHeight(map, buttonsPerRow);
				Label val = new Label();
				((Control)val).set_Parent(parent);
				val.set_Text(map.Name);
				((Control)val).set_Location(new Point(10, y));
				((Control)val).set_Size(new Point(320, entryHeight));
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_VerticalAlignment((VerticalAlignment)1);
				val.set_WrapText(false);
				for (int r = 0; r < buttonRows; r++)
				{
					int rowStart = r * buttonsPerRow;
					int buttonsInRow = Math.Min(buttonsPerRow, buttonCount - rowStart);
					int rowWidth = buttonsInRow * 90 + (buttonsInRow - 1) * 8;
					int rowStartX = width - 16 - rowWidth;
					for (int c = 0; c < buttonsInRow; c++)
					{
						int b = rowStart + c;
						int start = b * 12;
						int count = Math.Min(12, map.Codes.Length - start);
						StandardButton val2 = new StandardButton();
						((Control)val2).set_Parent(parent);
						val2.set_Text((buttonCount == 1) ? Strings.Copy : $"{Strings.Part} {b + 1}");
						((Control)val2).set_Size(new Point(90, 24));
						((Control)val2).set_Location(new Point(rowStartX + c * 98, y + r * 28));
						string clipboardText = BuildClipboardText(map.Codes, start, count);
						((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							TrySetClipboardText(clipboardText);
						});
					}
				}
				y += entryHeight + 6;
			}
			return y - startY;
		}

		private static string BuildClipboardText(string[] codes, int start, int count)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					sb.Append(Environment.NewLine);
				}
				sb.Append(start + i + 1).Append(' ').Append(codes[start + i]);
			}
			return sb.ToString();
		}

		private static void TrySetClipboardText(string text)
		{
			try
			{
				Clipboard.SetText(text);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to set clipboard.");
			}
		}
	}
}
