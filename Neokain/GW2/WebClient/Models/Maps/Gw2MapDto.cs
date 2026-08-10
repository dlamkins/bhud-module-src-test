namespace Neokain.GW2.WebClient.Models.Maps
{
	public class Gw2MapDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int MinLevel { get; set; }

		public int MaxLevel { get; set; }

		public int DefaultFloor { get; set; }

		public string Type { get; set; }

		public int? RegionId { get; set; }

		public string? RegionName { get; set; }

		public int? ContinentId { get; set; }

		public string? ContinentName { get; set; }

		public RectangleDto MapRect { get; set; }

		public RectangleDto ContinentRect { get; set; }

		public bool IsUnknown { get; set; }
	}
}
