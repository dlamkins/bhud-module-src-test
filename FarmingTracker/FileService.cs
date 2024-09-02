using System.IO;
using Blish_HUD.Modules.Managers;

namespace FarmingTracker
{
	public class FileService
	{
		public static string GetModelFilePath(string moduleFolderPath)
		{
			return Path.Combine(moduleFolderPath, "model.json");
		}

		public static string GetModuleFolderPath(DirectoriesManager directoriesManager)
		{
			string moduleFolderName = directoriesManager.get_RegisteredDirectories()[0];
			return directoriesManager.GetFullDirectoryPath(moduleFolderName);
		}
	}
}
