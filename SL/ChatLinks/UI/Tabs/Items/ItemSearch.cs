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
						goto IL_01e0;
					case 6:
						goto IL_0227;
					case 4:
						goto IL_028f;
					case 12:
						goto IL_02de;
					case 9:
						goto IL_0325;
					case 7:
						goto IL_0363;
					case 3:
						goto IL_03b7;
					case 10:
						goto IL_03e1;
					case 11:
						goto IL_040b;
					case 14:
						goto IL_0435;
					case 18:
						goto IL_0456;
					case 20:
						goto IL_049d;
					case 13:
						goto IL_04be;
					case 15:
						goto IL_04df;
					case 16:
						goto IL_0500;
					case 19:
						goto IL_053e;
					case 17:
						goto IL_055f;
					case 23:
						goto IL_0bee;
					case 24:
						goto IL_0c04;
					case 28:
						goto IL_0c72;
					case 22:
						goto IL_0c88;
					case 27:
						goto IL_0c9e;
					case 21:
						goto IL_0ce0;
					default:
						goto IL_13b2;
					}
					switch (category[3])
					{
					case 'o':
						break;
					case 's':
						goto IL_05bc;
					case 't':
						goto IL_05d2;
					case 'u':
						goto IL_05e8;
					case 'l':
						goto IL_05fe;
					case 'a':
						goto IL_0614;
					case 'f':
						goto IL_062a;
					case 'r':
						goto IL_0640;
					case 'c':
						goto IL_0656;
					case 'z':
						goto IL_066c;
					case 'm':
						goto IL_0682;
					case 'i':
						goto IL_0698;
					default:
						goto IL_13b2;
					}
					if (category == "armor")
					{
						queryable = context.Items.OfType<Armor>();
						goto IL_13ba;
					}
				}
				goto IL_13b2;
				IL_098b:
				if (!(category == "scepter"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Scepter>();
				goto IL_13ba;
				IL_0ce0:
				if (!(category == "shared_inventory_slot"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<SharedInventorySlot>();
				goto IL_13ba;
				IL_0325:
				char c = category[0];
				if ((uint)c <= 99u)
				{
					if (c != 'a')
					{
						if (c != 'c' || !(category == "container"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Container>();
					}
					else
					{
						if (!(category == "accessory"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Accessory>();
					}
				}
				else if (c != 'm')
				{
					if (c != 's' || !(category == "shoulders"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Shoulders>();
				}
				else
				{
					if (!(category == "miniature"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Miniature>();
				}
				goto IL_13ba;
				IL_0c9e:
				if (!(category == "mist_champion_skin_unlocker"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<MistChampionSkinUnlocker>();
				goto IL_13ba;
				IL_02de:
				switch (category[3])
				{
				case 'm':
					break;
				case 'g':
					goto IL_0899;
				case 'l':
					goto IL_08af;
				case 't':
					goto IL_08c5;
				case 'i':
					goto IL_08db;
				case 'v':
					goto IL_08f1;
				default:
					goto IL_13b2;
				}
				if (!(category == "helm_aquatic"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<HelmAquatic>();
				goto IL_13ba;
				IL_0c88:
				if (!(category == "jade_bot_skin_unlocker"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<JadeBotSkinUnlocker>();
				goto IL_13ba;
				IL_0743:
				if (!(category == "amulet"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Amulet>();
				goto IL_13ba;
				IL_0c72:
				if (!(category == "equipment_template_expansion"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<EquipmentTemplateExpansion>();
				goto IL_13ba;
				IL_08f1:
				if (!(category == "salvage_tool"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<SalvageTool>();
				goto IL_13ba;
				IL_0c04:
				if (!(category == "build_template_expansion"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<BuildTemplateExpansion>();
				goto IL_13ba;
				IL_05e8:
				if (!(category == "focus"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Focus>();
				goto IL_13ba;
				IL_0bee:
				if (!(category == "build_storage_expansion"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<BuildStorageExpansion>();
				goto IL_13ba;
				IL_05bc:
				if (!(category == "chest"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Coat>();
				goto IL_13ba;
				IL_055f:
				c = category[8];
				if ((uint)c <= 99u)
				{
					if (c != '_')
					{
						if (c != 'c' || !(category == "upgrade_component"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<UpgradeComponent>();
					}
					else
					{
						if (!(category == "crafting_material"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<CraftingMaterial>();
					}
				}
				else if (c != 'e')
				{
					if (c != 'l')
					{
						if (c != 'n' || !(category == "harvesting_sickle"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<HarvestingSickle>();
					}
					else
					{
						if (!(category == "universal_upgrade"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Gem>();
					}
				}
				else
				{
					if (!(category == "upgrade_extractor"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<UpgradeExtractor>();
				}
				goto IL_13ba;
				IL_0435:
				c = category[0];
				if (c != 'g')
				{
					if (c != 't' || !(category == "toy_two_handed"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<ToyTwoHanded>();
				}
				else
				{
					if (!(category == "gathering_tool"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<GatheringTool>();
				}
				goto IL_13ba;
				IL_08c5:
				if (!(category == "contract_npc"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<ContractNpc>();
				goto IL_13ba;
				IL_0759:
				if (!(category == "weapon"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Weapon>();
				goto IL_13ba;
				IL_08af:
				if (!(category == "small_bundle"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<SmallBundle>();
				goto IL_13ba;
				IL_0640:
				if (!(category == "sword"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Sword>();
				goto IL_13ba;
				IL_040b:
				c = category[0];
				if (c != 'h')
				{
					if (c != 'l')
					{
						if (c != 'm' || !(category == "mining_pick"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<MiningPick>();
					}
					else
					{
						if (!(category == "logging_axe"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<LoggingAxe>();
					}
				}
				else
				{
					if (!(category == "harpoon_gun"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<HarpoonGun>();
				}
				goto IL_13ba;
				IL_08db:
				if (!(category == "recipe_sheet"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<RecipeSheet>();
				goto IL_13ba;
				IL_05fe:
				if (!(category == "rifle"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Rifle>();
				goto IL_13ba;
				IL_05d2:
				if (!(category == "boots"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Boots>();
				goto IL_13ba;
				IL_0614:
				if (!(category == "spear"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Spear>();
				goto IL_13ba;
				IL_086d:
				if (!(category == "bait"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Bait>();
				goto IL_13ba;
				IL_028f:
				switch (category[3])
				{
				case 'm':
					break;
				case 'k':
					goto IL_07f3;
				case 'g':
					goto IL_0809;
				case 'e':
					goto IL_081f;
				case 'd':
					goto IL_0857;
				case 't':
					goto IL_086d;
				default:
					goto IL_13b2;
				}
				if (!(category == "helm"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Helm>();
				goto IL_13ba;
				IL_0899:
				if (!(category == "large_bundle"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<LargeBundle>();
				goto IL_13ba;
				IL_13b2:
				queryable = context.Items;
				goto IL_13ba;
				IL_0975:
				if (!(category == "longbow"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Longbow>();
				goto IL_13ba;
				IL_053e:
				c = category[0];
				if (c != 'i')
				{
					if (c != 'm' || !(category == "mount_skin_unlocker"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<MountSkinUnlocker>();
				}
				else
				{
					if (!(category == "immediate_container"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<ImmediateContainer>();
				}
				goto IL_13ba;
				IL_03e1:
				c = category[0];
				if (c != 'c')
				{
					if (c != 'g')
					{
						if (c != 'p' || !(category == "power_core"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<PowerCore>();
					}
					else
					{
						if (!(category == "greatsword"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Greatsword>();
					}
				}
				else
				{
					if (!(category == "consumable"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Consumable>();
				}
				goto IL_13ba;
				IL_081f:
				switch (category)
				{
				case "mace":
					break;
				case "lure":
					goto IL_12e0;
				case "rune":
					goto IL_1394;
				default:
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Mace>();
				goto IL_13ba;
				IL_12e0:
				queryable = context.Items.OfType<Lure>();
				goto IL_13ba;
				IL_0857:
				if (!(category == "food"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Food>();
				goto IL_13ba;
				IL_0682:
				if (!(category == "gizmo"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Gizmo>();
				goto IL_13ba;
				IL_0500:
				c = category[0];
				if ((uint)c <= 99u)
				{
					if (c != 'b')
					{
						if (c != 'c' || !(category == "content_unlocker"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<ContentUnlocker>();
					}
					else
					{
						if (!(category == "black_lion_chest"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<BlackLionChest>();
					}
				}
				else if (c != 'j')
				{
					if (c != 's' || !(category == "storage_expander"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<StorageExpander>();
				}
				else
				{
					if (!(category == "jade_tech_module"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<JadeTechModule>();
				}
				goto IL_13ba;
				IL_1394:
				queryable = context.Items.OfType<Rune>();
				goto IL_13ba;
				IL_0698:
				if (!(category == "relic"))
				{
					if (!(category == "sigil"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Sigil>();
				}
				else
				{
					queryable = context.Items.OfType<Relic>();
				}
				goto IL_13ba;
				IL_03b7:
				c = category[0];
				if (c != 'a')
				{
					if (c != 'd')
					{
						if (c != 't' || !(category == "toy"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Toy>();
					}
					else
					{
						if (!(category == "dye"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Dye>();
					}
				}
				else
				{
					if (!(category == "axe"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Axe>();
				}
				goto IL_13ba;
				IL_07f3:
				if (!(category == "back"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Backpack>();
				goto IL_13ba;
				IL_079b:
				if (!(category == "pistol"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Pistol>();
				goto IL_13ba;
				IL_062a:
				if (!(category == "staff"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Staff>();
				goto IL_13ba;
				IL_0809:
				if (!(category == "ring"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Ring>();
				goto IL_13ba;
				IL_0227:
				switch (category[0])
				{
				case 'g':
					break;
				case 'a':
					goto IL_0743;
				case 'w':
					goto IL_0759;
				case 'd':
					goto IL_076f;
				case 'h':
					goto IL_0785;
				case 'p':
					goto IL_079b;
				case 's':
					goto IL_07b1;
				case 't':
					goto IL_07c7;
				default:
					goto IL_13b2;
				}
				if (!(category == "gloves"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Gloves>();
				goto IL_13ba;
				IL_13ba:
				IQueryable<Item> query = queryable;
				if (!string.IsNullOrWhiteSpace(filter2.Text))
				{
					query = from item in query
						where EF.Functions.Like(item.Name, $"%{filter2.Text}%")
						orderby Levenshtein.LevenshteinDistance(filter2.Text, item.Name)
						select item;
				}
				resultContext.ResultTotal = await query.CountAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await foreach (Item item in query.OrderByDescending((Item item) => item.Id).Take(limit).AsAsyncEnumerable()
					.WithCancellation(cancellationToken)
					.ConfigureAwait(continueOnCapturedContext: false))
				{
					yield return item;
				}
				goto end_IL_00d7;
				IL_066c:
				if (!(category == "booze"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Booze>();
				goto IL_13ba;
				IL_07c7:
				if (!(category == "trophy"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Trophy>();
				goto IL_13ba;
				IL_0456:
				c = category[0];
				if ((uint)c <= 98u)
				{
					if (c != 'a')
					{
						if (c != 'b')
						{
							goto IL_13b2;
						}
						if (!(category == "bag_slot_expansion"))
						{
							if (!(category == "bank_tab_expansion"))
							{
								goto IL_13b2;
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
							goto IL_13b2;
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
							goto IL_13b2;
						}
						queryable = context.Items.OfType<TeleportToFriend>();
					}
					else
					{
						if (!(category == "miniature_unlocker"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<MiniatureUnlocker>();
					}
				}
				else
				{
					if (!(category == "generic_consumable"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<GenericConsumable>();
				}
				goto IL_13ba;
				IL_04df:
				c = category[0];
				if (c != 'o')
				{
					if (c != 'r' || !(category == "random_unlocker"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<RandomUnlocker>();
				}
				else
				{
					if (!(category == "outfit_unlocker"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<OutfitUnlocker>();
				}
				goto IL_13ba;
				IL_07b1:
				if (!(category == "shield"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Shield>();
				goto IL_13ba;
				IL_09e3:
				if (!(category == "utility"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Utility>();
				goto IL_13ba;
				IL_0656:
				if (!(category == "torch"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Torch>();
				goto IL_13ba;
				IL_0363:
				switch (category[3])
				{
				case 'n':
					break;
				case 'g':
					goto IL_0975;
				case 'p':
					goto IL_098b;
				case 'd':
					goto IL_09a1;
				case 'h':
					goto IL_09b7;
				case 'v':
					goto IL_09cd;
				case 'l':
					goto IL_09e3;
				default:
					goto IL_13b2;
				}
				if (!(category == "trinket"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Trinket>();
				goto IL_13ba;
				IL_09cd:
				if (!(category == "service"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Service>();
				goto IL_13ba;
				IL_04be:
				c = category[0];
				if (c != 'm')
				{
					if (c != 't' || !(category == "transmutation"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Transmutation>();
				}
				else
				{
					if (!(category == "mount_license"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<MountLicense>();
				}
				goto IL_13ba;
				IL_0785:
				if (!(category == "hammer"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Hammer>();
				goto IL_13ba;
				IL_01e0:
				c = category[0];
				if ((uint)c <= 103u)
				{
					if (c != 'c')
					{
						if (c != 'g' || !(category == "gift_box"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<GiftBox>();
					}
					else
					{
						if (!(category == "currency"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Currency>();
					}
				}
				else if (c != 'l')
				{
					if (c != 's')
					{
						if (c != 'u' || !(category == "unlocker"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Unlocker>();
					}
					else
					{
						if (!(category == "shortbow"))
						{
							goto IL_13b2;
						}
						queryable = context.Items.OfType<Shortbow>();
					}
				}
				else
				{
					if (!(category == "leggings"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<Leggings>();
				}
				goto IL_13ba;
				IL_09a1:
				if (!(category == "trident"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Trident>();
				goto IL_13ba;
				IL_09b7:
				if (!(category == "warhorn"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Warhorn>();
				goto IL_13ba;
				IL_076f:
				if (!(category == "dagger"))
				{
					goto IL_13b2;
				}
				queryable = context.Items.OfType<Dagger>();
				goto IL_13ba;
				IL_049d:
				c = category[0];
				if (c != 'g')
				{
					if (c != 'h' || !(category == "halloween_consumable"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<HalloweenConsumable>();
				}
				else
				{
					if (!(category == "glider_skin_unlocker"))
					{
						goto IL_13b2;
					}
					queryable = context.Items.OfType<GliderSkinUnlocker>();
				}
				goto IL_13ba;
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
