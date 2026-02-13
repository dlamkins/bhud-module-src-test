using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using CinemaModule.UI.Windows.Info;

namespace CinemaModule.Settings
{
	public class CinemaSettings
	{
		private ThirdPartyNoticesWindow _thirdPartyNoticesWindow;

		private bool _isShowingNotices;

		public SettingEntry<bool> EnabledSetting { get; }

		public bool IsEnabled => EnabledSetting.get_Value();

		public CinemaSettings(SettingCollection settings)
		{
			EnabledSetting = settings.DefineSetting<bool>("CinemaEnabled", false, (Func<string>)(() => "Enable Cinema"), (Func<string>)(() => "Toggle the CinemaHUD video player on or off"));
			settings.DefineSetting<bool>("ShowThirdPartyNotices", false, (Func<string>)(() => "Third-Party Notices"), (Func<string>)(() => "View third-party software notices and licenses")).add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue() && !_isShowingNotices)
				{
					_isShowingNotices = true;
					ShowThirdPartyNotices();
					_isShowingNotices = false;
				}
				else if (!e.get_NewValue() && _isShowingNotices)
				{
					_isShowingNotices = false;
				}
			});
		}

		private void ShowThirdPartyNotices()
		{
			if (_thirdPartyNoticesWindow == null)
			{
				_thirdPartyNoticesWindow = new ThirdPartyNoticesWindow();
			}
			((Control)_thirdPartyNoticesWindow).Show();
		}

		public void Dispose()
		{
			ThirdPartyNoticesWindow thirdPartyNoticesWindow = _thirdPartyNoticesWindow;
			if (thirdPartyNoticesWindow != null)
			{
				((Control)thirdPartyNoticesWindow).Dispose();
			}
		}
	}
}
