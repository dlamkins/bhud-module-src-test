using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MysticCrafting.Models;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class RecipeRepository : IRecipeRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<MysticRecipe> Recipes { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; set; }

		public string FileName => "recipes_data.json";

		public RecipeRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			Recipes = (await _dataService.LoadFromFileAsync<IList<MysticRecipe>>(FileName)) ?? new List<MysticRecipe>();
			Loaded = true;
			return $"{Recipes.Count} recipes loaded";
		}

		public IEnumerable<MysticRecipe> GetRecipes(int itemId)
		{
			if (Recipes == null)
			{
				return new List<MysticRecipe>();
			}
			return Recipes.Where((MysticRecipe r) => r.OutputItemId == itemId);
		}
	}
}
