using System;
using System.Collections.Generic;
using System.Linq;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public class Profile : Proofs
	{
		public static Profile Empty = new Profile
		{
			NotFound = true
		};

		private Proofs _totals;

		[JsonIgnore]
		public bool NotFound { get; private init; }

		[JsonProperty("account_name")]
		public string Name { get; set; }

		[JsonProperty("valid_api_key")]
		public bool ValidApiKey { get; set; }

		[JsonProperty("proof_url")]
		public string ProofUrl { get; set; }

		[JsonProperty("last_refresh")]
		public DateTime LastRefresh { get; set; }

		[JsonProperty("next_refresh")]
		public DateTime NextRefresh { get; set; }

		[JsonProperty("next_refresh_seconds")]
		[JsonConverter(typeof(SecondsUntilDateTimeConverter))]
		[Obsolete("Use NextRefresh instead.", true)]
		public DateTime NextRefreshFromSeconds { get; set; }

		[JsonProperty("kpid")]
		public string Id { get; set; }

		[JsonProperty("original_uce")]
		public OriginalUce OriginalUce { get; set; }

		[JsonProperty("linked")]
		public List<Profile> Linked { get; set; }

		[JsonProperty("linked_totals")]
		public Proofs Totals
		{
			get
			{
				return _totals ?? this;
			}
			set
			{
				_totals = value;
			}
		}

		[JsonIgnore]
		public List<Clear> Clears { get; set; }

		public List<Profile> Accounts => Linked?.Prepend(this).ToList() ?? new List<Profile> { this };

		public new bool IsEmpty => Totals.IsEmpty;

		public bool BelongsTo(string accountName, out Profile linkedProfile)
		{
			linkedProfile = Accounts?.FirstOrDefault((Profile profile) => !string.IsNullOrEmpty(profile.Name) && profile.Name.Equals(accountName, StringComparison.InvariantCultureIgnoreCase));
			return !(linkedProfile ?? Empty).NotFound;
		}

		public override Token GetToken(int id)
		{
			return HandleOriginalUce(id, base.GetToken(id));
		}

		private Token HandleOriginalUce(int id, Token token)
		{
			if (id != 81743)
			{
				return token;
			}
			Token originalUce = OriginalUce ?? token;
			if (token.Amount <= originalUce.Amount)
			{
				return originalUce;
			}
			return token;
		}
	}
}
