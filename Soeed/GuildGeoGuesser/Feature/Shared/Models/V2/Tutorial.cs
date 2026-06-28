using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD.Content;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class Tutorial
	{
		[JsonProperty("tutorialId")]
		public string Id { get; set; } = "";


		[JsonProperty("title")]
		public string Title { get; set; } = "";


		[JsonProperty("author")]
		public string Author { get; set; } = "";


		[JsonProperty("sortOrder")]
		public int SortOrder { get; set; }

		[JsonProperty("gcsFilePath")]
		public string GcsFilePath { get; set; } = "";


		[JsonProperty("location")]
		public Location Location { get; set; } = new Location();


		[JsonProperty("instructions")]
		[JsonConverter(typeof(TutorialInstructionsConverter))]
		public List<TutorialInstructionStep> Instructions { get; set; } = new List<TutorialInstructionStep>();


		[JsonProperty("hint")]
		public TutorialHint? Hint { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }

		[JsonProperty("isActive")]
		public bool IsActive { get; set; } = true;


		[JsonProperty("hasGuessed")]
		public bool HasGuessed { get; set; }

		[JsonProperty("distance")]
		public double Distance { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;


		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; } = DateTime.Now;


		public AsyncTexture2D GetImageTexture()
		{
			string ext = Path.GetExtension(GcsFilePath);
			if (string.IsNullOrEmpty(ext))
			{
				ext = ".png";
			}
			string localFileName = "tutorial_" + Id + ext;
			return Service.Textures.GetURLTexture(Service.GeoServerWrapper.GetTutorialImageUrl(Id), localFileName);
		}

		public string ScoreText()
		{
			if (Distance < 0.0)
			{
				return Service.Config.ScoreWrongMap;
			}
			return Service.Config.Scores.Find((ScoreModel score) => (double)score.Min <= Distance && (double)score.Max > Distance)?.Value ?? "Missing Score";
		}
	}
}
