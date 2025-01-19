using System.Collections.Generic;

namespace DrmTracker.Domain
{
	public class ApiIds
	{
		public int Clear { get; set; }

		public int FullCM { get; set; }

		public int Factions { get; set; }

		public List<int> GetIds()
		{
			return new List<int>(3) { Clear, FullCM, Factions };
		}
	}
}
