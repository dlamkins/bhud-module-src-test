using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class GuessUser
	{
		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("PuzzleGuess")]
		public Guess PuzzleGuess { get; set; } = new Guess();

	}
}
