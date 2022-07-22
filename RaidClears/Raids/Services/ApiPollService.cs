using System;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings;

namespace RaidClears.Raids.Services
{
	public class ApiPollService : IDisposable
	{
		private static int BUFFER_MS = 50;

		private static int MINUTE_MS = 60000;

		private bool _running;

		private double _runningTimer;

		private double _timeoutValue;

		private readonly SettingEntry<ApiPollPeriod> _apiPollSetting;

		public ApiPollService(SettingEntry<ApiPollPeriod> apiPollSetting)
		{
			_apiPollSetting = apiPollSetting;
			_apiPollSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)OnSettingUpdate);
			SetTimeoutValueInMinutes((int)_apiPollSetting.get_Value());
		}

		public void Dispose()
		{
			_apiPollSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)OnSettingUpdate);
		}

		public void Update(GameTime gameTime)
		{
			if (_running)
			{
				_runningTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			}
		}

		private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod> e)
		{
			SetTimeoutValueInMinutes((int)e.get_NewValue());
		}

		private void SetTimeoutValueInMinutes(int minutes)
		{
			_timeoutValue = minutes * MINUTE_MS + BUFFER_MS;
		}
	}
}
