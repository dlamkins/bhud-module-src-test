using System;
using System.Collections.Generic;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	public class Profile
	{
		public static Profile Empty = new Profile
		{
			IsEmpty = true
		};

		[JsonIgnore]
		public bool IsEmpty { get; private init; }

		[JsonProperty("account_name")]
		public string Name { get; set; }

		[JsonProperty("valid_api_key")]
		public bool ValidApiKey { get; set; }

		[JsonProperty("proof_url")]
		public string ProofUrl { get; set; }

		[JsonProperty("last_refresh")]
		public DateTime LastRefresh { get; set; }

		[JsonProperty("kpid")]
		public string Id { get; set; }

		[JsonProperty("tokens")]
		[JsonConverter(typeof(DictionaryConverter<Token>))]
		public List<Token> Tokens { get; set; }

		[JsonProperty("killproofs")]
		[JsonConverter(typeof(DictionaryConverter<Token>))]
		public List<Token> Killproofs { get; set; }

		[JsonProperty("titles")]
		[JsonConverter(typeof(DictionaryConverter<Title>))]
		public List<Title> Titles { get; set; }

		[JsonIgnore]
		public List<Clear> Clears { get; set; }
	}
}
