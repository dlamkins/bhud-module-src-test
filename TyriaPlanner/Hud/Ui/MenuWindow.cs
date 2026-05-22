using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using TyriaPlanner.Hud.Api;
using TyriaPlanner.Hud.Services;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Ui
{
	public sealed class MenuWindow : Container
	{
		private readonly ApiClient _api;

		private readonly ModuleSettings _settings;

		private readonly NotificationService _notify;

		private readonly NotificationHistory _history;

		private readonly Panel _titleBar;

		private readonly FlowPanel _content;

		private readonly Label _statusLabel;

		private readonly StandardButton _refreshButton;

		private readonly Checkbox _notifySignupsCheckbox;

		private readonly Checkbox _notifyNewEventsCheckbox;

		private readonly Timer _autoRefresh;

		private CancellationTokenSource _inFlight;

		private bool _dragging;

		private Point _dragOffset;

		private const int WindowWidth = 480;

		private const int WindowHeight = 560;

		private const int TitleBarHeight = 32;

		private const int TogglesHeight = 28;

		public MenuWindow(ApiClient api, ModuleSettings settings, NotificationService notify, NotificationHistory history)
			: this()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Expected O, but got Unknown
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Expected O, but got Unknown
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Expected O, but got Unknown
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Expected O, but got Unknown
			_api = api;
			_settings = settings;
			_notify = notify;
			_history = history;
			((Control)this).set_Width(480);
			((Control)this).set_Height(560);
			((Control)this).set_BackgroundColor(new Color(14, 14, 18, 240));
			((Control)this).set_Visible(false);
			((Control)this).set_ZIndex(200);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(480);
			((Control)val).set_Height(32);
			((Control)val).set_Location(Point.get_Zero());
			((Control)val).set_BackgroundColor(new Color(28, 26, 20, 255));
			_titleBar = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_titleBar);
			val2.set_Text("Tyria Planner");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_Goldenrod());
			((Control)val2).set_Location(new Point(12, 6));
			((Control)val2).set_Width(220);
			((Control)val2).set_Height(22);
			val2.set_AutoSizeWidth(false);
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_titleBar);
			val3.set_Text("X");
			((Control)val3).set_Width(28);
			((Control)val3).set_Height(22);
			((Control)val3).set_Location(new Point(444, 5));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				HideMenu();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_titleBar);
			val4.set_Text("Refresh");
			((Control)val4).set_Width(72);
			((Control)val4).set_Height(22);
			((Control)val4).set_Location(new Point(366, 5));
			_refreshButton = val4;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs __)
			{
				_ = RefreshAsync();
			});
			((Control)_titleBar).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnTitlePressed);
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Width(480);
			((Control)val5).set_Height(28);
			((Control)val5).set_Location(new Point(0, 32));
			((Control)val5).set_BackgroundColor(new Color(22, 22, 26, 255));
			Panel togglesRow = val5;
			Checkbox val6 = new Checkbox();
			((Control)val6).set_Parent((Container)(object)togglesRow);
			val6.set_Text("Signup reminders");
			((Control)val6).set_Location(new Point(12, 6));
			val6.set_Checked(_settings.NotifyOwnSignups.get_Value());
			_notifySignupsCheckbox = val6;
			_notifySignupsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent args)
			{
				_settings.NotifyOwnSignups.set_Value(args.get_Checked());
			});
			Checkbox val7 = new Checkbox();
			((Control)val7).set_Parent((Container)(object)togglesRow);
			val7.set_Text("New guild events");
			((Control)val7).set_Location(new Point(170, 6));
			val7.set_Checked(_settings.NotifyNewGuildEvents.get_Value());
			_notifyNewEventsCheckbox = val7;
			_notifyNewEventsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent args)
			{
				_settings.NotifyNewGuildEvents.set_Value(args.get_Checked());
			});
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Loading...");
			val8.set_Font(GameService.Content.get_DefaultFont12());
			val8.set_TextColor(new Color(190, 190, 190));
			((Control)val8).set_Location(new Point(12, 66));
			((Control)val8).set_Width(456);
			((Control)val8).set_Height(18);
			val8.set_AutoSizeWidth(false);
			_statusLabel = val8;
			int contentTop = 88;
			FlowPanel val9 = new FlowPanel();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Location(new Point(8, contentTop));
			((Control)val9).set_Width(464);
			((Control)val9).set_Height(560 - contentTop - 8);
			val9.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val9).set_CanScroll(true);
			val9.set_ControlPadding(new Vector2(0f, 6f));
			val9.set_OuterControlPadding(new Vector2(0f, 0f));
			_content = val9;
			_autoRefresh = new Timer(delegate(object _)
			{
				if (((Control)this).get_Visible())
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						_ = RefreshAsync();
					});
				}
			}, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
		}

		private void OnTitlePressed(object sender, MouseEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (!_dragging)
			{
				_dragging = true;
				Point mouse = GameService.Input.get_Mouse().get_Position();
				_dragOffset = new Point(mouse.X - ((Control)this).get_Location().X, mouse.Y - ((Control)this).get_Location().Y);
				GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
				GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalReleased);
			}
		}

		private void OnGlobalMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point mouse = GameService.Input.get_Mouse().get_Position();
				Screen screen = GameService.Graphics.get_SpriteScreen();
				int newX = Math.Max(-400, Math.Min(((Control)screen).get_Width() - 80, mouse.X - _dragOffset.X));
				int newY = Math.Max(0, Math.Min(((Control)screen).get_Height() - 40, mouse.Y - _dragOffset.Y));
				((Control)this).set_Location(new Point(newX, newY));
			}
		}

		private void OnGlobalReleased(object sender, MouseEventArgs e)
		{
			_dragging = false;
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalReleased);
		}

		public void ShowMenu()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			((Control)this).set_Parent((Container)(object)screen);
			((Control)this).set_Location(new Point((((Control)screen).get_Width() - ((Control)this).get_Width()) / 2, Math.Max(40, ((Control)screen).get_Height() / 6)));
			((Control)this).set_Visible(true);
			((Control)this).set_ZIndex(1000);
			_notifySignupsCheckbox.set_Checked(_settings.NotifyOwnSignups.get_Value());
			_notifyNewEventsCheckbox.set_Checked(_settings.NotifyNewGuildEvents.get_Value());
			_autoRefresh.Change(TimeSpan.FromSeconds(60.0), TimeSpan.FromSeconds(60.0));
			RefreshAsync();
		}

		public void HideMenu()
		{
			((Control)this).set_Visible(false);
			_autoRefresh.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
		}

		public void Toggle()
		{
			if (((Control)this).get_Visible())
			{
				HideMenu();
			}
			else
			{
				ShowMenu();
			}
		}

		private async Task RefreshAsync()
		{
			_inFlight?.Cancel();
			_inFlight = new CancellationTokenSource();
			CancellationToken cancel = _inFlight.Token;
			_statusLabel.set_Text("Loading...");
			((Control)_refreshButton).set_Enabled(false);
			try
			{
				string baseUrl = _settings.ApiBaseUrl.get_Value();
				string bearer = _settings.CachedBearer.get_Value();
				if (!string.IsNullOrWhiteSpace(bearer))
				{
					goto IL_0238;
				}
				if (string.IsNullOrWhiteSpace(_settings.Gw2ApiKey.get_Value()))
				{
					_statusLabel.set_Text("Paste your GW2 API key in module settings first.");
					return;
				}
				bearer = await _api.ExchangeAsync(baseUrl, _settings.Gw2ApiKey.get_Value(), cancel).ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrWhiteSpace(bearer))
				{
					_statusLabel.set_Text("GW2 key not recognised by Tyria Planner.");
					return;
				}
				_settings.CachedBearer.set_Value(bearer);
				goto IL_0238;
				IL_0238:
				UpcomingResponse resp = await _api.FetchBrowseAsync(baseUrl, bearer, cancel).ConfigureAwait(continueOnCapturedContext: false);
				if (resp == null)
				{
					_settings.CachedBearer.set_Value(string.Empty);
					bearer = await _api.ExchangeAsync(baseUrl, _settings.Gw2ApiKey.get_Value(), cancel).ConfigureAwait(continueOnCapturedContext: false);
					if (!string.IsNullOrWhiteSpace(bearer))
					{
						_settings.CachedBearer.set_Value(bearer);
						resp = await _api.FetchBrowseAsync(baseUrl, bearer, cancel).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				if (resp == null)
				{
					_statusLabel.set_Text("Failed to load. Check your API key or network.");
					return;
				}
				PendingApprovalsResponse approvals = null;
				try
				{
					approvals = await _api.FetchApprovalsAsync(baseUrl, bearer, cancel).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch
				{
				}
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					Populate(resp, approvals);
				});
			}
			catch (TaskCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.GetLogger<MenuWindow>().Warn(ex, "Browse refresh failed.");
				_statusLabel.set_Text("Refresh failed · try again.");
			}
			finally
			{
				((Control)_refreshButton).set_Enabled(true);
			}
		}

		private void Populate(UpcomingResponse resp, PendingApprovalsResponse approvals = null)
		{
			((Container)_content).ClearChildren();
			if (approvals?.Pending != null && approvals.Pending.Length != 0)
			{
				AddSectionHeader($"Pending approvals · {approvals.Pending.Length}");
				PendingApproval[] pending = approvals.Pending;
				foreach (PendingApproval p in pending)
				{
					AddApprovalRow(p);
				}
			}
			MySignup[] mySignups = resp.MySignups;
			AddSectionHeader($"My signups (next 7 days)  ·  {((mySignups != null) ? mySignups.Length : 0)}");
			if (resp.MySignups == null || resp.MySignups.Length == 0)
			{
				AddEmptyRow("Nothing scheduled. Sign up to events on the mobile app to see them here.");
			}
			else
			{
				MySignup[] mySignups2 = resp.MySignups;
				foreach (MySignup ev2 in mySignups2)
				{
					AddEventRow(ev2, showSqjoin: true);
				}
			}
			NewGuildEvent[] newGuildEvents = resp.NewGuildEvents;
			AddSectionHeader($"New guild events (last 24h)  ·  {((newGuildEvents != null) ? newGuildEvents.Length : 0)}");
			if (resp.NewGuildEvents == null || resp.NewGuildEvents.Length == 0)
			{
				AddEmptyRow("No new guild events you haven't signed up to yet.");
			}
			else
			{
				NewGuildEvent[] newGuildEvents2 = resp.NewGuildEvents;
				foreach (NewGuildEvent ev in newGuildEvents2)
				{
					AddEventRow(ev, showSqjoin: false);
				}
			}
			Announcement[] newAnnouncements = resp.NewAnnouncements;
			AddSectionHeader($"Guild announcements (last 24h)  ·  {((newAnnouncements != null) ? newAnnouncements.Length : 0)}");
			if (resp.NewAnnouncements == null || resp.NewAnnouncements.Length == 0)
			{
				AddEmptyRow("No recent guild announcements.");
			}
			else
			{
				Announcement[] newAnnouncements2 = resp.NewAnnouncements;
				foreach (Announcement ann in newAnnouncements2)
				{
					AddAnnouncementRow(ann);
				}
			}
			List<HistoryEntry> history = _history?.Snapshot();
			if (history != null && history.Count > 0)
			{
				AddSectionHeader($"Recent notifications  ·  last {Math.Min(history.Count, 10)}");
				foreach (HistoryEntry entry in history.GetRange(0, Math.Min(history.Count, 10)))
				{
					AddHistoryRow(entry);
				}
			}
			_statusLabel.set_Text($"Updated · server time {resp.ServerTime:HH:mm:ss}");
		}

		private void AddAnnouncementRow(Announcement ann)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont titleFont = _settings.TitleFont();
			BitmapFont bodyFont = _settings.BodyFont();
			int titleH = titleFont.get_LineHeight();
			int bodyH = bodyFont.get_LineHeight();
			int bodyBlock = bodyH * 3 + 4;
			int padTop = 8;
			int padMid = 4;
			int padBot = 8;
			int rowHeight = padTop + titleH + padMid + bodyH + padMid + bodyBlock + padBot;
			Color accent = Color.get_Goldenrod();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_content);
			((Control)val).set_Width(((Control)_content).get_Width() - 16);
			((Control)val).set_Height(rowHeight);
			((Control)val).set_BackgroundColor(new Color(22, 22, 26, 220));
			Panel row = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_BackgroundColor(accent);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(4);
			((Control)val2).set_Height(rowHeight);
			string head = (string.IsNullOrWhiteSpace(ann.GuildTag) ? ("\ud83d\udce2 " + ann.GuildName) : ("\ud83d\udce2 [" + ann.GuildTag + "] " + ann.GuildName));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(head);
			val3.set_Font(titleFont);
			val3.set_TextColor(accent);
			((Control)val3).set_Location(new Point(12, padTop));
			((Control)val3).set_Width(((Control)row).get_Width() - 22);
			((Control)val3).set_Height(titleH + 2);
			val3.set_AutoSizeWidth(false);
			string senderAndTime = (string.IsNullOrWhiteSpace(ann.SenderAccountName) ? FormatAgo(ann.CreatedAt) : (ann.SenderAccountName + " · " + FormatAgo(ann.CreatedAt)));
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(string.IsNullOrWhiteSpace(ann.Title) ? senderAndTime : (ann.Title + " · " + senderAndTime));
			val4.set_Font(bodyFont);
			val4.set_TextColor(new Color(220, 200, 140));
			((Control)val4).set_Location(new Point(12, padTop + titleH + padMid));
			((Control)val4).set_Width(((Control)row).get_Width() - 22);
			((Control)val4).set_Height(bodyH + 2);
			val4.set_AutoSizeWidth(false);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text(string.IsNullOrWhiteSpace(ann.Content) ? "(no content)" : ann.Content);
			val5.set_Font(bodyFont);
			val5.set_TextColor(new Color(210, 210, 210));
			((Control)val5).set_Location(new Point(12, padTop + titleH + padMid + bodyH + padMid));
			((Control)val5).set_Width(((Control)row).get_Width() - 22);
			((Control)val5).set_Height(bodyBlock);
			val5.set_AutoSizeWidth(false);
			val5.set_WrapText(true);
		}

		private void AddApprovalRow(PendingApproval p)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Expected O, but got Unknown
			Color typeColor = EventColors.For(p.EventType, _settings.ColorTheme.get_Value());
			BitmapFont titleFont = _settings.TitleFont();
			BitmapFont bodyFont = _settings.BodyFont();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_content);
			((Control)val).set_Width(((Control)_content).get_Width() - 16);
			((Control)val).set_Height(titleFont.get_LineHeight() + bodyFont.get_LineHeight() + 56);
			((Control)val).set_BackgroundColor(new Color(24, 24, 30, 220));
			Panel row = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_BackgroundColor(typeColor);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(4);
			((Control)val2).set_Height(((Control)row).get_Height());
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(p.EventTitle + " · " + PrettyType(p.EventType));
			val3.set_Font(titleFont);
			val3.set_TextColor(typeColor);
			((Control)val3).set_Location(new Point(12, 6));
			((Control)val3).set_Width(((Control)row).get_Width() - 22);
			((Control)val3).set_Height(titleFont.get_LineHeight() + 2);
			val3.set_AutoSizeWidth(false);
			string spec = ((!string.IsNullOrWhiteSpace(p.CharacterEliteSpec)) ? p.CharacterEliteSpec : p.CharacterProfession);
			string applicant = ((!string.IsNullOrWhiteSpace(p.ApplicantAccountName)) ? p.ApplicantAccountName : (p.ApplicantDisplayName ?? p.ApplicantUsername ?? "?"));
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(applicant + " · " + p.CharacterName + " (" + spec + ") · " + p.GuildName);
			val4.set_Font(bodyFont);
			val4.set_TextColor(new Color(210, 210, 210));
			((Control)val4).set_Location(new Point(12, 10 + titleFont.get_LineHeight()));
			((Control)val4).set_Width(((Control)row).get_Width() - 22);
			((Control)val4).set_Height(bodyFont.get_LineHeight() + 2);
			val4.set_AutoSizeWidth(false);
			int btnY = ((Control)row).get_Height() - 32;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text("Approve");
			((Control)val5).set_Width(90);
			((Control)val5).set_Height(26);
			((Control)val5).set_Location(new Point(12, btnY));
			StandardButton approve = val5;
			((Control)approve).add_Click((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs __)
			{
				_ = DecideAsync(p.SignupId, "approved", approve);
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)row);
			val6.set_Text("Reject");
			((Control)val6).set_Width(80);
			((Control)val6).set_Height(26);
			((Control)val6).set_Location(new Point(108, btnY));
			StandardButton reject = val6;
			((Control)reject).add_Click((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs __)
			{
				_ = DecideAsync(p.SignupId, "rejected", reject);
			});
		}

		private async Task DecideAsync(string signupId, string decision, StandardButton btn)
		{
			((Control)btn).set_Enabled(false);
			string baseUrl = _settings.ApiBaseUrl.get_Value();
			string bearer = _settings.CachedBearer.get_Value();
			(bool ok, int status) result = await _api.DecideApprovalAsync(baseUrl, bearer, signupId, decision, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			if (!result.ok && (result.status == 401 || result.status == 403) && !string.IsNullOrWhiteSpace(_settings.Gw2ApiKey.get_Value()))
			{
				_settings.CachedBearer.set_Value(string.Empty);
				string fresh = await _api.ExchangeAsync(baseUrl, _settings.Gw2ApiKey.get_Value(), CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrWhiteSpace(fresh))
				{
					_settings.CachedBearer.set_Value(fresh);
					result = await _api.DecideApprovalAsync(baseUrl, fresh, signupId, decision, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			var (ok, _) = result;
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				try
				{
					btn.set_Text(ok ? "✓" : "failed");
				}
				catch
				{
				}
			});
			if (ok)
			{
				await Task.Delay(700).ConfigureAwait(continueOnCapturedContext: false);
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					RefreshAsync();
				});
				return;
			}
			await Task.Delay(1500).ConfigureAwait(continueOnCapturedContext: false);
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				try
				{
					((Control)btn).set_Enabled(true);
					btn.set_Text((decision == "approved") ? "Approve" : "Reject");
				}
				catch
				{
				}
			});
		}

		private void AddHistoryRow(HistoryEntry entry)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			Color typeColor = EventColors.For(entry.EventType, _settings.ColorTheme.get_Value());
			BitmapFont bodyFont = _settings.BodyFont();
			BitmapFont titleFont = _settings.TitleFont();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_content);
			((Control)val).set_Width(((Control)_content).get_Width() - 16);
			((Control)val).set_Height(titleFont.get_LineHeight() + bodyFont.get_LineHeight() + 16);
			((Control)val).set_BackgroundColor(new Color(18, 18, 22, 200));
			Panel row = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_BackgroundColor(typeColor);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(3);
			((Control)val2).set_Height(((Control)row).get_Height());
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(entry.Title);
			val3.set_Font(titleFont);
			val3.set_TextColor(typeColor);
			((Control)val3).set_Location(new Point(10, 6));
			((Control)val3).set_Width(((Control)row).get_Width() - 50);
			((Control)val3).set_Height(titleFont.get_LineHeight() + 2);
			val3.set_AutoSizeWidth(false);
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text("×");
			((Control)val4).set_Width(32);
			((Control)val4).set_Height(24);
			((Control)val4).set_Location(new Point(((Control)row).get_Width() - 40, 4));
			Panel rowToDispose = row;
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_history.Remove(entry);
				try
				{
					((Control)rowToDispose).Dispose();
				}
				catch
				{
				}
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text(entry.Subtitle + " · " + FormatAgo(entry.At));
			val5.set_Font(bodyFont);
			val5.set_TextColor(new Color(180, 180, 180));
			((Control)val5).set_Location(new Point(10, 8 + titleFont.get_LineHeight()));
			((Control)val5).set_Width(((Control)row).get_Width() - 20);
			((Control)val5).set_Height(bodyFont.get_LineHeight() + 2);
			val5.set_AutoSizeWidth(false);
		}

		private static string FormatAgo(DateTime past)
		{
			double seconds = (DateTime.UtcNow - past).TotalSeconds;
			if (seconds < 60.0)
			{
				return $"{(int)seconds}s ago";
			}
			if (seconds < 3600.0)
			{
				return $"{(int)(seconds / 60.0)}m ago";
			}
			if (seconds < 86400.0)
			{
				return $"{(int)(seconds / 3600.0)}h ago";
			}
			return $"{(int)(seconds / 86400.0)}d ago";
		}

		private void AddSectionHeader(string text)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = _settings.TitleFont();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_content);
			val.set_Text(text);
			val.set_Font(font);
			val.set_TextColor(Color.get_Goldenrod());
			((Control)val).set_Width(((Control)_content).get_Width() - 8);
			((Control)val).set_Height(font.get_LineHeight() + 6);
			val.set_AutoSizeWidth(false);
		}

		private void AddEmptyRow(string text)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = _settings.BodyFont();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_content);
			val.set_Text(text);
			val.set_Font(font);
			val.set_TextColor(new Color(160, 160, 160));
			((Control)val).set_Width(((Control)_content).get_Width() - 16);
			((Control)val).set_Height(font.get_LineHeight() * 2 + 6);
			val.set_AutoSizeWidth(false);
			val.set_WrapText(true);
		}

		private void AddEventRow(EventBase ev, bool showSqjoin)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Expected O, but got Unknown
			Color typeColor = EventColors.For(ev.Type, _settings.ColorTheme.get_Value());
			BitmapFont titleFont = _settings.TitleFont();
			BitmapFont bodyFont = _settings.BodyFont();
			int titleH = titleFont.get_LineHeight();
			int bodyH = bodyFont.get_LineHeight();
			int padMid = 6;
			int btnH = 26;
			int padBot = 8;
			string subtitleLine1 = BuildSubtitleLine1(ev);
			string subtitleLine2 = BuildSubtitleLine2(ev);
			bool hasLine2 = !string.IsNullOrWhiteSpace(subtitleLine2);
			int line2WrapLines = (hasLine2 ? EstimateWrapLines(subtitleLine2, ((Control)_content).get_Width() - 38, 7) : 0);
			if (line2WrapLines < 1 && hasLine2)
			{
				line2WrapLines = 1;
			}
			if (line2WrapLines > 2)
			{
				line2WrapLines = 2;
			}
			int titleY = 8;
			int subtitleY = titleY + titleH + 2;
			int subtitle2Y = subtitleY + bodyH + 2;
			int subtitle2H = bodyH * line2WrapLines + ((line2WrapLines > 1) ? 2 : 0);
			int buttonY = (hasLine2 ? (subtitle2Y + subtitle2H) : (subtitleY + bodyH)) + padMid;
			int rowHeight = buttonY + btnH + padBot;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_content);
			((Control)val).set_Width(((Control)_content).get_Width() - 16);
			((Control)val).set_Height(rowHeight);
			((Control)val).set_BackgroundColor(new Color(22, 22, 26, 220));
			Panel row = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_BackgroundColor(typeColor);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(4);
			((Control)val2).set_Height(rowHeight);
			int countdownWidth = 100;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(BuildCountdown(ev));
			val3.set_Font(titleFont);
			val3.set_TextColor(typeColor);
			((Control)val3).set_Location(new Point(((Control)row).get_Width() - countdownWidth - 12, titleY));
			((Control)val3).set_Width(countdownWidth);
			((Control)val3).set_Height(titleH + 2);
			val3.set_AutoSizeWidth(false);
			val3.set_HorizontalAlignment((HorizontalAlignment)2);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text((ev.IsRecurring ? "[R] " : string.Empty) + (string.IsNullOrWhiteSpace(ev.Title) ? PrettyType(ev.Type) : ev.Title));
			val4.set_Font(titleFont);
			val4.set_TextColor(typeColor);
			((Control)val4).set_Location(new Point(12, titleY));
			((Control)val4).set_Width(((Control)row).get_Width() - countdownWidth - 30);
			((Control)val4).set_Height(titleH + 2);
			val4.set_AutoSizeWidth(false);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text(subtitleLine1);
			val5.set_Font(bodyFont);
			val5.set_TextColor(new Color(210, 210, 210));
			((Control)val5).set_Location(new Point(12, subtitleY));
			((Control)val5).set_Width(((Control)row).get_Width() - 22);
			((Control)val5).set_Height(bodyH + 2);
			val5.set_AutoSizeWidth(false);
			if (hasLine2)
			{
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)row);
				val6.set_Text(subtitleLine2);
				val6.set_Font(bodyFont);
				val6.set_TextColor(new Color(180, 180, 180));
				((Control)val6).set_Location(new Point(12, subtitle2Y));
				((Control)val6).set_Width(((Control)row).get_Width() - 22);
				((Control)val6).set_Height(subtitle2H);
				val6.set_AutoSizeWidth(false);
				val6.set_WrapText(true);
			}
			int x = 12;
			string commander = ev.CommanderAccountName;
			if (!string.IsNullOrWhiteSpace(commander))
			{
				if (showSqjoin)
				{
					AddCopyButton((Container)(object)row, "/sqjoin", "/sqjoin " + commander, ref x, buttonY, 80);
				}
				AddWhisperButton((Container)(object)row, commander, ref x, buttonY, 80);
			}
			if (!string.IsNullOrWhiteSpace(ev.VoiceChannelUrl))
			{
				AddVoiceButton((Container)(object)row, ev.VoiceChannelUrl, ref x, buttonY, 80);
			}
			MySignup signup = ev as MySignup;
			if (signup != null && signup.CheckinStatus == "pending" && signup.ScheduledAt > DateTime.UtcNow)
			{
				AddCheckinButton((Container)(object)row, signup.Id, ref x, buttonY, 80);
			}
			string openUrl = "https://tyriaplanner.com/event/" + ev.Id;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)row);
			val7.set_Text("Open");
			((Control)val7).set_Width(80);
			((Control)val7).set_Height(24);
			((Control)val7).set_Location(new Point(x, buttonY));
			StandardButton open = val7;
			((Control)open).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (SafeUrl.IsAllowed(openUrl))
				{
					Clipboard.Set(openUrl);
				}
				SafeUrl.Open(openUrl);
				FlashCopied(open, "Open");
			});
		}

		private void AddCheckinButton(Container parent, string eventId, ref int x, int y, int width)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(parent);
			val.set_Text("Check in");
			((Control)val).set_Width(width);
			((Control)val).set_Height(24);
			((Control)val).set_Location(new Point(x, y));
			StandardButton btn = val;
			Timer t;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				((Control)btn).set_Enabled(false);
				btn.set_Text("...");
				string baseUrl = _settings.ApiBaseUrl.get_Value();
				string bearer = _settings.CachedBearer.get_Value();
				bool ok = await _api.CheckinAsync(baseUrl, bearer, eventId, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					try
					{
						btn.set_Text(ok ? "✓ checked in" : "failed");
						if (!ok)
						{
							t = null;
							t = new Timer(delegate
							{
								GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
								{
									try
									{
										((Control)btn).set_Enabled(true);
										btn.set_Text("Check in");
									}
									catch
									{
									}
								});
								t?.Dispose();
							}, null, TimeSpan.FromSeconds(1.5), Timeout.InfiniteTimeSpan);
						}
					}
					catch
					{
					}
				});
			});
			x += ((Control)btn).get_Width() + 6;
		}

		private static void AddVoiceButton(Container parent, string url, ref int x, int y, int width)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(parent);
			val.set_Text("Voice");
			((Control)val).set_Width(width);
			((Control)val).set_Height(24);
			((Control)val).set_Location(new Point(x, y));
			StandardButton btn = val;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (SafeUrl.IsAllowed(url))
				{
					Clipboard.Set(url);
				}
				SafeUrl.Open(url);
				FlashCopied(btn, "Voice");
			});
			x += ((Control)btn).get_Width() + 6;
		}

		private static void AddWhisperButton(Container parent, string accountName, ref int x, int y, int width)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(parent);
			val.set_Text("Copy name");
			((Control)val).set_Width(width);
			((Control)val).set_Height(26);
			((Control)val).set_Location(new Point(x, y));
			StandardButton btn = val;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Clipboard.Set(accountName);
				}
				catch
				{
				}
				FlashCopied(btn, "Copy name");
			});
			x += width + 6;
		}

		private static void AddCopyButton(Container parent, string label, string payload, ref int x, int y, int width)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(parent);
			val.set_Text(label);
			((Control)val).set_Width(width);
			((Control)val).set_Height(24);
			((Control)val).set_Location(new Point(x, y));
			StandardButton btn = val;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Clipboard.Set(payload);
				FlashCopied(btn, label);
			});
			x += width + 6;
		}

		private static void FlashCopied(StandardButton btn, string original)
		{
			btn.set_Text("✓ copied");
			Timer t = null;
			t = new Timer(delegate
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					try
					{
						btn.set_Text(original);
					}
					catch
					{
					}
				});
				t?.Dispose();
			}, null, TimeSpan.FromMilliseconds(1400.0), Timeout.InfiniteTimeSpan);
		}

		private static string BuildSubtitleLine1(EventBase ev)
		{
			string guild = (string.IsNullOrWhiteSpace(ev.GuildName) ? "Public" : ev.GuildName);
			string commander = ((!string.IsNullOrWhiteSpace(ev.CommanderAccountName)) ? ev.CommanderAccountName : (ev.CommanderDisplayName ?? "?"));
			string head = guild + " · " + commander + " · " + PrettyType(ev.Type);
			if (ev.MaxSignups > 0)
			{
				head += $" · {ev.SignupCount}/{ev.MaxSignups} signed up";
			}
			return head;
		}

		private static string BuildSubtitleLine2(EventBase ev)
		{
			List<string> parts = new List<string>();
			MySignup ms = ev as MySignup;
			if (ms != null && ms.SignupCharacter != null)
			{
				string spec = ((!string.IsNullOrWhiteSpace(ms.SignupCharacter.EliteSpec)) ? ms.SignupCharacter.EliteSpec : ms.SignupCharacter.Profession);
				parts.Add("as " + ms.SignupCharacter.Name + " (" + spec + ")");
			}
			if (ev.KpRequirement != null && ev.KpRequirement.Amount > 0)
			{
				string modeShort = ((ev.KpRequirement.Mode == "average") ? "avg" : "min");
				parts.Add($"KP {modeShort} {ev.KpRequirement.Amount}");
			}
			if (ev.BossSlugs != null && ev.BossSlugs.Length != 0)
			{
				parts.Add(string.Join(", ", ev.BossSlugs));
			}
			return string.Join(" · ", parts);
		}

		private static string BuildCountdown(EventBase ev)
		{
			double totalMinutes = (ev.ScheduledAt - DateTime.UtcNow).TotalMinutes;
			if (totalMinutes < 0.0)
			{
				return "started";
			}
			if (totalMinutes < 1.0)
			{
				return "now";
			}
			if (totalMinutes < 60.0)
			{
				return $"in {(int)Math.Round(totalMinutes)}m";
			}
			if (totalMinutes < 1440.0)
			{
				int h = (int)(totalMinutes / 60.0);
				int i = (int)Math.Round(totalMinutes - (double)(h * 60));
				if (i >= 60)
				{
					h++;
					i = 0;
				}
				if (i <= 0)
				{
					return $"in {h}h";
				}
				return $"in {h}h {i}m";
			}
			int d = (int)(totalMinutes / 1440.0);
			int rh = (int)Math.Round((totalMinutes - (double)(d * 24 * 60)) / 60.0);
			if (rh >= 24)
			{
				d++;
				rh = 0;
			}
			if (rh <= 0)
			{
				return $"in {d}d";
			}
			return $"in {d}d {rh}h";
		}

		private static int EstimateWrapLines(string text, int widthPx, int pxPerChar)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}
			int charsPerLine = Math.Max(1, widthPx / pxPerChar);
			int lines = (int)Math.Ceiling((double)text.Length / (double)charsPerLine);
			return Math.Max(1, lines);
		}

		private static string PrettyType(string type)
		{
			return type switch
			{
				"raid" => "Raid", 
				"strike" => "Strike", 
				"fractal" => "Fractal", 
				"wvw" => "WvW", 
				"open_world" => "Open World", 
				_ => type ?? "Event", 
			};
		}

		protected override void DisposeControl()
		{
			_autoRefresh?.Dispose();
			_inFlight?.Cancel();
			_inFlight?.Dispose();
			((Container)this).DisposeControl();
		}
	}
}
