using System.IO;
using Charr.Timers_BlishHUD.Pathing.Content;

namespace Charr.Timers_BlishHUD.IO
{
	public class TimerStream
	{
		public Stream Stream { get; set; }

		public PathableResourceManager ResourceManager { get; set; }

		public string FileName { get; set; }

		public bool IsFromZip { get; set; }

		public string ZipFile { get; set; }

		public TimerStream(Stream stream, PathableResourceManager resourceManager, string fileName, bool isFromZip = false, string zipFile = "")
		{
			Stream = stream;
			ResourceManager = resourceManager;
			FileName = fileName;
			IsFromZip = isFromZip;
			ZipFile = zipFile;
		}
	}
}
