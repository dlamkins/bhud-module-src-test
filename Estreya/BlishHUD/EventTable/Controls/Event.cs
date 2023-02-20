using System;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class Event : RenderTargetControl
	{
		private readonly IconState _iconState;

		private readonly TranslationState _translationState;

		private readonly Func<DateTime> _getNowAction;

		private readonly DateTime _startTime;

		private readonly DateTime _endTime;

		private readonly Func<BitmapFont> _getFontAction;

		private readonly Func<bool> _getDrawBorders;

		private readonly Func<bool> _getDrawCrossout;

		private readonly Func<Color> _getTextColor;

		private readonly Func<Color> _getColorAction;

		private readonly Func<bool> _getDrawShadowAction;

		private readonly Func<Color> _getShadowColor;

		private Tooltip _tooltip;

		public Estreya.BlishHUD.EventTable.Models.Event Ev { get; private set; }

		public event EventHandler HideRequested;

		public event EventHandler DisableRequested;

		public event EventHandler FinishRequested;

		public Event(Estreya.BlishHUD.EventTable.Models.Event ev, IconState iconState, TranslationState translationState, Func<DateTime> getNowAction, DateTime startTime, DateTime endTime, Func<BitmapFont> getFontAction, Func<bool> getDrawBorders, Func<bool> getDrawCrossout, Func<Color> getTextColor, Func<Color> getColorAction, Func<bool> getDrawShadowAction, Func<Color> getShadowColor)
		{
			Ev = ev;
			_iconState = iconState;
			_translationState = translationState;
			_getNowAction = getNowAction;
			_startTime = startTime;
			_endTime = endTime;
			_getFontAction = getFontAction;
			_getDrawBorders = getDrawBorders;
			_getDrawCrossout = getDrawCrossout;
			_getTextColor = getTextColor;
			_getColorAction = getColorAction;
			_getDrawShadowAction = getDrawShadowAction;
			_getShadowColor = getShadowColor;
			BuildContextMenu();
		}

		private void BuildContextMenu()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Menu(new ContextMenuStrip());
			ContextMenuStripItem val = new ContextMenuStripItem("Disable");
			((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val).set_BasicTooltipText("Disables the event entirely.");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DisableRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem("Hide");
			((Control)val2).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val2).set_BasicTooltipText("Hides the event until the next reset.");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.HideRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem("Finish");
			((Control)val3).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val3).set_BasicTooltipText("Completes the event until the next reset.");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.FinishRequested?.Invoke(this, EventArgs.Empty);
			});
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			if (!Ev.Filler)
			{
				BuildOrUpdateTooltip();
				((Control)this).set_Tooltip(_tooltip);
			}
		}

		private void BuildOrUpdateTooltip()
		{
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			DateTime now = _getNowAction();
			bool num = _startTime.AddMinutes(Ev.Duration) < now;
			bool isNext = !num && _startTime > now;
			bool isCurrent = !num && !isNext;
			string description = Ev.Location + ((!string.IsNullOrWhiteSpace(Ev.Location)) ? "\n" : string.Empty) + "\n";
			if (num)
			{
				TimeSpan finishedSince = now - _startTime.AddMinutes(Ev.Duration);
				description = description + _translationState.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatTime(finishedSince);
			}
			else if (isNext)
			{
				TimeSpan startsIn = _startTime - now;
				description = description + _translationState.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatTime(startsIn);
			}
			else if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now);
				description = description + _translationState.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatTime(remaining);
			}
			description = description + " (" + _translationState.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatTime(_startTime.ToLocalTime()) + ")";
			_tooltip = new Tooltip((ITooltipView)(object)new TooltipView(Ev.Name, description, _iconState.GetIcon(Ev.Icon), _translationState));
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			BitmapFont font = _getFontAction();
			DrawBackground(spriteBatch);
			int nameWidth = ((!Ev.Filler) ? DrawName(spriteBatch, font) : 0);
			DrawRemainingTime(spriteBatch, font, nameWidth);
			DrawCrossout(spriteBatch);
		}

		private void DrawBackground(SpriteBatch spriteBatch)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			Rectangle backgroundRect = default(Rectangle);
			((Rectangle)(ref backgroundRect))._002Ector(0, 0, base.Width, base.Height);
			spriteBatch.DrawRectangle(Textures.get_Pixel(), RectangleF.op_Implicit(backgroundRect), _getColorAction(), _getDrawBorders() ? 1 : 0, Color.get_Black());
		}

		private int DrawName(SpriteBatch spriteBatch, BitmapFont font)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			int nameWidth = MathHelper.Clamp((int)Math.Ceiling(font.MeasureString(Ev.Name).Width) + 10, 0, base.Width - 10);
			Rectangle nameRect = default(Rectangle);
			((Rectangle)(ref nameRect))._002Ector(5, 0, nameWidth, base.Height);
			spriteBatch.DrawString(Ev.Name, font, RectangleF.op_Implicit(nameRect), _getTextColor(), wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), (HorizontalAlignment)0, (VerticalAlignment)1);
			return nameRect.Width;
		}

		private void DrawRemainingTime(SpriteBatch spriteBatch, BitmapFont font, int x)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			if (x > base.Width)
			{
				return;
			}
			TimeSpan remainingTime = GetTimeRemaining(_getNowAction());
			if (!(remainingTime == TimeSpan.Zero))
			{
				string remainingTimeString = FormatTimeRemaining(remainingTime);
				int timeWidth = (int)Math.Ceiling(font.MeasureString(remainingTimeString).Width) + 10;
				int maxWidth = base.Width - x;
				int centerX = maxWidth / 2 - timeWidth / 2;
				if (centerX < x)
				{
					centerX = x + 10;
				}
				if (centerX + timeWidth <= base.Width)
				{
					Rectangle timeRect = default(Rectangle);
					((Rectangle)(ref timeRect))._002Ector(centerX, 0, maxWidth, base.Height);
					Color textColor = _getTextColor();
					spriteBatch.DrawString(remainingTimeString, font, RectangleF.op_Implicit(timeRect), textColor, wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		private TimeSpan GetTimeRemaining(DateTime now)
		{
			if (!(now <= _startTime) && !(now >= _endTime))
			{
				return _startTime.AddMinutes(Ev.Duration) - now;
			}
			return TimeSpan.Zero;
		}

		private void DrawCrossout(SpriteBatch spriteBatch)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (_getDrawCrossout())
			{
				Rectangle fullRect = default(Rectangle);
				((Rectangle)(ref fullRect))._002Ector(0, 0, base.Width, base.Height);
				spriteBatch.DrawCrossOut(Textures.get_Pixel(), RectangleF.op_Implicit(fullRect), Color.get_Red());
			}
		}

		private string FormatTimeRemaining(TimeSpan ts)
		{
			if (ts.Days > 0)
			{
				return ts.ToString("dd\\.hh\\:mm\\:ss");
			}
			if (ts.Hours > 0)
			{
				return ts.ToString("hh\\:mm\\:ss");
			}
			return ts.ToString("mm\\:ss");
		}

		private string FormatTime(DateTime dt)
		{
			return FormatTime(dt.TimeOfDay);
		}

		private string FormatTime(TimeSpan ts)
		{
			if (ts.Days > 0)
			{
				return ts.ToString("dd\\.hh\\:mm\\:ss");
			}
			return ts.ToString("hh\\:mm\\:ss");
		}
	}
}
