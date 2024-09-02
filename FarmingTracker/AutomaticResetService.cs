using System;
using Blish_HUD;

namespace FarmingTracker
{
	public class AutomaticResetService : IDisposable
	{
		private readonly SettingService _settingsService;

		private bool _isModuleStart = true;

		public AutomaticResetService(Services services)
		{
			_settingsService = services.SettingService;
			services.SettingService.AutomaticResetSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<AutomaticReset>>)OnAutomaticResetSettingChanged);
			InitializeNextResetDateTimeIfNecessary();
		}

		public void Dispose()
		{
			_settingsService.AutomaticResetSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AutomaticReset>>)OnAutomaticResetSettingChanged);
		}

		public bool HasToResetAutomatically()
		{
			bool isModuleStart = _isModuleStart;
			_isModuleStart = false;
			bool isPastResetDate = _settingsService.NextResetDateTimeUtcSetting.get_Value() < DateTimeService.UtcNow;
			int num = _settingsService.AutomaticResetSetting.get_Value() switch
			{
				AutomaticReset.Never => 0, 
				AutomaticReset.OnModuleStart => isModuleStart ? 1 : 0, 
				AutomaticReset.MinutesAfterModuleShutdown => (isModuleStart && isPastResetDate) ? 1 : 0, 
				_ => isPastResetDate ? 1 : 0, 
			};
			if (num != 0)
			{
				Module.Logger.Info("Automatic reset required because reset date has been exceeded. " + $"isModuleStart: {isModuleStart} | " + $"AutomaticResetSetting: {_settingsService.AutomaticResetSetting.get_Value()} | " + $"ResetDateTimeUtc {_settingsService.NextResetDateTimeUtcSetting.get_Value()}");
			}
			return (byte)num != 0;
		}

		public void UpdateNextResetDateTimeForMinutesUntilResetAfterModuleShutdown()
		{
			if (_settingsService.AutomaticResetSetting.get_Value() == AutomaticReset.MinutesAfterModuleShutdown)
			{
				UpdateNextResetDateTime();
			}
		}

		public void UpdateNextResetDateTime()
		{
			_settingsService.NextResetDateTimeUtcSetting.set_Value(NextAutomaticResetCalculator.GetNextResetDateTimeUtc(DateTimeService.UtcNow, _settingsService.AutomaticResetSetting.get_Value(), _settingsService.MinutesUntilResetAfterModuleShutdownSetting.get_Value()));
		}

		private void OnAutomaticResetSettingChanged(object sender, ValueChangedEventArgs<AutomaticReset> e)
		{
			UpdateNextResetDateTime();
		}

		private void InitializeNextResetDateTimeIfNecessary()
		{
			if (_settingsService.NextResetDateTimeUtcSetting.get_Value() == NextAutomaticResetCalculator.UNDEFINED_RESET_DATE_TIME)
			{
				UpdateNextResetDateTime();
			}
		}
	}
}
