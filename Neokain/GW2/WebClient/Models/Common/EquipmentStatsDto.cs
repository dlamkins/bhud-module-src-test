using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Common
{
	internal class EquipmentStatsDto
	{
		public int Id { get; set; }

		public Dictionary<string, int> Attributes { get; set; } = new Dictionary<string, int>();

	}
}
