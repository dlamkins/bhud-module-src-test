using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class KillProof
	{
		[JsonProperty("linked")]
		public IList<KillProof> Linked { get; set; }

		[JsonProperty("valid_api_key")]
		public bool ValidApiKey { get; set; }

		[JsonProperty("titles")]
		public IList<Title> Titles { get; set; }

		[JsonProperty("proof_url")]
		public string ProofUrl { get; set; }

		[JsonProperty("coffers")]
		public IList<Token> Coffers { get; set; }

		[JsonProperty("tokens")]
		public IList<Token> Tokens { get; set; }

		[JsonProperty("killproofs")]
		public IList<Token> Killproofs { get; set; }

		[JsonProperty("kpid")]
		public string KpId { get; set; }

		[JsonProperty("last_refresh")]
		public DateTime LastRefresh { get; set; }

		[JsonProperty("account_name")]
		public string AccountName { get; set; }

		[JsonProperty("error")]
		public string Error { get; set; }

		public Token GetToken(int id)
		{
			return GetAllTokens().FirstOrDefault((Token x) => x.Id == id);
		}

		public Token GetToken(string name)
		{
			name = name.Split('|').Reverse().ToList()[0].Trim();
			return GetAllTokens().FirstOrDefault((Token x) => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public IEnumerable<Token> GetAllTokens()
		{
			IEnumerable<Token> tokens = Tokens;
			IEnumerable<Token> first = tokens ?? Enumerable.Empty<Token>();
			tokens = Killproofs;
			IEnumerable<Token> killproofs = tokens ?? Enumerable.Empty<Token>();
			return first.Concat(killproofs);
		}
	}
}
