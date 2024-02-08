using System.Collections.Generic;
using MysticCrafting.Models;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingRecipeRepository : LoggingRepository, IRecipeRepository, IRepository
	{
		private readonly IRecipeRepository _recipeRepository;

		public LoggingRecipeRepository(IRecipeRepository recipeRepository)
			: base(recipeRepository)
		{
			_recipeRepository = recipeRepository;
		}

		public IEnumerable<MysticRecipe> GetRecipes(int itemId)
		{
			return _recipeRepository.GetRecipes(itemId);
		}
	}
}
