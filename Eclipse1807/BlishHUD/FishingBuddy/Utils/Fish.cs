using System;
using Blish_HUD;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class Fish
	{
		[Flags]
		[JsonConverter(typeof(StringEnumConverter))]
		public enum TimeOfDay
		{
			None = 0x0,
			Dawn = 0x1,
			Day = 0x2,
			Dusk = 0x4,
			Night = 0x8,
			DawnDusk = 0x5,
			DuskDawn = 0x5,
			Any = 0xF
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(Fish));

		public string name { get; set; }

		public int itemId { get; set; }

		public string rarity { get; set; }

		public string fishingHole { get; set; }

		public string bait { get; set; }

		public TimeOfDay timeOfDay { get; set; }

		public bool openWater { get; set; }

		public string location { get; set; }

		public string achievement { get; set; }

		public int achievementId { get; set; }

		public string notes { get; set; }

		public RenderUrl icon { get; set; }
	}
}
