using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2.Chat;
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

		private static readonly Regex ChatLinkPattern = new Regex("^\\[&[A-Za-z0-9+/=]+\\]$", RegexOptions.Compiled);

		public ItemSearch(IDbContextFactory contextFactory, ILocale locale)
		{
			_003CcontextFactory_003EP = contextFactory;
			_003Clocale_003EP = locale;
			base._002Ector();
		}

		public async ValueTask<int> CountItems()
		{
			await using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			return await context.Items.CountAsync();
		}

		public async IAsyncEnumerable<Item> NewItems(int limit)
		{
			await using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			await foreach (Item item in (from item in context.Items.AsNoTracking()
				orderby item.Id descending
				select item).Take(limit).AsAsyncEnumerable())
			{
				yield return item;
			}
		}

		public async IAsyncEnumerable<Item> Search(string searchText, int limit, ResultContext resultContext, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			if (ChatLinkPattern.IsMatch(searchText))
			{
				ItemLink chatLink = ItemLink.Parse(searchText);
				await foreach (Item item in SearchByChatLink(chatLink, resultContext, cancellationToken))
				{
					yield return item;
				}
				yield break;
			}
			await using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			IQueryable<Item> query = context.Items.FromSqlInterpolated($"SELECT * FROM Items\r\nWHERE Name LIKE '%' || {searchText} || '%'\r\nORDER BY LevenshteinDistance({searchText}, Name)");
			resultContext.ResultTotal = await query.CountAsync(cancellationToken);
			await foreach (Item item2 in query.AsNoTracking().Take(limit).AsAsyncEnumerable()
				.WithCancellation(cancellationToken))
			{
				yield return item2;
			}
		}

		private async IAsyncEnumerable<Item> SearchByChatLink(ItemLink link, ResultContext resultContext, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			ItemLink link2 = link;
			await using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			Item item = await context.Items.AsNoTracking().SingleOrDefaultAsync((Item row) => row.Id == link2.ItemId, cancellationToken);
			if ((object)item == null)
			{
				yield break;
			}
			resultContext.ResultTotal++;
			yield return item;
			HashSet<int> relatedItems = new HashSet<int>();
			if (link2.SuffixItemId.HasValue)
			{
				relatedItems.Add(link2.SuffixItemId.Value);
			}
			if (link2.SecondarySuffixItemId.HasValue)
			{
				relatedItems.Add(link2.SecondarySuffixItemId.Value);
			}
			Weapon weapon = item as Weapon;
			if ((object)weapon == null)
			{
				Armor armor = item as Armor;
				if ((object)armor == null)
				{
					Backpack back = item as Backpack;
					if ((object)back == null)
					{
						Trinket trinket = item as Trinket;
						if ((object)trinket == null)
						{
							CraftingMaterial material = item as CraftingMaterial;
							if ((object)material != null)
							{
								foreach (InfusionSlotUpgradePath upgrade2 in material.UpgradesInto)
								{
									relatedItems.Add(upgrade2.ItemId);
								}
							}
						}
						else
						{
							if (trinket.SuffixItemId.HasValue)
							{
								relatedItems.Add(trinket.SuffixItemId.Value);
							}
							foreach (InfusionSlot slot4 in trinket.InfusionSlots)
							{
								if (slot4.ItemId.HasValue)
								{
									relatedItems.Add(slot4.ItemId.Value);
								}
							}
						}
					}
					else
					{
						if (back.SuffixItemId.HasValue)
						{
							relatedItems.Add(back.SuffixItemId.Value);
						}
						foreach (InfusionSlot slot3 in back.InfusionSlots)
						{
							if (slot3.ItemId.HasValue)
							{
								relatedItems.Add(slot3.ItemId.Value);
							}
						}
						foreach (InfusionSlotUpgradeSource source in back.UpgradesFrom)
						{
							relatedItems.Add(source.ItemId);
						}
						foreach (InfusionSlotUpgradePath upgrade in back.UpgradesInto)
						{
							relatedItems.Add(upgrade.ItemId);
						}
					}
				}
				else
				{
					if (armor.SuffixItemId.HasValue)
					{
						relatedItems.Add(armor.SuffixItemId.Value);
					}
					foreach (InfusionSlot slot2 in armor.InfusionSlots)
					{
						if (slot2.ItemId.HasValue)
						{
							relatedItems.Add(slot2.ItemId.Value);
						}
					}
				}
			}
			else
			{
				if (weapon.SuffixItemId.HasValue)
				{
					relatedItems.Add(weapon.SuffixItemId.Value);
				}
				if (weapon.SecondarySuffixItemId.HasValue)
				{
					relatedItems.Add(weapon.SecondarySuffixItemId.Value);
				}
				foreach (InfusionSlot slot in weapon.InfusionSlots)
				{
					if (slot.ItemId.HasValue)
					{
						relatedItems.Add(slot.ItemId.Value);
					}
				}
			}
			resultContext.ResultTotal += relatedItems.Count;
			await foreach (Item item2 in (from i in context.Items.AsNoTracking()
				where relatedItems.Contains(i.Id)
				select i).AsAsyncEnumerable().WithCancellation(cancellationToken))
			{
				yield return item2;
			}
		}
	}
}
