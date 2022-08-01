using System.ComponentModel;

namespace falcon.cmtracker
{
	public class SettingValue : INotifyPropertyChanged
	{
		private Boss _boss;

		private bool _value;

		private string _account;

		public Boss Boss
		{
			get
			{
				return _boss;
			}
			set
			{
				if (_boss != value)
				{
					_boss = value;
				}
			}
		}

		public bool Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value != value)
				{
					_value = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string Account
		{
			get
			{
				return _account;
			}
			set
			{
				if (!(_account == value))
				{
					_account = value;
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public SettingValue(string account, Boss boss, bool value)
		{
			_account = account;
			_boss = boss;
			_value = value;
		}

		protected void NotifyPropertyChanged(string propertyName = "")
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
