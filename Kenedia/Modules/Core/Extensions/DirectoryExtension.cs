using System;
using System.IO;

namespace Kenedia.Modules.Core.Extensions
{
	public static class DirectoryExtension
	{
		public static void MoveFiles(string sourceDirectory, string targetDirectory)
		{
			try
			{
				string[] files = Directory.GetFiles(sourceDirectory);
				foreach (string obj in files)
				{
					string fileName = Path.GetFileName(obj);
					string targetPath = Path.Combine(targetDirectory, fileName);
					File.Copy(obj, targetPath, overwrite: true);
					File.Delete(obj);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
