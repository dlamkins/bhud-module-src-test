using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Events_Module.Properties;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Events_Module
{
	public class EventNotification : Container
	{
		private const int NOTIFICATION_WIDTH = 280;

		private const int NOTIFICATION_HEIGHT = 64;

		private const int ICON_SIZE = 64;

		private static readonly Texture2D _textureBackground;

		private readonly AsyncTexture2D _icon;

		private static int _visibleNotifications;

		private Rectangle _layoutIconBounds;

		static EventNotification()
		{
			_textureBackground = EventsModule.ModuleInstance.ContentsManager.GetTexture("textures\\ns-button.png");
		}

		private EventNotification(string title, AsyncTexture2D icon, string message, string waypoint)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			EventNotification eventNotification = this;
			string tooltipText = Resources.Notification_Tooltip;
			_icon = icon;
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(280, 64));
			((Control)this).set_Location(new Point(EventsModule.ModuleInstance.NotificationPosition.X, EventsModule.ModuleInstance.NotificationPosition.Y + 79 * _visibleNotifications));
			((Control)this).set_BasicTooltipText(tooltipText);
			string wrappedTitle = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), title, (float)(((Control)this).get_Width() - 64 - 20));
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(74, 5));
			((Control)val).set_Size(new Point(((Control)this).get_Width() - 64 - 10, ((Control)this).get_Height() / 2));
			val.set_Font(Control.get_Content().get_DefaultFont14());
			((Control)val).set_BasicTooltipText(tooltipText);
			val.set_Text(wrappedTitle);
			string wrapped = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), message, (float)(((Control)this).get_Width() - 64 - 20));
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(74, ((Control)this).get_Height() / 2));
			((Control)val2).set_Size(new Point(((Control)this).get_Width() - 64 - 10, ((Control)this).get_Height() / 2));
			((Control)val2).set_BasicTooltipText(tooltipText);
			val2.set_Text(wrapped);
			_visibleNotifications++;
			((Control)this).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				((Control)eventNotification).Dispose();
			});
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint).ContinueWith(delegate(Task<bool> clipboardResult)
				{
					if (clipboardResult.IsFaulted)
					{
						ScreenNotification.ShowNotification(Resources.Failed_to_copy_waypoint_to_clipboard__Try_again_, (NotificationType)6, (Texture2D)null, 2);
					}
					else
					{
						ScreenNotification.ShowNotification(Resources.Copied_waypoint_to_clipboard_, (NotificationType)0, (Texture2D)null, 2);
					}
				});
				((Control)eventNotification).Dispose();
			});
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			int icoSize = 52;
			_layoutIconBounds = new Rectangle(32 - icoSize / 2, 32 - icoSize / 2, icoSize, icoSize);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureBackground, bounds, Color.get_White() * 0.85f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutIconBounds);
		}

		private void Show(float duration)
		{
			if (EventsModule.ModuleInstance.ChimeEnabled)
			{
				Control.get_Content().PlaySoundEffectByName("audio\\color-change");
			}
			((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay(duration)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		public static void ShowNotification(string title, AsyncTexture2D icon, string message, float duration, string waypoint)
		{
			EventNotification eventNotification = new EventNotification(title, icon, message, waypoint);
			((Control)eventNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			eventNotification.Show(duration);
		}

		protected override void DisposeControl()
		{
			_visibleNotifications--;
			((Container)this).DisposeControl();
		}
	}
}
