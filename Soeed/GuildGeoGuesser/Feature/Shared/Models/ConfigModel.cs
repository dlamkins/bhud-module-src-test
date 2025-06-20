using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	[Serializable]
	public class ConfigModel
	{
		[JsonProperty("version")]
		public string Version { get; set; } = "0.0.0";


		[JsonProperty("name")]
		public string Name { get; set; } = "Guild Geoguesser";


		[JsonProperty("default_duration_minutes")]
		public int DefaultDurationMinutes { get; set; } = 60;


		[JsonProperty("server_url")]
		public string ServerUrl { get; set; } = "http://localhost:3000";


		[JsonProperty("score_wrong_map")]
		public string ScoreWrongMap { get; set; } = "Needs a new Atlas";


		[JsonProperty("score_wrong_map_color")]
		public string ScoreWrongMapColor { get; set; } = "#FF0000";


		[JsonProperty("scores")]
		public List<ScoreModel> Scores { get; set; } = new List<ScoreModel>();


		[JsonProperty("help_screen_file_version")]
		public string HelpScreenFileVersion { get; set; } = "0.0.0";


		[JsonProperty("help_screen_url")]
		public string HelpScreenUrl { get; set; } = "http://localhost:3000/help_screens.json";


		[JsonProperty("show_solution_map")]
		public bool ShowSolutionMap { get; set; }

		[JsonProperty("show_score_distribution")]
		public bool ShowScoreDistribution { get; set; } = true;

	}
}
