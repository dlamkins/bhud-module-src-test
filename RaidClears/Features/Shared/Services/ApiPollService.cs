using System;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Enums;

namespace RaidClears.Features.Shared.Services
{
	public class ApiPollService : IDisposable
	{
		private const int BUFFER_MS = 50;

		private const int MINUTE_MS = 60000;

		private double _runningTimer = -20000.0;

		private double _timeoutValue;

		private readonly SettingEntry<ApiPollPeriod> _apiPollSetting;

		public event EventHandler<bool>? ApiPollingTrigger;

		public ApiPollService(SettingEntry<ApiPollPeriod> apiPollSetting)
		{
			_apiPollSetting = apiPollSetting;
			SetTimeoutValueInMinutes((int)_apiPollSetting.get_Value());
			_apiPollSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)OnSettingUpdate);
		}

		public void Dispose()
		{
			_apiPollSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)OnSettingUpdate);
		}

		public void Update(GameTime gameTime)
		{
			_runningTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_runningTimer >= _timeoutValue)
			{
				this.ApiPollingTrigger?.Invoke(this, e: true);
				_runningTimer = 0.0;
			}
		}

		public void Invoke()
		{
			_runningTimer = 0.0;
			this.ApiPollingTrigger?.Invoke(this, e: true);
		}

		private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod> e)
		{
			SetTimeoutValueInMinutes((int)e.get_NewValue());
		}

		private void SetTimeoutValueInMinutes(int minutes)
		{
			_timeoutValue = minutes * 60000 + 50;
		}
	}
}
