using System;
using System.Diagnostics;

namespace FarmingTracker
{
	public class FarmingDuration
	{
		private readonly SettingService _settingService;

		private readonly Stopwatch _stopwatch = new Stopwatch();

		private TimeSpan _value;

		public TimeSpan Elapsed
		{
			get
			{
				return _value + _stopwatch.Elapsed;
			}
			set
			{
				_value = value;
			}
		}

		public FarmingDuration(SettingService settingService)
		{
			_settingService = settingService;
			Elapsed = settingService.FarmingDurationTimeSpanSetting.get_Value();
			_stopwatch.Restart();
		}

		public void Restart()
		{
			_stopwatch.Restart();
			_value = TimeSpan.Zero;
			_settingService.FarmingDurationTimeSpanSetting.set_Value(TimeSpan.Zero);
		}

		public void SaveFarmingTime()
		{
			_settingService.FarmingDurationTimeSpanSetting.set_Value(Elapsed);
		}
	}
}
