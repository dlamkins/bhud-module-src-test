using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class BossEncounter : EncounterInterface, IEncounter
	{
		[JsonProperty("api_id", NullValueHandling = NullValueHandling.Ignore)]
		public string ApiId = "undefined";

		[JsonProperty("mapIds")]
		public List<int> MapIds = new List<int>();

		[JsonProperty("powerFavored")]
		public bool PowerFavored { get; set; }

		[JsonProperty("condiFavored")]
		public bool CondiFavored { get; set; }

		[JsonProperty("needsDefianceBreak")]
		public bool NeedsDefianceBreak { get; set; }

		[JsonProperty("mentor_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? MentorAchievementId { get; set; }

		[JsonProperty("mentor_achievement_max", NullValueHandling = NullValueHandling.Ignore)]
		public int? MentorAchievementMax { get; set; }

		[JsonProperty("daily_bounty_achievement_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? DailyBountyAchievementId { get; set; }

		[JsonProperty("resets", NullValueHandling = NullValueHandling.Ignore)]
		public string Resets { get; set; } = string.Empty;


		public string EncounterId
		{
			get
			{
				if (ApiId == null || !(ApiId != "undefined"))
				{
					return Id;
				}
				return ApiId;
			}
		}

		public bool IsStrike
		{
			get
			{
				if (MapIds != null)
				{
					return MapIds.Count > 0;
				}
				return false;
			}
		}

		string IEncounter.Id => EncounterId;

		int IEncounter.IconAssetId => AssetId;
	}
}
