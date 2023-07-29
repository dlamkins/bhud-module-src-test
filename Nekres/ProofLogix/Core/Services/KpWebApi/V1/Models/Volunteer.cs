using System;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	public sealed class Volunteer
	{
		[JsonProperty("account_name")]
		public string AccountName { get; set; }

		[JsonProperty("updated")]
		public DateTime Updated { get; set; }
	}
}
