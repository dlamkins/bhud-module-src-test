using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class Gobbler
	{
		public int itemId;

		public int count;

		public List<int> food;

		public string name;

		public Gobbler(int id_, List<int> food_, int count_, string name_)
		{
			itemId = id_;
			food = food_;
			count = count_;
			name = name_;
		}

		public Gobbler(int id_, int food_, int count_, string name_)
		{
			itemId = id_;
			food = new List<int> { food_ };
			count = count_;
			name = name_;
		}

		public Gobbler()
		{
			itemId = 0;
			food = null;
			count = 0;
			name = null;
		}
	}
}
