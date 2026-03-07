using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using gw2stacks_blish.reader;

namespace gw2stacks_blish.data
{
	internal class Model
	{
		public Dictionary<int, Item> items;

		private int materialStorageSize;

		private List<RecipeInfo> craftableRecipes;

		private Dictionary<int, Item> recipeResults;

		private List<int> appraisedItemIds;

		public bool includeConsumables;

		private int ectoSalvagePrice;

		public bool validData;

		private Logger log;

		public Dictionary<string, List<int?>> characterInventory;

		public List<int?> sharedInventory;

		public List<string> characterNames = new List<string>();

		public Dictionary<string, List<InventoryBagSlot>> inventoryBags = new Dictionary<string, List<InventoryBagSlot>>();

		public Armory legendaryArmory;

		public Unlocks unlocks;

		public void reset_state(int materialStorageSize_)
		{
			foreach (KeyValuePair<int, Item> item in items)
			{
				item.Value.sources.Clear();
			}
			materialStorageSize = materialStorageSize_;
			craftableRecipes = new List<RecipeInfo>();
			recipeResults = new Dictionary<int, Item>();
			appraisedItemIds = new List<int>();
			includeConsumables = true;
			ectoSalvagePrice = 0;
			characterNames = new List<string>();
			characterInventory = new Dictionary<string, List<int?>>();
			sharedInventory = new List<int?>();
			inventoryBags = new Dictionary<string, List<InventoryBagSlot>>();
			legendaryArmory = new Armory();
			unlocks = new Unlocks();
			validData = false;
		}

		public Model(Logger log_)
		{
			log = log_;
			items = new Dictionary<int, Item>();
			reset_state(0);
		}

		public async Task setup(Gw2Api api_)
		{
			validData = false;
			log.Debug("started building ecto price");
			await build_ecto_price(api_);
			log.Debug("started building inventory");
			await build_inventory(api_);
			log.Debug("started building recipes");
			await build_recipe_info();
			log.Debug("started building prices");
			await build_item_prices(api_);
			Magic.silkBag.build_basic_item_info();
			Magic.borealTrunk.build_basic_item_info();
			await build_legendary_armory(api_);
			await get_unlocks(api_);
			validData = true;
		}

		public void add_item(int id_, bool isAccountBound_, bool isCharacterBound_, Source source_)
		{
			if (!items.ContainsKey(id_))
			{
				items.Add(id_, new Item(id_, isCharacterBound_, isAccountBound_));
			}
			items[id_].add_source(source_);
			if (!items[id_].isAccountBound)
			{
				appraisedItemIds.Add(id_);
			}
			else
			{
				items[id_].price = 0;
			}
		}

		public bool has_item(int id_)
		{
			if (items.ContainsKey(id_))
			{
				Item item = items[id_];
				if (item != null && item.total_count() != 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool has_item(List<int> ids_)
		{
			bool result = false;
			foreach (int item in ids_)
			{
				result |= has_item(item);
			}
			return result;
		}

		public async Task build_inventory(Gw2Api api_)
		{
			ulong emptySlots = 0uL;
			foreach (Character character in await api_.characters())
			{
				characterNames.Add(character.Name);
				if (!characterInventory.ContainsKey(character.Name))
				{
					characterInventory.Add(character.Name, new List<int?>());
				}
				List<int?> inventory = new List<int?>();
				List<InventoryBagSlot> slots = new List<InventoryBagSlot>();
				foreach (CharacterInventoryBag bag in character.Bags!)
				{
					if (bag != null)
					{
						slots.Add(new InventoryBagSlot(bag.Id, bag.Size));
						foreach (AccountItem item4 in bag?.Inventory)
						{
							if (item4 == null)
							{
								emptySlots++;
								inventory.Add(null);
								continue;
							}
							inventory.Add(item4.Id);
							bool accountBound4 = false;
							bool characterBound4 = false;
							if (item4.Binding != null)
							{
								if (item4.Binding == ItemBinding.Account)
								{
									accountBound4 = true;
								}
								if (item4.Binding == ItemBinding.Character)
								{
									accountBound4 = true;
									characterBound4 = true;
								}
							}
							add_item(item4.Id, accountBound4, characterBound4, new Source(Convert.ToUInt64(item4.Count), character.Name));
						}
					}
					else
					{
						slots.Add(new InventoryBagSlot(0));
					}
				}
				characterInventory[character.Name] = inventory;
				inventoryBags.Add(character.Name, slots);
			}
			ulong maxCount = 0uL;
			foreach (AccountMaterial item3 in await api_.material_storage())
			{
				bool accountBound3 = false;
				bool characterBound3 = false;
				if (item3.Binding != null)
				{
					if (item3.Binding == ItemBinding.Account)
					{
						accountBound3 = true;
					}
					if (item3.Binding == ItemBinding.Character)
					{
						accountBound3 = true;
						characterBound3 = true;
					}
				}
				add_item(item3.Id, accountBound3, characterBound3, new Source(Convert.ToUInt64(item3.Count), "Material Storage"));
				maxCount = Math.Max(maxCount, (ulong)item3.Count);
			}
			materialStorageSize = Math.Max(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(maxCount / 250uL)) * 250.0), materialStorageSize);
			foreach (AccountItem item2 in await api_.bank())
			{
				if (item2 == null)
				{
					emptySlots++;
					continue;
				}
				bool accountBound2 = false;
				bool characterBound2 = false;
				if (item2.Binding != null)
				{
					if (item2.Binding == ItemBinding.Account)
					{
						accountBound2 = true;
					}
					if (item2.Binding == ItemBinding.Character)
					{
						accountBound2 = true;
						characterBound2 = true;
					}
				}
				add_item(item2.Id, accountBound2, characterBound2, new Source(Convert.ToUInt64(item2.Count), "Bank Storage"));
			}
			foreach (AccountItem item in await api_.shared_inventory())
			{
				if (item == null)
				{
					emptySlots++;
					sharedInventory.Add(null);
					continue;
				}
				sharedInventory.Add(item.Id);
				bool accountBound = false;
				bool characterBound = false;
				if (item.Binding != null)
				{
					if (item.Binding == ItemBinding.Account)
					{
						accountBound = true;
					}
					if (item.Binding == ItemBinding.Character)
					{
						accountBound = true;
						characterBound = true;
					}
				}
				add_item(item.Id, accountBound, characterBound, new Source(Convert.ToUInt64(item.Count), "Shared Storage"));
			}
		}

		public async Task build_item_prices(Gw2Api api_)
		{
			List<int> fullIds = appraisedItemIds.Distinct().ToList();
			if (fullIds.Count <= 0)
			{
				return;
			}
			foreach (CommercePrices price in await api_.item_prices(fullIds))
			{
				if (items.ContainsKey(price.Id))
				{
					items[price.Id].price = price.Sells.UnitPrice;
				}
				if (recipeResults.ContainsKey(price.Id))
				{
					recipeResults[price.Id].price = price.Sells.UnitPrice;
				}
			}
		}

		public async Task build_ecto_price(Gw2Api api_)
		{
			int ectoPrice = (await api_.item_price(Magic.ectoId)).Sells.UnitPrice;
			ectoSalvagePrice = Convert.ToInt32(((double)ectoPrice * Magic.tax * Magic.ectoChance - Magic.salvagePrice) / Magic.tax);
		}

		public async Task build_recipe_info()
		{
			List<int> outputItemIds = new List<int>();
			craftableRecipes = new List<RecipeInfo>();
			foreach (RecipeInfo recipe in Magic.jsonLut.recipeLut.Values)
			{
				if (!Magic.is_pertinent_recipe((RecipeType)recipe.Type))
				{
					continue;
				}
				bool valid = true;
				foreach (RecipeIngredient ingredient in recipe.Ingredients)
				{
					int id = ingredient.ItemId;
					if (!has_item(id) || (has_item(id) && items?[id].total_count() < Convert.ToUInt64(ingredient.Count)))
					{
						valid = false;
						break;
					}
				}
				if (!valid)
				{
					continue;
				}
				craftableRecipes.Add(recipe);
				outputItemIds.Add(recipe.OutputItemId);
				foreach (RecipeIngredient item2 in recipe.Ingredients)
				{
					if (!appraisedItemIds.Contains(item2.ItemId) && !items[item2.ItemId].isAccountBound)
					{
						appraisedItemIds.Add(item2.ItemId);
					}
				}
			}
			recipeResults = new Dictionary<int, Item>();
			foreach (int item3 in outputItemIds)
			{
				Item item = new Item(item3, isCharacterBound_: false, isAccountBound_: false);
				if (!recipeResults.ContainsKey(item.itemId))
				{
					recipeResults.Add(item.itemId, item);
				}
				if (!item.isAccountBound && !appraisedItemIds.Contains(item.itemId))
				{
					appraisedItemIds.Add(item.itemId);
				}
			}
		}

		public async Task build_legendary_armory(Gw2Api api_)
		{
			foreach (AccountLegendaryArmory item in await api_.get_legendary_armory())
			{
				if (Magic.jsonLut.itemLut.ContainsKey(item.Id))
				{
					ItemInfo temp = Magic.jsonLut.itemLut[item.Id];
					switch (temp.Type)
					{
					case 1:
						switch (temp.armorWeight)
						{
						case ItemWeightType.Heavy:
							legendaryArmory.heavyArmor[temp.armorType] = true;
							break;
						case ItemWeightType.Medium:
							legendaryArmory.mediumArmor[temp.armorType] = true;
							break;
						case ItemWeightType.Light:
							legendaryArmory.lightArmor[temp.armorType] = true;
							break;
						}
						continue;
					case 15:
						legendaryArmory.weapons[temp.weaponType] += item.Count;
						continue;
					case 2:
						legendaryArmory.backpack = true;
						continue;
					case 12:
						switch (temp.trinketType)
						{
						case ItemTrinketType.Ring:
							legendaryArmory.rings += item.Count;
							break;
						case ItemTrinketType.Accessory:
							legendaryArmory.trinkets += item.Count;
							break;
						case ItemTrinketType.Amulet:
							legendaryArmory.amulet = true;
							break;
						}
						continue;
					}
					if (item.Id == 101582)
					{
						legendaryArmory.relic = true;
					}
					switch (item.Id)
					{
					case 101582:
						legendaryArmory.relic = true;
						break;
					case 91505:
						legendaryArmory.sigils += item.Count;
						break;
					case 91536:
						legendaryArmory.runes += item.Count;
						break;
					}
				}
				else
				{
					log.Warn("Invalid legendary of id" + item.Id);
				}
			}
		}

		public async Task get_unlocks(Gw2Api api_)
		{
			Unlocks unlocks = this.unlocks;
			unlocks.skins = await api_.get_unlocked_skins();
			unlocks = this.unlocks;
			unlocks.minis = await api_.get_unlocked_minis();
			unlocks = this.unlocks;
			unlocks.recipes = await api_.get_unlocked_recipes();
		}

		public List<ItemForDisplay> get_stacks_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Item item in items.Values.Where((Item list_item) => list_item.get_advice_stacks(materialStorageSize).Count > 0))
			{
				result.Add(new ItemForDisplay(item, item.get_advice_stacks(materialStorageSize), "Combine these items into stacks"));
			}
			return result;
		}

		public List<ItemForDisplay> get_vendor_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Item item in items.Values.Where((Item list_item) => list_item.rarity == ItemRarity.Junk || (list_item.isSellable && !list_item.isSalvagable && list_item.isDeletable)))
			{
				result.Add(new ItemForDisplay(item, null, "Sell these items to a vendor"));
			}
			return result;
		}

		public List<ItemForDisplay> get_rare_salvage_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Item item in items.Values.Where((Item list_item) => list_item.isRareForSalvage))
			{
				if (item.price < ectoSalvagePrice)
				{
					if (has_item(67027))
					{
						result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 67027));
					}
					else if (has_item(23045) || (has_item(19983) && items[19983].total_count() > 3))
					{
						result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 23045));
					}
					else
					{
						result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 23043));
					}
				}
				else if (item.isAccountBound)
				{
					result.Add(new ItemForDisplay(item, null, "Sell these items on the TP"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_craft_luck_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (int id in Magic.luckIds)
			{
				if (has_item(id) && items[id].total_count() > 250)
				{
					result.Add(new ItemForDisplay(items[id], items[id].sources, "Craft these items into higher luck tiers"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_just_delete_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			IEnumerable<Item> first = items.Values.Where((Item list_item) => list_item.isDeletable && !list_item.isSellable && !list_item.isSalvagable);
			IEnumerable<Item> unlocks = items.Values.Where((Item list_item) => list_item.isDeletable && (list_item.isAccountBound || list_item.isCharacterBound) && (this.unlocks.recipes.Contains(list_item.recipeId) || this.unlocks.minis.Contains(list_item.miniId) || list_item.skinId.All((int unlocked_skin) => this.unlocks.skins.Any((int potentialSkin) => unlocked_skin == potentialSkin))));
			foreach (Item item in first.Union(unlocks))
			{
				result.Add(new ItemForDisplay(item, null, "Delete these items"));
			}
			return result;
		}

		public List<ItemForDisplay> get_just_salvage_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Item item2 in items.Values.Where((Item list_item) => (Magic.magicLists.salvageIds.Contains(list_item.itemId) && list_item.itemId != Magic.ectoId) || (list_item.isDeletable && list_item.isSalvagable)))
			{
				if (item2.rarity == ItemRarity.Basic || item2.rarity == ItemRarity.Fine || (item2.rarity == ItemRarity.Rare && !item2.isRareForSalvage))
				{
					if (has_item(44602))
					{
						result.Add(new SalvageItemForDisplay(item2, null, "Salvage these items", 44602));
					}
					else
					{
						result.Add(new SalvageItemForDisplay(item2, null, "Salvage these items", 23040));
					}
				}
				else if ((item2.rarity == ItemRarity.Masterwork || item2.rarity == ItemRarity.Exotic) && has_item(89409))
				{
					result.Add(new SalvageItemForDisplay(item2, null, "Salvage these items", 89409));
				}
				else
				{
					result.Add(new SalvageItemForDisplay(item2, null, "Salvage these items", 67027));
				}
			}
			foreach (Item item in items.Values.Where((Item entry) => entry.rarity == ItemRarity.Ascended && (entry.type == ItemType.Armor || entry.type == ItemType.Weapon || entry.type == ItemType.Back || entry.type == ItemType.Trinket)))
			{
				if (Magic.magicLists.gaetingSalvage.Contains(item.itemId))
				{
					result.Add(new SalvageItemForDisplay(item, null, "Salvage these items for gaeting crystals", 73481));
					continue;
				}
				if (Magic.magicLists.magnetiteSalvage.Contains(item.itemId))
				{
					result.Add(new SalvageItemForDisplay(item, null, "Salvage these items for magnetite shards", 73481));
					continue;
				}
				switch (item.type)
				{
				case ItemType.Armor:
					log.Debug("found ascended armor: " + item.name + " of id: " + item.itemId + " ,type:" + item.type);
					if (legendaryArmory.mediumArmor.ContainsKey(item.armorType))
					{
						switch (item.armorWeight)
						{
						case ItemWeightType.Heavy:
							if (legendaryArmory.heavyArmor[item.armorType])
							{
								result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
							}
							break;
						case ItemWeightType.Medium:
							if (legendaryArmory.mediumArmor[item.armorType])
							{
								result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
							}
							break;
						case ItemWeightType.Light:
							if (legendaryArmory.lightArmor[item.armorType])
							{
								result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
							}
							break;
						}
					}
					else
					{
						log.Warn("Invalid legendary armor type of armor: " + item.name + " of id: " + item.itemId + " ,type:" + item.type.ToString() + " and armour type: " + item.armorType);
					}
					break;
				case ItemType.Weapon:
					log.Debug("found ascended weapon: " + item.name + " of id: " + item.itemId + " ,type:" + item.type);
					if (legendaryArmory.weapons.ContainsKey(item.weaponType))
					{
						if (Magic.singularWeaponTypes.Contains(item.weaponType))
						{
							if (legendaryArmory.weapons[item.weaponType] >= 1)
							{
								result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
							}
						}
						else if (legendaryArmory.weapons[item.weaponType] >= 2)
						{
							result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
						}
					}
					else
					{
						log.Warn("Invalid legendary weapon type of weapon: " + item.name + " of id: " + item.itemId + ", type: " + item.type.ToString() + " and weapon type: " + item.weaponType);
					}
					break;
				case ItemType.Trinket:
					log.Debug("found ascended trinket: " + item.name + " of id: " + item.itemId + " ,type:" + item.type);
					switch (item.trinketType)
					{
					case ItemTrinketType.Accessory:
						if (legendaryArmory.trinkets >= 2)
						{
							result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
						}
						break;
					case ItemTrinketType.Ring:
						if (legendaryArmory.rings >= 2)
						{
							result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
						}
						break;
					case ItemTrinketType.Amulet:
						if (legendaryArmory.amulet)
						{
							result.Add(new SalvageItemForDisplay(item, null, "Salvage these items", 73481));
						}
						break;
					}
					break;
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_play_to_consume_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (int id in Magic.gameplayConsumables.Keys)
			{
				if (has_item(id))
				{
					result.Add(new ItemForDisplay(items[id], null, Magic.gameplayConsumables[id]));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_gobbler_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Gobbler gobbler in Magic.gobblers)
			{
				if (!has_item(gobbler.itemId) || !has_item(gobbler.food))
				{
					continue;
				}
				foreach (int food in gobbler.food)
				{
					if (items[food].total_count() > Convert.ToUInt64(materialStorageSize))
					{
						result.Add(new GobblerItemForDisplay(items[food], items[food].sources, "Feed these items to gobblers", gobbler.itemId));
					}
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_misc_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (MiscAdvice advice2 in Magic.miscAdvices)
			{
				if (has_item(advice2.itemId) && items[advice2.itemId].total_count() >= Convert.ToUInt64(advice2.minCount))
				{
					result.Add(new ItemForDisplay(items[advice2.itemId], null, advice2.advice));
				}
			}
			foreach (CraftingMiscAdvice advice in Magic.craftingMiscAdvices.Values)
			{
				foreach (KeyValuePair<int, int> item5 in advice.idCountMapping)
				{
					if (has_item(item5.Key) && items[item5.Key].total_count() >= Convert.ToUInt64(item5.Value))
					{
						Item output = new Item(advice.outputId, isCharacterBound_: false, isAccountBound_: false);
						result.Add(new MiscCraftingItemForDisplay(items[item5.Key], output, "Craft: "));
					}
				}
			}
			Dictionary<int, List<string>> bagIds = new Dictionary<int, List<string>>();
			Dictionary<int, int> bagSize = new Dictionary<int, int>();
			foreach (KeyValuePair<string, List<InventoryBagSlot>> entry2 in inventoryBags)
			{
				foreach (InventoryBagSlot bag3 in entry2.Value)
				{
					if (!bagIds.ContainsKey(bag3.get_id()))
					{
						bagIds.Add(bag3.get_id(), new List<string>());
					}
					bagIds[bag3.get_id()].Add(entry2.Key);
					if (!bagSize.ContainsKey(bag3.get_id()))
					{
						bagSize.Add(bag3.get_id(), bag3.get_size());
					}
				}
			}
			foreach (KeyValuePair<int, int> entry in bagSize)
			{
				if (entry.Value < 18 && entry.Value != 0)
				{
					Item bag2 = new Item(entry.Key, isCharacterBound_: false, isAccountBound_: false);
					foreach (string source2 in bagIds[entry.Key])
					{
						bag2.add_source(new Source(1uL, source2));
					}
					result.Add(new MiscCraftingItemForDisplay(bag2, Magic.silkBag, "Upgrade these bags to"));
				}
				if (entry.Value >= 32 || entry.Value == 0 || !has_item(83410) || items[83410].total_count() < 12)
				{
					continue;
				}
				Item bag = new Item(entry.Key, isCharacterBound_: false, isAccountBound_: false);
				foreach (string source in bagIds[entry.Key])
				{
					bag.add_source(new Source(1uL, source));
				}
				result.Add(new MiscCraftingItemForDisplay(bag, Magic.silkBag, "Potentially replace these bags with"));
			}
			Item wizardGobbler = new Item(104963, isCharacterBound_: true, isAccountBound_: true);
			Item wizardScroll = new Item(104772, isCharacterBound_: true, isAccountBound_: true);
			foreach (int item4 in Magic.wizardGobblers)
			{
				if (has_item(item4))
				{
					if (has_item(wizardGobbler.itemId))
					{
						result.Add(new MiscCraftingItemForDisplay(items[item4], wizardGobbler, "Delete and use"));
					}
					else
					{
						result.Add(new MiscCraftingItemForDisplay(items[item4], wizardGobbler, "Delete and aquire"));
					}
				}
			}
			foreach (int item3 in Magic.wizardScrolls)
			{
				if (has_item(item3))
				{
					if (has_item(wizardScroll.itemId))
					{
						result.Add(new MiscCraftingItemForDisplay(items[item3], wizardScroll, "Delete and use"));
					}
					else
					{
						result.Add(new MiscCraftingItemForDisplay(items[item3], wizardScroll, "Delete and aquire"));
					}
				}
			}
			foreach (Item id in items.Values.Where((Item list_item) => list_item.isDeletable && !list_item.isAccountBound && !list_item.isCharacterBound && (unlocks.recipes.Contains(list_item.recipeId) || unlocks.minis.Contains(list_item.miniId) || list_item.skinId.All((int unlocked_skin) => unlocks.skins.Any((int potentialSkin) => unlocked_skin == potentialSkin)))))
			{
				result.Add(new ItemForDisplay(id, null, "Sell these items on the TP"));
			}
			foreach (Item item2 in items.Values)
			{
				if (item2.type == ItemType.Container)
				{
					if (!item2.isAccountBound)
					{
						result.Add(new ItemForDisplay(item2, null, "Open or sell these containers on the TP"));
					}
					else
					{
						result.Add(new ItemForDisplay(item2, null, "Open these containers"));
					}
				}
			}
			foreach (Item item in items.Values.Where((Item list_item) => list_item.type == ItemType.CraftingMaterial && list_item.total_count() > (ulong)materialStorageSize))
			{
				if (item.isAccountBound)
				{
					result.Add(new ItemForDisplay(item, null, "Sell excess materials to a vendor"));
				}
				else
				{
					result.Add(new ItemForDisplay(item, null, "Sell excess materials on the TP"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_karma_consumables_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (int item in Magic.magicLists.karmaIds)
			{
				if (has_item(item) && items[item].total_count() != 0)
				{
					result.Add(new ItemForDisplay(items[item], null, "Consume these items for karma"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_living_world_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (int item3 in Magic.lws3Id)
			{
				if (has_item(item3) && items[item3].total_count() > Convert.ToUInt64(materialStorageSize))
				{
					result.Add(new ItemForDisplay(items[item3], items[item3].sources, "Consume these items for unbound magic"));
				}
			}
			foreach (int item2 in Magic.lws4Id)
			{
				if (has_item(item2) && items[item2].total_count() > Convert.ToUInt64(materialStorageSize))
				{
					result.Add(new ItemForDisplay(items[item2], items[item2].sources, "Consume these items for volatile magic"));
				}
			}
			foreach (int item in Magic.ibsId)
			{
				if (has_item(item) && items[item].total_count() > Convert.ToUInt64(materialStorageSize))
				{
					result.Add(new ItemForDisplay(items[item], items[item].sources, "Convert these items to LWS4 currency"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_crafting_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (RecipeInfo recipe in craftableRecipes)
			{
				if (!recipeResults.ContainsKey(recipe.OutputItemId))
				{
					continue;
				}
				bool canCraft = true;
				bool hasMoreThanStackIngredient = false;
				Dictionary<RecipeIngredient, List<Source>> parsedIngredients = new Dictionary<RecipeIngredient, List<Source>>();
				List<string> parsedDisciplines = new List<string>();
				int cost = 0;
				int value = recipeResults[recipe.OutputItemId].price;
				foreach (RecipeIngredient ingredient in recipe.Ingredients)
				{
					if (items[ingredient.ItemId].total_count() < Convert.ToUInt64(ingredient.Count))
					{
						canCraft = false;
					}
					if (items[ingredient.ItemId].total_count() > Convert.ToUInt64(materialStorageSize))
					{
						hasMoreThanStackIngredient = true;
					}
					cost += ingredient.Count * items[ingredient.ItemId].price;
					parsedIngredients.Add(ingredient, items[ingredient.ItemId].sources);
				}
				foreach (int discipline in recipe.Disciplines)
				{
					parsedDisciplines.Add(Magic.get_local_discipline((CraftingDisciplineType)discipline));
				}
				if (canCraft && hasMoreThanStackIngredient && cost < value)
				{
					result.Add(new CraftingItemForDisplay(recipeResults[recipe.OutputItemId], parsedIngredients, "Craft these items", recipe.OutputItemId));
				}
			}
			return result;
		}
	}
}
