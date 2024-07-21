using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Crafting;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class SwapItemSource : ItemSource
	{
		public IList<Recipe> Recipes { get; set; }

		public IList<int> SwappableItemIds { get; set; }

		public SwapItemSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.SwapIcon;
		}
	}
}
