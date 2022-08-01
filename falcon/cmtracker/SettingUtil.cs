using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace falcon.cmtracker
{
	internal class SettingUtil : INotifyPropertyChanged
	{
		private List<SettingValue> Setting = new List<SettingValue>();

		private string _localSettingValue = "";

		public string SettingString
		{
			get
			{
				return _localSettingValue;
			}
			set
			{
				if (!(_localSettingValue == value))
				{
					_localSettingValue = value;
					NotifyPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public SettingUtil()
		{
		}

		public SettingUtil(string value)
		{
			_localSettingValue = value;
			UpdateSettting(value);
		}

		public void UpdateSettting(string stringValue)
		{
			if (!string.IsNullOrEmpty(stringValue))
			{
				string[] array = stringValue.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string[] SplitSetting = array[i].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					Enum.TryParse<Boss>(SplitSetting[1], out var boss);
					SettingValue NewSetting = new SettingValue(SplitSetting[0], boss, bool.Parse(SplitSetting[2]));
					NewSetting.PropertyChanged += WhenSettingValueChanged;
					Setting.Add(NewSetting);
				}
			}
		}

		private void WhenSettingValueChanged(object sender, PropertyChangedEventArgs e)
		{
			SettingValue tempSetting = (SettingValue)sender;
			foreach (SettingValue Item in Setting)
			{
				if (Item.Account == tempSetting.Account && Item.Boss == tempSetting.Boss)
				{
					Item.Value = tempSetting.Value;
					break;
				}
			}
			SettingString = ConvirtListToString(Setting);
		}

		public string ConvirtListToString(List<SettingValue> arrayValues)
		{
			string localValue = "";
			foreach (SettingValue Item in arrayValues)
			{
				localValue += $"{Item.Account}:{Item.Boss}:{Item.Value};";
			}
			return localValue;
		}

		public static SettingValue GetSettingForBoss(List<SettingValue> settingList, string account, Boss boss)
		{
			foreach (SettingValue Item in settingList)
			{
				if (Item.Account == account && Item.Boss == boss)
				{
					return Item;
				}
			}
			return null;
		}

		public List<SettingValue> GetSettingForAccount(string account)
		{
			List<SettingValue> localSetting = new List<SettingValue>();
			foreach (SettingValue Item in Setting)
			{
				if (Item.Account == account)
				{
					localSetting.Add(Item);
				}
			}
			return localSetting;
		}

		protected void NotifyPropertyChanged(string propertyName = "")
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void AddNewAccount(string accountName)
		{
			Boss[] array = (Boss[])Enum.GetValues(typeof(Boss));
			foreach (Boss boss in array)
			{
				Setting.Add(new SettingValue(accountName, boss, value: false));
			}
			SettingString = ConvirtListToString(Setting);
		}

		public HashSet<string> GetAllAccounts()
		{
			HashSet<string> accounts = new HashSet<string>();
			foreach (SettingValue Item in Setting)
			{
				accounts.Add(Item.Account);
			}
			return accounts;
		}

		public void ResetAllValues()
		{
			foreach (SettingValue item in Setting)
			{
				item.Value = false;
			}
			SettingString = ConvirtListToString(Setting);
		}
	}
}
