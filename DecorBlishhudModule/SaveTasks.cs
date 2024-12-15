using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;

namespace DecorBlishhudModule
{
	internal static class SaveTasks
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

		public static async Task FadePanel(Panel panel, float startOpacity, float endOpacity, int duration)
		{
			int steps = 30;
			float stepDuration = duration / steps;
			float opacityStep = (endOpacity - startOpacity) / (float)steps;
			for (int i = 0; i < steps; i++)
			{
				((Control)panel).set_Opacity(startOpacity + opacityStep * (float)i);
				await Task.Delay((int)stepDuration);
			}
			((Control)panel).set_Opacity(endOpacity);
		}

		public static async void ShowSavedPanel(Panel savedPanel)
		{
			((Control)savedPanel).set_Visible(true);
			await FadePanel(savedPanel, 0f, 1f, 100);
			await Task.Delay(100);
			await FadePanel(savedPanel, 1f, 0f, 200);
			((Control)savedPanel).set_Visible(false);
		}

		public static void CopyTextToClipboard(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					Clipboard.SetText(text);
					Logger.Info("Copied '" + text + "' to clipboard.");
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to copy text to clipboard. Error: " + ex.ToString());
				}
			}
		}
	}
}
