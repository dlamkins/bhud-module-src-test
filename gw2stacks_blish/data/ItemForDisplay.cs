using System.Collections.Generic;
using System.Linq;

namespace gw2stacks_blish.data
{
	internal class ItemForDisplay
	{
		protected Item item;

		protected List<Source> sources;

		protected string advice;

		public ItemForDisplay(Item item_, List<Source> sources_, string advice_)
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

		public virtual bool applicable_to_id(int id_)
		{
			if (item.itemId == id_)
			{
				return true;
			}
			return false;
		}

		protected virtual string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", sources);
		}

		public virtual bool has_source(string place_)
		{
			return sources.Any((Source source) => source.place == place_);
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

		public virtual string get_advice(string name = null)
		{
			return Magic.get_current_translated_string(advice);
		}

		public virtual string print(string name = null)
		{
			return Magic.get_local_name(get_id()) + "\n" + item.total_count() + "x\n" + Magic.get_current_translated_string(advice) + "\n" + get_source_string();
		}

		public override string ToString()
		{
			return print();
		}
	}
}
