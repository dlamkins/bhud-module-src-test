namespace NpcFinder.Models
{
	public class NpcCandidateHit
	{
		public string Title { get; set; }

		public string MapName { get; set; }

		public int? MapId { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public override string ToString()
		{
			string map = ((!string.IsNullOrWhiteSpace(MapName)) ? MapName : (MapId.HasValue ? $"Map {MapId}" : "Unknown map"));
			return $"{Title} — {map} — ({X},{Y})";
		}
	}
}
