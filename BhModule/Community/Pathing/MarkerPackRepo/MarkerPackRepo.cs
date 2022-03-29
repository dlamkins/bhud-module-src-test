using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Flurl.Http;

namespace BhModule.Community.Pathing.MarkerPackRepo
{
	public class MarkerPackRepo
	{
		private static readonly Logger Logger = Logger.GetLogger<MarkerPackRepo>();

		private const string REPO_SETTING = "PackRepoUrl";

		private const string PUBLIC_REPOURL = "https://mp-repo.blishhud.com/repo.json";

		private string _repoUrl;

		public MarkerPackPkg[] MarkerPackages { get; private set; } = Array.Empty<MarkerPackPkg>();


		public void Init()
		{
			Thread thread = new Thread((ThreadStart)async delegate
			{
				await Load(PathingModule.Instance.GetModuleProgressHandler());
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private async Task Load(IProgress<string> progress)
		{
			DefineRepoSettings();
			MarkerPackages = await LoadMarkerPackPkgs(progress);
			Logger.Info($"Found {MarkerPackages.Length} marker packs from {_repoUrl}.");
			LoadLocalPackInfo();
			progress.Report(null);
		}

		private void DefineRepoSettings()
		{
			_repoUrl = PathingModule.Instance.SettingsManager.get_ModuleSettings().DefineSetting<string>("PackRepoUrl", "https://mp-repo.blishhud.com/repo.json", (Func<string>)null, (Func<string>)null).get_Value();
		}

		private void LoadLocalPackInfo()
		{
			string fullDirectoryPath = PathingModule.Instance.DirectoriesManager.GetFullDirectoryPath("markers");
			string[] existingTacoPacks = Directory.GetFiles(fullDirectoryPath, "*.taco", SearchOption.AllDirectories);
			string[] existingZipPacks = Directory.GetFiles(fullDirectoryPath, "*.zip", SearchOption.AllDirectories);
			string[] existingPacks = existingTacoPacks.Concat(existingZipPacks).ToArray();
			MarkerPackPkg[] markerPackages = MarkerPackages;
			foreach (MarkerPackPkg pack in markerPackages)
			{
				string basePackName = Path.GetFileNameWithoutExtension(pack.FileName).ToLowerInvariant();
				string associatedLocalPack = null;
				string[] array = existingPacks;
				foreach (string existingFile in array)
				{
					if (existingFile.ToLowerInvariant().Contains(basePackName))
					{
						associatedLocalPack = existingFile;
						break;
					}
				}
				if (associatedLocalPack != null)
				{
					pack.CurrentDownloadDate = File.GetLastWriteTimeUtc(associatedLocalPack);
					if (pack.CurrentDownloadDate != default(DateTime) && pack.LastUpdate > pack.CurrentDownloadDate)
					{
						PackHandlingUtil.DownloadPack(pack, OnUpdateComplete);
					}
				}
			}
		}

		private static void OnUpdateComplete(MarkerPackPkg markerPackPkg, bool success)
		{
			markerPackPkg.IsDownloading = false;
			if (success)
			{
				markerPackPkg.CurrentDownloadDate = DateTime.UtcNow;
			}
		}

		private async Task<MarkerPackPkg[]> LoadMarkerPackPkgs(IProgress<string> progress)
		{
			progress.Report("Requesting latest list of marker packs...");
			var (releases, exception) = await RequestMarkerPacks();
			if (exception != null)
			{
				progress.Report("Failed to get a list of marker packs.\r\n" + exception.Message);
			}
			return releases;
		}

		private async Task<(MarkerPackPkg[] Releases, Exception Exception)> RequestMarkerPacks()
		{
			try
			{
				return (await GeneratedExtensions.GetJsonAsync<MarkerPackPkg[]>(GeneratedExtensions.WithHeader(_repoUrl, "User-Agent", (object)"Blish-HUD"), default(CancellationToken), (HttpCompletionOption)0), null);
			}
			catch (FlurlHttpException val)
			{
				FlurlHttpException ex = val;
				Logger.Warn((Exception)(object)ex, "Failed to get list of marker packs");
				return (Array.Empty<MarkerPackPkg>(), (Exception)(object)ex);
			}
		}
	}
}
