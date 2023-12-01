using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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
		private static ToastNotifier _toastNotifier = ToastNotificationManager.CreateToastNotifier("Estreya BlishHUD Event Table");

		private static EventNotification _lastShown;

		private readonly BitmapFont _titleFont;

		private readonly BitmapFont _messageFont;

		private Rectangle _fullRect;

		private Rectangle _iconRect;

		private Rectangle _titleRect;

		private Rectangle _messageRect;

		private readonly string _title;

		private readonly string _message;

		private string _formattedTitle;

		private string _formattedMessage;

		private AsyncTexture2D _icon;

		private IconService _iconService;

		private readonly bool _captureMouseClicks;

		private readonly int _x;

		private readonly int _y;

		private readonly int _iconSize;

		private readonly EventReminderStackDirection _stackDirection;

		private readonly EventReminderStackDirection _overflowStackDirection;

		private Tween _showAnimation;

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		public float BackgroundOpacity { get; set; } = 1f;


		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string title, string message, AsyncTexture2D icon, int x, int y, int width, int height, int iconSize, EventReminderStackDirection stackDirection, EventReminderStackDirection overflowStackDirection, FontSize titleFontSize, FontSize messageFontSize, IconService iconService, bool captureMouseClicks = false)
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			Model = ev;
			_title = title;
			_message = message;
			_x = x;
			_y = y;
			_iconSize = iconSize;
			_stackDirection = stackDirection;
			_overflowStackDirection = overflowStackDirection;
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

		public EventNotification(Estreya.BlishHUD.EventTable.Models.Event ev, string message, int x, int y, int width, int height, int iconSize, EventReminderStackDirection stackDirection, EventReminderStackDirection overflowStackDirection, FontSize titleFontSize, FontSize messageFontSize, IconService iconService, bool captureMouseClicks = false)
			: this(ev, ev?.Name, message, null, x, y, width, height, iconSize, stackDirection, overflowStackDirection, titleFontSize, messageFontSize, iconService, captureMouseClicks)
		{
		}//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)


		public override void RecalculateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			_fullRect = new Rectangle(0, 0, base.Width, base.Height);
			_iconRect = new Rectangle(10, base.Height / 2 - _iconSize / 2, _iconSize, _iconSize);
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
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = ((Control)((Control)this).get_Parent()).get_AbsoluteBounds();
			if (((Rectangle)(ref absoluteBounds)).Contains(((Control)this).get_AbsoluteBounds()))
			{
				return ((Control)this).get_Location();
			}
			return (Point)(_stackDirection switch
			{
				EventReminderStackDirection.Top => _overflowStackDirection switch
				{
					EventReminderStackDirection.Top => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Down => throw new InvalidOperationException("Can't overflow to the bottom."), 
					EventReminderStackDirection.Left => new Point(((Control)this).get_Left() - base.Width - spacing, _y), 
					EventReminderStackDirection.Right => new Point(((Control)this).get_Right() + spacing, _y), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {_stackDirection}"), 
				}, 
				EventReminderStackDirection.Down => _overflowStackDirection switch
				{
					EventReminderStackDirection.Top => throw new InvalidOperationException("Can't overflow to the top."), 
					EventReminderStackDirection.Down => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Left => new Point(((Control)this).get_Left() - base.Width - spacing, _y), 
					EventReminderStackDirection.Right => new Point(((Control)this).get_Right() + spacing, _y), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {_stackDirection}"), 
				}, 
				EventReminderStackDirection.Left => _overflowStackDirection switch
				{
					EventReminderStackDirection.Top => new Point(_x, ((Control)this).get_Top() - base.Height - spacing), 
					EventReminderStackDirection.Down => new Point(_x, ((Control)this).get_Bottom() + spacing), 
					EventReminderStackDirection.Left => throw new InvalidOperationException("Can't overflow to same direction."), 
					EventReminderStackDirection.Right => throw new InvalidOperationException("Can't overflow to the right."), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {_stackDirection}"), 
				}, 
				EventReminderStackDirection.Right => _overflowStackDirection switch
				{
					EventReminderStackDirection.Top => new Point(_x, ((Control)this).get_Top() - base.Height - spacing), 
					EventReminderStackDirection.Down => new Point(_x, ((Control)this).get_Bottom() + spacing), 
					EventReminderStackDirection.Left => throw new InvalidOperationException("Can't overflow to the left."), 
					EventReminderStackDirection.Right => throw new InvalidOperationException("Can't overflow to same direction."), 
					_ => throw new ArgumentException($"Invalid overflow stack direction: {_stackDirection}"), 
				}, 
				_ => throw new ArgumentException($"Invalid stack direction: {_stackDirection}"), 
			});
		}

		public void Show(TimeSpan duration)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			int spacing = 15;
			((Control)this).set_Location((Point)(_stackDirection switch
			{
				EventReminderStackDirection.Top => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Top() - base.Height - spacing) : _y), 
				EventReminderStackDirection.Down => new Point((_lastShown != null) ? ((Control)_lastShown).get_Left() : _x, (_lastShown != null) ? (((Control)_lastShown).get_Bottom() + spacing) : _y), 
				EventReminderStackDirection.Left => new Point((_lastShown != null) ? (((Control)_lastShown).get_Left() - base.Width - spacing) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				EventReminderStackDirection.Right => new Point((_lastShown != null) ? (((Control)_lastShown).get_Right() + spacing) : _x, (_lastShown != null) ? ((Control)_lastShown).get_Top() : _y), 
				_ => throw new ArgumentException($"Invalid stack direction: {_stackDirection}"), 
			}));
			((Control)this).set_Location(GetOverflowLocation(spacing));
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
			if (!string.IsNullOrWhiteSpace(_formattedTitle))
			{
				spriteBatch.DrawString(_formattedTitle, _titleFont, RectangleF.op_Implicit(_titleRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (!string.IsNullOrWhiteSpace(_formattedMessage))
			{
				spriteBatch.DrawString(_formattedMessage, _messageFont, RectangleF.op_Implicit(_messageRect), Color.get_White(), wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
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
