using System;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Presenter;
using Blish_HUD;

namespace BhModule.Community.Pathing.Content
{
	public class MarkerPackDownloader
	{
		private static readonly Logger Logger = Logger.GetLogger<MarkerPackDownloader>();

		private readonly PackRepoPresenter.MarkerPackPkg _markerPackPkg;

		private readonly IProgress<string> _progress;

		public MarkerPackDownloader(PackRepoPresenter.MarkerPackPkg markerPackPkg, IProgress<string> progress)
		{
			_markerPackPkg = markerPackPkg;
			_progress = progress;
		}

		public void Start()
		{
			Thread thread = new Thread((ThreadStart)async delegate
			{
				await Download();
			});
			thread.IsBackground = true;
			thread.Start();
		}

		public async Task Download()
		{
		}
	}
}
