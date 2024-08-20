using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;

namespace Blish_HUD.Extended
{
	public static class ContentsManagerExtensions
	{
		public static async Task Extract(this ContentsManager contentsManager, string refFilePath, string outFilePath, bool overwrite = true)
		{
			if (string.IsNullOrEmpty(refFilePath))
			{
				throw new ArgumentException("refFilePath cannot be empty.", "refFilePath");
			}
			if (string.IsNullOrEmpty(outFilePath))
			{
				throw new ArgumentException("outFilePath cannot be empty.", "outFilePath");
			}
			if (!overwrite && File.Exists(outFilePath))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(outFilePath));
				using Stream stream = contentsManager.GetFileStream(refFilePath);
				if (stream == null)
				{
					throw new FileNotFoundException("File not found: '" + refFilePath + "'");
				}
				stream.Position = 0L;
				using FileStream file = File.Create(outFilePath);
				file.Position = 0L;
				await stream.CopyToAsync(file);
			}
			catch (IOException e)
			{
				Logger.GetLogger<ContentsManager>().Warn((Exception)e, e.Message);
			}
		}
	}
}
