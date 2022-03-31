using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.MarkerPackRepo;
using Blish_HUD;

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
			progress.Report("Downloading pack '" + markerPackPkg.Name + "'...");
			markerPackPkg.IsDownloading = true;
			markerPackPkg.DownloadError = null;
			string tempPackDownloadDestination = Path.GetTempFileName();
			try
			{
				File.Delete(tempPackDownloadDestination);
				using WebClient webClient = new WebClient();
				webClient.Headers.Add("user-agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
				webClient.DownloadProgressChanged += delegate(object s, DownloadProgressChangedEventArgs e)
				{
					markerPackPkg.DownloadProgress = e.ProgressPercentage;
				};
				await webClient.DownloadFileTaskAsync(markerPackPkg.Download, tempPackDownloadDestination);
			}
			catch (Exception ex3)
			{
				markerPackPkg.DownloadError = "Marker pack download failed.";
				Logger.Error(ex3, "Failed to download marker pack " + markerPackPkg.Name + " from " + markerPackPkg.Download + " to " + tempPackDownloadDestination + ".");
				funcOnComplete(markerPackPkg, arg2: false);
				return;
			}
			progress.Report("Finalizing new pack download...");
			string finalPath = Path.Combine(DataDirUtil.MarkerDir, markerPackPkg.FileName);
			try
			{
				bool needsInit = true;
				using (FileStream packStream = File.Open(tempPackDownloadDestination, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					if (new ZipArchive((Stream)packStream).get_Entries().Count <= 0)
					{
						throw new InvalidDataException();
					}
				}
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
			catch (InvalidDataException ex2)
			{
				markerPackPkg.DownloadError = "Marker pack download is corrupt.";
				Logger.Warn((Exception)ex2, "Failed downloading marker pack " + markerPackPkg.Name + " from " + tempPackDownloadDestination + " (it appears to be corrupt).");
			}
			catch (Exception ex)
			{
				markerPackPkg.DownloadError = "Failed to import the new marker pack.";
				Logger.Warn(ex, "Failed moving marker pack " + markerPackPkg.Name + " from " + tempPackDownloadDestination + " to " + finalPath + ".");
			}
			progress.Report(string.Empty);
			funcOnComplete(markerPackPkg, markerPackPkg.DownloadError == null);
		}
	}
}
