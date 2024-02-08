using System.Collections.Generic;
using MysticCrafting.Models;

namespace MysticCrafting.Module.Repositories
{
	public interface IRecipeRepository : IRepository
	{
		IEnumerable<MysticRecipe> GetRecipes(int itemId);
	}
}
