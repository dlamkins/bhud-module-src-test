using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Controls
{
	internal class ProfileScrollList : Panel
	{
		private class MouseWheelPanel : Panel
		{
			protected override CaptureType CapturesInput()
			{
				return (CaptureType)12;
			}

			public MouseWheelPanel()
				: this()
			{
			}
		}

		private const int ScrollbarWidth = 12;

		private const int ScrollbarGap = 4;

		private readonly int _listWidth;

		private readonly int _rowHeight;

		private readonly int _rowGap;

		private readonly Panel _rowsPanel;

		private readonly Scrollbar _scrollbar;

		public ProfileScrollList(int listWidth, int listHeight, int rowHeight, int rowGap = 4)
			: this()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			_listWidth = listWidth;
			_rowHeight = rowHeight;
			_rowGap = rowGap;
			((Panel)this).set_ShowBorder(false);
			((Control)this).set_Size(new Point(listWidth + 4 + 12, listHeight));
			MouseWheelPanel mouseWheelPanel = new MouseWheelPanel();
			((Panel)mouseWheelPanel).set_ShowBorder(false);
			((Control)mouseWheelPanel).set_Location(Point.get_Zero());
			((Control)mouseWheelPanel).set_Size(new Point(listWidth, listHeight));
			((Control)mouseWheelPanel).set_BackgroundColor(new Color(0, 0, 0, 0));
			((Control)mouseWheelPanel).set_ClipsBounds(true);
			((Control)mouseWheelPanel).set_Parent((Container)(object)this);
			_rowsPanel = (Panel)(object)mouseWheelPanel;
			Scrollbar val = new Scrollbar((Container)(object)_rowsPanel);
			((Control)val).set_Location(new Point(listWidth + 4, 0));
			((Control)val).set_Size(new Point(12, listHeight));
			((Control)val).set_Parent((Container)(object)this);
			_scrollbar = val;
		}

		public Panel AddRow(int index, string tooltipText)
		{
			return AddRow(index, null, tooltipText);
		}

		public Panel AddRow(int index, Tooltip tooltip)
		{
			return AddRow(index, tooltip, string.Empty);
		}

		private Panel AddRow(int index, Tooltip tooltip, string tooltipText)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Location(new Point(0, index * (_rowHeight + _rowGap)));
			((Control)val).set_Size(new Point(_listWidth, _rowHeight));
			((Control)val).set_BackgroundColor((index % 2 == 0) ? new Color(0, 0, 0, 70) : new Color(20, 20, 20, 70));
			((Control)val).set_Parent((Container)(object)_rowsPanel);
			((Control)val).set_BasicTooltipText((tooltip == null) ? (tooltipText ?? string.Empty) : null);
			((Control)val).set_Tooltip(tooltip);
			return val;
		}

		public Label AddCell(Container parent, string text, int x, int y, int width, Color color)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			BitmapFont font = GameService.Content.get_DefaultFont14();
			Label val = new Label();
			val.set_Text(FitCellText(text, width, font));
			val.set_Font(font);
			val.set_TextColor(color);
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, Math.Max(24, _rowHeight - y)));
			((Control)val).set_Parent(parent);
			return val;
		}

		private static string FitCellText(string text, int width, BitmapFont font)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			text = (string.IsNullOrWhiteSpace(text) ? "-" : text.Trim());
			if (font == null || width <= 0 || font.MeasureString(text).Width <= (float)width)
			{
				return text;
			}
			if (font.MeasureString("...").Width >= (float)width)
			{
				return "...";
			}
			while (text.Length > 0 && font.MeasureString(text + "...").Width > (float)width)
			{
				text = text.Substring(0, text.Length - 1).TrimEnd();
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text + "...";
			}
			return "...";
		}

		public void ShowEmptyMessage(string text)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			ClearRows();
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(new Color(220, 220, 220));
			((Control)val).set_Location(new Point(12, 12));
			((Control)val).set_Size(new Point(Math.Max(0, _listWidth - 24), 30));
			((Control)val).set_Parent((Container)(object)_rowsPanel);
		}

		public void ClearRows(bool resetScroll = true)
		{
			Control[] array = ((Container)_rowsPanel).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			if (resetScroll)
			{
				ResetScroll();
			}
		}

		public void ResetScroll()
		{
			((Container)_rowsPanel).set_VerticalScrollOffset(0);
			if (_scrollbar != null)
			{
				_scrollbar.set_ScrollDistance(0f);
			}
		}

		public static Panel AddInteractionLayer(Container row, Tooltip tooltip, Action click, int rightInset = 0)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005e: Expected O, but got Unknown
			if (row == null)
			{
				return null;
			}
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Location(Point.get_Zero());
			((Control)val).set_Size(new Point(Math.Max(0, ((Control)row).get_Width() - rightInset), ((Control)row).get_Height()));
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			((Control)val).set_Parent(row);
			((Control)val).set_ZIndex(100);
			WireInteraction((Control)val, tooltip, click);
			return val;
		}

		public static void WireInteraction(Control control, string tooltipText, Action click)
		{
			control.set_BasicTooltipText(tooltipText ?? string.Empty);
			control.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				click?.Invoke();
			});
		}

		public static void WireInteraction(Control control, Tooltip tooltip, Action click)
		{
			control.set_BasicTooltipText((string)null);
			control.set_Tooltip(tooltip);
			control.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				click?.Invoke();
			});
		}
	}
}
