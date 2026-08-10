using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models
{
	internal class MaterialCategoryDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Order { get; set; }

		public List<int> ItemIds { get; set; }

		public MaterialCategoryDto()
		{
			Name = string.Empty;
			ItemIds = new List<int>();
		}
	}
}
