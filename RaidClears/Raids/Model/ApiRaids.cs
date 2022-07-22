using System.Collections.Generic;

namespace RaidClears.Raids.Model
{
	public class ApiRaids
	{
		public List<string> Clears { get; } = new List<string>();


		public ApiRaids()
		{
		}

		public ApiRaids(List<string> clears)
		{
			Clears = clears;
		}
	}
}
