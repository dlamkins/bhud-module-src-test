using System;
using System.Threading.Tasks;
using Blish_HUD.Settings;

namespace Nekres.Stream_Out.Core.Services
{
	internal abstract class ExportService : IDisposable
	{
		private DateTime _prevApiRequestTime;

		private SettingEntry<DateTime> _nextResetTimeDaily;

		private SettingEntry<DateTime> _nextResetTimeWeekly;

		private SettingEntry<DateTime> _lastResetTimeDaily;

		private SettingEntry<DateTime> _lastResetTimeWeekly;

		protected ExportService(SettingCollection settings)
		{
			_prevApiRequestTime = DateTime.UtcNow;
			_nextResetTimeDaily = settings.DefineSetting<DateTime>(GetType().Name + "_nextResetDaily", DateTime.UtcNow.AddSeconds(1.0), (Func<string>)null, (Func<string>)null);
			_nextResetTimeWeekly = settings.DefineSetting<DateTime>(GetType().Name + "_nextResetWeekly", DateTime.UtcNow.AddSeconds(1.0), (Func<string>)null, (Func<string>)null);
			_lastResetTimeDaily = settings.DefineSetting<DateTime>(GetType().Name + "_lastResetDaily", DateTime.UtcNow, (Func<string>)null, (Func<string>)null);
			_lastResetTimeWeekly = settings.DefineSetting<DateTime>(GetType().Name + "_lastResetWeekly", DateTime.UtcNow, (Func<string>)null, (Func<string>)null);
		}

		public async Task DoUpdate()
		{
			if (!(DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 300.0))
			{
				_prevApiRequestTime = DateTime.UtcNow;
				await DoResetDaily();
				await DoResetWeekly();
				await Update();
			}
		}

		private async Task DoResetDaily()
		{
			if (!(_lastResetTimeDaily.get_Value() < _nextResetTimeDaily.get_Value()) && await ResetDaily())
			{
				_lastResetTimeDaily.set_Value(DateTime.UtcNow);
				_nextResetTimeDaily.set_Value(Gw2Util.GetDailyResetTime());
			}
		}

		private async Task DoResetWeekly()
		{
			if (!(_lastResetTimeWeekly.get_Value() < _nextResetTimeWeekly.get_Value()) && await ResetWeekly())
			{
				_lastResetTimeWeekly.set_Value(DateTime.UtcNow);
				_nextResetTimeWeekly.set_Value(Gw2Util.GetWeeklyResetTime());
			}
		}

		public virtual async Task Initialize()
		{
		}

		protected virtual async Task Update()
		{
		}

		protected virtual async Task<bool> ResetDaily()
		{
			return true;
		}

		protected virtual async Task<bool> ResetWeekly()
		{
			return true;
		}

		public abstract Task Clear();

		public abstract void Dispose();
	}
}
