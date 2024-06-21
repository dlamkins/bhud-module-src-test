using System;
using System.Collections.Generic;
using Estreya.BlishHUD.EventTable.Models;

namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct AddEvent
	{
		public string CategoryKey { get; set; }

		public string Key { get; set; }

		public string Name { get; set; }

		public string Icon { get; set; }

		public TimeSpan Offset { get; set; }

		public TimeSpan Repeat { get; set; }

		public DateTime? StartingDate { get; set; }

		public string Location { get; set; }

		public int[] MapIds { get; set; }

		public EventWaypoints Waypoints { get; set; }

		public string Wiki { get; set; }

		public int Duration { get; set; }

		public string BackgroundColorCode { get; set; }

		public string[] BackgroundColorGradientCodes { get; set; }

		public APICodeType? APICodeType { get; set; }

		public string APICode { get; set; }

		public bool Filler { get; set; }

		public List<DateTime> Occurences { get; set; }

		public TimeSpan[] ReminderTimes { get; set; }

		public AddEvent()
		{
			CategoryKey = null;
			Key = null;
			Name = null;
			Icon = null;
			Offset = TimeSpan.Zero;
			Repeat = TimeSpan.Zero;
			StartingDate = null;
			Location = null;
			MapIds = null;
			Waypoints = default(EventWaypoints);
			Wiki = null;
			Duration = 0;
			BackgroundColorCode = null;
			BackgroundColorGradientCodes = null;
			APICodeType = null;
			APICode = null;
			Filler = false;
			Occurences = null;
			ReminderTimes = new TimeSpan[1] { TimeSpan.FromMinutes(10.0) };
		}
	}
}
