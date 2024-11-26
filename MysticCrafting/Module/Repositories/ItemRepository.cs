using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
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
		private SQLiteConnection Connection { get; set; }

		private ISqliteDbService Service { get; set; }

		public void Initialize(ISqliteDbService service)
		{
			Service = service;
			Connection = new SQLiteConnection(service.DatabaseFilePath);
		}

		public Item GetItem(int itemId)
		{
			using SQLiteConnection connection = new SQLiteConnection(Service.DatabaseFilePath);
			return ReadOperations.FindWithChildren<Item>(connection, (object)itemId, true);
		}

		public IList<int> GetItemIds()
		{
			return (from i in Connection.Query<Item>("SELECT Id from Item", Array.Empty<object>())
				select i.Id).ToList();
		}

		public IEnumerable<Item> FilterItems(MysticItemFilter filter)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected I4, but got Unknown
			string query = "SELECT Item.*, COUNT(Recipe.Id) AS RecipeCount, COUNT(VendorSellsItem.Id) AS VendorSellsItemCount, Recipe.DisciplinesBlobbed, l.* FROM Item LEFT JOIN Recipe ON Item.Id = Recipe.OutputItemId AND OutputQuantity = 1 LEFT JOIN VendorSellsItem ON Item.Id = VendorSellsItem.ItemId AND VendorSellsItem.IsHistorical = 0 LEFT JOIN (SELECT Locale, Name, Description, ItemId FROM ItemLocalization) as l ON l.ItemId = Item.Id AND l.Locale = ? WHERE ";
			List<object> parameters = new List<object> { (int)GameService.Overlay.get_UserLocale().get_Value() };
			if (filter.IsFavorite)
			{
				IList<int> favoriteIds = ServiceContainer.FavoritesRepository.GetAll();
				query = query + "Item.Id IN (" + string.Join(",", favoriteIds.Select((int id) => "?")) + ") AND ";
				foreach (int id3 in favoriteIds)
				{
					parameters.Add(id3);
				}
			}
			if (filter.Ids != null && filter.Ids.Any())
			{
				query = query + "Item.Id IN (" + string.Join(",", filter.Ids.Select((int id) => "?")) + ") AND ";
				foreach (int id2 in filter.Ids)
				{
					parameters.Add(id2);
				}
			}
			if (filter.Type != 0)
			{
				query += "Item.Type = ? AND ";
				parameters.Add((int)filter.Type);
			}
			if (filter.Types != null && filter.Types.Any())
			{
				query = query + "Item.Type IN (" + string.Join(",", filter.Types.Select((int id) => " ? ")) + ") AND ";
				foreach (int type in filter.Types)
				{
					parameters.Add(type);
				}
			}
			if (filter.Categories.Any())
			{
				query = query + "Item.CategoryId IN (" + string.Join(",", filter.Categories.Select((int id) => " ? ")) + ") AND ";
				foreach (int category in filter.Categories)
				{
					parameters.Add(category);
				}
			}
			if (!string.IsNullOrEmpty(filter.DetailsType))
			{
				query += "Item.DetailsType = ? AND ";
				parameters.Add(filter.DetailsType);
			}
			if (filter.Rarities != null && filter.Rarities.Any())
			{
				query = query + "Rarity IN (" + string.Join(",", filter.Rarities.Select((ItemRarity id) => " ? ")) + ") AND ";
				foreach (ItemRarity rarity in filter.Rarities)
				{
					parameters.Add((int)rarity);
				}
			}
			if (filter.WeightClasses != null && filter.WeightClasses.Any())
			{
				query = query + "ArmorWeight IN (" + string.Join(",", filter.WeightClasses.Select((WeightClass id) => " ? ")) + ") AND ";
				foreach (WeightClass weightClass in filter.WeightClasses)
				{
					parameters.Add((int)weightClass);
				}
			}
			if ((filter.Disciplines != null && filter.Disciplines.Any()) || filter.IsTradeable || filter.SoldByVendor || filter.HasMysticForgeRecipe)
			{
				query += "(";
				if (filter.Disciplines != null && filter.Disciplines.Any())
				{
					query = query + "EXISTS (SELECT 1 FROM json_each(DisciplinesBlobbed) WHERE value IN (" + string.Join(",", filter.Disciplines.Select((Discipline id) => " ? ")) + ")) OR ";
					foreach (Discipline discipline in filter.Disciplines)
					{
						parameters.Add((int)discipline);
					}
				}
				if (filter.IsTradeable)
				{
					query += "CanBeTraded = 1 OR ";
				}
				if (filter.SoldByVendor)
				{
					query += "SoldByVendor = 1 OR ";
				}
				if (filter.HasMysticForgeRecipe)
				{
					query += "Item.HasMysticForgeRecipe = 1 OR ";
				}
				query = query.Remove(query.LastIndexOf("OR "));
				query += ") AND ";
			}
			if (!string.IsNullOrEmpty(filter.NameContainsText))
			{
				query += "UPPER(Item.Name) LIKE ? AND ";
				parameters.Add("%" + filter.NameContainsText.ToUpper().Replace("'", "''") + "%");
			}
			query = query.Remove(query.LastIndexOf("AND "));
			query += "GROUP BY Item.Id ";
			query += "limit 300";
			using SQLiteConnection connection = new SQLiteConnection(Service.DatabaseFilePath);
			return connection.Query<Item>(query, parameters.ToArray());
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Connection?.Dispose();
			}
		}
	}
}
