using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class GobblerItemForDisplay : ItemForDisplay
	{
		public int gobblerId;

		public GobblerItemForDisplay(Item item_, List<Source> sources_, string advice_, int gobblerId_)
			: base(item_, sources_, advice_)
		{
			gobblerId = gobblerId_;
		}

		protected override string get_source_string()
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

		public override string get_advice(string name = null)
		{
			return Magic.get_current_translated_string(advice) + " (" + Magic.get_local_name(gobblerId) + ")";
		}

		public override string print(string name = null)
		{
			return Magic.get_local_name(get_id()) + "\n" + Magic.get_current_translated_string(advice) + " (" + Magic.get_local_name(gobblerId) + ")\n" + get_source_string();
		}

		public override string ToString()
		{
			return print();
		}
	}
}
