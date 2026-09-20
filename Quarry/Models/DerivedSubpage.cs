using System.Collections.Generic;

namespace Quarry.Models
{
	public class DerivedSubpage
	{
		public string Coordinates { get; set; }

		public string Title { get; set; }

		public List<DerivedSubpagePlace> Places { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }
	}
}
