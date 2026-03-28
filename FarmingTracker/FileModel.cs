using System.Collections.Generic;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class FileModel
	{
		[JsonProperty("FileStats")]
		public List<FileStat> FileStats { get; set; } = new List<FileStat>();

	}
}
