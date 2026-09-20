namespace Quarry.Models
{
	public class RemainingObjective
	{
		public int Bit { get; set; }

		public int Row { get; set; }

		public string Name { get; set; }

		public double DistanceMetres { get; set; }

		public string Namespace { get; set; }

		public bool IsTrail { get; set; }

		public string Waypoint { get; set; }

		public bool GroundDistanceOnly { get; set; }

		public string AreaHint { get; set; }
	}
}
