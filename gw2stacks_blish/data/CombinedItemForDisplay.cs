using System.Collections.Generic;
using System.Linq;

namespace gw2stacks_blish.data
{
	internal class CombinedItemForDisplay : ItemForDisplay
	{
		private List<ItemForDisplay> itemList;

		public CombinedItemForDisplay(Item item_, List<ItemForDisplay> itemList_)
			: base(item_, null, null)
		{
			itemList = itemList_;
		}

		public override bool applicable_to_id(int id_)
		{
			if (item.itemId == id_)
			{
				return true;
			}
			return false;
		}

		protected override string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", item.sources);
		}

		public override int get_id()
		{
			return base.get_id();
		}

		public override int get_iconId()
		{
			return base.get_iconId();
		}

		public override string print(string name = null)
		{
			if (!itemList.Any())
			{
				return Magic.get_local_name(get_id()) + "\n" + Magic.get_current_translated_string("No current advice") + "\n" + get_source_string();
			}
			string combinedAdvice = "";
			foreach (ItemForDisplay item in itemList)
			{
				combinedAdvice = combinedAdvice + item.get_advice() + "\n";
			}
			return Magic.get_local_name(get_id()) + "\n" + combinedAdvice + "\n" + get_source_string();
		}

		public override string ToString()
		{
			return print();
		}
	}
}
