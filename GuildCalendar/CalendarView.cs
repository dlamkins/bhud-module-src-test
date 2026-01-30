using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace GuildCalendar
{
	public class CalendarView : View
	{
		private const int HEADER_HEIGHT = 50;

		private const int DAY_HEADER_HEIGHT = 30;

		private const int FIXED_CELL_HEIGHT = 90;

		private DateTime _currentMonth;

		private List<GuildEvent> _events;

		private Func<bool> _canEdit;

		private Label _monthLabel;

		private Label _statusLabel;

		private Panel _gridPanel;

		private string _currentStatusText = "";

		private Color _currentStatusColor = Color.get_Transparent();

		private readonly TimeZoneInfo _etZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

		public CalendarView(Func<bool> canEdit)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_canEdit = canEdit;
			_currentMonth = DateTime.Now;
			_events = new List<GuildEvent>();
		}

		public void UpdateStatus(string message, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			_currentStatusText = ((color == Color.get_Green()) ? "" : message);
			_currentStatusColor = ((color == Color.get_Green()) ? Color.get_Transparent() : color);
			if (_statusLabel != null)
			{
				_statusLabel.set_Text(_currentStatusText);
				_statusLabel.set_TextColor(_currentStatusColor);
			}
		}

		public void UpdateEvents(List<GuildEvent> events)
		{
			_events = events ?? new List<GuildEvent>();
			if (_gridPanel != null)
			{
				RenderCalendar();
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.8f);
			((Control)val).set_ZIndex(0);
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width(), 50));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_ZIndex(1);
			Panel headerPanel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Text("<");
			((Control)val3).set_Width(30);
			((Control)val3).set_Location(new Point(20, 10));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChangeMonth(-1);
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)headerPanel);
			val4.set_Text(_currentMonth.ToString("MMMM yyyy"));
			val4.set_Font(GameService.Content.get_DefaultFont32());
			val4.set_AutoSizeWidth(false);
			((Control)val4).set_Width(250);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_Location(new Point(50, 0));
			((Control)val4).set_Size(new Point(250, 40));
			_monthLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)headerPanel);
			val5.set_Text(">");
			((Control)val5).set_Width(30);
			((Control)val5).set_Location(new Point(300, 10));
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChangeMonth(1);
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)headerPanel);
			val6.set_Text(_currentStatusText);
			val6.set_TextColor(_currentStatusColor);
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Location(new Point(400, 15));
			_statusLabel = val6;
			Panel val7 = new Panel();
			((Control)val7).set_Parent(buildPanel);
			((Control)val7).set_Location(new Point(0, 50));
			((Control)val7).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height() - 50));
			((Control)val7).set_ZIndex(1);
			_gridPanel = val7;
			RenderCalendar();
		}

		private void ChangeMonth(int offset)
		{
			_currentMonth = _currentMonth.AddMonths(offset);
			if (_monthLabel != null)
			{
				_monthLabel.set_Text(_currentMonth.ToString("MMMM yyyy"));
			}
			RenderCalendar();
		}

		private DateTime GetLocalEventDate(GuildEvent evt)
		{
			return TimeZoneInfo.ConvertTime(evt.Date, _etZone, TimeZoneInfo.Local).Date;
		}

		private void RenderCalendar()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			if (_gridPanel == null)
			{
				return;
			}
			((Container)_gridPanel).ClearChildren();
			int cellWidth = ((Control)_gridPanel).get_Width() / 7;
			string[] days = new string[7] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
			for (int j = 0; j < 7; j++)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_gridPanel);
				val.set_Text(days[j]);
				((Control)val).set_Size(new Point(cellWidth, 30));
				((Control)val).set_Location(new Point(j * cellWidth, 0));
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_TextColor(Color.get_Yellow());
			}
			DateTime firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
			int startOffset = (int)(firstDayOfMonth.DayOfWeek - 1);
			if (startOffset < 0)
			{
				startOffset = 6;
			}
			DateTime startDate = firstDayOfMonth.AddDays(-startOffset);
			for (int i = 0; i < 35; i++)
			{
				DateTime cellDate = startDate.AddDays(i);
				int row = i / 7;
				int col = i % 7;
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)_gridPanel);
				((Control)val2).set_Size(new Point(cellWidth - 2, 88));
				((Control)val2).set_Location(new Point(col * cellWidth, row * 90 + 30));
				((Control)val2).set_BackgroundColor((cellDate.Month == _currentMonth.Month) ? (Color.get_Black() * 0.4f) : (Color.get_Black() * 0.1f));
				Panel cell = val2;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)cell);
				val3.set_Text(cellDate.Day.ToString());
				((Control)val3).set_Location(new Point(5, 5));
				val3.set_TextColor((cellDate.Date == DateTime.Now.Date) ? Color.get_Red() : Color.get_White());
				List<GuildEvent> list = _events.Where((GuildEvent e) => GetLocalEventDate(e) == cellDate.Date).ToList();
				int yPos = 25;
				foreach (GuildEvent evt in list)
				{
					if (yPos > 70)
					{
						break;
					}
					Label val4 = new Label();
					((Control)val4).set_Parent((Container)(object)cell);
					val4.set_Text(evt.Title);
					((Control)val4).set_Location(new Point(4, yPos));
					val4.set_AutoSizeWidth(false);
					((Control)val4).set_Width(cellWidth - 6);
					val4.set_TextColor(Color.get_LightBlue());
					val4.set_Font(GameService.Content.get_DefaultFont12());
					((Control)val4).set_BasicTooltipText("Click for Details");
					((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ShowDetails(evt);
					});
					yPos += 16;
				}
			}
		}

		private void ShowDetails(GuildEvent evt)
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Expected O, but got Unknown
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Expected O, but got Unknown
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Expected O, but got Unknown
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Expected O, but got Unknown
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			int contentWidth = 720;
			DateTime localTime = TimeZoneInfo.ConvertTime(evt.Date, _etZone, TimeZoneInfo.Local);
			string descText = (string.IsNullOrEmpty(evt.Description) ? "No notes provided." : evt.Description.Replace("\\,", ",").Replace("\\;", ";").Replace("\\\\", "\\")
				.Replace("\\n", "\n")
				.Replace("\\N", "\n")
				.Replace("\r", "")
				.Trim());
			StandardWindow val = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Event Details");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156022)));
			((Control)val).set_Location(new Point(300, 200));
			((WindowBase2)val).set_SavesPosition(true);
			((Control)val).set_Size(new Point(850, 650));
			StandardWindow window = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)window);
			((Control)val2).set_Size(new Point(((Container)window).get_ContentRegion().Width, ((Container)window).get_ContentRegion().Height));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_BackgroundColor(Color.get_Black() * 0.85f);
			Panel content = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)content);
			val3.set_Text(evt.Title);
			val3.set_Font(GameService.Content.get_DefaultFont32());
			val3.set_TextColor(Color.get_White());
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Width(contentWidth);
			((Control)val3).set_Location(new Point(30, 40));
			val3.set_WrapText(true);
			Label titleLbl = val3;
			string timeStr = $"{localTime:dddd, MMMM dd • h:mm tt} local time ({evt.Date:h:mm tt} ET)";
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)content);
			val4.set_Text(timeStr);
			val4.set_Font(GameService.Content.get_DefaultFont32());
			val4.set_TextColor(Color.get_LightCyan());
			((Control)val4).set_Location(new Point(30, ((Control)titleLbl).get_Bottom() + 30));
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Width(contentWidth);
			Label timeLbl = val4;
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)content);
			((Control)val5).set_BackgroundColor(Color.get_Gray());
			((Control)val5).set_Height(2);
			((Control)val5).set_Width(contentWidth);
			((Control)val5).set_Location(new Point(30, ((Control)timeLbl).get_Bottom() + 40));
			Panel sep = val5;
			int remainingHeight = ((Control)content).get_Height() - ((Control)sep).get_Bottom() - 100;
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)content);
			((Control)val6).set_Location(new Point(30, ((Control)sep).get_Bottom() + 40));
			((Control)val6).set_Size(new Point(contentWidth + 40, remainingHeight));
			val6.set_CanScroll(true);
			((Control)val6).set_BackgroundColor(Color.get_Black() * 0.7f);
			Panel descPanel = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)descPanel);
			val7.set_Text(descText);
			((Control)val7).set_Width(contentWidth);
			((Control)val7).set_Location(new Point(20, 20));
			val7.set_AutoSizeHeight(true);
			val7.set_WrapText(true);
			val7.set_TextColor(Color.get_Beige());
			val7.set_Font(GameService.Content.get_DefaultFont18());
			((Control)window).Show();
		}
	}
}
