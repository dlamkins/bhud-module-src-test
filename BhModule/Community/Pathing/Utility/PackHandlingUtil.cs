using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.MarkerPackRepo;
using Blish_HUD;
using Flurl.Http;

namespace BhModule.Community.Pathing.Utility
{
	public static class PackHandlingUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(PackHandlingUtil));

		private const string DOWNLOAD_UA = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		public static void DownloadPack(MarkerPackPkg markerPackPkg, Action<MarkerPackPkg, bool> funcOnComplete)
		{
			Thread thread = new Thread((ThreadStart)async delegate
			{
				await BeginPackDownload(markerPackPkg, PathingModule.Instance.GetModuleProgressHandler(), funcOnComplete);
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private static async Task BeginPackDownload(MarkerPackPkg markerPackPkg, IProgress<string> progress, Action<MarkerPackPkg, bool> funcOnComplete)
		{
			Logger.Info("Updating pack '" + markerPackPkg.Name + "'...");
			progress.Report("Updating pack '" + markerPackPkg.Name + "'...");
			markerPackPkg.IsDownloading = true;
			string tempPackDownloadDestination = Path.GetTempFileName();
			try
			{
				File.Delete(tempPackDownloadDestination);
				await DownloadExtensions.DownloadFileAsync(GeneratedExtensions.WithHeader(markerPackPkg.Download, "user-agent", (object)"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"), Path.GetDirectoryName(tempPackDownloadDestination), Path.GetFileName(tempPackDownloadDestination), 4096, default(CancellationToken));
			}
			catch (Exception ex2)
			{
				Logger.Error(ex2, "Failed to update marker pack " + markerPackPkg.Name + " from " + markerPackPkg.Download + " to " + tempPackDownloadDestination + ".");
				funcOnComplete(markerPackPkg, arg2: false);
				return;
			}
			progress.Report("Finalizing new pack download...");
			string finalPath = Path.Combine(DataDirUtil.MarkerDir, markerPackPkg.FileName);
			try
			{
				bool needsInit = true;
				if (File.Exists(finalPath))
				{
					needsInit = false;
					while (PathingModule.Instance.PackInitiator.IsLoading)
					{
						Thread.Sleep(1000);
					}
					File.Delete(finalPath);
				}
				File.Move(tempPackDownloadDestination, finalPath);
				if (needsInit)
				{
					await PathingModule.Instance.PackInitiator.LoadPackedPackFiles(new string[1] { finalPath });
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed moving marker pack " + markerPackPkg.Name + " from " + tempPackDownloadDestination + " to " + finalPath + ".");
				funcOnComplete(markerPackPkg, arg2: false);
				return;
			}
			progress.Report(string.Empty);
			funcOnComplete(markerPackPkg, arg2: true);
		}
	}
}
