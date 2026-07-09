using System;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace rp.spark
{
	internal static class SparkUiThread
	{
		private sealed class LogContext
		{
		}

		private static readonly Logger Logger = Logger.GetLogger<LogContext>();

		public static void Queue(Action action)
		{
			if (action == null)
			{
				return;
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "SPARK main-thread UI update failed.");
				}
			});
		}
	}
}
