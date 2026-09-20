using System;
using System.Collections.Generic;

namespace Quarry.Models.Persistence
{
	public class Storage
	{
		public Dictionary<int, List<int>> ManualCompletedAchievements { get; set; } = new Dictionary<int, List<int>>();


		public List<int> TrackedAchievements { get; set; } = new List<int>();


		public List<int> HiddenAchievements { get; set; } = new List<int>();


		public Dictionary<int, DateTime> SnoozedAchievements { get; set; } = new Dictionary<int, DateTime>();


		public Dictionary<int, List<string>> HuntEnabledNamespaces { get; set; } = new Dictionary<int, List<string>>();


		public int TrackWindowLocationX { get; set; } = -1;


		public int TrackWindowLocationY { get; set; } = -1;


		public bool ShowTrackWindow { get; set; }

		public int TrackWindowCompactWidth { get; set; } = -1;


		public int TrackWindowCompactHeight { get; set; } = -1;


		public int OverviewWindowWidth { get; set; } = -1;


		public int OverviewWindowHeight { get; set; } = -1;

	}
}
