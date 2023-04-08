using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KpRefresher.Domain
{
	public class KpApiModel
	{
		[JsonProperty("kpid")]
		public string Id { get; set; }

		[JsonProperty("last_refresh")]
		public DateTime LastRefresh { get; set; }

		[JsonProperty("account_name")]
		public string AccountName { get; set; }

		[JsonProperty("killproofs")]
		public List<Killproof> Killproofs { get; set; }

		[JsonProperty("linked")]
		public List<KpApiModel> LinkedAccounts { get; set; }
	}
}
