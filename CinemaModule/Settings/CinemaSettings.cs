using System;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using CinemaModule.UI.Windows.Info;

namespace CinemaModule.Settings
{
	public class CinemaSettings : IDisposable
	{
		private ThirdPartyNoticesWindow _thirdPartyNoticesWindow;

		public SettingEntry<bool> EnabledSetting { get; }

		public bool IsEnabled => EnabledSetting.get_Value();

		public CinemaSettings(SettingCollection settings)
		{
			EnabledSetting = settings.DefineSetting<bool>("CinemaEnabled", false, (Func<string>)(() => "Enable Cinema"), (Func<string>)(() => "Toggle the CinemaHUD video player on or off"));
		}

		public void ShowThirdPartyNotices()
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
