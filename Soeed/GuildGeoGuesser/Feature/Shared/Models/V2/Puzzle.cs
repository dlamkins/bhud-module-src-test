using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class Puzzle
	{
		[JsonProperty("PuzzleId")]
		public string Id { get; set; } = "";


		[JsonProperty("GuildId")]
		public string GuildId { get; set; } = "";


		[JsonProperty("Title")]
		public string Title { get; set; } = "";


		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("Image")]
		public string Image { get; set; } = "";


		[JsonProperty("Duration")]
		public int Duration { get; set; } = Service.Config.DefaultDurationMinutes;


		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;


		[JsonProperty("ExpiresAt")]
		public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(Service.Config.DefaultDurationMinutes);


		[JsonProperty("Location")]
		public Location Location { get; set; } = new Location();


		[JsonProperty("UpvoteCount")]
		public int Upvotes { get; set; }

		[JsonProperty("Upvoters")]
		public List<User> Votes { get; set; } = new List<User>();


		[JsonProperty("GuessCount")]
		public int GuessCount { get; set; }

		[JsonProperty("PuzzleGuesses")]
		public List<GuessUser> Guesses { get; set; } = new List<GuessUser>();


		public int ActualGuessCount => Guesses?.Count ?? 0;

		public bool IsAuthor(string name)
		{
			return AccountName == name;
		}

		public bool UserHasGuessed(string name)
		{
			string name2 = name;
			if (AccountName == name2)
			{
				return true;
			}
			return Guesses.Exists((GuessUser x) => x.AccountName == name2);
		}

		public GuessUser? GetUserGuess(string name)
		{
			string name2 = name;
			return Guesses.FirstOrDefault((GuessUser x) => x.AccountName == name2);
		}

		public bool HasUserUpvoted(string name)
		{
			string name2 = name;
			if (AccountName == name2)
			{
				return true;
			}
			return Votes.Exists((User x) => x.AccountName.Equals(name2));
		}

		public AsyncTexture2D GetImageTexture()
		{
			string localFileName = Image;
			return Service.Textures.GetURLTexture(Service.GeoServerWrapper.GetImageUrl(Id), localFileName);
		}

		public AsyncTexture2D GetSolutionMapTexture(string? userName = null)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			string localFileName = Id + "solution.png";
			if (string.IsNullOrEmpty(userName) || !UserHasGuessed(userName))
			{
				return Service.Textures.GetURLTexture(Service.GeoServerWrapper.GetPuzzleMapUrl(Id), localFileName);
			}
			GuessUser guess = GetUserGuess(userName);
			if (guess?.PuzzleGuess?.Location == null)
			{
				return Service.Textures.GetURLTexture(Service.GeoServerWrapper.GetPuzzleMapUrl(Id), localFileName);
			}
			Location guessLocation = guess.PuzzleGuess.Location;
			float[] array = new float[2];
			Coordinates2 mapCoord = guessLocation.MapCoord;
			array[0] = (float)((Coordinates2)(ref mapCoord)).get_X();
			mapCoord = guessLocation.MapCoord;
			array[1] = (float)((Coordinates2)(ref mapCoord)).get_Y();
			float[] guessCoords = array;
			return Service.Textures.GetURLTexture(Service.GeoServerWrapper.GetPuzzleMapWithGuessUrl(Id, guessLocation.MapId, guessCoords), localFileName);
		}

		public AsyncTexture2D GetAuthorSolutionMapTexture(string authorName, int? cacheBuster = null)
		{
			string localFileName = Id + "author_solution.png";
			string url = Service.GeoServerWrapper.GetSolutionMapUrl(Id, authorName);
			if (cacheBuster.HasValue)
			{
				url += $"?cb={cacheBuster.Value}";
			}
			return Service.Textures.GetURLTexture(url, localFileName);
		}
	}
}
