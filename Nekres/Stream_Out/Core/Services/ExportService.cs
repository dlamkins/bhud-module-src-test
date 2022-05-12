using System;
using System.Threading.Tasks;

namespace Nekres.Stream_Out.Core.Services
{
	internal abstract class ExportService : IDisposable
	{
		private DateTime _prevApiRequestTime;

		protected ExportService()
		{
			_prevApiRequestTime = DateTime.UtcNow;
		}

		public async Task DoUpdate()
		{
			await DoResetDaily().ContinueWith((Func<Task, Task>)async delegate
			{
				if (!(DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 300.0))
				{
					_prevApiRequestTime = DateTime.UtcNow;
					await Update();
				}
			});
		}

		private async Task DoResetDaily()
		{
			if (!(DateTime.UtcNow < StreamOutModule.Instance.ResetTimeDaily.get_Value()))
			{
				StreamOutModule.Instance.ResetTimeDaily.set_Value(Gw2Util.GetDailyResetTime());
				await ResetDaily();
			}
		}

		public virtual async Task Initialize()
		{
		}

		protected virtual async Task Update()
		{
		}

		protected virtual async Task ResetDaily()
		{
		}

		public abstract Task Clear();

		public abstract void Dispose();
	}
}
