using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminCharacterDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Profession { get; set; }

		public int Level { get; set; }

		public string? Race { get; set; }

		public string? Gender { get; set; }
	}
}
