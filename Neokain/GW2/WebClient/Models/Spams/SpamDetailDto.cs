using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamDetailDto : SpamDto
	{
		public List<SpamLineDto> SpamLines { get; set; } = new List<SpamLineDto>();

	}
}
