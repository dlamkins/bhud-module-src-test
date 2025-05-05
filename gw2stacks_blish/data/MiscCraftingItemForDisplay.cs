namespace gw2stacks_blish.data
{
	internal class MiscCraftingItemForDisplay : ItemForDisplay
	{
		private Item output;

		public MiscCraftingItemForDisplay(Item input_, Item output_, string advice_)
			: base(input_, null, advice_)
		{
			output = output_;
		}

		public override bool applicable_to_id(int id_)
		{
			return item.itemId == id_;
		}

		protected override string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", item.sources);
		}

		public override string print(string name = null)
		{
			return Magic.get_local_name(get_id()) + "\n" + Magic.get_current_translated_string(advice) + " (" + Magic.get_local_name(output.itemId) + ")\n" + get_source_string();
		}

		public override string ToString()
		{
			return print();
		}
	}
}
