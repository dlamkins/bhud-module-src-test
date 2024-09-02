using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace FarmingTracker
{
	public class DateTimeService : IDisposable
	{
		private static DateTime _utcNowDebug = DateTime.UtcNow;

		private static bool _debugEnabled;

		private static readonly Stopwatch _stopWatch = new Stopwatch();

		private SettingEntry<bool> _debugDateTimeEnabledSetting;

		private SettingEntry<DateTime> _debugDateTimeValueSetting;

		public static DateTime UtcNow
		{
			get
			{
				if (!_debugEnabled)
				{
					return DateTime.UtcNow;
				}
				return _utcNowDebug + _stopWatch.Elapsed;
			}
			set
			{
				_utcNowDebug = value;
				_stopWatch.Restart();
			}
		}

		public DateTimeService(SettingCollection settings)
		{
			_stopWatch.Restart();
			DefineSettings(settings);
			_utcNowDebug = _debugDateTimeValueSetting.get_Value();
		}

		public void CreateDateTimeDebugPanel(Container parent)
		{
			DateTimeDebugPanelService.CreateDateTimeDebugPanel(parent, _debugDateTimeEnabledSetting, _debugDateTimeValueSetting);
		}

		private void DefineSettings(SettingCollection settings)
		{
			_debugDateTimeEnabledSetting = settings.DefineSetting<bool>("debug dateTime enabled", false, (Func<string>)(() => "use debug dateTime"), (Func<string>)(() => "Use debug dateTime instead of system time."));
			_debugDateTimeValueSetting = settings.DefineSetting<DateTime>("debug dateTime value", DateTime.UtcNow, (Func<string>)(() => "use debug dateTime"), (Func<string>)(() => "Use debug dateTime instead of system time."));
			_debugDateTimeEnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnDebugDateTimeEnabledSettingChanged);
			OnDebugDateTimeEnabledSettingChanged();
		}

		public void Dispose()
		{
			_debugDateTimeEnabledSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnDebugDateTimeEnabledSettingChanged);
		}

		private void OnDebugDateTimeEnabledSettingChanged(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_debugEnabled = _debugDateTimeEnabledSetting.get_Value();
		}
	}
}
