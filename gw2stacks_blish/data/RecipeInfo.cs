using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	internal class RecipeInfo
	{
		public int Id;

		public int Type;

		public IReadOnlyList<RecipeIngredient> Ingredients;

		public int OutputItemId;

		public List<int> Disciplines;

		public string chatLink;

		public RecipeInfo()
		{
			Id = 0;
			Type = 0;
			Ingredients = null;
			OutputItemId = 0;
			Disciplines = new List<int>();
			chatLink = "";
		}
	}
}
