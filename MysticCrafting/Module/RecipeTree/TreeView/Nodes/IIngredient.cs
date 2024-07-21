namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public interface IIngredient
	{
		int UnitCount { get; set; }

		string FullPath { get; }
	}
}
