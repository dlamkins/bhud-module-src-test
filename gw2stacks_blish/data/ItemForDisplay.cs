using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class ItemForDisplay
	{
		private Item item;

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

		public virtual string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", sources);
		}

		public virtual int get_id()
		{
			return item.itemId;
		}

		public virtual string get_chatlink()
		{
			return item.chatLink;
		}

		public virtual int get_iconId()
		{
			return item.iconId;
		}

		public override string ToString()
		{
			return item.total_count() + "x\n" + Magic.get_current_translated_string(advice) + "\n" + get_source_string();
		}
	}
}
