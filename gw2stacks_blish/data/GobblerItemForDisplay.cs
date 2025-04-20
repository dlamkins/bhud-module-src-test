using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class GobblerItemForDisplay : ItemForDisplay
	{
		public int gobblerId;

		public GobblerItemForDisplay(Item item_, List<Source> sources_ = null, string advice_ = null, int id_ = 0)
			: base(item_, sources_, advice_)
		{
			gobblerId = id_;
		}

		public override string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", sources);
		}

		public override int get_id()
		{
			return base.get_id();
		}

		public override int get_iconId()
		{
			return base.get_iconId();
		}

		public override string ToString()
		{
			return Magic.get_current_translated_string(advice) + " (" + Magic.get_local_name(gobblerId) + ")\n" + get_source_string();
		}
	}
}
