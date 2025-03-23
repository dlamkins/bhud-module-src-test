using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class RecipeSource : Source
	{
		public List<IngredientSource> recipeIngredients;

		public List<string> disciplines;

		public RecipeSource(List<IngredientSource> recipeIngredients_, List<string> disciplines_)
			: base(0uL, null)
		{
			recipeIngredients = recipeIngredients_;
			disciplines = disciplines_;
		}

		public override string ToString()
		{
			return string.Join(", ", recipeIngredients) + "\n" + string.Join(", ", disciplines);
		}
	}
}
