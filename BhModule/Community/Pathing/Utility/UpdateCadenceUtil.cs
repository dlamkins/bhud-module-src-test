using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility
{
	public static class UpdateCadenceUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(UpdateCadenceUtil));

		private static HashSet<IntPtr> _asyncStateMonitor = new HashSet<IntPtr>();

		public static void UpdateWithCadence(Action<GameTime> call, GameTime gameTime, double cadence, ref double lastCheck)
		{
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= cadence || lastCheck < 1.0)
			{
				call(gameTime);
				lastCheck = 1.0;
			}
		}

		public static void UpdateAsyncWithCadence(Func<GameTime, Task> call, GameTime gameTime, double cadence, ref double lastCheck)
		{
			lock (_asyncStateMonitor)
			{
				if (_asyncStateMonitor.Contains(call.Method.MethodHandle.Value))
				{
					Logger.Debug("Async " + call.Method.Name + " has skipped its cadence because it has not completed running.");
					return;
				}
			}
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= cadence || lastCheck < 1.0)
			{
				call(gameTime).ContinueWith(delegate
				{
					_asyncStateMonitor.Remove(call.Method.MethodHandle.Value);
				}, TaskContinuationOptions.NotOnFaulted);
				lastCheck = 1.0;
			}
		}
	}
}
