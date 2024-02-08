using System.Linq;
using Blish_HUD.Content;
using MysticCrafting.Models;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class RecipeSource : ItemSource
	{
		public MysticRecipe Recipe { get; set; }

		public override AsyncTexture2D Icon
		{
			get
			{
				ITextureRepository textureRepo = ServiceContainer.TextureRepository;
				AsyncTexture2D icon = textureRepo.GetRefTexture("mystic_forge.png");
				if (base.DisplayName == "Automatic" || base.DisplayName == "Recipe sheet")
				{
					if (this != null)
					{
						icon = ((Recipe.Disciplines.Count() != 1) ? textureRepo.Textures.CraftingIcon : IconHelper.GetIcon(Recipe.Disciplines.FirstOrDefault()));
					}
				}
				else if (base.DisplayName == "Other")
				{
					icon = textureRepo.Textures.QuestionMark;
				}
				return icon;
			}
		}

		public RecipeSource(string uniqueId)
			: base(uniqueId)
		{
		}
	}
}
