using System;
using System.IO;
using System.IO.Compression;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class ZipArchiveExtensions
	{
		public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
		{
			if (!overwrite)
			{
				archive.ExtractToDirectory(destinationDirectoryName);
				return;
			}
			string destinationDirectoryFullPath = Directory.CreateDirectory(destinationDirectoryName).FullName;
			foreach (ZipArchiveEntry file in archive.get_Entries())
			{
				string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.get_FullName()));
				if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
				{
					throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
				}
				if (file.get_Name() == "")
				{
					Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
				}
				else
				{
					file.ExtractToFile(completeFileName, overwrite: true);
				}
			}
		}
	}
}
