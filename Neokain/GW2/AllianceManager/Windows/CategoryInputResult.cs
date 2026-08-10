using System;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class CategoryInputResult
	{
		public Guid? CategoryId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int DisplayOrder { get; set; }
	}
}
