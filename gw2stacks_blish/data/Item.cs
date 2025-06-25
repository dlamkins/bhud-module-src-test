using System;
using System.Collections.Generic;
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

		public int VendorValue;

		public bool isSellable;

		public bool isSalvagable;

		public string chatLink;

		public ItemType type;

		public ItemWeightType armorWeight;

		public ItemArmorSlotType armorType;

		public ItemWeaponType weaponType;

		public ItemTrinketType trinketType;

		public Item(int id_, bool isCharacterBound_, bool isAccountBound_, bool delayedCreate = false)
		{
			itemId = id_;
			sources = new List<Source>();
			isFoodOrUtility = false;
			isCharacterBound = isCharacterBound_;
			isAccountBound = isAccountBound_;
			name = null;
			description = null;
			iconId = 63369;
			rarity = null;
			isStackable = true;
			isDeletable = false;
			isRareForSalvage = false;
			price = 0;
			hasInformation = false;
			VendorValue = 0;
			isSellable = true;
			isSalvagable = true;
			chatLink = "";
			type = ItemType.Unknown;
			armorWeight = ItemWeightType.Unknown;
			armorType = ItemArmorSlotType.Unknown;
			weaponType = ItemWeaponType.Unknown;
			trinketType = ItemTrinketType.Unknown;
			if (!delayedCreate)
			{
				build_basic_item_info();
			}
		}

		public void build_basic_item_info()
		{
			ItemInfo info_;
			if (Magic.jsonLut.itemLut.ContainsKey(itemId))
			{
				info_ = Magic.jsonLut.itemLut[itemId];
			}
			else
			{
				info_ = Magic.unknown;
				info_.Id = itemId;
			}
			if (Magic.is_luck_essence(itemId))
			{
				name = Magic.get_local_name(itemId);
			}
			else
			{
				name = info_.Name;
			}
			iconId = info_.IconId;
			rarity = (ApiEnum<ItemRarity>)(ItemRarity)info_.Rarity;
			description = info_.Description;
			isFoodOrUtility = info_.isFoodOrUtility;
			VendorValue = info_.VendorValue;
			string urlName = name.Replace(" ", "_");
			wikiLink = "wiki.guildwars2.com/wiki/" + urlName;
			type = (ItemType)info_.Type;
			bool salvagable = true;
			if (Magic.is_non_stackable_type((ItemType)info_.Type))
			{
				isStackable = false;
			}
			foreach (int flag in info_.Flags)
			{
				if (flag == 10)
				{
					salvagable = false;
				}
				if (flag == 14)
				{
					isAccountBound = true;
					isCharacterBound = true;
					isStackable = false;
				}
				if (flag == 2)
				{
					isAccountBound = true;
				}
				if (flag == 10)
				{
					isSalvagable = false;
				}
				if (flag == 11)
				{
					isSellable = false;
				}
			}
			if (Magic.collectionOnlyIds.Contains(info_.Id))
			{
				isDeletable = true;
			}
			if (Magic.is_salvagable_equipment((ItemType)info_.Type) && info_.Rarity == 5 && salvagable && info_.Level > 67)
			{
				isRareForSalvage = true;
			}
			armorWeight = info_.armorWeight;
			armorType = info_.armorType;
			weaponType = info_.weaponType;
			trinketType = info_.trinketType;
			hasInformation = true;
		}

		public void add_source(Source source_)
		{
			foreach (Source source in sources)
			{
				if (source.place == source_.place)
				{
					source.count += source_.count;
					source.stacks++;
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
			List<Source> stackableSources = get_partial_stacks(materialStorageSize_);
			int numberOfPartialStacks = 0;
			int partialStackAmount = 0;
			foreach (Source source in stackableSources)
			{
				numberOfPartialStacks++;
				partialStackAmount += Convert.ToInt32(source.count % 250uL);
			}
			int remainder = 0;
			int numberOfConsolidatedStacks = Math.DivRem(partialStackAmount, 250, out remainder);
			if (remainder != 0)
			{
				numberOfConsolidatedStacks++;
			}
			if (numberOfPartialStacks > 1 && numberOfPartialStacks > numberOfConsolidatedStacks)
			{
				return stackableSources;
			}
			return new List<Source>();
		}

		public List<Source> get_partial_stacks(int materialStorageSize_)
		{
			List<Source> partialStacks = new List<Source>();
			foreach (Source currentSource in sources)
			{
				if (currentSource.count % Convert.ToUInt64(250) != Convert.ToUInt64(0) || (currentSource.place == "Material Storage" && currentSource.count < Convert.ToUInt64(materialStorageSize_)))
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
