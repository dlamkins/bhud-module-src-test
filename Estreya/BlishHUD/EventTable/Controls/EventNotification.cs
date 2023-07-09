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

		private readonly string _message;

		private Estreya.BlishHUD.EventTable.Models.Event _event;

		private AsyncTexture2D _eventIcon;

		private IconService _iconService;

		private readonly int _x;

		private readonly int _y;

		public float BackgroundOpacity { get; set; } = 1f;


		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string message, int x, int y, IconService iconService)
		{
			_event = ev;
			_message = message;
			_x = x;
			_y = y;
			_iconService = iconService;
			_eventIcon = _iconService?.GetIcon(ev.Icon);
			base.Width = 350;
			base.Height = 96;
			((Control)this).set_Visible(false);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		public void Show(TimeSpan duration)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(_x, (_lastShown != null) ? (((Control)_lastShown).get_Bottom() + 15) : _y));
			((Control)this).Show();
			_lastShown = this;
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<EventNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay((float)duration.TotalSeconds)
				.Reflect()
				.OnComplete((Action)delegate
				{
					Hide();
				});
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
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(Textures.get_Pixel(), _fullRect, Color.get_Black() * BackgroundOpacity);
			if (_eventIcon != null && _eventIcon.get_HasSwapped())
			{
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_eventIcon), _iconRect, Color.get_White());
			}
			spriteBatch.DrawString(_event.Name, _titleFont, RectangleF.op_Implicit(_titleRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			spriteBatch.DrawString(_message, _messageFont, RectangleF.op_Implicit(_messageRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		protected override void InternalDispose()
		{
			_event = null;
			_iconService = null;
			_eventIcon = null;
		}
	}
}
