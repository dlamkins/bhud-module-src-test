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

		private Dictionary<int, List<Source>> upgradableBags;

		public void reset_state()
		{
			foreach (KeyValuePair<int, Item> item in items)
			{
				item.Value.sources.Clear();
			}
			materialStorageSize = 0;
			craftableRecipes = new List<RecipeInfo>();
			recipeResults = new Dictionary<int, Item>();
			appraisedItemIds = new List<int>();
			includeConsumables = true;
			ectoSalvagePrice = 0;
			upgradableBags = new Dictionary<int, List<Source>>();
			validData = false;
		}

		public Model(Logger log_)
		{
			log = log_;
			items = new Dictionary<int, Item>();
			reset_state();
		}

		public async Task setup(Gw2Api api_)
		{
			validData = false;
			log.Info("started building ecto price");
			await build_ecto_price(api_);
			log.Info("started building inventory");
			await build_inventory(api_);
			log.Info("started building recipes");
			await build_recipe_info();
			log.Info("started building prices");
			await build_item_prices(api_);
			Magic.inventoryBag.build_basic_item_info();
			validData = true;
		}

		public void add_item(int id_, bool isAccountBound_, bool isCharacterBound_, Source source_)
		{
			if (!items.ContainsKey(id_))
			{
				items.Add(id_, new Item(id_, isCharacterBound_, isAccountBound_));
			}
			items[id_].add_source(source_);
			if (items[id_].isRareForSalvage)
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
				foreach (CharacterInventoryBag bag in character.Bags!)
				{
					if (bag == null)
					{
						continue;
					}
					if (bag.Size < 32)
					{
						if (upgradableBags.ContainsKey(bag.Size))
						{
							bool found = false;
							foreach (Source source in upgradableBags[bag.Size])
							{
								if (source.place == character.Name)
								{
									source.count++;
									found = true;
								}
							}
							if (!found)
							{
								upgradableBags[bag.Size].Add(new Source(1uL, character.Name));
							}
						}
						else
						{
							upgradableBags.Add(bag.Size, new List<Source>
							{
								new Source(1uL, character.Name)
							});
						}
					}
					foreach (AccountItem item4 in bag?.Inventory)
					{
						if (item4 == null)
						{
							emptySlots++;
							continue;
						}
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
			materialStorageSize = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(maxCount / 250uL)) * 250.0);
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
					continue;
				}
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
					result.Add(new ItemForDisplay(item, null, "Salvage these items"));
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
			foreach (Item item in items.Values.Where((Item list_item) => list_item.isDeletable && !list_item.isSellable && !list_item.isSalvagable))
			{
				result.Add(new ItemForDisplay(item, null, "Delete these items"));
			}
			return result;
		}

		public List<ItemForDisplay> get_just_salvage_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (Item item in items.Values.Where((Item list_item) => (Magic.salvageIds.Contains(list_item.itemId) && list_item.itemId != Magic.ectoId) || (list_item.isDeletable && list_item.isSalvagable)))
			{
				result.Add(new ItemForDisplay(item, null, "Salvage these items"));
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
						result.Add(new ItemForDisplay(items[food], items[food].sources, "Feed these items to gobblers", gobbler.itemId));
					}
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_misc_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (MiscAdvice advice in Magic.miscAdvices)
			{
				if (has_item(advice.itemId) && items[advice.itemId].total_count() >= Convert.ToUInt64(advice.minCount))
				{
					result.Add(new ItemForDisplay(items[advice.itemId], null, advice.advice));
				}
			}
			foreach (KeyValuePair<int, List<Source>> slots in upgradableBags)
			{
				if (slots.Key < 18)
				{
					result.Add(new ItemForDisplay(Magic.inventoryBag, slots.Value, "Upgrade these bags to 18 slots"));
				}
				if (has_item(83410) && slots.Key < 32 && items[83410].total_count() >= 12)
				{
					result.Add(new ItemForDisplay(Magic.inventoryBag, slots.Value, "Potentially replace these bags with boreal trunks"));
				}
			}
			return result;
		}

		public List<ItemForDisplay> get_karma_consumables_advice()
		{
			List<ItemForDisplay> result = new List<ItemForDisplay>();
			foreach (int item in Magic.karmaIds)
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
				List<IngredientSource> parsedIngredients = new List<IngredientSource>();
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
					parsedIngredients.Add(new IngredientSource(Convert.ToUInt64(ingredient.Count), ingredient.ItemId));
				}
				foreach (int discipline in recipe.Disciplines)
				{
					parsedDisciplines.Add(Magic.get_local_discipline((CraftingDisciplineType)discipline));
				}
				if (canCraft && hasMoreThanStackIngredient && cost < value)
				{
					result.Add(new ItemForDisplay(recipeResults[recipe.OutputItemId], new List<Source>
					{
						new RecipeSource(parsedIngredients, parsedDisciplines)
					}, "Craft these items"));
				}
			}
			return result;
		}
	}
}
