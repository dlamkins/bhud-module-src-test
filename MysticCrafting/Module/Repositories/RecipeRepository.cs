using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace MysticCrafting.Module.Repositories
{
	public class RecipeRepository : IRecipeRepository, IDisposable
	{
		private SQLiteConnection Connection { get; set; }

		public void Initialize(IDataService service)
		{
			Connection = new SQLiteConnection(service.DatabaseFilePath);
		}

		public IList<Recipe> GetRecipes(int itemId)
		{
			if (ReadOperations.GetWithChildren<Item>(Connection, (object)itemId, true)?.Recipes == null)
			{
				new List<Recipe>();
			}
			return ReadOperations.GetWithChildren<Item>(Connection, (object)itemId, true)?.Recipes ?? new List<Recipe>();
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
