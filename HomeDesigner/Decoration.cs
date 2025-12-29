using System.Collections.Generic;

namespace HomeDesigner
{
	public class Decoration
	{
		public int id;

		public string name;

		public string description;

		public int max_count;

		public int icon;

		public List<int> categories;

		public Decoration()
		{
			id = 0;
			name = null;
			description = null;
			max_count = 0;
			icon = 0;
			categories = new List<int>();
		}
	}
}
