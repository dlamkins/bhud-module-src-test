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

		private const int FIXED_CELL_HEIGHT = 100;

		private DateTime _currentMonth;

		private List<GuildEvent> _events;

		private Func<bool> _canEdit;

		private Label _monthLabel;

		private Label _statusLabel;

		private Panel _gridPanel;

		private string _currentStatusText = "";

		private Color _currentStatusColor = Color.get_Transparent();

		private readonly Color _themeDarkBg = new Color(20, 24, 29);

		private readonly Color _themeCellBg = new Color(35, 40, 47);

		private readonly Color _themeCellOffMonth = new Color(25, 28, 33);

		private readonly Color _themeTodayBg = new Color(60, 50, 25);

		private readonly Color _themeAgedGold = new Color(201, 168, 76);

		private readonly Color _themeEventPill = new Color(45, 90, 130);

		private readonly TimeZoneInfo _etZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

		public CalendarView(Func<bool> canEdit)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_BackgroundColor(_themeDarkBg);
			((Control)val).set_ZIndex(0);
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width(), 50));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_ZIndex(1);
			((Control)val2).set_BackgroundColor(Color.get_Black() * 0.4f);
			Panel headerPanel = val2;
			int centerX = ((Control)headerPanel).get_Width() / 2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Text("Prev");
			((Control)val3).set_Width(50);
			((Control)val3).set_Height(30);
			((Control)val3).set_Location(new Point(centerX - 160, 10));
			((Control)val3).set_BasicTooltipText("Previous Month");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChangeMonth(-1);
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)headerPanel);
			val4.set_Text(_currentMonth.ToString("MMMM yyyy").ToUpper());
			val4.set_Font(GameService.Content.get_DefaultFont32());
			val4.set_TextColor(_themeAgedGold);
			((Control)val4).set_Size(new Point(200, 50));
			((Control)val4).set_Location(new Point(centerX - 100, 0));
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			_monthLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)headerPanel);
			val5.set_Text("Next");
			((Control)val5).set_Width(50);
			((Control)val5).set_Height(30);
			((Control)val5).set_Location(new Point(centerX + 110, 10));
			((Control)val5).set_BasicTooltipText("Next Month");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChangeMonth(1);
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)headerPanel);
			val6.set_Text(_currentStatusText);
			val6.set_TextColor(_currentStatusColor);
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Location(new Point(((Control)headerPanel).get_Width() - 150, 15));
			val6.set_Font(GameService.Content.get_DefaultFont16());
			_statusLabel = val6;
			int perfectGridWidth = (((Control)buildPanel).get_Width() - 40) / 7 * 7;
			Panel val7 = new Panel();
			((Control)val7).set_Parent(buildPanel);
			((Control)val7).set_Location(new Point((((Control)buildPanel).get_Width() - perfectGridWidth) / 2, 60));
			((Control)val7).set_Size(new Point(perfectGridWidth, ((Control)buildPanel).get_Height() - 50 - 20));
			((Control)val7).set_BackgroundColor(Color.get_Black() * 0.8f);
			((Control)val7).set_ZIndex(1);
			_gridPanel = val7;
			RenderCalendar();
		}

		private void ChangeMonth(int offset)
		{
			_currentMonth = _currentMonth.AddMonths(offset);
			if (_monthLabel != null)
			{
				_monthLabel.set_Text(_currentMonth.ToString("MMMM yyyy").ToUpper());
			}
			RenderCalendar();
		}

		private DateTime GetLocalEventDate(GuildEvent evt)
		{
			return TimeZoneInfo.ConvertTime(evt.Date, _etZone, TimeZoneInfo.Local).Date;
		}

		private void RenderCalendar()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Expected O, but got Unknown
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Expected O, but got Unknown
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			if (_gridPanel == null)
			{
				return;
			}
			((Container)_gridPanel).ClearChildren();
			int cellWidth = ((Control)_gridPanel).get_Width() / 7;
			string[] days = new string[7] { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_gridPanel);
			((Control)val).set_Size(new Point(((Control)_gridPanel).get_Width(), 30));
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.5f);
			Panel dayHeaderBg = val;
			for (int j = 0; j < 7; j++)
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)dayHeaderBg);
				val2.set_Text(days[j]);
				((Control)val2).set_Size(new Point(cellWidth, 30));
				((Control)val2).set_Location(new Point(j * cellWidth, 0));
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				val2.set_TextColor(_themeAgedGold);
				val2.set_Font(GameService.Content.get_DefaultFont14());
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
				bool isToday = cellDate.Date == DateTime.Now.Date;
				bool isCurrentMonth = cellDate.Month == _currentMonth.Month;
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)_gridPanel);
				((Control)val3).set_Size(new Point(cellWidth - 1, 99));
				((Control)val3).set_Location(new Point(col * cellWidth + 1, row * 100 + 30 + 1));
				((Control)val3).set_BackgroundColor(isToday ? _themeTodayBg : (isCurrentMonth ? _themeCellBg : _themeCellOffMonth));
				((Control)val3).set_ClipsBounds(true);
				Panel cell = val3;
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)cell);
				val4.set_Text(cellDate.Day.ToString());
				((Control)val4).set_Location(new Point(5, 5));
				val4.set_TextColor(isToday ? Color.get_White() : (isCurrentMonth ? Color.get_LightGray() : (Color.get_Gray() * 0.5f)));
				val4.set_Font(isToday ? GameService.Content.get_DefaultFont16() : GameService.Content.get_DefaultFont14());
				val4.set_AutoSizeWidth(true);
				List<GuildEvent> daysEvents = _events.Where((GuildEvent e) => GetLocalEventDate(e) == cellDate.Date).ToList();
				int yPos = 28;
				foreach (GuildEvent evt in daysEvents)
				{
					if (yPos > 78)
					{
						Label val5 = new Label();
						((Control)val5).set_Parent((Container)(object)cell);
						val5.set_Text($" +{daysEvents.Count - ((Container)cell).get_Children().get_Count() + 1} more");
						((Control)val5).set_Location(new Point(5, yPos));
						val5.set_TextColor(_themeAgedGold);
						val5.set_Font(GameService.Content.get_DefaultFont12());
						val5.set_AutoSizeWidth(true);
						break;
					}
					Panel val6 = new Panel();
					((Control)val6).set_Parent((Container)(object)cell);
					((Control)val6).set_Size(new Point(cellWidth - 10, 24));
					((Control)val6).set_Location(new Point(5, yPos));
					((Control)val6).set_BackgroundColor(_themeEventPill);
					((Control)val6).set_ClipsBounds(true);
					((Control)val6).set_BasicTooltipText(evt.Title + "\n\nClick for details.");
					Panel pill = val6;
					Panel val7 = new Panel();
					((Control)val7).set_Parent((Container)(object)pill);
					((Control)val7).set_Size(new Point(3, ((Control)pill).get_Height()));
					((Control)val7).set_Location(new Point(0, 0));
					((Control)val7).set_BackgroundColor(Color.get_LightSkyBlue());
					Label val8 = new Label();
					((Control)val8).set_Parent((Container)(object)pill);
					val8.set_Text(evt.Title);
					((Control)val8).set_Location(new Point(8, 3));
					val8.set_AutoSizeWidth(true);
					val8.set_AutoSizeHeight(true);
					val8.set_TextColor(Color.get_White());
					val8.set_Font(GameService.Content.get_DefaultFont12());
					val8.set_WrapText(false);
					((Control)pill).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ShowDetails(evt);
					});
					yPos += 28;
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
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Expected O, but got Unknown
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Expected O, but got Unknown
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Expected O, but got Unknown
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			int contentWidth = 720;
			DateTime localTime = TimeZoneInfo.ConvertTime(evt.Date, _etZone, TimeZoneInfo.Local);
			string descText = (string.IsNullOrEmpty(evt.Description) ? "No notes provided for this event." : evt.Description.Replace("\\,", ",").Replace("\\;", ";").Replace("\\\\", "\\")
				.Replace("\\n", "\n")
				.Replace("\\N", "\n")
				.Replace("\r", "")
				.Trim());
			StandardWindow val = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Guild Event Details");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156022)));
			((Control)val).set_Location(new Point(300, 200));
			((WindowBase2)val).set_SavesPosition(true);
			((Control)val).set_Size(new Point(850, 650));
			StandardWindow window = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)window);
			((Control)val2).set_Size(new Point(((Container)window).get_ContentRegion().Width, ((Container)window).get_ContentRegion().Height));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_BackgroundColor(_themeDarkBg);
			Panel content = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)content);
			val3.set_Text(evt.Title);
			val3.set_Font(GameService.Content.get_DefaultFont32());
			val3.set_TextColor(_themeAgedGold);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Width(contentWidth);
			((Control)val3).set_Location(new Point(30, 30));
			val3.set_WrapText(true);
			Label titleLbl = val3;
			string timeStr = $"{localTime:dddd, MMMM dd}  •  {localTime:h:mm tt} (Local)   |   {evt.Date:h:mm tt} (ET)";
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)content);
			val4.set_Text(timeStr);
			val4.set_Font(GameService.Content.get_DefaultFont18());
			val4.set_TextColor(Color.get_LightSkyBlue());
			((Control)val4).set_Location(new Point(30, ((Control)titleLbl).get_Bottom() + 10));
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Width(contentWidth);
			Label timeLbl = val4;
			Image val5 = new Image();
			val5.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			((Control)val5).set_Parent((Container)(object)content);
			((Control)val5).set_Location(new Point(30, ((Control)timeLbl).get_Bottom() + 20));
			((Control)val5).set_Size(new Point(contentWidth, 2));
			val5.set_Tint(_themeAgedGold * 0.5f);
			int remainingHeight = ((Control)content).get_Height() - ((Control)timeLbl).get_Bottom() - 60;
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)content);
			((Control)val6).set_Location(new Point(30, ((Control)timeLbl).get_Bottom() + 40));
			((Control)val6).set_Size(new Point(contentWidth, remainingHeight));
			((Control)val6).set_BackgroundColor(Color.get_Black() * 0.4f);
			val6.set_ShowBorder(true);
			Panel descPanelBg = val6;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)descPanelBg);
			((Control)val7).set_Size(new Point(contentWidth - 10, remainingHeight - 10));
			((Control)val7).set_Location(new Point(5, 5));
			((Panel)val7).set_CanScroll(true);
			val7.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel descScroll = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)descScroll);
			val8.set_Text(descText);
			((Control)val8).set_Width(contentWidth - 30);
			val8.set_AutoSizeHeight(true);
			val8.set_WrapText(true);
			val8.set_TextColor(Color.get_WhiteSmoke());
			val8.set_Font(GameService.Content.get_DefaultFont16());
			((Control)window).Show();
		}
	}
}
