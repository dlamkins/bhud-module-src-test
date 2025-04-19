using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using SL.ChatLinks.Storage;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemSearch
	{
		[CompilerGenerated]
		private IDbContextFactory _003CcontextFactory_003EP;

		[CompilerGenerated]
		private ILocale _003Clocale_003EP;

		public ItemSearch(IDbContextFactory contextFactory, ILocale locale)
		{
			_003CcontextFactory_003EP = contextFactory;
			_003Clocale_003EP = locale;
			base._002Ector();
		}

		public async ValueTask<int> CountItems()
		{
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				return await context.Items.CountAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		public async IAsyncEnumerable<Item> NewItems(int limit)
		{
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await foreach (Item item in context.Items.OrderByDescending((Item item) => item.Id).Take(limit).AsAsyncEnumerable()
					.ConfigureAwait(continueOnCapturedContext: false))
				{
					yield return item;
				}
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		public async IAsyncEnumerable<Item> FilterItems(ItemsFilter filter, int limit, ResultContext resultContext, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			ItemsFilter filter2 = filter;
			ThrowHelper.ThrowIfNull(filter2, "filter");
			ThrowHelper.ThrowIfNull(resultContext, "resultContext");
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string category = filter2.Category;
				IQueryable<Item> queryable;
				if (category != null)
				{
					switch (category.Length)
					{
					case 5:
						break;
					case 8:
						goto IL_01eb;
					case 6:
						goto IL_0243;
					case 4:
						goto IL_02ab;
					case 12:
						goto IL_02fa;
					case 9:
						goto IL_0341;
					case 7:
						goto IL_0388;
					case 3:
						goto IL_03dc;
					case 10:
						goto IL_0406;
					case 11:
						goto IL_0445;
					case 14:
						goto IL_0483;
					case 18:
						goto IL_04a4;
					case 20:
						goto IL_04eb;
					case 13:
						goto IL_050c;
					case 15:
						goto IL_052d;
					case 16:
						goto IL_054e;
					case 19:
						goto IL_058c;
					case 17:
						goto IL_05ad;
					case 23:
						goto IL_0cf6;
					case 24:
						goto IL_0d0c;
					case 28:
						goto IL_0d7a;
					case 22:
						goto IL_0d90;
					case 27:
						goto IL_0da6;
					case 21:
						goto IL_0de8;
					default:
						goto IL_1708;
					}
					switch (category[3])
					{
					case 'o':
						break;
					case 's':
						goto IL_0614;
					case 't':
						goto IL_062a;
					case 'u':
						goto IL_0640;
					case 'l':
						goto IL_0656;
					case 'a':
						goto IL_066c;
					case 'f':
						goto IL_0682;
					case 'r':
						goto IL_0698;
					case 'c':
						goto IL_06ae;
					case 'z':
						goto IL_06c4;
					case 'm':
						goto IL_06da;
					case 'i':
						goto IL_06f0;
					case 'e':
						goto IL_0717;
					case 'p':
						goto IL_072d;
					default:
						goto IL_1708;
					}
					if (category == "armor")
					{
						queryable = context.Items.OfType<Armor>();
						goto IL_1710;
					}
				}
				goto IL_1708;
				IL_1710:
				IQueryable<Item> query2 = queryable;
				query2 = ((!string.IsNullOrWhiteSpace(filter2.Text)) ? (from item in query2
					where EF.Functions.Like(item.Name, $"%{filter2.Text}%")
					orderby Levenshtein.LevenshteinDistance(filter2.Text, item.Name)
					select item) : query2.OrderByDescending((Item item) => item.Id));
				resultContext.ResultTotal = await query2.CountAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await foreach (Item item in query2.Take(limit).AsAsyncEnumerable().WithCancellation(cancellationToken)
					.ConfigureAwait(continueOnCapturedContext: false))
				{
					yield return item;
				}
				goto end_IL_00d7;
				IL_0de8:
				if (!(category == "shared_inventory_slot"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<SharedInventorySlot>();
				goto IL_1710;
				IL_0717:
				if (!(category == "jewel"))
				{
					goto IL_1708;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND UpgradeComponentFlags -> '$.Trinket' = 'true'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
				goto IL_1710;
				IL_0da6:
				if (!(category == "mist_champion_skin_unlocker"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<MistChampionSkinUnlocker>();
				goto IL_1710;
				IL_0445:
				char c = category[0];
				if ((uint)c <= 104u)
				{
					if (c != 'f')
					{
						if (c != 'h' || !(category == "harpoon_gun"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<HarpoonGun>();
					}
					else
					{
						if (!(category == "fishing_rod"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<FishingRod>();
					}
				}
				else if (c != 'l')
				{
					if (c != 'm' || !(category == "mining_pick"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<MiningPick>();
				}
				else
				{
					if (!(category == "logging_axe"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<LoggingAxe>();
				}
				goto IL_1710;
				IL_0d90:
				if (!(category == "jade_bot_skin_unlocker"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<JadeBotSkinUnlocker>();
				goto IL_1710;
				IL_02ab:
				switch (category[3])
				{
				case 'm':
					break;
				case 'k':
					goto IL_08a3;
				case 'g':
					goto IL_08b9;
				case 'e':
					goto IL_08cf;
				case 'd':
					goto IL_0907;
				case 't':
					goto IL_091d;
				default:
					goto IL_1708;
				}
				if (!(category == "helm"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Helm>();
				goto IL_1710;
				IL_0d7a:
				if (!(category == "equipment_template_expansion"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<EquipmentTemplateExpansion>();
				goto IL_1710;
				IL_06c4:
				if (!(category == "booze"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Booze>();
				goto IL_1710;
				IL_0d0c:
				if (!(category == "build_template_expansion"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<BuildTemplateExpansion>();
				goto IL_1710;
				IL_098b:
				if (!(category == "recipe_sheet"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<RecipeSheet>();
				goto IL_1710;
				IL_0cf6:
				if (!(category == "build_storage_expansion"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<BuildStorageExpansion>();
				goto IL_1710;
				IL_08cf:
				switch (category)
				{
				case "mace":
					break;
				case "lure":
					goto IL_1498;
				case "rune":
					goto IL_155e;
				default:
					goto IL_1708;
				}
				queryable = context.Items.OfType<Mace>();
				goto IL_1710;
				IL_05ad:
				c = category[10];
				if ((uint)c <= 109u)
				{
					if (c != '_')
					{
						if (c != 'a')
						{
							if (c != 'm' || !(category == "upgrade_component"))
							{
								goto IL_1708;
							}
							queryable = context.Items.OfType<UpgradeComponent>();
						}
						else
						{
							if (!(category == "crafting_material"))
							{
								goto IL_1708;
							}
							queryable = context.Items.OfType<CraftingMaterial>();
						}
					}
					else
					{
						if (!(category == "harvesting_sickle"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<HarvestingSickle>();
					}
				}
				else if (c != 'n')
				{
					if (c != 't')
					{
						if (c != 'u' || !(category == "universal_upgrade"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Gem>();
					}
					else
					{
						if (!(category == "upgrade_extractor"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<UpgradeExtractor>();
					}
				}
				else
				{
					if (!(category == "default_container"))
					{
						goto IL_1708;
					}
					queryable = context.Items.Where((Item item) => EF.Property<string>(item, "Type") == "container");
				}
				goto IL_1710;
				IL_0907:
				if (!(category == "food"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Food>();
				goto IL_1710;
				IL_091d:
				if (!(category == "bait"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Bait>();
				goto IL_1710;
				IL_08b9:
				if (!(category == "ring"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Ring>();
				goto IL_1710;
				IL_06ae:
				if (!(category == "torch"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Torch>();
				goto IL_1710;
				IL_08a3:
				if (!(category == "back"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Backpack>();
				goto IL_1710;
				IL_02fa:
				switch (category[3])
				{
				case 'm':
					break;
				case 'g':
					goto IL_0949;
				case 'l':
					goto IL_095f;
				case 't':
					goto IL_0975;
				case 'i':
					goto IL_098b;
				case 'v':
					goto IL_09a1;
				default:
					goto IL_1708;
				}
				if (!(category == "helm_aquatic"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<HelmAquatic>();
				goto IL_1710;
				IL_0406:
				switch (category[2])
				{
				case 'e':
					break;
				case 'n':
					goto IL_0b17;
				case 'w':
					goto IL_0b2d;
				case 'r':
					goto IL_0b43;
				case 'p':
					goto IL_0b59;
				default:
					goto IL_1708;
				}
				if (!(category == "greatsword"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Greatsword>();
				goto IL_1710;
				IL_1498:
				queryable = context.Items.OfType<Lure>();
				goto IL_1710;
				IL_0b59:
				if (!(category == "expansions"))
				{
					goto IL_1708;
				}
				queryable = context.Items.Where((Item item) => item is BagSlotExpansion || item is BankTabExpansion || item is StorageExpander || item is BuildStorageExpansion || item is BuildTemplateExpansion || item is EquipmentTemplateExpansion || item is SharedInventorySlot);
				goto IL_1710;
				IL_155e:
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'rune'\r\nAND NOT EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				goto IL_1710;
				IL_0975:
				if (!(category == "contract_npc"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<ContractNpc>();
				goto IL_1710;
				IL_0243:
				switch (category[0])
				{
				case 'g':
					break;
				case 'a':
					goto IL_07f3;
				case 'w':
					goto IL_0809;
				case 'd':
					goto IL_081f;
				case 'h':
					goto IL_0835;
				case 'p':
					goto IL_084b;
				case 's':
					goto IL_0861;
				case 't':
					goto IL_0877;
				default:
					goto IL_1708;
				}
				if (!(category == "gloves"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Gloves>();
				goto IL_1710;
				IL_0877:
				if (!(category == "trophy"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Trophy>();
				goto IL_1710;
				IL_0b17:
				if (!(category == "consumable"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Consumable>();
				goto IL_1710;
				IL_0b2d:
				if (!(category == "power_core"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<PowerCore>();
				goto IL_1710;
				IL_095f:
				if (!(category == "small_bundle"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<SmallBundle>();
				goto IL_1710;
				IL_0b43:
				if (!(category == "enrichment"))
				{
					goto IL_1708;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'true'");
				goto IL_1710;
				IL_0698:
				if (!(category == "sword"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Sword>();
				goto IL_1710;
				IL_058c:
				c = category[0];
				if (c != 'i')
				{
					if (c != 'm' || !(category == "mount_skin_unlocker"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<MountSkinUnlocker>();
				}
				else
				{
					if (!(category == "immediate_container"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<ImmediateContainer>();
				}
				goto IL_1710;
				IL_06da:
				if (!(category == "gizmo"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Gizmo>();
				goto IL_1710;
				IL_0861:
				if (!(category == "shield"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Shield>();
				goto IL_1710;
				IL_084b:
				if (!(category == "pistol"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Pistol>();
				goto IL_1710;
				IL_03dc:
				c = category[0];
				if (c != 'a')
				{
					if (c != 'd')
					{
						if (c != 't' || !(category == "toy"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Toy>();
					}
					else
					{
						if (!(category == "dye"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Dye>();
					}
				}
				else
				{
					if (!(category == "axe"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<Axe>();
				}
				goto IL_1710;
				IL_0835:
				if (!(category == "hammer"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Hammer>();
				goto IL_1710;
				IL_054e:
				c = category[0];
				if ((uint)c <= 99u)
				{
					if (c != 'b')
					{
						if (c != 'c' || !(category == "content_unlocker"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<ContentUnlocker>();
					}
					else
					{
						if (!(category == "black_lion_chest"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<BlackLionChest>();
					}
				}
				else if (c != 'j')
				{
					if (c != 's' || !(category == "storage_expander"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<StorageExpander>();
				}
				else
				{
					if (!(category == "jade_tech_module"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<JadeTechModule>();
				}
				goto IL_1710;
				IL_0656:
				if (!(category == "rifle"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Rifle>();
				goto IL_1710;
				IL_081f:
				if (!(category == "dagger"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Dagger>();
				goto IL_1710;
				IL_06f0:
				if (!(category == "relic"))
				{
					if (!(category == "sigil"))
					{
						goto IL_1708;
					}
					queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'sigil'\r\nAND NOT EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				}
				else
				{
					queryable = context.Items.OfType<Relic>();
				}
				goto IL_1710;
				IL_0388:
				switch (category[3])
				{
				case 'n':
					break;
				case 'g':
					goto IL_0a3b;
				case 'p':
					goto IL_0a51;
				case 'd':
					goto IL_0a67;
				case 'h':
					goto IL_0a7d;
				case 'v':
					goto IL_0a93;
				case 'l':
					goto IL_0aa9;
				default:
					goto IL_1708;
				}
				if (!(category == "trinket"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Trinket>();
				goto IL_1710;
				IL_066c:
				if (!(category == "spear"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Spear>();
				goto IL_1710;
				IL_0aa9:
				if (!(category == "utility"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Utility>();
				goto IL_1710;
				IL_0483:
				c = category[0];
				if (c != 'g')
				{
					if (c != 't' || !(category == "toy_two_handed"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<ToyTwoHanded>();
				}
				else
				{
					if (!(category == "gathering_tool"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<GatheringTool>();
				}
				goto IL_1710;
				IL_07f3:
				if (!(category == "amulet"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Amulet>();
				goto IL_1710;
				IL_076f:
				if (!(category == "currency"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Currency>();
				goto IL_1710;
				IL_0a7d:
				if (!(category == "warhorn"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Warhorn>();
				goto IL_1710;
				IL_0a93:
				if (!(category == "service"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Service>();
				goto IL_1710;
				IL_09a1:
				if (!(category == "salvage_tool"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<SalvageTool>();
				goto IL_1710;
				IL_052d:
				c = category[0];
				if (c != 'o')
				{
					if (c != 'r' || !(category == "random_unlocker"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<RandomUnlocker>();
				}
				else
				{
					if (!(category == "outfit_unlocker"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<OutfitUnlocker>();
				}
				goto IL_1710;
				IL_0a67:
				if (!(category == "trident"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Trident>();
				goto IL_1710;
				IL_0a51:
				if (!(category == "scepter"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Scepter>();
				goto IL_1710;
				IL_01eb:
				switch (category[0])
				{
				case 'l':
					break;
				case 's':
					goto IL_0759;
				case 'c':
					goto IL_076f;
				case 'u':
					goto IL_0785;
				case 'g':
					goto IL_079b;
				case 'r':
					goto IL_07b1;
				case 'i':
					goto IL_07c7;
				default:
					goto IL_1708;
				}
				if (!(category == "leggings"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Leggings>();
				goto IL_1710;
				IL_0682:
				if (!(category == "staff"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Staff>();
				goto IL_1710;
				IL_0a3b:
				if (!(category == "longbow"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Longbow>();
				goto IL_1710;
				IL_050c:
				c = category[0];
				if (c != 'm')
				{
					if (c != 't' || !(category == "transmutation"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<Transmutation>();
				}
				else
				{
					if (!(category == "mount_license"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<MountLicense>();
				}
				goto IL_1710;
				IL_07c7:
				if (!(category == "infusion"))
				{
					goto IL_1708;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'true'");
				goto IL_1710;
				IL_0640:
				if (!(category == "focus"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Focus>();
				goto IL_1710;
				IL_0341:
				c = category[5];
				if ((uint)c <= 100u)
				{
					if (c != '_')
					{
						if (c != 'd' || !(category == "shoulders"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Shoulders>();
					}
					else
					{
						if (!(category == "sigil_pvp"))
						{
							goto IL_1708;
						}
						queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'sigil'\r\nAND EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
					}
				}
				else if (c != 'i')
				{
					if (c != 's')
					{
						if (c != 't' || !(category == "miniature"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Miniature>();
					}
					else
					{
						if (!(category == "accessory"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<Accessory>();
					}
				}
				else
				{
					if (!(category == "container"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<Container>();
				}
				goto IL_1710;
				IL_0809:
				if (!(category == "weapon"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Weapon>();
				goto IL_1710;
				IL_0614:
				if (!(category == "chest"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Coat>();
				goto IL_1710;
				IL_04eb:
				c = category[0];
				if (c != 'g')
				{
					if (c != 'h' || !(category == "halloween_consumable"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<HalloweenConsumable>();
				}
				else
				{
					if (!(category == "glider_skin_unlocker"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<GliderSkinUnlocker>();
				}
				goto IL_1710;
				IL_0949:
				if (!(category == "large_bundle"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<LargeBundle>();
				goto IL_1710;
				IL_0785:
				if (!(category == "unlocker"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Unlocker>();
				goto IL_1710;
				IL_079b:
				if (!(category == "gift_box"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<GiftBox>();
				goto IL_1710;
				IL_1708:
				queryable = context.Items;
				goto IL_1710;
				IL_062a:
				if (!(category == "boots"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Boots>();
				goto IL_1710;
				IL_04a4:
				c = category[0];
				if ((uint)c <= 98u)
				{
					if (c != 'a')
					{
						if (c != 'b')
						{
							goto IL_1708;
						}
						if (!(category == "bag_slot_expansion"))
						{
							if (!(category == "bank_tab_expansion"))
							{
								goto IL_1708;
							}
							queryable = context.Items.OfType<BankTabExpansion>();
						}
						else
						{
							queryable = context.Items.OfType<BagSlotExpansion>();
						}
					}
					else
					{
						if (!(category == "appearance_changer"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<AppearanceChanger>();
					}
				}
				else if (c != 'g')
				{
					if (c != 'm')
					{
						if (c != 't' || !(category == "teleport_to_friend"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<TeleportToFriend>();
					}
					else
					{
						if (!(category == "miniature_unlocker"))
						{
							goto IL_1708;
						}
						queryable = context.Items.OfType<MiniatureUnlocker>();
					}
				}
				else
				{
					if (!(category == "generic_consumable"))
					{
						goto IL_1708;
					}
					queryable = context.Items.OfType<GenericConsumable>();
				}
				goto IL_1710;
				IL_07b1:
				if (!(category == "rune_pvp"))
				{
					goto IL_1708;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'rune'\r\nAND EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				goto IL_1710;
				IL_0759:
				if (!(category == "shortbow"))
				{
					goto IL_1708;
				}
				queryable = context.Items.OfType<Shortbow>();
				goto IL_1710;
				IL_072d:
				if (!(category == "glyph"))
				{
					goto IL_1708;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND UpgradeComponentFlags -> '$.Axe' = 'false'\r\nAND UpgradeComponentFlags -> '$.Trinket' = 'false'\r\nAND UpgradeComponentFlags -> '$.MediumArmor' = 'false'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
				goto IL_1710;
				end_IL_00d7:;
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}
	}
}
