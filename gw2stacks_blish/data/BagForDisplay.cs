namespace gw2stacks_blish.data
{
	internal class BagForDisplay
	{
		private int? id;

		private int size;

		public BagForDisplay(int? id_, int size_)
		{
			id = id_;
			size = size_;
		}

		public int get_id()
		{
			if (!id.HasValue)
			{
				return 0;
			}
			return id.Value;
		}

		public int get_size()
		{
			return size;
		}

		public int get_icon_id()
		{
			if (!id.HasValue)
			{
				return 1414044;
			}
			if (Magic.jsonLut.itemLut.ContainsKey(id.Value))
			{
				return Magic.jsonLut.itemLut[id.Value].IconId;
			}
			return 63369;
		}

		public string get_name()
		{
			if (!id.HasValue)
			{
				return "Empty";
			}
			return Magic.get_local_name(id.Value);
		}

		public string get_advice()
		{
			if (size == 0)
			{
				return Magic.get_current_translated_string("Equip:") + " (" + Magic.get_local_name(Magic.silkBag.itemId) + ")";
			}
			if (size < 18)
			{
				return Magic.get_current_translated_string("Upgrade these bags to") + " (" + Magic.get_local_name(Magic.silkBag.itemId) + ")";
			}
			return Magic.get_current_translated_string("Potentially replace these bags with") + " (" + Magic.get_local_name(Magic.borealTrunk.itemId) + ")";
		}
	}
}
