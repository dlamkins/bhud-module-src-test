using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	internal class Item
	{
		public int itemId;

		public List<Source> sources;

		public bool isCharacterBound;

		public bool isAccountBound;

		public string name;

		public string description;

		public int iconId;

		public ApiEnum<ItemRarity> rarity;

		public bool isStackable;

		public bool isDeletable;

		public bool isRareForSalvage;

		public string wikiLink;

		public int price;

		public bool isFoodOrUtility;

		public bool hasInformation;

		public Item(int id_)
		{
			itemId = id_;
			sources = new List<Source>();
			isFoodOrUtility = false;
			isCharacterBound = false;
			isAccountBound = false;
			name = null;
			description = null;
			iconId = 63369;
			rarity = null;
			isStackable = false;
			isDeletable = false;
			isRareForSalvage = false;
			price = 0;
			hasInformation = false;
		}

		public void add_source(Source source_)
		{
			foreach (Source source in sources)
			{
				if (source.place == source_.place)
				{
					source.count += source_.count;
					return;
				}
			}
			sources.Add(source_);
		}

		public List<Source> get_advice_stacks(int materialStorageSize_)
		{
			if (isCharacterBound || !isStackable)
			{
				return new List<Source>();
			}
			List<Source> stackableSources = new List<Source>();
			List<Source> stackableSource = get_partial_stacks(materialStorageSize_);
			ulong numberOfPartialStacks = Convert.ToUInt64(stackableSource.Count());
			ulong numberOfConsolidatedStacks = Convert.ToUInt64(Math.Ceiling(Convert.ToDouble(total_count() / 250uL)));
			if (isStackable && numberOfPartialStacks > 1 && numberOfPartialStacks > numberOfConsolidatedStacks)
			{
				stackableSources.AddRange(stackableSource);
			}
			return stackableSources;
		}

		public List<Source> get_partial_stacks(int materialStorageSize_)
		{
			List<Source> partialStacks = new List<Source>();
			foreach (Source currentSource in sources)
			{
				if (currentSource.count % 250uL != 0L || (currentSource.place == "Material Storage" && currentSource.count < Convert.ToUInt64(materialStorageSize_)))
				{
					partialStacks.Add(currentSource);
				}
			}
			return partialStacks;
		}

		public ulong total_count()
		{
			ulong total = 0uL;
			foreach (Source current_Source in sources)
			{
				total += current_Source.count;
			}
			return total;
		}

		public override string ToString()
		{
			return itemId + " " + name + " " + string.Join(", ", sources);
		}
	}
}
