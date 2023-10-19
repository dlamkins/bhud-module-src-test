using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventNotification : RenderTarget2DControl
	{
		private static EventNotification _lastShown;

		private readonly BitmapFont _titleFont;

		private readonly BitmapFont _messageFont;

		private Rectangle _fullRect;

		private Rectangle _iconRect;

		private Rectangle _titleRect;

		private Rectangle _messageRect;

		private readonly string _title;

		private readonly string _message;

		private AsyncTexture2D _icon;

		private IconService _iconService;

		private readonly bool _captureMouseClicks;

		private readonly int _x;

		private readonly int _y;

		private readonly int _iconSize;

		private readonly EventReminderStackDirection _stackDirection;

		private Tween _showAnimation;

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		public float BackgroundOpacity { get; set; } = 1f;


		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string title, string message, AsyncTexture2D icon, int x, int y, int width, int height, int iconSize, EventReminderStackDirection stackDirection, FontSize titleFontSize, FontSize messageFontSize, IconService iconService, bool captureMouseClicks = false)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			Model = ev;
			_title = title;
			_message = message;
			_x = x;
			_y = y;
			_iconSize = iconSize;
			_stackDirection = stackDirection;
			_iconService = iconService;
			_captureMouseClicks = captureMouseClicks;
			if (icon != null)
			{
				_icon = icon;
			}
			else if (ev != null && ev.Icon != null)
			{
				_icon = ((ev == null || ev.Icon == null) ? null : _iconService?.GetIcon(ev.Icon));
			}
			_titleFont = GameService.Content.GetFont((FontFace)0, titleFontSize, (FontStyle)0);
			_messageFont = GameService.Content.GetFont((FontFace)0, messageFontSize, (FontStyle)0);
			base.Width = width;
			base.Height = height;
			((Control)this).set_Visible(false);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			if (_iconSize > base.Height)
			{
				throw new ArgumentOutOfRangeException("iconSize", "The icon size can't be higher than the total height.");
			}
		}

		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string message, int x, int y, int width, int height, int iconSize, EventReminderStackDirection stackDirection, FontSize titleFontSize, FontSize messageFontSize, IconService iconService, bool captureMouseClicks = false)
			: this(ev, ev?.Name, message, null, x, y, width, height, iconSize, stackDirection, titleFontSize, messageFontSize, iconService, captureMouseClicks)
		{
		}//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)


		public override void RecalculateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			_fullRect = new Rectangle(0, 0, base.Width, base.Height);
			_iconRect = new Rectangle(10, base.Height / 2 - _iconSize / 2, _iconSize, _iconSize);
			_titleRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, 10, _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5), _titleFont.get_LineHeight());
			_messageRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, ((Rectangle)(ref _titleRect)).get_Bottom(), _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5), base.Height - _titleRect.Height);
		}

		public void Show(TimeSpan duration)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location((Point)(_stackDirection switch
			{
				EventReminderStackDirection.Top => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Top() - base.Height - 15) : _y), 
				EventReminderStackDirection.Down => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Bottom() + 15) : _y), 
				EventReminderStackDirection.Left => new Point((_lastShown != null) ? (((Control)_lastShown).get_Left() - base.Width - 15) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				EventReminderStackDirection.Right => new Point((_lastShown != null) ? (((Control)_lastShown).get_Right() + 15) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				_ => throw new ArgumentException($"Invalid stack direction: {_stackDirection}"), 
			}));
			((Control)this).Show();
			_lastShown = this;
			Tween showAnimation = _showAnimation;
			if (showAnimation != null)
			{
				showAnimation.Cancel();
			}
			_showAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<EventNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay((float)duration.TotalSeconds)
				.Reflect()
				.OnComplete((Action)delegate
				{
					((Control)this).Hide();
					((Control)this).Dispose();
					if (_lastShown == this)
					{
						_lastShown = null;
					}
				});
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(Textures.get_Pixel(), _fullRect, Color.get_Black() * BackgroundOpacity);
			if (_icon != null)
			{
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_icon), _iconRect, Color.get_White());
			}
			if (!string.IsNullOrWhiteSpace(_title))
			{
				spriteBatch.DrawString(_title, _titleFont, RectangleF.op_Implicit(_titleRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (!string.IsNullOrWhiteSpace(_message))
			{
				spriteBatch.DrawString(_message, _messageFont, RectangleF.op_Implicit(_messageRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (_captureMouseClicks)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}

		protected override void InternalDispose()
		{
			Tween showAnimation = _showAnimation;
			if (showAnimation != null)
			{
				showAnimation.Cancel();
			}
			_showAnimation = null;
			Model = null;
			_iconService = null;
			_icon = null;
		}
	}
}
