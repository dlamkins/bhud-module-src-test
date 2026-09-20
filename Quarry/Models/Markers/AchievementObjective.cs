namespace Quarry.Models.Markers
{
	public class AchievementObjective
	{
		public string Namespace { get; set; }

		public int Bit { get; set; }

		public int MapId { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public float Z { get; set; }

		public bool IsTrail { get; set; }

		public string Waypoint { get; set; }

		public bool HeightUnknown { get; set; }

		public string SectorName { get; set; }

		public ObjectiveSource Source { get; set; }
	}
}
