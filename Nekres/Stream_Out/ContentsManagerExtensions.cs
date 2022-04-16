using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD.Modules.Managers;

namespace Nekres.Stream_Out
{
	internal static class ContentsManagerExtensions
	{
		public static void ExtractIcons(this ContentsManager contentsManager, string archiveFilePath, string outFilePath)
		{
			if (File.Exists(outFilePath))
			{
				return;
			}
			Directory.CreateDirectory(Path.GetDirectoryName(outFilePath) ?? string.Empty);
			using Stream texStr = contentsManager.GetFileStream(archiveFilePath);
			using Bitmap icon = new Bitmap(texStr);
			icon.Save(outFilePath, ImageFormat.Png);
		}
	}
}
