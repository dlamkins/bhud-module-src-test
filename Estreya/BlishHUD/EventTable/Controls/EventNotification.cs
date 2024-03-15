using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventNotification : RenderTarget2DControl
	{
		private static Logger Logger = Logger.GetLogger<EventNotification>();

		private static ToastNotifier _toastNotifier = ToastNotificationManager.CreateToastNotifier("Estreya BlishHUD Event Table");

		private static SynchronizedCollection<EventNotification> _activeNotifications = new SynchronizedCollection<EventNotification>();

		private static ConcurrentDictionary<FontSize, BitmapFont> _fonts = new ConcurrentDictionary<FontSize, BitmapFont>();

		private BitmapFont _titleFont;

		private BitmapFont _messageFont;

		private Rectangle _fullRect;

		private Rectangle _iconRect;

		private Rectangle _titleRect;

		private Rectangle _messageRect;

		private readonly string _title;

		private readonly string _message;

		private readonly ModuleSettings _moduleSettings;

		private string _formattedTitle;

		private string _formattedMessage;

		private AsyncTexture2D _icon;

		private readonly bool _captureMouseClicks;

		private Tween _showAnimation;

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		private EventNotification(Estreya.BlishHUD.EventTable.Models.Event model, string title, string message, AsyncTexture2D icon, ModuleSettings moduleSettings)
		{
			Model = model;
			_title = title;
			_message = message;
			_moduleSettings = moduleSettings;
			_captureMouseClicks = moduleSettings.ReminderLeftClickAction.get_Value() != 0 || moduleSettings.ReminderRightClickAction.get_Value() != EventReminderRightClickAction.None;
			_icon = icon;
			UpdateFonts();
			SetWidthAndHeight();
			SetLocation();
			((Control)this).set_Visible(false);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_moduleSettings.ReminderSize.Icon.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)ReminderIconSize_SettingChanged);
		}

		private void ReminderIconSize_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			((Control)this).RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			int iconSize = _moduleSettings.ReminderSize.Icon.get_Value();
			_fullRect = new Rectangle(0, 0, base.Width, base.Height);
			_iconRect = new Rectangle(10, base.Height / 2 - iconSize / 2, iconSize, iconSize);
			int maxTitleWidth = _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5);
			_formattedTitle = DrawUtil.WrapText(_titleFont, _title, (float)(maxTitleWidth - 10));
			Size2 titleSize = _titleFont.MeasureString(_formattedTitle);
			_titleRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, 10, maxTitleWidth, (int)Math.Ceiling(titleSize.Height));
			int maxMessageWidth = _fullRect.Width - (((Rectangle)(ref _iconRect)).get_Right() + 5);
			_formattedMessage = DrawUtil.WrapText(_messageFont, _message, (float)(maxMessageWidth - 10));
			_messageRect = new Rectangle(((Rectangle)(ref _iconRect)).get_Right() + 5, ((Rectangle)(ref _titleRect)).get_Bottom(), maxMessageWidth, base.Height - _titleRect.Height);
		}

		private Point GetOverflowLocation(int spacing)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			int initialXLocation = _moduleSettings.ReminderPosition.X.get_Value();
			int initialYLocation = _moduleSettings.ReminderPosition.Y.get_Value();
			EventReminderStackDirection stackDirection = _moduleSettings.ReminderStackDirection.get_Value();
			EventReminderStackDirection overflowStackDirection = _moduleSettings.ReminderOverflowStackDirection.get_Value();
			Container parent = ((Control)this).get_Parent();
			if (parent == null)
			{
				return ((Control)this).get_Location();
			}
			Rectangle absoluteBounds = ((Control)parent).get_AbsoluteBounds();
			if (((Rectangle)(ref absoluteBounds)).Contains(((Control)this).get_AbsoluteBounds()))
			{
				return ((Control)this).get_Location();
			}
			return (Point)(stackDirection switch
			{
				EventReminderStackDirection.Top => overflowStackDirection switch
				{
					EventReminderStackDirection.Top => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Down => throw new InvalidOperationException("Can't overflow to the bottom."), 
					EventReminderStackDirection.Left => new Point(((Control)this).get_Left() - base.Width - spacing, initialYLocation), 
					EventReminderStackDirection.Right => new Point(((Control)this).get_Right() + spacing, initialYLocation), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {overflowStackDirection}"), 
				}, 
				EventReminderStackDirection.Down => overflowStackDirection switch
				{
					EventReminderStackDirection.Top => throw new InvalidOperationException("Can't overflow to the top."), 
					EventReminderStackDirection.Down => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Left => new Point(((Control)this).get_Left() - base.Width - spacing, initialYLocation), 
					EventReminderStackDirection.Right => new Point(((Control)this).get_Right() + spacing, initialYLocation), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {overflowStackDirection}"), 
				}, 
				EventReminderStackDirection.Left => overflowStackDirection switch
				{
					EventReminderStackDirection.Top => new Point(initialXLocation, ((Control)this).get_Top() - base.Height - spacing), 
					EventReminderStackDirection.Down => new Point(initialXLocation, ((Control)this).get_Bottom() + spacing), 
					EventReminderStackDirection.Left => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Right => throw new InvalidOperationException("Can't overflow to the right."), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {overflowStackDirection}"), 
				}, 
				EventReminderStackDirection.Right => overflowStackDirection switch
				{
					EventReminderStackDirection.Top => new Point(initialXLocation, ((Control)this).get_Top() - base.Height - spacing), 
					EventReminderStackDirection.Down => new Point(initialXLocation, ((Control)this).get_Bottom() + spacing), 
					EventReminderStackDirection.Left => throw new InvalidOperationException("Can't overflow to the left."), 
					EventReminderStackDirection.Right => throw new InvalidOperationException("Can't overflow to same direction."), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {overflowStackDirection}"), 
				}, 
				_ => throw new ArgumentException($"Invalid stack direction: {stackDirection}"), 
			});
		}

		private BitmapFont GetTitleFont()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return _fonts.GetOrAdd(_moduleSettings.ReminderFonts.TitleSize.get_Value(), (Func<FontSize, BitmapFont>)((FontSize size) => GameService.Content.GetFont((FontFace)0, size, (FontStyle)0)));
		}

		private BitmapFont GetMessageFont()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return _fonts.GetOrAdd(_moduleSettings.ReminderFonts.MessageSize.get_Value(), (Func<FontSize, BitmapFont>)((FontSize size) => GameService.Content.GetFont((FontFace)0, size, (FontStyle)0)));
		}

		private void SetWidthAndHeight()
		{
			base.Width = _moduleSettings.ReminderSize.X.get_Value();
			base.Height = _moduleSettings.ReminderSize.Y.get_Value();
		}

		private void SetLocation()
		{
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			int spacing = 15;
			int initialXLocation = _moduleSettings.ReminderPosition.X.get_Value();
			int initialYLocation = _moduleSettings.ReminderPosition.Y.get_Value();
			List<EventNotification> notifications = _activeNotifications.ToList();
			int indexInNotifications = notifications.IndexOf(this);
			bool flag = (((uint)(indexInNotifications - -1) <= 1u) ? true : false);
			EventNotification lastShown = (flag ? null : notifications[indexInNotifications - 1]);
			((Control)this).set_Location((Point)(_moduleSettings.ReminderStackDirection.get_Value() switch
			{
				EventReminderStackDirection.Top => new Point((lastShown != null) ? ((Control)lastShown).get_Left() : initialXLocation, (lastShown != null) ? (((Control)lastShown).get_Top() - base.Height - spacing) : initialYLocation), 
				EventReminderStackDirection.Down => new Point((lastShown != null) ? ((Control)lastShown).get_Left() : initialXLocation, (lastShown != null) ? (((Control)lastShown).get_Bottom() + spacing) : initialYLocation), 
				EventReminderStackDirection.Left => new Point((lastShown != null) ? (((Control)lastShown).get_Left() - base.Width - spacing) : initialXLocation, (lastShown != null) ? ((Control)lastShown).get_Top() : initialYLocation), 
				EventReminderStackDirection.Right => new Point((lastShown != null) ? (((Control)lastShown).get_Right() + spacing) : initialXLocation, (lastShown != null) ? ((Control)lastShown).get_Top() : initialYLocation), 
				_ => throw new ArgumentException($"Invalid stack direction: {_moduleSettings.ReminderStackDirection.get_Value()}"), 
			}));
			((Control)this).set_Location(GetOverflowLocation(spacing));
		}

		private void UpdateFonts()
		{
			BitmapFont newTitleFont = GetTitleFont();
			BitmapFont newMessageFont = GetMessageFont();
			bool recalculate = false;
			if (newTitleFont != _titleFont || newMessageFont != _messageFont)
			{
				recalculate = true;
			}
			_titleFont = newTitleFont;
			_messageFont = newMessageFont;
			if (recalculate)
			{
				((Control)this).RecalculateLayout();
			}
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateFonts();
			SetWidthAndHeight();
			SetLocation();
		}

		private void Show(TimeSpan duration)
		{
			((Control)this).Show();
			_activeNotifications.Add(this);
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
					((Control)this).Dispose();
				});
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			Color backgroundColor = ((_moduleSettings.ReminderColors.Background.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(_moduleSettings.ReminderColors.Background.get_Value().get_Cloth()));
			spriteBatch.Draw(Textures.get_Pixel(), _fullRect, backgroundColor * _moduleSettings.ReminderBackgroundOpacity.get_Value());
			if (_icon != null)
			{
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(_icon), _iconRect, Color.get_White());
			}
			if (!string.IsNullOrWhiteSpace(_formattedTitle))
			{
				Color titleColor = ((_moduleSettings.ReminderColors.TitleText.get_Value().get_Id() == 1) ? Color.get_White() : ColorExtensions.ToXnaColor(_moduleSettings.ReminderColors.TitleText.get_Value().get_Cloth()));
				spriteBatch.DrawString(_formattedTitle, _titleFont, RectangleF.op_Implicit(_titleRect), titleColor * _moduleSettings.ReminderTitleOpacity.get_Value(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (!string.IsNullOrWhiteSpace(_formattedMessage))
			{
				Color messageColor = ((_moduleSettings.ReminderColors.MessageText.get_Value().get_Id() == 1) ? Color.get_White() : ColorExtensions.ToXnaColor(_moduleSettings.ReminderColors.MessageText.get_Value().get_Cloth()));
				spriteBatch.DrawString(_formattedMessage, _messageFont, RectangleF.op_Implicit(_messageRect), messageColor * _moduleSettings.ReminderMessageOpacity.get_Value(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
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
			_moduleSettings.ReminderSize.Icon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)ReminderIconSize_SettingChanged);
			((Control)this).Hide();
			_activeNotifications.Remove(this);
			Tween showAnimation = _showAnimation;
			if (showAnimation != null)
			{
				showAnimation.Cancel();
			}
			_showAnimation = null;
			Model = null;
			_icon = null;
		}

		public static EventNotification ShowAsControl(string title, string message, AsyncTexture2D icon, IconService iconService, ModuleSettings moduleSettings)
		{
			return ShowAsControl(null, title, message, icon, iconService, moduleSettings);
		}

		public static EventNotification ShowAsControl(Estreya.BlishHUD.EventTable.Models.Event ev, string title, string message, AsyncTexture2D icon, IconService iconService, ModuleSettings moduleSettings)
		{
			return ShowAsControl(ev, title, message, icon, iconService, moduleSettings, TimeSpan.FromSeconds(moduleSettings.ReminderDuration.get_Value()));
		}

		public static EventNotification ShowAsControl(Estreya.BlishHUD.EventTable.Models.Event ev, string title, string message, AsyncTexture2D icon, IconService iconService, ModuleSettings moduleSettings, TimeSpan timeout)
		{
			EventNotification eventNotification = new EventNotification(ev, title, message, icon ?? ((!string.IsNullOrWhiteSpace(ev.Icon)) ? iconService.GetIcon(ev.Icon) : null), moduleSettings);
			eventNotification.Show(timeout);
			return eventNotification;
		}

		public static EventNotification ShowAsControlTest(string title, string message, AsyncTexture2D icon, IconService iconService, ModuleSettings moduleSettings)
		{
			return ShowAsControl(null, title, message, icon, iconService, moduleSettings, TimeSpan.FromHours(1.0));
		}

		public static async Task ShowAsWindowsNotification(string title, string message, AsyncTexture2D icon)
		{
			XmlDocument template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
			XmlNodeList elementsByTagName = template.GetElementsByTagName("text");
			((IReadOnlyList<IXmlNode>)elementsByTagName)[0].AppendChild(template.CreateTextNode(title));
			((IReadOnlyList<IXmlNode>)elementsByTagName)[1].AppendChild(template.CreateTextNode(message));
			XmlNodeList toastImageElements = template.GetElementsByTagName("image");
			await icon.WaitUntilSwappedAsync(TimeSpan.FromSeconds(10.0));
			string tempImagePath = Path.GetTempFileName();
			tempImagePath = Path.ChangeExtension(tempImagePath, "png");
			using FileStream fs = new FileStream(tempImagePath, FileMode.OpenOrCreate);
			icon.get_Texture().SaveAsPng((Stream)fs, icon.get_Texture().get_Width(), icon.get_Texture().get_Height());
			((XmlElement)((IReadOnlyList<IXmlNode>)toastImageElements)[0]).SetAttribute("src", tempImagePath);
			((XmlElement)template.SelectSingleNode("/toast")).SetAttribute("duration", "short");
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				ToastNotification toastNotification = new ToastNotification(template)
				{
					ExpirationTime = DateTimeOffset.Now.AddMinutes(5.0),
					ExpiresOnReboot = true
				};
				ToastNotification toastNotification2 = toastNotification;
				WindowsRuntimeMarshal.AddEventHandler((Func<TypedEventHandler<ToastNotification, ToastDismissedEventArgs>, EventRegistrationToken>)toastNotification2.add_Dismissed, (Action<EventRegistrationToken>)toastNotification2.remove_Dismissed, (TypedEventHandler<ToastNotification, ToastDismissedEventArgs>)delegate
				{
					File.Delete(tempImagePath);
				});
				_toastNotifier.Show(toastNotification);
			});
		}
	}
}
