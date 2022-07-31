using System.Collections.Generic;

namespace RaidClears.Dungeons.Model
{
	public class ApiDungeons
	{
		public List<string> Clears { get; } = new List<string>();


		public List<string> Frequenter { get; } = new List<string>();


		public ApiDungeons()
		{
		}

		public ApiDungeons(List<string> clears, List<string> frequented)
		{
			Clears = clears;
			Frequenter = frequented;
		}
	}
}
