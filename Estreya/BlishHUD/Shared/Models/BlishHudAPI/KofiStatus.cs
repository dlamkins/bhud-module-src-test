using System;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.BlishHudAPI
{
	public class KofiStatus
	{
		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("lastPayment")]
		public DateTimeOffset? LastPayment { get; set; }
	}
}
