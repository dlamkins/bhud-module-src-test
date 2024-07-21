using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Crafting;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public interface IRecipeRepository : IDisposable
	{
		void Initialize(IDataService service);

		IList<Recipe> GetRecipes(int itemId);
	}
}
