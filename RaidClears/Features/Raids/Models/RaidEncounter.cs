using System;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	[Serializable]
	public class RaidEncounter : EncounterInterface
	{
		[JsonProperty("api_id")]
		public string ApiId = "undefined";

		[JsonProperty("name")]
		private string _name = "undefined";

		[JsonProperty("abbriviation")]
		private string _abbriviation = "undefined";

		[JsonProperty("powerFavored")]
		public bool PowerFavored { get; set; }

		[JsonProperty("condiFavored")]
		public bool CondiFavored { get; set; }

		[JsonProperty("needsDefianceBreak")]
		public bool NeedsDefianceBreak { get; set; }

		[JsonProperty("mentor_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? MentorAchievementId { get; set; }

		public new string Name
		{
			get
			{
				return GetLocalizedName(_name);
			}
			set
			{
				_name = value;
			}
		}

		public new string Abbriviation
		{
			get
			{
				return GetLocalizedAbbreviation(_abbriviation);
			}
			set
			{
				_abbriviation = value;
			}
		}
	}
}
