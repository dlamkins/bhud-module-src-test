using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WvWPipTally.Models;

namespace WvWPipTally.Services
{
	public static class ScanService
	{
		public static string DebugDirectory { get; set; }

		public static async Task<ScanResult> ScanMatchOverviewAsync()
		{
			try
			{
				EnsureDebugDirectory();
				ClearStaleDebugFiles();
				using Bitmap capture = GameWindowCapture.CaptureGw2Client();
				SaveDebugImage(capture, "full");
				string text = await WindowsOcrService.RecognizeAsync(capture).ConfigureAwait(continueOnCapturedContext: false);
				SaveDebugText("full", text);
				return RewardTrackParser.Parse(text);
			}
			catch (Exception ex)
			{
				return new ScanResult
				{
					Success = false,
					Message = "Scan failed: " + ex.Message,
					RawText = string.Empty
				};
			}
		}

		private static void EnsureDebugDirectory()
		{
			if (!string.IsNullOrWhiteSpace(DebugDirectory))
			{
				Directory.CreateDirectory(DebugDirectory);
			}
		}

		private static void ClearStaleDebugFiles()
		{
			if (string.IsNullOrWhiteSpace(DebugDirectory) || !Directory.Exists(DebugDirectory))
			{
				return;
			}
			try
			{
				string[] files = Directory.GetFiles(DebugDirectory, "scan-*");
				foreach (string path in files)
				{
					string name = Path.GetFileName(path);
					if (name.StartsWith("scan-crop", StringComparison.OrdinalIgnoreCase) || name.StartsWith("scan-rewards", StringComparison.OrdinalIgnoreCase) || name.StartsWith("scan-last-", StringComparison.OrdinalIgnoreCase))
					{
						File.Delete(path);
					}
				}
			}
			catch
			{
			}
		}

		private static void SaveDebugImage(Bitmap bitmap, string label)
		{
			if (!string.IsNullOrWhiteSpace(DebugDirectory) && bitmap != null)
			{
				try
				{
					bitmap.Save(Path.Combine(DebugDirectory, "scan-" + label + ".png"), ImageFormat.Png);
				}
				catch
				{
				}
			}
		}

		private static void SaveDebugText(string label, string text)
		{
			if (!string.IsNullOrWhiteSpace(DebugDirectory))
			{
				try
				{
					File.WriteAllText(Path.Combine(DebugDirectory, "scan-" + label + ".txt"), text ?? string.Empty, Encoding.UTF8);
				}
				catch
				{
				}
			}
		}
	}
}
