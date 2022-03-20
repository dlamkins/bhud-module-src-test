using System.IO;

namespace BhModule.Community.Pathing.Utility
{
	public static class DataDirUtil
	{
		public const string COMMON_STATE = "states";

		public const string COMMON_USER = "user";

		private const string MARKER_DIR = "markers";

		private const string DATA_DIR = "data";

		public static string MarkerDir => PathingModule.Instance.DirectoriesManager.GetFullDirectoryPath("markers");

		public static string GetSafeDataDir(string dir)
		{
			string combined = Path.Combine(MarkerDir, "data", dir);
			if (!Directory.Exists(combined))
			{
				Directory.CreateDirectory(combined);
			}
			return combined;
		}
	}
}
