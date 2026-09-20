using System;

namespace Quarry.Models.Markers
{
	public class PackCacheEntry
	{
		public string FileName { get; set; }

		public long Length { get; set; }

		public DateTime LastWriteTimeUtc { get; set; }
	}
}
