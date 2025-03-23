using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class ItemForDisplay
	{
		public Item item;

		public List<Source> sources;

		public string advice;

		public ItemForDisplay(Item item_, List<Source> sources_ = null, string advice_ = null)
		{
			item = item_;
			if (sources_ == null)
			{
				sources = item_.sources;
			}
			else
			{
				sources = sources_;
			}
			advice = advice_;
		}

		public string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", sources);
		}

		public override string ToString()
		{
			return (item?.ToString() ?? " ") + " " + (advice?.ToString() ?? " ") + " " + string.Join(", ", sources);
		}
	}
}
