using System;

namespace Kenedia.Modules.Core.Models
{
	public class ConditionalNotification
	{
		public bool IsConditionMatched { get; private set; }

		public Func<bool> Condition { get; set; }

		public Func<string> SetLocalizedNotificationText { get; set; }

		public string NotificationText
		{
			get
			{
				return SetLocalizedNotificationText?.Invoke();
			}
			set
			{
				string value2 = value;
				SetLocalizedNotificationText = () => value2;
			}
		}

		public event EventHandler ConditionMatched;

		public ConditionalNotification()
		{
		}

		public ConditionalNotification(string notificationText, Func<bool> condition)
		{
			NotificationText = notificationText;
			Condition = condition;
		}

		public ConditionalNotification(Func<string> notificationText, Func<bool> condition)
		{
			SetLocalizedNotificationText = notificationText;
			Condition = condition;
		}

		public void CheckCondition()
		{
			if (Condition?.Invoke() ?? false)
			{
				IsConditionMatched = true;
				this.ConditionMatched?.Invoke(this, new EventArgs());
			}
		}
	}
}
