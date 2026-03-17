using Newtonsoft.Json;

namespace GW2StoryTimes.Models
{
	public class TimeEstimate
	{
		[JsonProperty("seed_mins")]
		public double? SeedMins { get; set; }

		[JsonProperty("avg_mins")]
		public double? AvgMins { get; set; }

		[JsonProperty("submissions")]
		public int Submissions { get; set; }

		[JsonProperty("min_mins")]
		public double? MinMins { get; set; }

		[JsonProperty("max_mins")]
		public double? MaxMins { get; set; }

		public bool HasCommunityData => Submissions > 0;

		public string FormattedEstimate
		{
			get
			{
				if (!AvgMins.HasValue)
				{
					return "No estimate";
				}
				double mins = AvgMins.Value;
				if (mins < 60.0)
				{
					return $"~{mins:F0} min";
				}
				int hours = (int)(mins / 60.0);
				int remaining = (int)(mins % 60.0);
				if (remaining <= 0)
				{
					return $"~{hours}h";
				}
				return $"~{hours}h {remaining}m";
			}
		}

		public string FormattedRange
		{
			get
			{
				if (!MinMins.HasValue || !MaxMins.HasValue || !HasCommunityData)
				{
					return null;
				}
				return FormatShort(MinMins.Value) + "–" + FormatShort(MaxMins.Value);
			}
		}

		private static string FormatShort(double mins)
		{
			if (mins < 60.0)
			{
				return $"{mins:F0}m";
			}
			int h = (int)(mins / 60.0);
			int i = (int)(mins % 60.0);
			if (i <= 0)
			{
				return $"{h}h";
			}
			return $"{h}h{i}m";
		}
	}
}
