using System.Collections.Generic;
using MysticCrafting.Models;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class SwapItemSource : ItemSource
	{
		public IList<MysticRecipe> Recipes { get; set; }

		public IList<int> SwappableItemIds { get; set; }

		public SwapItemSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.SwapIcon;
		}
	}
}
