namespace rp.spark.Models.Api
{
	public class NearbyPresenceSearchRequest
	{
		public ProfileRegion Region { get; set; }

		public bool IncludeMature { get; set; }

		public int MapId { get; set; }

		public uint ShardId { get; set; }

		public string ServerAddress { get; set; } = string.Empty;


		public bool HasPosition { get; set; }

		public double X { get; set; }

		public double Y { get; set; }

		public double Z { get; set; }

		public double MaxDistanceMeters { get; set; } = 600.0;

	}
}
