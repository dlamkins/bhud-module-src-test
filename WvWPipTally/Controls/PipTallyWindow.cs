using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using WvWPipTally.Models;
using WvWPipTally.Services;

namespace WvWPipTally.Controls
{
	public sealed class PipTallyWindow : StandardWindow
	{
		private static readonly Rectangle BgWindowRegion = new Rectangle(40, 26, 913, 691);

		private static readonly Rectangle BgContentRegion = new Rectangle(70, 36, 839, 605);

		private static readonly Color Gold = new Color(235, 220, 170);

		private static readonly Color Soft = new Color(230, 230, 230);

		private static readonly Color Muted = new Color(155, 160, 170);

		private static readonly Color Accent = new Color(200, 160, 70) * 0.85f;

		private const int Pad = 12;

		private const int LabelW = 96;

		private const int ValueX = 114;

		private const int InnerW = 340;

		private readonly Label _chestLabel;

		private readonly Label _pipsValue;

		private readonly Label _remainingValue;

		private readonly Label _ticketsValue;

		private readonly Label _timeValue;

		private readonly Label _statusLabel;

		private readonly StandardButton _scanButton;

		private readonly StandardButton _prevButton;

		private readonly StandardButton _nextButton;

		private readonly StandardButton _helpButton;

		public event EventHandler ScanClicked;

		public event EventHandler PrevChestClicked;

		public event EventHandler NextChestClicked;

		public event EventHandler HelpClicked;

		public PipTallyWindow(AsyncTexture2D background, AsyncTexture2D emblem)
			: this(background, BgWindowRegion, BgContentRegion, new Point(420, 415))
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Expected O, but got Unknown
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Expected O, but got Unknown
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Expected O, but got Unknown
			((WindowBase2)this).set_Title("WvW Pip Tally");
			((WindowBase2)this).set_Subtitle("Rewards");
			if (emblem != null)
			{
				((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(emblem));
			}
			((WindowBase2)this).set_Id("WvWPipTally_MainWindow");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_CanClose(true);
			((WindowBase2)this).set_CanCloseWithEscape(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			MakeMuted("CURRENT CHEST", 12, 4);
			Label val = new Label();
			val.set_Text(string.Empty);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Gold);
			val.set_ShowShadow(true);
			val.set_StrokeText(false);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(12, 20));
			((Control)val).set_Parent((Container)(object)this);
			_chestLabel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("<  Prev chest");
			((Control)val2).set_Width(148);
			((Control)val2).set_Height(26);
			((Control)val2).set_Location(new Point(12, 46));
			((Control)val2).set_BasicTooltipText("Move to the previous reward on the track");
			((Control)val2).set_Parent((Container)(object)this);
			_prevButton = val2;
			((Control)_prevButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.PrevChestClicked?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val3 = new StandardButton();
			val3.set_Text("Next chest  >");
			((Control)val3).set_Width(148);
			((Control)val3).set_Height(26);
			((Control)val3).set_Location(new Point(168, 46));
			((Control)val3).set_BasicTooltipText("Move to the next reward on the track");
			((Control)val3).set_Parent((Container)(object)this);
			_nextButton = val3;
			((Control)_nextButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.NextChestClicked?.Invoke(this, EventArgs.Empty);
			});
			Panel val4 = new Panel();
			((Control)val4).set_Location(new Point(12, 80));
			((Control)val4).set_Size(new Point(340, 2));
			((Control)val4).set_BackgroundColor(Accent);
			val4.set_ShowBorder(false);
			((Control)val4).set_Parent((Container)(object)this);
			MakeMuted("PROGRESS", 12, 90);
			int row1 = 112;
			int row2 = 134;
			int row3 = 156;
			int row4 = 178;
			MakeStatLabel("Pips / tick", row1);
			MakeStatLabel("Pips left", row2);
			MakeStatLabel("Tickets left", row3);
			MakeStatLabel("Time left", row4);
			_pipsValue = MakeStatValue(row1);
			_remainingValue = MakeStatValue(row2);
			_ticketsValue = MakeStatValue(row3);
			_timeValue = MakeStatValue(row4);
			Label val5 = new Label();
			val5.set_Text("Open Match Overview (B), then Scan.");
			val5.set_Font(GameService.Content.get_DefaultFont14());
			val5.set_TextColor(Muted);
			val5.set_ShowShadow(false);
			val5.set_WrapText(true);
			((Control)val5).set_Width(340);
			((Control)val5).set_Height(22);
			((Control)val5).set_Location(new Point(12, 206));
			((Control)val5).set_Parent((Container)(object)this);
			_statusLabel = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Scan Match Overview");
			((Control)val6).set_Width(230);
			((Control)val6).set_Height(28);
			((Control)val6).set_Location(new Point(12, 232));
			((Control)val6).set_Parent((Container)(object)this);
			_scanButton = val6;
			((Control)_scanButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ScanClicked?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Help");
			((Control)val7).set_Width(70);
			((Control)val7).set_Height(28);
			((Control)val7).set_Location(new Point(252, 232));
			((Control)val7).set_BasicTooltipText("Show which Match Overview screen to open");
			((Control)val7).set_Parent((Container)(object)this);
			_helpButton = val7;
			((Control)_helpButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.HelpClicked?.Invoke(this, EventArgs.Empty);
			});
			Apply(null, "Open Match Overview (B), then Scan.");
		}

		public void SetScanBusy(bool busy)
		{
			((Control)_scanButton).set_Enabled(!busy);
			_scanButton.set_Text(busy ? "Scanning..." : "Scan Match Overview");
			((Control)_prevButton).set_Enabled(!busy);
			((Control)_nextButton).set_Enabled(!busy);
			((Control)_helpButton).set_Enabled(!busy);
		}

		public void Apply(PipResults results, string status, int segmentIndex = 0)
		{
			_chestLabel.set_Text(PipCalculator.FormatSegmentLabel(segmentIndex));
			if (results == null)
			{
				_pipsValue.set_Text("-");
				_remainingValue.set_Text("-");
				_ticketsValue.set_Text("-");
				_timeValue.set_Text("-");
			}
			else
			{
				_pipsValue.set_Text(results.PipsPerTick.ToString());
				_remainingValue.set_Text(results.RemainingPips + "  (" + results.PipPercent + "% done)");
				_ticketsValue.set_Text(results.RemainingTickets + "  (" + results.TicketPercent + "% done)");
				_timeValue.set_Text((results.TicksRemaining == 0) ? "done (Diamond)" : (results.HoursRemaining + "h " + results.MinutesRemaining + "m  (" + results.TicksRemaining + " ticks)"));
			}
			string clean = status ?? string.Empty;
			if (clean.Length > 60)
			{
				clean = clean.Substring(0, 57) + "...";
			}
			_statusLabel.set_Text(clean);
			((Control)_statusLabel).set_BasicTooltipText(status ?? string.Empty);
		}

		private void MakeMuted(string text, int x, int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(Muted);
			val.set_ShowShadow(true);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Parent((Container)(object)this);
		}

		private void MakeStatLabel(string text, int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Soft);
			val.set_ShowShadow(false);
			((Control)val).set_Width(96);
			((Control)val).set_Height(20);
			((Control)val).set_Location(new Point(12, y));
			((Control)val).set_Parent((Container)(object)this);
		}

		private Label MakeStatValue(int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(string.Empty);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Gold);
			val.set_ShowShadow(false);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(114, y));
			((Control)val).set_Parent((Container)(object)this);
			return val;
		}
	}
}
