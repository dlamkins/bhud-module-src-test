using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD.Content;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class RecipeSource : ItemSource
	{
		public Recipe Recipe { get; set; }

		public RecipeType Source { get; set; }

		public override AsyncTexture2D Icon => ServiceContainer.TextureRepository.GetRecipeSourceIcon(this);

		public RecipeSource(string uniqueId)
			: base(uniqueId)
		{
		}
	}
}
