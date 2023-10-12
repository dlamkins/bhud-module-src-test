using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Models;
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
		public const int NOTIFICATION_WIDTH = 350;

		public const int NOTIFICATION_HEIGHT = 96;

		private const int ICON_SIZE = 64;

		private static EventNotification _lastShown;

		private static readonly BitmapFont _titleFont = GameService.Content.get_DefaultFont18();

		private static readonly BitmapFont _messageFont = GameService.Content.get_DefaultFont16();

		private static readonly Rectangle _fullRect = new Rectangle(0, 0, 350, 96);

		private static readonly Rectangle _iconRect = new Rectangle(10, 16, 64, 64);

		private static readonly Rectangle _titleRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, 10, _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5), _titleFont.get_LineHeight());

		private static readonly Rectangle _messageRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, ((Rectangle)(ref _titleRect)).get_Bottom(), _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5), 96 - _titleRect.Height);

		private readonly string _title;

		private readonly string _message;

		private AsyncTexture2D _icon;

		private IconService _iconService;

		private readonly bool _captureMouseClicks;

		private readonly int _x;

		private readonly int _y;

		private readonly ReminderStackDirection _stackDirection;

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		public float BackgroundOpacity { get; set; } = 1f;


		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string title, string message, AsyncTexture2D icon, int x, int y, ReminderStackDirection stackDirection, IconService iconService, bool captureMouseClicks = false)
		{
			Model = ev;
			_title = title;
			_message = message;
			_x = x;
			_y = y;
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
			base.Width = 350;
			base.Height = 96;
			((Control)this).set_Visible(false);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string message, int x, int y, ReminderStackDirection stackDirection, IconService iconService, bool captureMouseClicks = false)
			: this(ev, ev?.Name, message, null, x, y, stackDirection, iconService, captureMouseClicks)
		{
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
				ReminderStackDirection.Top => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Top() - base.Height - 15) : _y), 
				ReminderStackDirection.Down => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Bottom() + 15) : _y), 
				ReminderStackDirection.Left => new Point((_lastShown != null) ? (((Control)_lastShown).get_Left() - base.Width - 15) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				ReminderStackDirection.Right => new Point((_lastShown != null) ? (((Control)_lastShown).get_Right() + 15) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				_ => throw new ArgumentException($"Invalid stack direction: {_stackDirection}"), 
			}));
			((Control)this).Show();
			_lastShown = this;
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<EventNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay((float)duration.TotalSeconds)
				.Reflect()
				.OnComplete((Action)Hide);
		}

		public void Hide()
		{
			((Control)this).Hide();
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<EventNotification>(this, (object)new
			{
				Opacity = 0f
			}, 0.4f, 0f, true).OnComplete((Action)delegate
			{
				((Control)this).Dispose();
				if (_lastShown == this)
				{
					_lastShown = null;
				}
			});
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
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
			Model = null;
			_iconService = null;
			_icon = null;
		}
	}
}
