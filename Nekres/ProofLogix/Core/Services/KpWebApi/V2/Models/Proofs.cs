using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public class Proofs : BaseResponse
	{
		public bool IsEmpty
		{
			get
			{
				if (!GetTokens().Any() || GetTokens().All((Token t) => t.Amount == 0))
				{
					List<Title> titles = Titles;
					if (titles == null)
					{
						return true;
					}
					return !titles.Any();
				}
				return false;
			}
		}

		[JsonProperty("tokens")]
		public List<Token> Tokens { get; set; }

		[JsonProperty("killproofs")]
		public List<Token> Killproofs { get; set; }

		[JsonProperty("coffers")]
		public List<Token> Coffers { get; set; }

		[JsonProperty("titles")]
		public List<Title> Titles { get; set; }

		public Token GetToken(int id)
		{
			return Tokens?.FirstOrDefault((Token x) => x.Id == id) ?? Killproofs?.FirstOrDefault((Token x) => x.Id == id) ?? Coffers?.FirstOrDefault((Token x) => x.Id == id) ?? Token.Empty;
		}

		public IEnumerable<Token> GetTokens(bool excludeCoffers = false)
		{
			IEnumerable<Token> tokens = Enumerable.Empty<Token>();
			if (Tokens != null)
			{
				tokens = tokens.Concat(Tokens);
			}
			if (Killproofs != null)
			{
				tokens = tokens.Concat(Killproofs);
			}
			if (Coffers != null && !excludeCoffers)
			{
				tokens = tokens.Concat(Coffers);
			}
			return from token in tokens
				group token by token.Id into @group
				select @group.First();
		}
	}
}
