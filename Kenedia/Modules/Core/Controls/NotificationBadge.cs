using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class NotificationBadge : Control
	{
		private readonly List<ConditionalNotification> _removeNotifications = new List<ConditionalNotification>();

		private DetailedTexture _badge = new DetailedTexture(222246);

		private Control _anchor;

		private float _opcacity = 1f;

		private double _lastChecked;

		private bool _deleting;

		private string _message;

		public CaptureType? CaptureInput { get; set; }

		public ObservableCollection<ConditionalNotification> Notifications { get; } = new ObservableCollection<ConditionalNotification>();


		public float HoveredOpacity { get; set; } = 1f;


		public float Opacity
		{
			get
			{
				return _opcacity;
			}
			set
			{
				_opcacity = value;
				SetOpacity();
			}
		}

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, OnAnchorChanged);
			}
		}

		public Action ClickAction { get; set; }

		public NotificationBadge()
			: this()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(32));
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			Notifications.CollectionChanged += Notifications_CollectionChanged;
		}

		private void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!_deleting)
			{
				e.OldItems?.Cast<ConditionalNotification>().ForEach(delegate(ConditionalNotification n)
				{
					n.ConditionMatched -= Notification_ConditionMatched;
				});
				e.NewItems?.Cast<ConditionalNotification>().ForEach(delegate(ConditionalNotification n)
				{
					n.ConditionMatched += Notification_ConditionMatched;
				});
				_message = ((Notifications.Count > 0) ? string.Join(Environment.NewLine, Notifications.Select((ConditionalNotification e) => e.NotificationText).Distinct().Enumerate(Environment.NewLine, "[{0}]: ")) : string.Empty);
				((Control)this).set_BasicTooltipText(_message);
				((Control)this).set_Visible(Notifications.Count > 0);
			}
		}

		private void Notification_ConditionMatched(object sender, EventArgs e)
		{
			_removeNotifications.Add(sender as ConditionalNotification);
		}

		private void OnAnchorChanged(object sender, ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.remove_MouseEntered((EventHandler<MouseEventArgs>)SetHoveredOpacity);
				e.OldValue!.remove_MouseLeft((EventHandler<MouseEventArgs>)SetOpacity);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.add_MouseEntered((EventHandler<MouseEventArgs>)SetHoveredOpacity);
				e.NewValue!.add_MouseLeft((EventHandler<MouseEventArgs>)SetOpacity);
			}
		}

		private void SetOpacity(object sender = null, EventArgs e = null)
		{
			((Control)this).set_Opacity(Opacity);
		}

		private void SetHoveredOpacity(object sender = null, EventArgs e = null)
		{
			((Control)this).set_Opacity(HoveredOpacity);
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			_badge.Bounds = new Rectangle(Point.get_Zero(), ((Control)this).get_Size());
		}

		public void UserLocale_SettingChanged(object sender = null, ValueChangedEventArgs<Locale> e = null)
		{
			_message = ((Notifications.Count > 0) ? string.Join(Environment.NewLine, Notifications.Select((ConditionalNotification e) => e.NotificationText).Distinct().Enumerate(Environment.NewLine, "[{0}]: ")) : string.Empty);
			((Control)this).set_BasicTooltipText(_message);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			SetHoveredOpacity();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			SetOpacity();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			SetHoveredOpacity();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			ClickAction?.Invoke();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_badge.Draw((Control)(object)this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Anchor = null;
			_badge = null;
			Notifications?.Clear();
			LocalizingService.LocaleChanged -= UserLocale_SettingChanged;
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				return ((Control)this).CapturesInput();
			}
			return (CaptureType)(((_003F?)CaptureInput) ?? ((Control)this).CapturesInput());
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _lastChecked < 1000.0 || Notifications.Count <= 0)
			{
				return;
			}
			_lastChecked = gameTime.get_TotalGameTime().TotalMilliseconds;
			foreach (ConditionalNotification notification in Notifications)
			{
				notification.CheckCondition();
			}
			for (int i = 0; i < _removeNotifications.Count; i++)
			{
				_deleting = i < _removeNotifications.Count - 1;
				Notifications.Remove(_removeNotifications[i]);
			}
		}

		public void AddNotification(ConditionalNotification notification)
		{
			if (notification != null)
			{
				Notifications.Add(notification);
			}
		}
	}
}
