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
						goto IL_046f;
					case 18:
						goto IL_0490;
					case 20:
						goto IL_04d7;
					case 13:
						goto IL_04f8;
					case 15:
						goto IL_0519;
					case 16:
						goto IL_053a;
					case 19:
						goto IL_0578;
					case 17:
						goto IL_0599;
					case 23:
						goto IL_0ccc;
					case 24:
						goto IL_0ce2;
					case 28:
						goto IL_0d50;
					case 22:
						goto IL_0d66;
					case 27:
						goto IL_0d7c;
					case 21:
						goto IL_0dbe;
					default:
						goto IL_16cc;
					}
					switch (category[3])
					{
					case 'o':
						break;
					case 's':
						goto IL_0600;
					case 't':
						goto IL_0616;
					case 'u':
						goto IL_062c;
					case 'l':
						goto IL_0642;
					case 'a':
						goto IL_0658;
					case 'f':
						goto IL_066e;
					case 'r':
						goto IL_0684;
					case 'c':
						goto IL_069a;
					case 'z':
						goto IL_06b0;
					case 'm':
						goto IL_06c6;
					case 'i':
						goto IL_06dc;
					case 'e':
						goto IL_0703;
					case 'p':
						goto IL_0719;
					default:
						goto IL_16cc;
					}
					if (category == "armor")
					{
						queryable = context.Items.OfType<Armor>();
						goto IL_16d4;
					}
				}
				goto IL_16cc;
				IL_02fa:
				switch (category[3])
				{
				case 'm':
					break;
				case 'g':
					goto IL_0935;
				case 'l':
					goto IL_094b;
				case 't':
					goto IL_0961;
				case 'i':
					goto IL_0977;
				case 'v':
					goto IL_098d;
				default:
					goto IL_16cc;
				}
				if (!(category == "helm_aquatic"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<HelmAquatic>();
				goto IL_16d4;
				IL_0dbe:
				if (!(category == "shared_inventory_slot"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<SharedInventorySlot>();
				goto IL_16d4;
				IL_08f3:
				if (!(category == "food"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Food>();
				goto IL_16d4;
				IL_0d7c:
				if (!(category == "mist_champion_skin_unlocker"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<MistChampionSkinUnlocker>();
				goto IL_16d4;
				IL_0445:
				char c = category[0];
				if (c != 'h')
				{
					if (c != 'l')
					{
						if (c != 'm' || !(category == "mining_pick"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<MiningPick>();
					}
					else
					{
						if (!(category == "logging_axe"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<LoggingAxe>();
					}
				}
				else
				{
					if (!(category == "harpoon_gun"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<HarpoonGun>();
				}
				goto IL_16d4;
				IL_0d66:
				if (!(category == "jade_bot_skin_unlocker"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<JadeBotSkinUnlocker>();
				goto IL_16d4;
				IL_094b:
				if (!(category == "small_bundle"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<SmallBundle>();
				goto IL_16d4;
				IL_0d50:
				if (!(category == "equipment_template_expansion"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<EquipmentTemplateExpansion>();
				goto IL_16d4;
				IL_1522:
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'rune'\r\nAND NOT EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				goto IL_16d4;
				IL_0ce2:
				if (!(category == "build_template_expansion"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<BuildTemplateExpansion>();
				goto IL_16d4;
				IL_069a:
				if (!(category == "torch"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Torch>();
				goto IL_16d4;
				IL_0ccc:
				if (!(category == "build_storage_expansion"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<BuildStorageExpansion>();
				goto IL_16d4;
				IL_08a5:
				if (!(category == "ring"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Ring>();
				goto IL_16d4;
				IL_0599:
				c = category[10];
				if ((uint)c <= 109u)
				{
					if (c != '_')
					{
						if (c != 'a')
						{
							if (c != 'm' || !(category == "upgrade_component"))
							{
								goto IL_16cc;
							}
							queryable = context.Items.OfType<UpgradeComponent>();
						}
						else
						{
							if (!(category == "crafting_material"))
							{
								goto IL_16cc;
							}
							queryable = context.Items.OfType<CraftingMaterial>();
						}
					}
					else
					{
						if (!(category == "harvesting_sickle"))
						{
							goto IL_16cc;
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
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Gem>();
					}
					else
					{
						if (!(category == "upgrade_extractor"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<UpgradeExtractor>();
					}
				}
				else
				{
					if (!(category == "default_container"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.Where((Item item) => EF.Property<string>(item, "Type") == "container");
				}
				goto IL_16d4;
				IL_145c:
				queryable = context.Items.OfType<Lure>();
				goto IL_16d4;
				IL_08bb:
				switch (category)
				{
				case "mace":
					break;
				case "lure":
					goto IL_145c;
				case "rune":
					goto IL_1522;
				default:
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Mace>();
				goto IL_16d4;
				IL_0406:
				switch (category[2])
				{
				case 'e':
					break;
				case 'n':
					goto IL_0b03;
				case 'w':
					goto IL_0b19;
				case 'r':
					goto IL_0b2f;
				case 'p':
					goto IL_0b45;
				default:
					goto IL_16cc;
				}
				if (!(category == "greatsword"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Greatsword>();
				goto IL_16d4;
				IL_0935:
				if (!(category == "large_bundle"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<LargeBundle>();
				goto IL_16d4;
				IL_0b2f:
				if (!(category == "enrichment"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'true'");
				goto IL_16d4;
				IL_0b45:
				if (!(category == "expansions"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.Where((Item item) => item is BagSlotExpansion || item is BankTabExpansion || item is StorageExpander || item is BuildStorageExpansion || item is BuildTemplateExpansion || item is EquipmentTemplateExpansion || item is SharedInventorySlot);
				goto IL_16d4;
				IL_0243:
				switch (category[0])
				{
				case 'g':
					break;
				case 'a':
					goto IL_07df;
				case 'w':
					goto IL_07f5;
				case 'd':
					goto IL_080b;
				case 'h':
					goto IL_0821;
				case 'p':
					goto IL_0837;
				case 's':
					goto IL_084d;
				case 't':
					goto IL_0863;
				default:
					goto IL_16cc;
				}
				if (!(category == "gloves"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Gloves>();
				goto IL_16d4;
				IL_088f:
				if (!(category == "back"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Backpack>();
				goto IL_16d4;
				IL_0b19:
				if (!(category == "power_core"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<PowerCore>();
				goto IL_16d4;
				IL_06dc:
				if (!(category == "relic"))
				{
					if (!(category == "sigil"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'sigil'\r\nAND NOT EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				}
				else
				{
					queryable = context.Items.OfType<Relic>();
				}
				goto IL_16d4;
				IL_0863:
				if (!(category == "trophy"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Trophy>();
				goto IL_16d4;
				IL_0977:
				if (!(category == "recipe_sheet"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<RecipeSheet>();
				goto IL_16d4;
				IL_06c6:
				if (!(category == "gizmo"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Gizmo>();
				goto IL_16d4;
				IL_03dc:
				c = category[0];
				if (c != 'a')
				{
					if (c != 'd')
					{
						if (c != 't' || !(category == "toy"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Toy>();
					}
					else
					{
						if (!(category == "dye"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Dye>();
					}
				}
				else
				{
					if (!(category == "axe"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<Axe>();
				}
				goto IL_16d4;
				IL_0684:
				if (!(category == "sword"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Sword>();
				goto IL_16d4;
				IL_084d:
				if (!(category == "shield"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Shield>();
				goto IL_16d4;
				IL_0b03:
				if (!(category == "consumable"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Consumable>();
				goto IL_16d4;
				IL_0837:
				if (!(category == "pistol"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Pistol>();
				goto IL_16d4;
				IL_0578:
				c = category[0];
				if (c != 'i')
				{
					if (c != 'm' || !(category == "mount_skin_unlocker"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<MountSkinUnlocker>();
				}
				else
				{
					if (!(category == "immediate_container"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<ImmediateContainer>();
				}
				goto IL_16d4;
				IL_0821:
				if (!(category == "hammer"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Hammer>();
				goto IL_16d4;
				IL_046f:
				c = category[0];
				if (c != 'g')
				{
					if (c != 't' || !(category == "toy_two_handed"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<ToyTwoHanded>();
				}
				else
				{
					if (!(category == "gathering_tool"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<GatheringTool>();
				}
				goto IL_16d4;
				IL_080b:
				if (!(category == "dagger"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Dagger>();
				goto IL_16d4;
				IL_0642:
				if (!(category == "rifle"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Rifle>();
				goto IL_16d4;
				IL_0658:
				if (!(category == "spear"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Spear>();
				goto IL_16d4;
				IL_053a:
				c = category[0];
				if ((uint)c <= 99u)
				{
					if (c != 'b')
					{
						if (c != 'c' || !(category == "content_unlocker"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<ContentUnlocker>();
					}
					else
					{
						if (!(category == "black_lion_chest"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<BlackLionChest>();
					}
				}
				else if (c != 'j')
				{
					if (c != 's' || !(category == "storage_expander"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<StorageExpander>();
				}
				else
				{
					if (!(category == "jade_tech_module"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<JadeTechModule>();
				}
				goto IL_16d4;
				IL_0703:
				if (!(category == "jewel"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND UpgradeComponentFlags -> '$.Trinket' = 'true'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
				goto IL_16d4;
				IL_0a95:
				if (!(category == "utility"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Utility>();
				goto IL_16d4;
				IL_0a7f:
				if (!(category == "service"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Service>();
				goto IL_16d4;
				IL_0909:
				if (!(category == "bait"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Bait>();
				goto IL_16d4;
				IL_07df:
				if (!(category == "amulet"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Amulet>();
				goto IL_16d4;
				IL_0a69:
				if (!(category == "warhorn"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Warhorn>();
				goto IL_16d4;
				IL_0388:
				switch (category[3])
				{
				case 'n':
					break;
				case 'g':
					goto IL_0a27;
				case 'p':
					goto IL_0a3d;
				case 'd':
					goto IL_0a53;
				case 'h':
					goto IL_0a69;
				case 'v':
					goto IL_0a7f;
				case 'l':
					goto IL_0a95;
				default:
					goto IL_16cc;
				}
				if (!(category == "trinket"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Trinket>();
				goto IL_16d4;
				IL_0961:
				if (!(category == "contract_npc"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<ContractNpc>();
				goto IL_16d4;
				IL_066e:
				if (!(category == "staff"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Staff>();
				goto IL_16d4;
				IL_0a3d:
				if (!(category == "scepter"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Scepter>();
				goto IL_16d4;
				IL_0a53:
				if (!(category == "trident"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Trident>();
				goto IL_16d4;
				IL_01eb:
				switch (category[0])
				{
				case 'l':
					break;
				case 's':
					goto IL_0745;
				case 'c':
					goto IL_075b;
				case 'u':
					goto IL_0771;
				case 'g':
					goto IL_0787;
				case 'r':
					goto IL_079d;
				case 'i':
					goto IL_07b3;
				default:
					goto IL_16cc;
				}
				if (!(category == "leggings"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Leggings>();
				goto IL_16d4;
				IL_0519:
				c = category[0];
				if (c != 'o')
				{
					if (c != 'r' || !(category == "random_unlocker"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<RandomUnlocker>();
				}
				else
				{
					if (!(category == "outfit_unlocker"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<OutfitUnlocker>();
				}
				goto IL_16d4;
				IL_0a27:
				if (!(category == "longbow"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Longbow>();
				goto IL_16d4;
				IL_07f5:
				if (!(category == "weapon"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Weapon>();
				goto IL_16d4;
				IL_062c:
				if (!(category == "focus"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Focus>();
				goto IL_16d4;
				IL_07b3:
				if (!(category == "infusion"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'true'");
				goto IL_16d4;
				IL_0341:
				c = category[5];
				if ((uint)c <= 100u)
				{
					if (c != '_')
					{
						if (c != 'd' || !(category == "shoulders"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Shoulders>();
					}
					else
					{
						if (!(category == "sigil_pvp"))
						{
							goto IL_16cc;
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
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Miniature>();
					}
					else
					{
						if (!(category == "accessory"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<Accessory>();
					}
				}
				else
				{
					if (!(category == "container"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<Container>();
				}
				goto IL_16d4;
				IL_04f8:
				c = category[0];
				if (c != 'm')
				{
					if (c != 't' || !(category == "transmutation"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<Transmutation>();
				}
				else
				{
					if (!(category == "mount_license"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<MountLicense>();
				}
				goto IL_16d4;
				IL_0600:
				if (!(category == "chest"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Coat>();
				goto IL_16d4;
				IL_098d:
				if (!(category == "salvage_tool"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<SalvageTool>();
				goto IL_16d4;
				IL_0771:
				if (!(category == "unlocker"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Unlocker>();
				goto IL_16d4;
				IL_02ab:
				switch (category[3])
				{
				case 'm':
					break;
				case 'k':
					goto IL_088f;
				case 'g':
					goto IL_08a5;
				case 'e':
					goto IL_08bb;
				case 'd':
					goto IL_08f3;
				case 't':
					goto IL_0909;
				default:
					goto IL_16cc;
				}
				if (!(category == "helm"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Helm>();
				goto IL_16d4;
				IL_0787:
				if (!(category == "gift_box"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<GiftBox>();
				goto IL_16d4;
				IL_04d7:
				c = category[0];
				if (c != 'g')
				{
					if (c != 'h' || !(category == "halloween_consumable"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<HalloweenConsumable>();
				}
				else
				{
					if (!(category == "glider_skin_unlocker"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<GliderSkinUnlocker>();
				}
				goto IL_16d4;
				IL_16cc:
				queryable = context.Items;
				goto IL_16d4;
				IL_075b:
				if (!(category == "currency"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Currency>();
				goto IL_16d4;
				IL_0745:
				if (!(category == "shortbow"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Shortbow>();
				goto IL_16d4;
				IL_079d:
				if (!(category == "rune_pvp"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items item\r\nWHERE item.Type = 'rune'\r\nAND EXISTS (\r\n\tSELECT 1\r\n\tFROM json_each(GameTypes)\r\n\tWHERE json_each.value = 'Pvp'\r\n)");
				goto IL_16d4;
				IL_0616:
				if (!(category == "boots"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Boots>();
				goto IL_16d4;
				IL_0490:
				c = category[0];
				if ((uint)c <= 98u)
				{
					if (c != 'a')
					{
						if (c != 'b')
						{
							goto IL_16cc;
						}
						if (!(category == "bag_slot_expansion"))
						{
							if (!(category == "bank_tab_expansion"))
							{
								goto IL_16cc;
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
							goto IL_16cc;
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
							goto IL_16cc;
						}
						queryable = context.Items.OfType<TeleportToFriend>();
					}
					else
					{
						if (!(category == "miniature_unlocker"))
						{
							goto IL_16cc;
						}
						queryable = context.Items.OfType<MiniatureUnlocker>();
					}
				}
				else
				{
					if (!(category == "generic_consumable"))
					{
						goto IL_16cc;
					}
					queryable = context.Items.OfType<GenericConsumable>();
				}
				goto IL_16d4;
				IL_06b0:
				if (!(category == "booze"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.OfType<Booze>();
				goto IL_16d4;
				IL_16d4:
				IQueryable<Item> query2 = queryable;
				query2 = (string.IsNullOrWhiteSpace(filter2.Text) ? query2.OrderByDescending((Item item) => item.Id) : (from item in query2
					where EF.Functions.Like(item.Name, $"%{filter2.Text}%")
					orderby Levenshtein.LevenshteinDistance(filter2.Text, item.Name)
					select item));
				resultContext.ResultTotal = await query2.CountAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await foreach (Item item in query2.Take(limit).AsAsyncEnumerable().WithCancellation(cancellationToken)
					.ConfigureAwait(continueOnCapturedContext: false))
				{
					yield return item;
				}
				goto end_IL_00d7;
				IL_0719:
				if (!(category == "glyph"))
				{
					goto IL_16cc;
				}
				queryable = context.Items.FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\nAND UpgradeComponentFlags -> '$.Axe' = 'false'\r\nAND UpgradeComponentFlags -> '$.Trinket' = 'false'\r\nAND UpgradeComponentFlags -> '$.MediumArmor' = 'false'\r\nAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\nAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
				goto IL_16d4;
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
