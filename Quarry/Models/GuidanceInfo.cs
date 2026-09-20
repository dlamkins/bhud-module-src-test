using Microsoft.Xna.Framework;

namespace Quarry.Models
{
	public class GuidanceInfo
	{
		public static readonly GuidanceInfo None = new GuidanceInfo();

		public GuidanceTier Tier { get; set; }

		public int RemainingTagged { get; set; }

		public bool HasRoute { get; set; }

		public bool HasTrail { get; set; }

		public string Label => Tier switch
		{
			GuidanceTier.Tagged => "* Guided", 
			GuidanceTier.Coordinate => "+ Coords", 
			GuidanceTier.Route => "~ Route", 
			GuidanceTier.Area => "· Area", 
			_ => null, 
		};

		public Color Color => (Color)(Tier switch
		{
			GuidanceTier.Tagged => GuidanceStyle.Tagged, 
			GuidanceTier.Coordinate => GuidanceStyle.Coordinate, 
			GuidanceTier.Route => GuidanceStyle.Route, 
			_ => GuidanceStyle.Area, 
		});

		public string Describe()
		{
			switch (Tier)
			{
			case GuidanceTier.Tagged:
			{
				string objectives = ((RemainingTagged == 1) ? "objective" : "objectives");
				string tagged = $"{RemainingTagged} {objectives} left here, each tracked on its own — the markers disappear as you finish them.";
				if (!HasRoute)
				{
					return tagged;
				}
				return tagged + "\nA route without completion data also covers this map.";
			}
			case GuidanceTier.Coordinate:
				return "Locations come from the wiki, not a marker pack — a distance to count down, but no icon in the world.";
			case GuidanceTier.Route:
				if (!HasTrail)
				{
					return "Markers cover this map, but they carry no achievement data — they won't disappear as you finish steps.";
				}
				return "A trail is drawn across this map, but it carries no achievement data — it won't shorten as you finish steps.";
			case GuidanceTier.Area:
				return "The wiki names an area for this, but no exact spot — no distance to count down.";
			default:
				return null;
			}
		}
	}
}
