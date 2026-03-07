using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	internal class CraftingItemForDisplay : ItemForDisplay
	{
		private Dictionary<RecipeIngredient, List<Source>> ingredientSources;

		private int outputId;

		public CraftingItemForDisplay(Item item_, Dictionary<RecipeIngredient, List<Source>> ingredientSources_, string advice_, int outputId_)
			: base(item_, null, advice_)
		{
			ingredientSources = ingredientSources_;
			outputId = outputId_;
		}

		public override bool applicable_to_id(int id_)
		{
			foreach (RecipeIngredient key in ingredientSources.Keys)
			{
				if (key.ItemId == id_)
				{
					return true;
				}
			}
			return false;
		}

		public string get_ingredient_string(string place_ = null)
		{
			string output = "";
			foreach (KeyValuePair<RecipeIngredient, List<Source>> item in ingredientSources)
			{
				List<Source> sufficientCount = new List<Source>();
				List<Source> sorted = item.Value.OrderByDescending((Source source) => source.count).ToList();
				if (place_ != null)
				{
					int index = sorted.FindIndex((Source source) => source.place == place_);
					if (index != -1)
					{
						sufficientCount.Add(sorted.ElementAt(index));
						sorted.RemoveAt(index);
					}
					else
					{
						sufficientCount.Add(sorted.First());
						sorted.RemoveAt(0);
					}
				}
				else
				{
					sufficientCount.Add(sorted.First());
					sorted.RemoveAt(0);
				}
				while (sufficientCount.Sum((Source questionable) => Convert.ToInt32(questionable.count)) < item.Key.Count)
				{
					sufficientCount.Add(sorted.First());
					sorted.RemoveAt(0);
				}
				string partial = item.Key.Count + " x " + Magic.get_local_name(item.Key.ItemId) + " / " + string.Join("\n", sufficientCount) + "\n";
				output += partial;
			}
			return output;
		}

		public override bool has_source(string place_)
		{
			bool result = false;
			foreach (List<Source> list in ingredientSources.Values)
			{
				result |= list.Any((Source source) => source.place == place_);
			}
			return result;
		}

		public override int get_id()
		{
			return item.itemId;
		}

		public override string get_chatlink()
		{
			return item.chatLink;
		}

		public override int get_iconId()
		{
			return item.iconId;
		}

		public override string get_advice(string name = null)
		{
			return Magic.get_current_translated_string(advice) + ": " + Magic.get_local_name(outputId) + "\n" + get_ingredient_string(name);
		}

		public override string print(string name = null)
		{
			return Magic.get_local_name(get_id()) + "\n" + item.total_count() + "x\n" + Magic.get_current_translated_string(advice) + ": " + Magic.get_local_name(outputId) + "\n" + item.print_prices() + get_ingredient_string(name);
		}

		public override string ToString()
		{
			return print();
		}
	}
}
