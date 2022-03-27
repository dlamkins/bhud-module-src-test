using System.IO;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;

namespace Nekres.Inquest_Module
{
	internal static class ContentsManagerExtensions
	{
		public static async Task ExtractFile(this ContentsManager contentsManager, string outDir, string refFilePath)
		{
			string fullPath = Path.Combine(outDir, refFilePath);
			if (!File.Exists(fullPath))
			{
				using Stream fs = contentsManager.GetFileStream(refFilePath);
				fs.Position = 0L;
				byte[] buffer = new byte[fs.Length];
				await fs.ReadAsync(buffer, 0, (int)fs.Length);
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
				File.WriteAllBytes(fullPath, buffer);
			}
		}
	}
}
