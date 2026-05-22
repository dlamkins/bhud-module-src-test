using System;
using System.Threading.Tasks;
using Blish_HUD;

namespace TyriaPlanner.Hud.Ui
{
	public static class Clipboard
	{
		public static void Set(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			try
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text).ContinueWith(delegate(Task<bool> t)
				{
					if (t.IsFaulted)
					{
						Logger.GetLogger(typeof(Clipboard)).Warn((Exception)t.Exception, "Clipboard write failed.");
					}
				}, TaskScheduler.Default);
			}
			catch (Exception ex)
			{
				Logger.GetLogger(typeof(Clipboard)).Warn(ex, "Clipboard write threw.");
			}
		}

		public static async Task SetAndRestoreAsync(string text, TimeSpan restoreAfter)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string previous = string.Empty;
				try
				{
					previous = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch
				{
				}
				try
				{
					await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch
				{
				}
				try
				{
					await Task.Delay(restoreAfter).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch
				{
				}
				try
				{
					await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(previous ?? string.Empty).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch
				{
				}
			}
		}
	}
}
