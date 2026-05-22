using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Ui
{
	public sealed class EventToast : Container
	{
		private readonly string _commanderAccountName;

		private readonly string _voiceChannelUrl;

		private readonly string _eventBaseUrl;

		private readonly bool _showSqjoin;

		private readonly bool _showJoinFromAppHint;

		private readonly Action<int> _onSnooze;

		public EventToast(ModuleSettings settings, string title, string subtitle, ToastAccent accent, string eventType, string commanderAccountName, string eventId, string eventBaseUrl, string voiceChannelUrl = null, bool showSqjoin = true, bool showJoinFromAppHint = false, bool isRecurring = false, Action<int> onSnooze = null)
			: this()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			_commanderAccountName = commanderAccountName;
			_voiceChannelUrl = voiceChannelUrl;
			_eventBaseUrl = eventBaseUrl;
			_showSqjoin = showSqjoin;
			_showJoinFromAppHint = showJoinFromAppHint;
			_onSnooze = onSnooze;
			BitmapFont titleFont = settings.TitleFont();
			BitmapFont bodyFont = settings.BodyFont();
			bool snoozeRow = _onSnooze != null;
			int baseHeight = (showJoinFromAppHint ? 130 : 110);
			if (snoozeRow)
			{
				baseHeight += 32;
			}
			((Control)this).set_Height(baseHeight);
			((Control)this).set_BackgroundColor(new Color(14, 14, 18, 235));
			Color typeColor = EventColors.For(eventType, settings.ColorTheme.get_Value());
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(typeColor);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(4);
			((Control)val).set_Height(baseHeight);
			string titleText = (isRecurring ? "[R] " : string.Empty) + title;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(titleText);
			val2.set_Font(titleFont);
			val2.set_TextColor(typeColor);
			((Control)val2).set_Location(new Point(12, 8));
			((Control)val2).set_Width(320);
			((Control)val2).set_Height(titleFont.get_LineHeight() + 4);
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
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(subtitle);
			val4.set_Font(bodyFont);
			val4.set_TextColor(new Color(220, 220, 220));
			((Control)val4).set_Location(new Point(12, 12 + titleFont.get_LineHeight()));
			((Control)val4).set_Width(360);
			((Control)val4).set_Height(bodyFont.get_LineHeight() + 4);
			val4.set_WrapText(false);
			val4.set_AutoSizeWidth(false);
			if (_showJoinFromAppHint)
			{
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)this);
				val5.set_Text("Sign up via the mobile app first");
				val5.set_Font(bodyFont);
				val5.set_TextColor(new Color(255, 200, 90));
				((Control)val5).set_Location(new Point(12, 14 + titleFont.get_LineHeight() + bodyFont.get_LineHeight()));
				((Control)val5).set_Width(360);
				((Control)val5).set_Height(bodyFont.get_LineHeight() + 4);
				val5.set_AutoSizeWidth(false);
			}
			int actionsY = baseHeight - (snoozeRow ? 60 : 32);
			int x = 12;
			if (_showSqjoin && !string.IsNullOrWhiteSpace(_commanderAccountName))
			{
				AddCopyButton("/sqjoin", "/sqjoin " + _commanderAccountName, ref x, actionsY, 84);
			}
			if (!string.IsNullOrWhiteSpace(_commanderAccountName))
			{
				AddWhisperButton(_commanderAccountName, ref x, actionsY, 84);
			}
			if (!string.IsNullOrWhiteSpace(_voiceChannelUrl))
			{
				AddVoiceButton(_voiceChannelUrl, ref x, actionsY, 70);
			}
			AddOpenButton(ref x, actionsY);
			if (snoozeRow)
			{
				int sy = actionsY + 32;
				int sx = 12;
				AddSnoozeButton("Snooze 5 min", 5, ref sx, sy);
				AddSnoozeButton("Snooze 15 min", 15, ref sx, sy);
			}
		}

		private void AddCopyButton(string label, string payload, ref int x, int y, int width)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(label);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(width);
			((Control)val).set_Height(26);
			StandardButton btn = val;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Clipboard.Set(payload);
				FlashCopied(btn, label);
			});
			x += width + 6;
		}

		private void AddWhisperButton(string accountName, ref int x, int y, int width)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Copy name");
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(width);
			((Control)val).set_Height(26);
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

		private void AddVoiceButton(string url, ref int x, int y, int width)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Voice");
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(width);
			((Control)val).set_Height(26);
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
			x += width + 6;
		}

		private void AddOpenButton(ref int x, int y)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Open");
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(70);
			((Control)val).set_Height(26);
			StandardButton open = val;
			((Control)open).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (SafeUrl.IsAllowed(_eventBaseUrl))
				{
					Clipboard.Set(_eventBaseUrl);
				}
				SafeUrl.Open(_eventBaseUrl);
				FlashCopied(open, "Open");
			});
			x += ((Control)open).get_Width() + 6;
		}

		private void AddSnoozeButton(string label, int minutes, ref int x, int y)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(label);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(110);
			((Control)val).set_Height(26);
			StandardButton btn = val;
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_onSnooze?.Invoke(minutes);
				((Control)this).Dispose();
			});
			x += ((Control)btn).get_Width() + 6;
		}

		private static void FlashCopied(StandardButton btn, string originalText)
		{
			btn.set_Text("✓ copied");
			Timer t = null;
			t = new Timer(delegate
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					try
					{
						btn.set_Text(originalText);
					}
					catch
					{
					}
				});
				t?.Dispose();
			}, null, TimeSpan.FromMilliseconds(1400.0), Timeout.InfiniteTimeSpan);
		}
	}
}
