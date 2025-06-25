using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class EmptyItemForDisplay : ItemForDisplay
	{
		public EmptyItemForDisplay()
			: base(Magic.borealTrunk, new List<Source>(), null)
		{
		}

		protected override string get_source_string()
		{
			return null;
		}

		public override bool applicable_to_id(int id_)
		{
			return false;
		}

		public override int get_id()
		{
			return 0;
		}

		public override int get_iconId()
		{
			return 156900;
		}

		public override string print(string name = null)
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
