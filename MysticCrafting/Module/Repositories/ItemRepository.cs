using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace MysticCrafting.Module.Repositories
{
	public class ItemRepository : IItemRepository, IDisposable
	{
		private List<int> IgnoreIds = new List<int>
		{
			85380, 31049, 85406, 91205, 91235, 90986, 91183, 91255, 91231, 91218,
			91196, 91206, 91115, 91178, 91197, 37104
		};

		private SQLiteConnection Connection { get; set; }

		public void Initialize(IDataService service)
		{
			Connection = new SQLiteConnection(service.DatabaseFilePath);
		}

		public Item GetItem(int itemId)
		{
			return ReadOperations.FindWithChildren<Item>(Connection, (object)itemId, true);
		}

		public IList<int> GetItemIds()
		{
			return (from i in Connection.Query<Item>("SELECT Id from Item", Array.Empty<object>())
				select i.Id).ToList();
		}

		public IEnumerable<Item> FilterItems(MysticItemFilter filter)
		{
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected I4, but got Unknown
			List<string> whereClauses = new List<string>();
			if (filter.IsFavorite)
			{
				IList<int> favoriteIds = ServiceContainer.FavoritesRepository.GetAll();
				whereClauses.Add("Id IN (" + string.Join(",", favoriteIds) + ")");
			}
			if (filter.Type != 0)
			{
				whereClauses.Add($"Type = {(int)filter.Type}");
			}
			if (!string.IsNullOrEmpty(filter.DetailsType))
			{
				whereClauses.Add("DetailsType = '" + filter.DetailsType + "'");
			}
			if (filter.Rarity != 0)
			{
				whereClauses.Add($"Rarity = {(int)filter.Rarity}");
			}
			if (!string.IsNullOrEmpty(filter.NameContainsText))
			{
				whereClauses.Add("UPPER(Item.Name) LIKE '%" + filter.NameContainsText.ToUpper().Replace("'", "''") + "%'");
			}
			if (filter.Weight != 0 && !filter.WeightFilterDisabled)
			{
				whereClauses.Add($"ArmorWeight = {(int)filter.Weight}");
			}
			if (!string.IsNullOrEmpty(filter.LegendaryType) && !filter.WeightFilterDisabled)
			{
				string type = filter.LegendaryType.Split(' ')[0];
				if (!string.IsNullOrEmpty(type))
				{
					whereClauses.Add("Item.Name LIKE '" + type + "%'");
				}
			}
			string query = "SELECT DISTINCT * from Item LEFT JOIN (SELECT Recipe.OutputItemId, COUNT(1) AS RecipeCount FROM Recipe WHERE Recipe.IsMysticForgeRecipe = 1 OR Recipe.Name LIKE 'Obsidian%' GROUP BY Recipe.Id) as Recipe ON Recipe.OutputItemId = Item.Id" + $" LEFT JOIN (SELECT Locale, Name, Description, ItemId FROM ItemLocalization) as ItemLocalization ON ItemLocalization.ItemId = Item.Id AND ItemLocalization.Locale = {(int)GameService.Overlay.get_UserLocale().get_Value()}" + " WHERE " + string.Join(" AND ", whereClauses) + " AND RecipeCount > 0 GROUP BY Item.Id";
			return FilterByAvailable(Connection.Query<Item>(query, Array.Empty<object>())).ToList();
		}

		private IEnumerable<Item> FilterByAvailable(IEnumerable<Item> items)
		{
			return items?.Where((Item i) => !IgnoreIds.Contains(i.Id) && !i.Name.Contains("Mistforged") && !i.Name.Contains("Infusion") && !i.Name.Contains("Infused") && !i.Name.Contains("Attuned") && !i.Name.Contains("Rurik") && !i.Name.Contains("Eingestimmt") && !i.Name.Contains("eingestimmt") && !i.Name.Contains("Infundiert") && !i.Name.Contains("infundiert") && !i.Name.Contains("Nebelgeschmiedeter"));
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
