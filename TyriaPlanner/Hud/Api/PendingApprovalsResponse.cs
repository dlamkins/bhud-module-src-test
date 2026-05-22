using System;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class PendingApprovalsResponse
	{
		[JsonProperty("pending")]
		public PendingApproval[] Pending { get; set; } = Array.Empty<PendingApproval>();


		[JsonProperty("serverTime")]
		public DateTime ServerTime { get; set; }
	}
}
