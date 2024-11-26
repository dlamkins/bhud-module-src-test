using Atzie.MysticCrafting.Models.Account;
using Blish_HUD.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class AchievementNode : IngredientNode
	{
		public AchievementNode(int id, int quantity, Container parent)
			: base(id, quantity, parent)
		{
		}

		public AchievementNode(Achievement achievement, Container parent, int? index = null, bool loadingChildren = false)
			: base(achievement.Id, 1, parent, index, showUnitCount: false, loadingChildren)
		{
			base.Name = achievement.Name;
			base.IconTexture = ServiceContainer.TextureRepository.GetTexture(155061);
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
