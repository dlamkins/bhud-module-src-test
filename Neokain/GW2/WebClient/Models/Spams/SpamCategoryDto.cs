using System;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamCategoryDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int DisplayOrder { get; set; }

		public int SpamCount { get; set; }
	}
}
