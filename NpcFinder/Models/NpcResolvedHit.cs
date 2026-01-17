namespace NpcFinder.Models
{
	public class NpcResolvedHit
	{
		public string Title { get; set; } = "";


		public int MapId { get; set; }

		public string MapName { get; set; } = "";


		public int TargetContinentId { get; set; }

		public double TargetContinentX { get; set; }

		public double TargetContinentY { get; set; }

		public int ContinentId
		{
			get
			{
				return TargetContinentId;
			}
			set
			{
				TargetContinentId = value;
			}
		}

		public double ContinentX
		{
			get
			{
				return TargetContinentX;
			}
			set
			{
				TargetContinentX = value;
			}
		}

		public double ContinentY
		{
			get
			{
				return TargetContinentY;
			}
			set
			{
				TargetContinentY = value;
			}
		}

		public string Source { get; set; } = "";


		public string Debug { get; set; } = "";

	}
}
