using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace rp.spark.UI.Views
{
	internal static class SparkUiActions
	{
		private sealed class LogContext
		{
		}

		private static readonly Logger Logger = Logger.GetLogger<LogContext>();

		public static void BindClick(StandardButton button, Func<Task> action, Action<string> setStatus = null, string failureStatus = "Action failed.")
		{
			if (button != null)
			{
				((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					await RunAsync(button, action, setStatus, failureStatus);
				});
			}
		}

		private static async Task RunAsync(StandardButton button, Func<Task> action, Action<string> setStatus, string failureStatus)
		{
			if (button == null || action == null || !((Control)button).get_Enabled())
			{
				return;
			}
			bool wasEnabled = ((Control)button).get_Enabled();
			((Control)button).set_Enabled(false);
			string errorStatus = null;
			try
			{
				await action();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "SPARK UI action failed.");
				errorStatus = failureStatus;
			}
			finally
			{
				SparkUiThread.Queue(delegate
				{
					if (!string.IsNullOrWhiteSpace(errorStatus))
					{
						setStatus?.Invoke(errorStatus);
					}
					if (((Control)button).get_Parent() != null)
					{
						((Control)button).set_Enabled(wasEnabled);
					}
				});
			}
		}
	}
}
