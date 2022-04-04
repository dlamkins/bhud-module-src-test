using System;
using Blish_HUD.Settings;

namespace BhModule.Community.Pathing.MarkerPackRepo
{
	public class MarkerPackPkg
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Download { get; set; }

		public string Info { get; set; }

		public string FileName { get; set; }

		public string Categories { get; set; }

		public string Version { get; set; }

		public float Size { get; set; }

		public int TotalDownloads { get; set; }

		public string AuthorName { get; set; }

		public string AuthorUsername { get; set; }

		public DateTime LastUpdate { get; set; }

		public DateTime CurrentDownloadDate { get; set; }

		public bool IsDownloading { get; set; }

		public string DownloadError { get; set; }

		public int DownloadProgress { get; set; }

		public SettingEntry<bool> AutoUpdate { get; set; }
	}
}
