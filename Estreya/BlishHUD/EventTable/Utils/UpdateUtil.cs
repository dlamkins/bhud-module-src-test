using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Utils
{
	public static class UpdateUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(UpdateUtil));

		private static readonly HashSet<IntPtr> _asyncStateMonitor = new HashSet<IntPtr>();

		public static void Update(Action<GameTime> call, GameTime gameTime, double interval, ref double lastCheck)
		{
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= interval)
			{
				call(gameTime);
				lastCheck = 0.0;
			}
		}

		public static Task UpdateAsync(Func<GameTime, Task> call, GameTime gameTime, double interval, ref double lastCheck)
		{
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= interval)
			{
				lock (_asyncStateMonitor)
				{
					if (_asyncStateMonitor.Contains(call.Method.MethodHandle.Value))
					{
						Logger.Debug("Async " + call.Method.Name + " has skipped its cadence because it has not completed running.");
						return Task.CompletedTask;
					}
					_asyncStateMonitor.Add(call.Method.MethodHandle.Value);
				}
				Task result = call(gameTime).ContinueWith(delegate
				{
					lock (_asyncStateMonitor)
					{
						_asyncStateMonitor.Remove(call.Method.MethodHandle.Value);
					}
				});
				lastCheck = 0.0;
				return result;
			}
			return Task.CompletedTask;
		}
	}
}
