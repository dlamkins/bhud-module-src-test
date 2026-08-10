using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamUpdateDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string CooldownFormatted { get; set; }

		public Guid? CategoryId { get; set; }

		public List<SpamLineDto> SpamLines { get; set; } = new List<SpamLineDto>();

	}
}
