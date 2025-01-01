using System;
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

namespace SL.ChatLinks.UI.Tabs.Items.Services
{
	public sealed class ItemSearch
	{
		[CompilerGenerated]
		private ChatLinksContext _003Ccontext_003EP;

		private static readonly Regex ChatLinkPattern = new Regex("^\\[&[A-Za-z0-9+/=]+\\]$", RegexOptions.Compiled);

		private readonly IQueryable<Item> _items;

		public ItemSearch(ChatLinksContext context)
		{
			_003Ccontext_003EP = context;
			_items = _003Ccontext_003EP.Items.AsNoTracking();
			base._002Ector();
		}

		public IAsyncEnumerable<Item> NewItems(int limit)
		{
			return _items.OrderByDescending((Item item) => item.Id).Take(limit).AsAsyncEnumerable();
		}

		public IAsyncEnumerable<T> OfType<T>() where T : Item
		{
			return _items.OfType<T>().AsAsyncEnumerable();
		}

		public async IAsyncEnumerable<Item> Search(string query, int limit, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			if (ChatLinkPattern.IsMatch(query))
			{
				ItemLink chatLink = ItemLink.Parse(query);
				await foreach (Item item in SearchByChatLink(chatLink, cancellationToken))
				{
					yield return item;
				}
				yield break;
			}
			await foreach (Item item2 in _003Ccontext_003EP.Items.FromSqlInterpolated($"SELECT * FROM Items\r\nWHERE Name LIKE '%' || {query} || '%'\r\nORDER BY LevenshteinDistance({query}, Name)").AsNoTracking().Take(limit)
				.AsAsyncEnumerable()
				.WithCancellation(cancellationToken))
			{
				yield return item2;
			}
		}

		private async IAsyncEnumerable<Item> SearchByChatLink(ItemLink link, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			ItemLink link2 = link;
			Item item = await _items.SingleOrDefaultAsync((Item row) => row.Id == link2.ItemId, cancellationToken);
			if ((object)item == null)
			{
				yield break;
			}
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
			await foreach (Item item2 in _items.Where((Item i) => relatedItems.Contains(i.Id)).AsAsyncEnumerable().WithCancellation(cancellationToken))
			{
				yield return item2;
			}
		}

		private static int LevenshteinDistance(string a, string b)
		{
			if (string.IsNullOrEmpty(a))
			{
				if (!string.IsNullOrEmpty(b))
				{
					return b.Length;
				}
				return 0;
			}
			if (string.IsNullOrEmpty(b))
			{
				return a.Length;
			}
			int[,] costs = new int[a.Length + 1, b.Length + 1];
			for (int j = 0; j <= a.Length; j++)
			{
				costs[j, 0] = j;
			}
			for (int l = 0; l <= b.Length; l++)
			{
				costs[0, l] = l;
			}
			for (int i = 1; i <= a.Length; i++)
			{
				for (int k = 1; k <= b.Length; k++)
				{
					int cost = ((b[k - 1] != a[i - 1]) ? 1 : 0);
					costs[i, k] = Math.Min(Math.Min(costs[i - 1, k] + 1, costs[i, k - 1] + 1), costs[i - 1, k - 1] + cost);
				}
			}
			return costs[a.Length, b.Length];
		}
	}
}
