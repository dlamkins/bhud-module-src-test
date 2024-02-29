using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using MysticCrafting.Models;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class RecipeRepository : IRecipeRepository
	{
		private IDocumentCollection<MysticRecipe> Recipes { get; } = ServiceContainer.Store.GetCollection<MysticRecipe>();


		public IEnumerable<MysticRecipe> GetRecipes(int itemId)
		{
			if (Recipes == null)
			{
				return new List<MysticRecipe>();
			}
			return from r in Recipes.AsQueryable()
				where r.OutputItemId == itemId
				select r;
		}
	}
}
