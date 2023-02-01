using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class ScreenNotification : Control
	{
		public enum NotificationType
		{
			Info,
			Warning,
			Error,
			Gray,
			Blue,
			Green,
			Red
		}

		private const int DURATION_DEFAULT = 4;

		private const int NOTIFICATION_WIDTH = 1024;

		private const int NOTIFICATION_HEIGHT = 256;

		private static readonly SynchronizedCollection<ScreenNotification> _activeScreenNotifications = new SynchronizedCollection<ScreenNotification>();

		private static readonly BitmapFont _fontMenomonia36Regular = Control.get_Content().GetFont((FontFace)0, (FontSize)36, (FontStyle)0);

		private static readonly Texture2D _textureGrayBackground = Control.get_Content().GetTexture("controls/notification/notification-gray");

		private static readonly Texture2D _textureBlueBackground = Control.get_Content().GetTexture("controls/notification/notification-blue");

		private static readonly Texture2D _textureGreenBackground = Control.get_Content().GetTexture("controls/notification/notification-green");

		private static readonly Texture2D _textureRedBackground = Control.get_Content().GetTexture("controls/notification/notification-red");

		private NotificationType _type;

		private int _duration;

		private Texture2D _icon;

		private string _message;

		private Tween _animFadeLifecycle;

		private int _targetTop;

		private Tween _slideDownTween;

		private Rectangle _layoutMessageBounds;

		private Rectangle _layoutIconBounds;

		public NotificationType Type
		{
			get
			{
				return _type;
			}
			set
			{
				((Control)this).SetProperty<NotificationType>(ref _type, value, true, "Type");
			}
		}

		public int Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _duration, value, false, "Duration");
			}
		}

		public Texture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<Texture2D>(ref _icon, value, false, "Icon");
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _message, value, false, "Message");
			}
		}

		public ScreenNotification(string message, NotificationType type = NotificationType.Info, Texture2D icon = null, int duration = 4)
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			_message = message;
			_type = type;
			_icon = icon;
			_duration = duration;
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(1024, 256));
			((Control)this).set_ZIndex(2147483615);
			((Control)this).set_Location(new Point(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() / 2 - ((Control)this).get_Size().X / 2, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height() / 4 - ((Control)this).get_Size().Y / 2));
			_targetTop = ((Control)this).get_Top();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			int calculatedNewTop = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height() / 4 - ((Control)this).get_Size().Y / 2;
			if (calculatedNewTop > _targetTop)
			{
				_targetTop += calculatedNewTop;
				Tween slideDownTween = _slideDownTween;
				if (slideDownTween != null)
				{
					slideDownTween.Cancel();
				}
				((TweenerImpl)GameService.Animation.get_Tweener()).Update((float)gameTime.get_ElapsedGameTime().TotalSeconds);
				((Control)this).set_Top(_targetTop);
			}
			((Control)this).set_Left(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() / 2 - ((Control)this).get_Size().X / 2);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void RecalculateLayout()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			switch (_type)
			{
			case NotificationType.Info:
			case NotificationType.Warning:
			case NotificationType.Error:
				_layoutMessageBounds = ((Control)this).get_LocalBounds();
				break;
			case NotificationType.Gray:
			case NotificationType.Blue:
			case NotificationType.Green:
			case NotificationType.Red:
				_layoutMessageBounds = ((Control)this).get_LocalBounds();
				break;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(_message))
			{
				Color messageColor = Color.get_White();
				Texture2D notificationBackground = null;
				switch (_type)
				{
				case NotificationType.Info:
					messageColor = Color.get_White();
					break;
				case NotificationType.Warning:
					messageColor = StandardColors.get_Yellow();
					break;
				case NotificationType.Error:
					messageColor = StandardColors.get_Red();
					break;
				case NotificationType.Gray:
					notificationBackground = _textureGrayBackground;
					break;
				case NotificationType.Blue:
					notificationBackground = _textureBlueBackground;
					break;
				case NotificationType.Green:
					notificationBackground = _textureGreenBackground;
					break;
				case NotificationType.Red:
					notificationBackground = _textureRedBackground;
					break;
				}
				if (notificationBackground != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, notificationBackground, _layoutMessageBounds);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Message, _fontMenomonia36Regular, RectangleExtension.OffsetBy(bounds, 1, 1), Color.get_Black(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Message, _fontMenomonia36Regular, bounds, messageColor, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		public override void Show()
		{
			_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ScreenNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay((float)Duration)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
			((Control)this).Show();
		}

		private void SlideDown(int distance)
		{
			_targetTop += distance;
			Tween slideDownTween = _slideDownTween;
			if (slideDownTween != null)
			{
				slideDownTween.Cancel();
			}
			_slideDownTween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ScreenNotification>(this, (object)new
			{
				Top = _targetTop
			}, 0.1f, 0f, true);
			if (!(base._opacity < 1f))
			{
				_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ScreenNotification>(this, (object)new
				{
					Opacity = 0f
				}, 1f, 0f, true).OnComplete((Action)((Control)this).Dispose);
			}
		}

		protected override void DisposeControl()
		{
			Tween slideDownTween = _slideDownTween;
			if (slideDownTween != null)
			{
				slideDownTween.Cancel();
			}
			_slideDownTween = null;
			_activeScreenNotifications.Remove(this);
			((Control)this).DisposeControl();
		}

		public static void ShowNotification(string message, NotificationType type = NotificationType.Info, Texture2D icon = null, int duration = 4)
		{
			ScreenNotification screenNotification = new ScreenNotification(message, type, icon, duration);
			((Control)screenNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			ScreenNotification nNot = screenNotification;
			((Control)nNot).set_ZIndex(_activeScreenNotifications.DefaultIfEmpty(nNot).Max((ScreenNotification n) => ((Control)n).get_ZIndex()) + 1);
			foreach (ScreenNotification activeScreenNotification in _activeScreenNotifications)
			{
				activeScreenNotification.SlideDown((int)((float)_fontMenomonia36Regular.get_LineHeight() * 0.75f));
			}
			_activeScreenNotifications.Add(nNot);
			((Control)nNot).Show();
		}

		public static void ShowNotification(string[] messages, NotificationType type = NotificationType.Info, Texture2D icon = null, int duration = 4)
		{
			for (int i = messages.Length - 1; i >= 0; i--)
			{
				ShowNotification(messages[i], type, icon, duration);
			}
		}
	}
}
