using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Presenter;
using Blish_HUD;
using Flurl.Http;

namespace BhModule.Community.Pathing.Utility
{
	public static class PackHandlingUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(PackHandlingUtil));

		private const string DOWNLOAD_UA = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		public static void DownloadPack(PackRepoPresenter.MarkerPackPkg markerPackPkg)
		{
			Thread thread = new Thread((ThreadStart)async delegate
			{
				await BeginPackDownload(markerPackPkg, PathingModule.Instance.GetModuleProgressHandler());
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private static async Task BeginPackDownload(PackRepoPresenter.MarkerPackPkg markerPackPkg, IProgress<string> progress)
		{
			progress.Report("Downloading pack '" + markerPackPkg.Name + "'...");
			string tempPackDownloadDestination = Path.GetTempFileName();
			try
			{
				File.Delete(tempPackDownloadDestination);
				await DownloadExtensions.DownloadFileAsync(GeneratedExtensions.WithHeader(markerPackPkg.Download, "user-agent", (object)"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"), Path.GetDirectoryName(tempPackDownloadDestination), Path.GetFileName(tempPackDownloadDestination), 4096, default(CancellationToken));
			}
			catch (Exception ex2)
			{
				Logger.Error(ex2, "Failed to download marker pack " + markerPackPkg.Name + " from " + markerPackPkg.Download + " to " + tempPackDownloadDestination + ".");
				return;
			}
			progress.Report("Finalizing new pack download...");
			string finalPath = Path.Combine(DataDirUtil.MarkerDir, markerPackPkg.FileName);
			try
			{
				if (File.Exists(finalPath))
				{
					File.Delete(finalPath);
				}
				File.Move(tempPackDownloadDestination, finalPath);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed moving marker pack " + markerPackPkg.Name + " from " + tempPackDownloadDestination + " to " + finalPath + ".");
				return;
			}
			progress.Report(string.Empty);
		}
	}
}
