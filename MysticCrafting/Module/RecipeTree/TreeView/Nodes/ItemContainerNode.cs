using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class ItemContainerNode : IngredientNode
	{
		public ItemContainerNode(int id, int quantity, Container parent)
			: base(id, quantity, parent)
		{
		}

		public ItemContainerNode(MysticItemContainer container, Container parent, int? index = null, bool loadingChildren = false)
			: base(container.ItemId, 1, parent, index, showUnitCount: false, loadingChildren)
		{
			base.Name = container.Name;
			base.IconTexture = ServiceContainer.TextureRepository.GetRefTexture("map_explored.png");
		}

		protected override void BuildMenuStrip()
		{
		}

		public override bool UpdatePlayerUnitCount()
		{
			return false;
		}
	}
}
