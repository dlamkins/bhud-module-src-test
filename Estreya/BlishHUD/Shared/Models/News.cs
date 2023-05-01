using System;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models
{
	public class News
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }

		[JsonProperty("important")]
		public bool Important { get; set; }

		[JsonProperty("asPoints")]
		public bool AsPoints { get; set; }

		[JsonProperty("content")]
		public string[] Content { get; set; }
	}
}
