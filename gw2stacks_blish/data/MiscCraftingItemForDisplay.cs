using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class MiscCraftingItemForDisplay : ItemForDisplay
	{
		public int outputId;

		public MiscCraftingItemForDisplay(Item item_, List<Source> sources_ = null, string advice_ = null, int outputId_ = 0)
			: base(item_, sources_, advice_)
		{
			outputId = outputId_;
		}

		private string get_ingredient_names()
		{
			string output = "";
			foreach (KeyValuePair<int, int> item in Magic.craftingMiscAdvices[outputId].idCountMapping)
			{
				output = output + "0-" + item.Value + "x " + Magic.get_local_name(item.Key) + "\n";
			}
			return output;
		}

		public override string get_source_string()
		{
			return "Sources:\n" + string.Join("\n", sources);
		}

		public override int get_id()
		{
			return outputId;
		}

		public override int get_iconId()
		{
			if (Magic.jsonLut.itemLut.ContainsKey(outputId))
			{
				return Magic.jsonLut.itemLut[outputId].IconId;
			}
			return Magic.unknown.IconId;
		}

		public override string get_chatlink()
		{
			return Magic.jsonLut.itemLut[outputId].chatLink;
		}

		public override string ToString()
		{
			return Magic.get_current_translated_string(advice) + " (" + Magic.get_local_name(outputId) + ")\n" + get_ingredient_names() + get_source_string();
		}
	}
}
