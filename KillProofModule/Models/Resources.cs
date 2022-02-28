using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class Resources
	{
		[JsonProperty("general_tokens")]
		public IList<Token> GeneralTokens { get; set; }

		[JsonProperty("fractals")]
		public IReadOnlyList<Token> Fractals { get; set; }

		[JsonProperty("raids")]
		public IReadOnlyList<Raid> Raids { get; set; }

		public Wing GetWing(int index)
		{
			Wing[] allWings = GetAllWings().ToArray();
			for (int i = 0; i < allWings.Count(); i++)
			{
				if (i == index)
				{
					return allWings[i];
				}
			}
			return null;
		}

		public Wing GetWing(string id)
		{
			if (Regex.IsMatch(id, "^[Ww]\\d+$"))
			{
				return GetWing(int.Parse(string.Join(string.Empty, from m in Regex.Matches(id, "\\d+$").OfType<Match>()
					select m.Value)) - 1);
			}
			return GetAllWings().FirstOrDefault((Wing wing) => wing.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
		}

		public Wing GetWing(Token token)
		{
			return GetAllWings().FirstOrDefault((Wing wing) => wing.Events.Where((Event encounter) => encounter?.Token != null).Any((Event encounters) => encounters.Token.Id.Equals(token.Id)));
		}

		public IEnumerable<Wing> GetAllWings()
		{
			return from wing in Raids.Where((Raid raid) => raid != null).SelectMany((Raid raid) => raid.Wings)
				where wing != null
				select wing;
		}

		private IEnumerable<Event> GetAllEvents()
		{
			return (from wing in GetAllWings()
				where wing.Events != null
				select wing).SelectMany((Wing wing) => wing.Events);
		}

		public IEnumerable<Token> GetAllTokens()
		{
			return GeneralTokens.Concat(from fractal in Fractals
				where fractal != null
				select (fractal)).Concat(from encounter in GetAllEvents()
				where encounter.Token != null
				select encounter.Token);
		}

		public Token GetToken(int id)
		{
			return GetAllTokens().FirstOrDefault((Token token) => token.Id == id);
		}

		public Token GetToken(string name)
		{
			name = name.Split('|').Reverse().ToList()[0].Trim();
			return (from token in GetAllTokens()
				where token.Name != null
				select token).FirstOrDefault((Token token) => token.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private IEnumerable<Miniature> GetAllMiniatures()
		{
			return GetAllEvents().SelectMany((Event encounter) => encounter.Miniatures);
		}

		public Event GetEvent(string id)
		{
			return GetAllEvents().FirstOrDefault((Event encounter) => encounter.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
