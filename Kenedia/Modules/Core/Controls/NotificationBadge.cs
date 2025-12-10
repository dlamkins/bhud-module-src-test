using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
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

		private double _lastChecked;

		private bool _deleting;

		private string _message;

		public CaptureType? CaptureInput { get; set; }

		public ObservableCollection<ConditionalNotification> Notifications { get; } = new ObservableCollection<ConditionalNotification>();


		public float HoveredOpacity { get; set; } = 1f;


		public new float Opacity
		{
			[CompilerGenerated]
			get
			{
				return _003COpacity_003Ek__BackingField;
			}
			set
			{
				_003COpacity_003Ek__BackingField = value;
				SetOpacity();
			}
		}

		public Control Anchor
		{
			[CompilerGenerated]
			get
			{
				return _003CAnchor_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CAnchor_003Ek__BackingField, value, new ValueChangedEventHandler<Control>(OnAnchorChanged));
			}
		}

		public Action ClickAction { get; set; }

		public NotificationBadge()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			_003COpacity_003Ek__BackingField = 1f;
			base._002Ector();
			base.Size = new Point(32);
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			Notifications.CollectionChanged += Notifications_CollectionChanged;
		}

		private void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!_deleting)
			{
				e.OldItems?.Cast<ConditionalNotification>().ForEach(delegate(ConditionalNotification n)
				{
					n.ConditionMatched -= new EventHandler(Notification_ConditionMatched);
				});
				e.NewItems?.Cast<ConditionalNotification>().ForEach(delegate(ConditionalNotification n)
				{
					n.ConditionMatched += new EventHandler(Notification_ConditionMatched);
				});
				_message = ((Notifications.Count > 0) ? string.Join(Environment.NewLine, Notifications.Select((ConditionalNotification e) => e.NotificationText).Distinct().Enumerate(Environment.NewLine, "[{0}]: ")) : string.Empty);
				base.BasicTooltipText = _message;
				base.Visible = Notifications.Count > 0;
			}
		}

		private void Notification_ConditionMatched(object sender, EventArgs e)
		{
			_removeNotifications.Add(sender as ConditionalNotification);
		}

		private void OnAnchorChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.MouseEntered -= SetHoveredOpacity;
				e.OldValue!.MouseLeft -= SetOpacity;
			}
			if (e.NewValue != null)
			{
				e.NewValue!.MouseEntered += SetHoveredOpacity;
				e.NewValue!.MouseLeft += SetOpacity;
			}
		}

		private void SetOpacity(object sender = null, EventArgs e = null)
		{
			base.Opacity = Opacity;
		}

		private void SetHoveredOpacity(object sender = null, EventArgs e = null)
		{
			base.Opacity = HoveredOpacity;
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_badge.Bounds = new Rectangle(Point.get_Zero(), base.Size);
		}

		public void UserLocale_SettingChanged(object sender = null, Blish_HUD.ValueChangedEventArgs<Locale> e = null)
		{
			_message = ((Notifications.Count > 0) ? string.Join(Environment.NewLine, Notifications.Select((ConditionalNotification e) => e.NotificationText).Distinct().Enumerate(Environment.NewLine, "[{0}]: ")) : string.Empty);
			base.BasicTooltipText = _message;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
			SetHoveredOpacity();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			base.OnMouseLeft(e);
			SetOpacity();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			base.OnMouseEntered(e);
			SetHoveredOpacity();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			ClickAction?.Invoke();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_badge.Draw(this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Anchor = null;
			_badge = null;
			Notifications?.Clear();
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		protected override CaptureType CapturesInput()
		{
			if (base.MouseOver)
			{
				return base.CapturesInput();
			}
			return CaptureInput ?? base.CapturesInput();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
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
				notification.ConditionMatched += new EventHandler(Notification_ConditionMatched);
				Notifications.Add(notification);
			}
		}
	}
}
